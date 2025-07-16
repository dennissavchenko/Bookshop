namespace BookShopServer.Repositories.OrderRepositories;

/// <summary>
/// Defines contract methods for managing item entries in a customer's cart (order items).
/// </summary>
public interface IOrderItemRepository
{
    /// <summary>
    /// Adds an item with a specified quantity to an existing cart (order).
    /// </summary>
    /// <param name="orderId">The ID of the order (cart).</param>
    /// <param name="itemId">The ID of the item to add.</param>
    /// <param name="quantity">The quantity of the item.</param>
    Task AddItemToCartAsync(int orderId, int itemId, int quantity);

    /// <summary>
    /// Removes a specific item from the cart (order).
    /// </summary>
    /// <param name="orderId">The ID of the order (cart).</param>
    /// <param name="itemId">The ID of the item to remove.</param>
    Task RemoveItemFromCartAsync(int orderId, int itemId);

    /// <summary>
    /// Updates the quantity of a specific item in the cart (order).
    /// </summary>
    /// <param name="orderId">The ID of the order (cart).</param>
    /// <param name="itemId">The ID of the item to update.</param>
    /// <param name="quantity">The new quantity to set.</param>
    Task UpdateItemQuantityInCartAsync(int orderId, int itemId, int quantity);

    /// <summary>
    /// Checks whether a specific item exists in the given cart (order).
    /// </summary>
    /// <param name="orderId">The ID of the order (cart).</param>
    /// <param name="itemId">The ID of the item to check.</param>
    /// <returns><c>true</c> if the item exists in the order; otherwise, <c>false</c>.</returns>
    Task<bool> OrderItemExistsAsync(int orderId, int itemId);

    /// <summary>
    /// Gets the quantity of a specific item in the cart (order).
    /// </summary>
    /// <param name="orderId">The ID of the order (cart).</param>
    /// <param name="itemId">The ID of the item.</param>
    /// <returns>The quantity of the item in the order.</returns>
    Task<int> GetItemQuantityInCartAsync(int orderId, int itemId);
}
