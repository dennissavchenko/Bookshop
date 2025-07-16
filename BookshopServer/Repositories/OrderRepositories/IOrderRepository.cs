using BookShopServer.Entities;

namespace BookShopServer.Repositories.OrderRepositories;

/// <summary>
/// Defines contract methods for managing orders and shopping carts in the system.
/// </summary>
public interface IOrderRepository
{
    /// <summary>
    /// Retrieves an order by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the order to retrieve.</param>
    /// <returns>The order with the specified ID.</returns>
    Task<Order?> GetOrderByIdAsync(int id);

    /// <summary>
    /// Retrieves all orders in the system.
    /// </summary>
    /// <returns>A collection of all orders.</returns>
    Task<IEnumerable<Order>> GetAllOrdersAsync();

    /// <summary>
    /// Retrieves the current cart (open order) for a specific customer.
    /// </summary>
    /// <param name="customerId">The ID of the customer.</param>
    /// <returns>The cart associated with the specified customer, or null if none exists.</returns>
    Task<Order?> GetCartByCustomerIdAsync(int customerId);

    /// <summary>
    /// Adds a new order to the system.
    /// </summary>
    /// <param name="order">The order to add.</param>
    /// <returns>The ID of the newly created order.</returns>
    Task<int> AddOrderAsync(Order order);

    /// <summary>
    /// Updates an existing order in the system.
    /// </summary>
    /// <param name="order">The order with updated information.</param>
    Task UpdateOrderAsync(Order order);
    
    /// <summary>
    /// Updates an existing order in the system.
    /// </summary>
    /// <param name="orders">Collection of orders to update</param>
    Task UpdateOrdersAsync(IEnumerable<Order> orders);

    /// <summary>
    /// Checks whether an order with the given ID exists.
    /// </summary>
    /// <param name="id">The ID of the order.</param>
    /// <returns><c>true</c> if the order exists; otherwise, <c>false</c>.</returns>
    Task<bool> OrderExistsAsync(int id);

    /// <summary>
    /// Checks whether a cart (open order) with the given ID exists.
    /// </summary>
    /// <param name="id">The ID of the cart.</param>
    /// <returns><c>true</c> if the cart exists; otherwise, <c>false</c>.</returns>
    Task<bool> CartExistsAsync(int id);

    /// <summary>
    /// Deletes a cart (open order) by its ID.
    /// </summary>
    /// <param name="id">The ID of the cart to delete.</param>
    Task DeleteCartAsync(int id);
    
    /// <summary>
    /// Deletes carts (open orders).
    /// </summary>
    /// <param name="carts">Collection of carts to delete</param>
    Task DeleteCartsAsync(IEnumerable<Order> carts);

    /// <summary>
    /// Retrieves all carts (orders that are not yet finalized).
    /// </summary>
    /// <returns>A collection of all current carts.</returns>
    Task<IEnumerable<Order>> GetAllCartsAsync();

    /// <summary>
    /// Retrieves all finalized orders placed by a specific customer.
    /// </summary>
    /// <param name="customerId">The ID of the customer.</param>
    /// <returns>A collection of orders placed by the specified customer.</returns>
    Task<ICollection<Order>> GetOrdersByCustomerIdAsync(int customerId);

    /// <summary>
    /// Retrieves all orders that contain a specific item.
    /// </summary>
    /// <param name="itemId">The ID of the item.</param>
    /// <returns>A collection of orders containing the specified item.</returns>
    Task<IEnumerable<Order>> GetOrdersByItemIdAsync(int itemId);

    /// <summary>
    /// Retrieves all orders that have a specific status.
    /// </summary>
    /// <param name="status">The status to filter orders by.</param>
    /// <returns>A collection of orders with the specified status.</returns>
    Task<IEnumerable<Order>> GetAllOrdersOfStatusAsync(OrderStatus status);
    
}
