using BookShopServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookShopServer.Repositories.OrderRepositories;

/// <summary>
/// Repository class for managing orders and shopping carts, 
/// including operations related to creation, retrieval, updating, and deletion.
/// </summary>
public class OrderRepository : IOrderRepository
{
    private readonly Context _context;

    /// <summary>
    /// Initializes the repository with the specified database context.
    /// </summary>
    /// <param name="context">Database context.</param>
    public OrderRepository(Context context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves an order by its ID, including order items, their publishers, reviews, and book details.
    /// </summary>
    /// <param name="id">Order ID.</param>
    /// <returns>The order with related data.</returns>
    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        var order = await _context.Orders
            .Include(x => x.OrderItems)
                .ThenInclude(x => x.Item)
                    .ThenInclude(x => x.Publisher)
            .Include(x => x.OrderItems)
                .ThenInclude(x => x.Item)
                    .ThenInclude(x => x.Reviews)
            .Include(x => x.OrderItems)
                .ThenInclude(x => x.Item)
                    .ThenInclude(x => x.Book)
            .FirstAsync(x => x.Id == id);

        // Load additional book details if the item is a book
        foreach (var orderItem in order.OrderItems)
        {
            if (orderItem.Item.Book != null)
            {
                await _context.Entry(orderItem.Item.Book).Collection(b => b.Authors).LoadAsync();
                await _context.Entry(orderItem.Item.Book).Collection(g => g.Genres).LoadAsync();
            }
        }

        return order;
    }

    /// <summary>
    /// Retrieves all orders with their order items and related data.
    /// </summary>
    /// <returns>Collection of all orders.</returns>
    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders
            .Include(x => x.OrderItems)
                .ThenInclude(x => x.Item)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves all orders placed by a specific customer.
    /// </summary>
    /// <param name="customerId">Customer ID.</param>
    /// <returns>Collection of orders.</returns>
    public async Task<ICollection<Order>> GetOrdersByCustomerIdAsync(int customerId)
    {
        return await _context.Orders
            .Where(x => x.CustomerId == customerId)
            .Include(x => x.OrderItems)
                .ThenInclude(x => x.Item)
            .ToListAsync();
    }

    /// <summary>
    /// Adds a new order to the database.
    /// </summary>
    /// <param name="order">Order to add.</param>
    /// <returns>Generated ID of the new order.</returns>
    public async Task<int> AddOrderAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
        return order.Id;
    }

    /// <summary>
    /// Updates an existing order in the database.
    /// </summary>
    /// <param name="order">Order to update.</param>
    public async Task UpdateOrderAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Checks whether an order with the specified ID exists.
    /// </summary>
    /// <param name="id">Order ID.</param>
    /// <returns>True if the order exists, otherwise false.</returns>
    public async Task<bool> OrderExistsAsync(int id)
    {
        return await _context.Orders.AnyAsync(x => x.Id == id);
    }

    /// <summary>
    /// Retrieves the active cart for a specific customer, including item details and books.
    /// </summary>
    /// <param name="customerId">Customer ID.</param>
    /// <returns>The customer's cart, or null if not found.</returns>
    public async Task<Order?> GetCartByCustomerIdAsync(int customerId)
    {
        var cart = await _context.Orders
            .Where(x => x.CustomerId == customerId && x.OrderStatus == OrderStatus.Cart)
            .Include(x => x.OrderItems)
                .ThenInclude(x => x.Item)
                    .ThenInclude(x => x.Publisher)
            .Include(x => x.OrderItems)
                .ThenInclude(x => x.Item)
                    .ThenInclude(x => x.Reviews)
            .Include(x => x.OrderItems)
                .ThenInclude(x => x.Item)
                    .ThenInclude(x => x.Book)
            .FirstOrDefaultAsync();

        if (cart == null)
            return null;

        foreach (var orderItem in cart.OrderItems)
        {
            if (orderItem.Item.Book != null)
            {
                await _context.Entry(orderItem.Item.Book).Collection(b => b.Authors).LoadAsync();
                await _context.Entry(orderItem.Item.Book).Collection(g => g.Genres).LoadAsync();
            }
        }

        return cart;
    }

    /// <summary>
    /// Checks whether a cart with the specified ID exists.
    /// </summary>
    /// <param name="id">Order ID.</param>
    /// <returns>True if a cart with this ID exists, otherwise false.</returns>
    public async Task<bool> CartExistsAsync(int id)
    {
        return await _context.Orders.AnyAsync(x => x.Id == id && x.OrderStatus == OrderStatus.Cart);
    }

    /// <summary>
    /// Deletes the cart with the specified ID, if it has status "Cart".
    /// </summary>
    /// <param name="id">Order ID.</param>
    public async Task DeleteCartAsync(int id)
    {
        var order = await _context.Orders.FirstAsync(x => x.Id == id);
        if (order.OrderStatus == OrderStatus.Cart)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Retrieves all carts in the system.
    /// </summary>
    /// <returns>Collection of cart orders.</returns>
    public async Task<IEnumerable<Order>> GetAllCartsAsync()
    {
        return await _context.Orders
            .Where(x => x.OrderStatus == OrderStatus.Cart)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves all orders with a specific status.
    /// </summary>
    /// <param name="status">Order status to filter by.</param>
    /// <returns>Collection of orders with the specified status.</returns>
    public async Task<IEnumerable<Order>> GetAllOrdersOfStatusAsync(OrderStatus status)
    {
        return await _context.Orders
            .Where(x => x.OrderStatus == status)
            .Include(x => x.OrderItems)
                .ThenInclude(x => x.Item)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves all orders that contain a specific item.
    /// </summary>
    /// <param name="itemId">Item ID.</param>
    /// <returns>Collection of orders containing the item.</returns>
    public async Task<IEnumerable<Order>> GetOrdersByItemIdAsync(int itemId)
    {
        return await _context.Orders
            .Where(x => x.OrderItems.Any(oi => oi.ItemId == itemId))
            .Include(x => x.OrderItems)
                .ThenInclude(x => x.Item)
            .ToListAsync();
    }

    /// <summary>
    /// Deletes carts (open orders).
    /// </summary>
    /// <param name="carts">Collection of carts to delete</param>
    public async Task DeleteCartsAsync(IEnumerable<Order> carts)
    {
        foreach (var cart in carts)
        {
            if(cart.OrderStatus == OrderStatus.Cart)
            {
                _context.Orders.Remove(cart);
            }
        }
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing order in the system.
    /// </summary>
    /// <param name="orders">Collection of orders to update</param>
    public async Task UpdateOrdersAsync(IEnumerable<Order> orders)
    {
        foreach (var order in orders)
        {
            _context.Orders.Update(order);
        }
        await _context.SaveChangesAsync();
    }
    
}
