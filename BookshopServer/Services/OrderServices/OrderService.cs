using BookShopServer.DTOs.ItemDTOs;
using BookShopServer.DTOs.OrderDTOs;
using BookShopServer.Entities;
using BookShopServer.Exceptions;
using BookShopServer.Repositories.ItemRepositories;
using BookShopServer.Repositories.OrderRepositories;
using BookShopServer.Repositories.UserRepositories;

namespace BookShopServer.Services.OrderServices;

/// <summary>
/// Provides services for managing order operations, including retrieving orders by status or customer,
/// processing checkout and confirmation, handling cancellations, changing order states,
/// assigning orders to a 'deleted customer', and retrieving items within an order.
/// </summary>
public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IItemRepository _itemRepository;

    /// <summary>
    /// Initializes a new instance of the <c>OrderService</c> class.
    /// </summary>
    /// <param name="orderRepository">The repository for order data.</param>
    /// <param name="customerRepository">The repository for customer data.</param>
    /// <param name="paymentRepository">The repository for payment data.</param>
    /// <param name="itemRepository">The repository for item data.</param>
    public OrderService(IOrderRepository orderRepository, ICustomerRepository customerRepository, IPaymentRepository paymentRepository, IItemRepository itemRepository)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _paymentRepository = paymentRepository;
        _itemRepository = itemRepository;
    }

    /// <summary>
    /// Retrieves a collection of all orders with a specific status, mapped to simplified DTOs.
    /// </summary>
    /// <param name="orderStatus">The status of the orders to retrieve.</param>
    /// <returns>A collection of <see cref="SimpleOrderDto"/> representing orders with the specified status.</returns>
    public async Task<IEnumerable<SimpleOrderDto>> GetAllOrdersOfStatusAsync(OrderStatus orderStatus)
    {
        var orders = await _orderRepository.GetAllOrdersOfStatusAsync(orderStatus);
        // Map order entities to SimpleOrderDto
        return orders.Select(order => new SimpleOrderDto
        {
            Id = order.Id,
            Status = order.OrderStatus.ToString(),
            TotalPrice = order.GetTotalPrice(),
            LastUpdatedAt = order.GetLastUpdatedAt(),
            CustomerId = order.CustomerId
        });
    }

    /// <summary>
    /// Retrieves a collection of all orders in the system, mapped to simplified DTOs.
    /// </summary>
    /// <returns>A collection of <see cref="SimpleOrderDto"/> representing all orders.</returns>
    public async Task<IEnumerable<SimpleOrderDto>> GetAllOrdersAsync()
    {
        var orders = await _orderRepository.GetAllOrdersAsync();
        // Map all order entities to SimpleOrderDto
        return orders.Select(order => new SimpleOrderDto
        {
            Id = order.Id,
            Status = order.OrderStatus.ToString(),
            TotalPrice = order.GetTotalPrice(),
            LastUpdatedAt = order.GetLastUpdatedAt(),
            CustomerId = order.CustomerId
        }).OrderByDescending(x => x.LastUpdatedAt);
    }

    /// <summary>
    /// Retrieves a customer's orders (excluding cart and pending states), ordered by their last update timestamp.
    /// </summary>
    /// <param name="customerId">The unique identifier of the customer.</param>
    /// <returns>A collection of <see cref="SimpleOrderDto"/> representing the customer's orders.</returns>
    /// <exception cref="NotFoundException">Thrown if the customer does not exist.</exception>
    public async Task<IEnumerable<SimpleOrderDto>> GetOrdersByCustomerIdOrderedByLastUpdateAsync(int customerId)
    {
        // Validate customer existence
        if(!await _customerRepository.CustomerExistsAsync(customerId))
            throw new NotFoundException("Customer with the given ID does not exist.");

        // Retrieve orders for the customer and filter out 'Cart' and 'Pending' statuses
        var orders = await _orderRepository.GetOrdersByCustomerIdAsync(customerId);
        orders = orders.Where(order => order.OrderStatus != OrderStatus.Cart && order.OrderStatus != OrderStatus.Pending).ToList();

        // Map and order the results by last update timestamp
        return orders.Select(order => new SimpleOrderDto
        {
            Id = order.Id,
            Status = order.OrderStatus.ToString(),
            TotalPrice = order.GetTotalPrice(),
            LastUpdatedAt = order.GetLastUpdatedAt(),
            CustomerId = order.CustomerId
        }).OrderByDescending(x => x.LastUpdatedAt);
    }

    /// <summary>
    /// Retrieves a detailed confirmed order by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the order.</param>
    /// <returns>A <see cref="ConfirmedOrderDto"/> containing the detailed order information.</returns>
    /// <exception cref="NotFoundException">Thrown if the order does not exist.</exception>
    public async Task<ConfirmedOrderDto> GetOrderByIdAsync(int id)
    {
        // Retrieve the order from the repository
        var order = await _orderRepository.GetOrderByIdAsync(id);
        
        // Validate order existence
        if (order == null)
            throw new NotFoundException("Order with the given ID does not exist.");

        // Map the order entity to a ConfirmedOrderDto
        return new ConfirmedOrderDto
        {
            Id = order.Id,
            Status = order.OrderStatus.ToString(),
            TotalPrice = order.GetTotalPrice(),
            ConfirmedAt = order.ConfirmedAt ?? DateTime.Now, // Default to current time if null
            PreparationStartedAt = order.PreparationStartedAt,
            ShippedAt = order.ShippedAt,
            DeliveredAt = order.DeliveredAt,
            CancelledAt = order.CancelledAt,
            Items = order.OrderItems.Select(x => new OrderItemDto
            {
                Item = SimpleItemDto.MapItem(x.Item),
                Quantity = x.Quantity
            }).ToList(),
            CustomerId = order.CustomerId
        };
    }

    /// <summary>
    /// Transitions a shopping cart to a pending order state (checkout process).
    /// </summary>
    /// <param name="id">The ID of the order (cart) to checkout.</param>
    /// <exception cref="NotFoundException">Thrown if the order does not exist.</exception>
    /// <exception cref="BadRequestException">Thrown if the order is not in the 'Cart' state.</exception>
    public async Task CheckoutOrderAsync(int id)
    {
        var order = await _orderRepository.GetOrderByIdAsync(id);
        
        // Validate order existence
        if(order == null)
            throw new NotFoundException("Order with the given ID does not exist.");
        
        // Ensure the order is in 'Cart' status before checkout
        if(order.OrderStatus != OrderStatus.Cart)
            throw new BadRequestException("Order is not in the cart state. Cannot checkout.");
        
        // Change order status to 'Pending'
        order.OrderStatus = OrderStatus.Pending;
        await _orderRepository.UpdateOrderAsync(order);
    }

    /// <summary>
    /// Confirms a pending order, processes payment, and decreases item stock.
    /// </summary>
    /// <param name="id">The ID of the order to confirm.</param>
    /// <param name="paymentType">The type of payment used for the order.</param>
    /// <exception cref="NotFoundException">Thrown if the order does not exist.</exception>
    /// <exception cref="BadRequestException">Thrown if the order is not in the 'Pending' state or if there's insufficient item stock.</exception>
    public async Task ConfirmOrderAsync(int id, PaymentType paymentType)
    {
        var order = await _orderRepository.GetOrderByIdAsync(id);
        
        // Validate order existence
        if(order == null)
            throw new NotFoundException("Order with the given ID does not exist.");
        
        // Ensure the order is in 'Pending' status before confirmation
        if(order.OrderStatus != OrderStatus.Pending)
            throw new BadRequestException("Order is not in the pending state. Cannot confirm.");
        
        // Set order status to 'Confirmed' and record confirmation timestamp
        order.OrderStatus = OrderStatus.Confirmed;
        order.ConfirmedAt = DateTime.Now;
        
        // Validate and decrease stock for each item in the order
        foreach (var orderItem in order.OrderItems)
        {
            if(await _itemRepository.GetItemAmountInStockAsync(orderItem.Item.Id) < orderItem.Quantity)
                throw new BadRequestException($"Not enough stock for item {orderItem.Item.Name}. Available: {orderItem.Item.AmountInStock}, Requested: {orderItem.Quantity}");
            await _itemRepository.DecreaseItemAmountInStockAsync(orderItem.Item.Id, orderItem.Quantity);
        }
        
        // Create and add payment record
        var payment = new Payment
        {
            OrderId = order.Id,
            Amount = order.GetTotalPrice(),
            PaymentType = paymentType,
            TimeStamp = DateTime.Now
        };
        await _paymentRepository.AddPaymentAsync(payment);

        // Update the order in the repository
        await _orderRepository.UpdateOrderAsync(order);
    }

    /// <summary>
    /// Cancels a confirmed order.
    /// </summary>
    /// <param name="id">The ID of the order to cancel.</param>
    /// <exception cref="NotFoundException">Thrown if the order does not exist.</exception>
    /// <exception cref="BadRequestException">Thrown if the order is not in the 'Confirmed' state.</exception>
    public async Task CancelOrderAsync(int id)
    {
        var order = await _orderRepository.GetOrderByIdAsync(id);
        
        // Validate order existence
        if(order == null)
            throw new NotFoundException("Order with the given ID does not exist.");
        
        // Ensure only 'Confirmed' orders can be cancelled
        if(order.OrderStatus != OrderStatus.Confirmed)
            throw new BadRequestException("Only confirmed orders can be cancelled.");
        
        // Set order status to 'Cancelled' and record cancellation timestamp
        order.OrderStatus = OrderStatus.Cancelled;
        order.CancelledAt = DateTime.Now;
        await _orderRepository.UpdateOrderAsync(order);
    }

    /// <summary>
    /// Changes the state of an order to a new <see cref="OrderStatus"/>.
    /// This method enforces a strict progression of order states.
    /// </summary>
    /// <param name="id">The ID of the order to change the state for.</param>
    /// <param name="orderStatus">The new status to set for the order.</param>
    /// <exception cref="NotFoundException">Thrown if the order does not exist.</exception>
    /// <exception cref="BadRequestException">Thrown if an invalid state transition is attempted.</exception>
    public async Task ChangeStateAsync(int id, OrderStatus orderStatus)
    {
        var order = await _orderRepository.GetOrderByIdAsync(id);
        
        // Validate order existence
        if(order == null)
            throw new NotFoundException("Order with the given ID does not exist.");

        // Prevent state changes for final or cart states
        if(order.OrderStatus == OrderStatus.Cancelled || order.OrderStatus == OrderStatus.Delivered)
            throw new BadRequestException("Cannot change the state of a cancelled or delivered order.");
        if(order.OrderStatus == OrderStatus.Cart)
            throw new BadRequestException("Cannot change the state of a cart order directly. Please checkout the cart first.");

        // Enforce valid state transitions
        if(order.OrderStatus == OrderStatus.Pending && orderStatus != OrderStatus.Cart)
            throw new BadRequestException("You can only change the state of a pending order to Cart."); 
        if(order.OrderStatus == OrderStatus.Confirmed && orderStatus != OrderStatus.Preparation)
            throw new BadRequestException("You can only change the state of a confirmed order to Preparation.");
        if(order.OrderStatus == OrderStatus.Preparation && orderStatus != OrderStatus.Shipped)
            throw new BadRequestException("You can only change the state of a preparation order to Shipped.");
        if(order.OrderStatus == OrderStatus.Shipped && orderStatus != OrderStatus.Delivered)
            throw new BadRequestException("You can only change the state of a shipped order to Delivered.");
        
        // Set the new order status
        order.OrderStatus = orderStatus;

        // Record timestamp for specific state transitions
        if (orderStatus == OrderStatus.Preparation)
        {
            order.PreparationStartedAt = DateTime.Now;
        }
        else if (orderStatus == OrderStatus.Shipped)
        {
            order.ShippedAt = DateTime.Now;
        }
        else if (orderStatus == OrderStatus.Delivered)
        {
            order.DeliveredAt = DateTime.Now;
        }

        // Update the order in the repository
        await _orderRepository.UpdateOrderAsync(order);
    }

    /// <summary>
    /// Reassigns all orders of a given customer to a special 'DeletedUser' customer.
    /// This is typically used for data anonymization upon customer deletion, ensuring order history is retained.
    /// Cart orders for the deleted customer are directly removed.
    /// </summary>
    /// <param name="customerId">The ID of the customer whose orders are to be reassigned.</param>
    public async Task AssignOrdersOfCustomerToDeletedCustomerAsync(int customerId)
    {
        // Only proceed if the customer exists
        if (await _customerRepository.CustomerExistsAsync(customerId))
        {
            // Retrieve all orders for the customer
            var orders = await _orderRepository.GetOrdersByCustomerIdAsync(customerId);
            
            // Retrieve the 'DeletedUser' customer entity
            var deletedCustomer = await _customerRepository.GetDeletedCustomerAsync();

            // Get users cart
            var cart = orders.FirstOrDefault(o => o.OrderStatus == OrderStatus.Cart);
            // Get orders to reassign (those that are not carts)
            var ordersToReassign = orders.Where(o => o.OrderStatus != OrderStatus.Cart).ToList();

            // Remove carts from the repository
            if (cart != null)
                await _orderRepository.DeleteCartAsync(cart.Id);
            
            // Reassign orders to the 'DeletedUser' customer
            foreach (var order in ordersToReassign)
            {
                order.CustomerId = deletedCustomer.UserId;
            }

            // Update the orders in the repository
            if(ordersToReassign.Count > 0)
                await _orderRepository.UpdateOrdersAsync(ordersToReassign);
        }
    }

    /// <summary>
    /// Retrieves a collection of simplified item DTOs representing all items within a specific order.
    /// </summary>
    /// <param name="orderId">The ID of the order.</param>
    /// <returns>A collection of <see cref="SimpleItemDto"/> representing the items in the order.</returns>
    /// <exception cref="NotFoundException">Thrown if the order does not exist.</exception>
    public async Task<IEnumerable<SimpleItemDto>> GetItemsInOrderAsync(int orderId)
    {
        // Retrieve the order and map its associated items to SimpleItemDto
        var order = await _orderRepository.GetOrderByIdAsync(orderId);
        
        // Validate order existence
        if (order == null)
            throw new NotFoundException("Order with the given ID does not exist.");
        
        return SimpleItemDto.MapItems(order.OrderItems.Select(x => x.Item));
    }
}