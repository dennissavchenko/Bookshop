using BookShopServer.DTOs.ItemDTOs;
using BookShopServer.DTOs.OrderDTOs;
using BookShopServer.DTOs.OrderDTOs.CartDTOs;
using BookShopServer.Entities;
using BookShopServer.Exceptions;
using BookShopServer.Repositories.ItemRepositories;
using BookShopServer.Repositories.OrderRepositories;
using BookShopServer.Repositories.UserRepositories;

namespace BookShopServer.Services.OrderServices;

/// <summary>
/// Provides services for managing shopping cart operations, including creating, retrieving,
/// adding/removing items, updating quantities, and handling cart expiration.
/// </summary>
public class CartService : ICartService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IOrderItemRepository _orderItemRepository;
    
    /// <summary>
    /// Initializes a new instance of the <c>CartService</c> class.
    /// </summary>
    /// <param name="orderRepository">The repository for order (cart) data.</param>
    /// <param name="customerRepository">The repository for customer data.</param>
    /// <param name="itemRepository">The repository for item data.</param>
    /// <param name="orderItemRepository">The repository for order item (cart item) data.</param>
    public CartService(IOrderRepository orderRepository, ICustomerRepository customerRepository, IItemRepository itemRepository, IOrderItemRepository orderItemRepository)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _itemRepository = itemRepository;
        _orderItemRepository = orderItemRepository;
    }
    
    /// <summary>
    /// Retrieves a customer's active shopping cart by their ID.
    /// </summary>
    /// <param name="customerId">The unique identifier of the customer.</param>
    /// <returns>A <see cref="CartDto"/> representing the customer's cart.</returns>
    /// <exception cref="NotFoundException">Thrown if the customer or their cart does not exist.</exception>
    public async Task<CartDto> GetCartByCustomerIdAsync(int customerId)
    {
        // Validate customer existence
        if(!await _customerRepository.CustomerExistsAsync(customerId))
            throw new NotFoundException("Customer with the given ID does not exist.");
        
        // Retrieve the cart from the repository
        var cart = await _orderRepository.GetCartByCustomerIdAsync(customerId);
        
        // Throw exception if cart is not found
        if (cart == null)
            throw new NotFoundException("Cart not found for the given customer ID.");
        
        // Map the cart entity to a CartDto and return
        return new CartDto
        {
            Id = cart.Id,
            CreatedAt = cart.CreatedAt,
            TotalPrice = cart.GetTotalPrice(),
            Items = cart.OrderItems.Select(x => new OrderItemDto
            {
                Item = SimpleItemDto.MapItem(x.Item),
                Quantity = x.Quantity
            }).ToList(),
            UserId = cart.CustomerId
        };
    }

    /// <summary>
    /// Adds a specified quantity of an item to a customer's shopping cart.
    /// If the item already exists in the cart, its quantity will be updated.
    /// </summary>
    /// <param name="addItemToCartDto">The DTO containing the cart ID, item ID, and quantity to add.</param>
    /// <exception cref="NotFoundException">Thrown if the cart or item does not exist.</exception>
    /// <exception cref="BadRequestException">Thrown if there isn't enough stock to fulfill the request.</exception>
    public async Task AddItemToCartAsync(AddItemToCartDto addItemToCartDto)
    {
        // Validate cart and item existence
        if (!await _itemRepository.ItemExistsAsync(addItemToCartDto.ItemId))
            throw new NotFoundException("Item with the given ID does not exist.");
        
        var order = await _orderRepository.GetOrderByIdAsync(addItemToCartDto.OrderId);
        
        if (order == null || order.OrderStatus != OrderStatus.Cart)
            throw new NotFoundException("Cart with the given ID does not exist.");
        
        // Check if the customer is old enough to purchase the item
        if (!await _customerRepository.CustomerOldEnoughForPurchaseAsync(order.CustomerId, addItemToCartDto.ItemId))
            throw new BadRequestException("Customer is not old enough to purchase this item.");
        
        // Validate sufficient stock
        if(await _itemRepository.GetItemAmountInStockAsync(addItemToCartDto.ItemId) < addItemToCartDto.Quantity)
            throw new BadRequestException("Not enough items in stock to add to the cart.");
        
        // Check if the item is already in the cart
        if (await _orderItemRepository.OrderItemExistsAsync(addItemToCartDto.OrderId, addItemToCartDto.ItemId))
        {
            // If item exists, update its quantity
            var currentQuantity = await _orderItemRepository.GetItemQuantityInCartAsync(addItemToCartDto.OrderId, addItemToCartDto.ItemId);
            if (currentQuantity + addItemToCartDto.Quantity > await _itemRepository.GetItemAmountInStockAsync(addItemToCartDto.ItemId))
                throw new BadRequestException("Not enough items in stock to update the cart.");
            await _orderItemRepository.UpdateItemQuantityInCartAsync(addItemToCartDto.OrderId, addItemToCartDto.ItemId, currentQuantity + addItemToCartDto.Quantity);
        }
        else
        {
            // If item does not exist, add it to the cart
            await _orderItemRepository.AddItemToCartAsync(addItemToCartDto.OrderId, addItemToCartDto.ItemId, addItemToCartDto.Quantity);
        }
    }

    /// <summary>
    /// Removes a specific item from a shopping cart.
    /// </summary>
    /// <param name="orderId">The ID of the cart.</param>
    /// <param name="itemId">The ID of the item to remove.</param>
    /// <exception cref="NotFoundException">Thrown if the cart, item, or the item within the cart does not exist.</exception>
    public async Task RemoveItemFromCartAsync(int orderId, int itemId)
    {
        // Validate cart and item existence
        if(!await _orderRepository.CartExistsAsync(orderId))
            throw new NotFoundException("Cart with the given ID does not exist.");
        if(!await _itemRepository.ItemExistsAsync(itemId))
            throw new NotFoundException("Item with the given ID does not exist.");
        
        // Validate if the item is actually in the cart
        if(!await _orderItemRepository.OrderItemExistsAsync(orderId, itemId))
            throw new NotFoundException("Item is not in the cart.");
        
        // Remove the item from the cart
        await _orderItemRepository.RemoveItemFromCartAsync(orderId, itemId);
    }

    /// <summary>
    /// Updates the quantity of a specific item within a shopping cart.
    /// </summary>
    /// <param name="addItemToCartDto">The DTO containing the cart ID, item ID, and the new quantity.</param>
    /// <exception cref="NotFoundException">Thrown if the cart, item, or the item within the cart does not exist.</exception>
    /// <exception cref="BadRequestException">Thrown if the requested quantity exceeds the available stock.</exception>
    public async Task UpdateItemQuantityInCartAsync(AddItemToCartDto addItemToCartDto)
    {
        // Validate cart and item existence
        if(!await _orderRepository.CartExistsAsync(addItemToCartDto.OrderId))
            throw new NotFoundException("Cart with the given ID does not exist.");
        if(!await _itemRepository.ItemExistsAsync(addItemToCartDto.ItemId))
            throw new NotFoundException("Item with the given ID does not exist.");
        
        // Validate if the item is in the cart
        if(!await _orderItemRepository.OrderItemExistsAsync(addItemToCartDto.OrderId, addItemToCartDto.ItemId))
            throw new NotFoundException("Item is not in the cart.");
        
        // Validate sufficient stock for the updated quantity
        if (addItemToCartDto.Quantity > await _itemRepository.GetItemAmountInStockAsync(addItemToCartDto.ItemId))
            throw new BadRequestException("Not enough items in stock to update the cart.");
        
        // Update the item quantity in the cart
        await _orderItemRepository.UpdateItemQuantityInCartAsync(addItemToCartDto.OrderId, addItemToCartDto.ItemId, addItemToCartDto.Quantity);
    }

    /// <summary>
    /// Creates a new shopping cart for a customer with an initial item.
    /// </summary>
    /// <param name="newCartDto">The DTO containing the customer ID, initial item ID, and quantity.</param>
    /// <exception cref="NotFoundException">Thrown if the customer or initial item does not exist.</exception>
    /// <exception cref="BadRequestException">Thrown if the customer is a 'DeletedUser', already has an active cart, or there's insufficient stock for the initial item.</exception>
    public async Task CreateCartAsync(NewCartDto newCartDto)
    {
        var customer = await _customerRepository.GetCustomerByIdAsync(newCartDto.CustomerId);
        
        // Validate customer existence
        if (customer == null)
            throw new NotFoundException("Customer with the given ID does not exist.");
        
        // Prevent creating a cart for a 'DeletedUser'
        if(customer.User.Username == "DeletedUser")
            throw new BadRequestException("Cannot create a cart for a deleted user.");
        
        // Check if the customer already has an active cart
        var existingCart = await _orderRepository.GetCartByCustomerIdAsync(newCartDto.CustomerId);
        if (existingCart != null)
            throw new BadRequestException("Customer already has an active cart.");
        
        // Validate initial item existence and stock
        if(!await _itemRepository.ItemExistsAsync(newCartDto.ItemId))
            throw new NotFoundException("Item with the given ID does not exist.");
        if (await _itemRepository.GetItemAmountInStockAsync(newCartDto.ItemId) < newCartDto.Quantity)
            throw new BadRequestException("Not enough items in stock to create the cart.");

        // Check if the customer is old enough to purchase the item
        if (!await _customerRepository.CustomerOldEnoughForPurchaseAsync(newCartDto.CustomerId, newCartDto.ItemId))
            throw new BadRequestException("Customer is not old enough to purchase this item.");
        
        // Create the new cart entity
        var newCart = new Order
        {
            CustomerId = newCartDto.CustomerId,
            OrderStatus = OrderStatus.Cart,
            CreatedAt = DateTime.UtcNow 
        };
        
        // Add the new cart to the repository and get its ID
        int cartId = await _orderRepository.AddOrderAsync(newCart);

        // Add the initial item to the newly created cart
        await AddItemToCartAsync(new AddItemToCartDto
        {
            OrderId = cartId,
            ItemId = newCartDto.ItemId,
            Quantity = newCartDto.Quantity
        });
    }

    /// <summary>
    /// Removes all shopping carts that have expired (e.g., inactive for more than 30 days).
    /// </summary>
    public async Task RemoveExpiredCartsAsync()
    {
        // Retrieve all active carts
        var carts = await _orderRepository.GetAllCartsAsync();
        
        // Filter for expired carts (status 'Cart' and older than 30 days)
        var expiredCarts = carts.Where(c => c.OrderStatus == OrderStatus.Cart && (DateTime.UtcNow - c.CreatedAt).TotalDays > 30).ToList();
        
        // Delete each expired cart
        foreach (var cart in expiredCarts)
        {
            await _orderRepository.DeleteCartAsync(cart.Id);
        }
    }
}