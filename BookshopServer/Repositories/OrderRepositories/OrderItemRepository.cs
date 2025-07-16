using BookShopServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookShopServer.Repositories.OrderRepositories;

/// <summary>
/// Provides operations for managing order items within shopping carts.
/// </summary>
public class OrderItemRepository : IOrderItemRepository
{
    private readonly Context _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderItemRepository"/> class.
    /// </summary>
    /// <param name="context">The database context used for data operations.</param>
    public OrderItemRepository(Context context)
    {
        _context = context;
    }

    /// <summary>
    /// Adds an item with a specified quantity to the shopping cart.
    /// </summary>
    /// <param name="orderId">The ID of the order (cart).</param>
    /// <param name="itemId">The ID of the item to add.</param>
    /// <param name="quantity">The quantity of the item.</param>
    public async Task AddItemToCartAsync(int orderId, int itemId, int quantity)
    {
        var cart = await _context.Orders.Include(x => x.OrderItems).FirstAsync(x => x.Id == orderId);
        var item = await _context.Items.FirstAsync(x => x.Id == itemId);
        var orderItem = new OrderItem
        {
            Item = item,
            ItemId = item.Id,
            Order = cart,
            OrderId = cart.Id,
            Quantity = quantity
        };
        await _context.OrderItems.AddAsync(orderItem);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Removes an item from the shopping cart.
    /// </summary>
    /// <param name="orderId">The ID of the order (cart).</param>
    /// <param name="itemId">The ID of the item to remove.</param>
    public async Task RemoveItemFromCartAsync(int orderId, int itemId)
    {
        var orderItem = await _context.OrderItems.FirstAsync(x => x.OrderId == orderId && x.ItemId == itemId);
        _context.OrderItems.Remove(orderItem);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates the quantity of a specific item in the shopping cart.
    /// </summary>
    /// <param name="orderId">The ID of the order (cart).</param>
    /// <param name="itemId">The ID of the item to update.</param>
    /// <param name="quantity">The new quantity to set.</param>
    public async Task UpdateItemQuantityInCartAsync(int orderId, int itemId, int quantity)
    {
        var orderItem = await _context.OrderItems.FirstAsync(x => x.OrderId == orderId && x.ItemId == itemId);
        orderItem.Quantity = quantity;
        _context.OrderItems.Update(orderItem);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Checks whether an item exists in the specified shopping cart.
    /// </summary>
    /// <param name="orderId">The ID of the order (cart).</param>
    /// <param name="itemId">The ID of the item to check.</param>
    /// <returns>True if the item exists in the cart; otherwise, false.</returns>
    public async Task<bool> OrderItemExistsAsync(int orderId, int itemId)
    {
        return await _context.OrderItems.AnyAsync(x => x.OrderId == orderId && x.ItemId == itemId);
    }

    /// <summary>
    /// Retrieves the quantity of a specific item in the shopping cart.
    /// </summary>
    /// <param name="orderId">The ID of the order (cart).</param>
    /// <param name="itemId">The ID of the item.</param>
    /// <returns>The quantity of the item in the cart.</returns>
    public async Task<int> GetItemQuantityInCartAsync(int orderId, int itemId)
    {
        var orderItem = await _context.OrderItems.FirstAsync(x => x.OrderId == orderId && x.ItemId == itemId);
        return orderItem.Quantity;
    }
}
