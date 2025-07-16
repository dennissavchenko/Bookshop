using BookShopServer.DTOs.ItemDTOs;
using BookShopServer.DTOs.OrderDTOs;
using BookShopServer.Entities;

namespace BookShopServer.Services.OrderServices;

/// <summary>
/// Defines the contract for services that manage order operations.
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Retrieves a collection of all orders with a specific status.
    /// </summary>
    /// <param name="orderStatus">The status of the orders to retrieve.</param>
    /// <returns>A collection of <see cref="SimpleOrderDto"/> representing orders with the specified status.</returns>
    Task<IEnumerable<SimpleOrderDto>> GetAllOrdersOfStatusAsync(OrderStatus orderStatus);

    /// <summary>
    /// Retrieves a collection of all orders in the system.
    /// </summary>
    /// <returns>A collection of <see cref="SimpleOrderDto"/> representing all orders.</returns>
    Task<IEnumerable<SimpleOrderDto>> GetAllOrdersAsync();

    /// <summary>
    /// Retrieves a collection of orders for a specific customer, ordered by their last update timestamp.
    /// </summary>
    /// <param name="customerId">The ID of the customer.</param>
    /// <returns>A collection of <see cref="SimpleOrderDto"/> representing the customer's orders.</returns>
    Task<IEnumerable<SimpleOrderDto>> GetOrdersByCustomerIdOrderedByLastUpdateAsync(int customerId);

    /// <summary>
    /// Retrieves a detailed confirmed order by its ID.
    /// </summary>
    /// <param name="id">The ID of the order to retrieve.</param>
    /// <returns>A <see cref="ConfirmedOrderDto"/> representing the detailed order.</returns>
    Task<ConfirmedOrderDto> GetOrderByIdAsync(int id);

    /// <summary>
    /// Processes the checkout for a given order.
    /// </summary>
    /// <param name="id">The ID of the order to checkout.</param>
    Task CheckoutOrderAsync(int id);

    /// <summary>
    /// Confirms an order with a specified payment type.
    /// </summary>
    /// <param name="id">The ID of the order to confirm.</param>
    /// <param name="paymentType">The type of payment used for the order.</param>
    Task ConfirmOrderAsync(int id, PaymentType paymentType);

    /// <summary>
    /// Cancels an existing order.
    /// </summary>
    /// <param name="id">The ID of the order to cancel.</param>
    Task CancelOrderAsync(int id);

    /// <summary>
    /// Changes the status of an order.
    /// </summary>
    /// <param name="id">The ID of the order to change the status for.</param>
    /// <param name="orderStatus">The new status to set for the order.</param>
    Task ChangeStateAsync(int id, OrderStatus orderStatus);

    /// <summary>
    /// Assigns all orders of a customer to a 'deleted customer' entity, typically for data retention while anonymizing.
    /// </summary>
    /// <param name="customerId">The ID of the customer whose orders are to be reassigned.</param>
    Task AssignOrdersOfCustomerToDeletedCustomerAsync(int customerId);

    /// <summary>
    /// Retrieves a collection of simplified item DTOs that are part of a specific order.
    /// </summary>
    /// <param name="orderId">The ID of the order.</param>
    /// <returns>A collection of <see cref="SimpleItemDto"/> representing items in the order.</returns>
    Task<IEnumerable<SimpleItemDto>> GetItemsInOrderAsync(int orderId);
}