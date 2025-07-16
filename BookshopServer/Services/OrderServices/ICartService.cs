using BookShopServer.DTOs.OrderDTOs.CartDTOs;

namespace BookShopServer.Services.OrderServices;

/// <summary>
/// Defines the contract for services that manage shopping cart operations.
/// </summary>
public interface ICartService
{
    /// <summary>
    /// Retrieves a shopping cart by the customer's ID.
    /// </summary>
    /// <param name="id">The ID of the customer.</param>
    /// <returns>A <see cref="CartDto"/> representing the customer's cart.</returns>
    Task<CartDto> GetCartByCustomerIdAsync(int id);

    /// <summary>
    /// Creates a new shopping cart for a customer.
    /// </summary>
    /// <param name="newCartDto">The DTO containing information to create the new cart.</param>
    Task CreateCartAsync(NewCartDto newCartDto);

    /// <summary>
    /// Adds an item to an existing shopping cart.
    /// </summary>
    /// <param name="addItemToCartDto">The DTO containing details of the item to add and the cart ID.</param>
    Task AddItemToCartAsync(AddItemToCartDto addItemToCartDto);

    /// <summary>
    /// Removes an item from a shopping cart.
    /// </summary>
    /// <param name="orderId">The ID of the order (cart) from which to remove the item.</param>
    /// <param name="itemId">The ID of the item to remove.</param>
    Task RemoveItemFromCartAsync(int orderId, int itemId);

    /// <summary>
    /// Updates the quantity of a specific item in a shopping cart.
    /// </summary>
    /// <param name="addItemToCartDto">The DTO containing the cart ID, item ID, and the new quantity.</param>
    Task UpdateItemQuantityInCartAsync(AddItemToCartDto addItemToCartDto);

    /// <summary>
    /// Removes all expired shopping carts from the system.
    /// </summary>
    Task RemoveExpiredCartsAsync();
}