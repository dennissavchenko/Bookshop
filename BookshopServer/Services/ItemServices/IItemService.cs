using BookShopServer.DTOs.ItemDTOs;
using BookShopServer.DTOs.OrderDTOs;

namespace BookShopServer.Services.ItemServices;

/// <summary>
/// Defines the contract for services that manage item-related operations.
/// </summary>
public interface IItemService
{
    /// <summary>
    /// Retrieves an item by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the item to retrieve.</param>
    /// <returns>The <see cref="ItemDto"/> if found.</returns>
    Task<ItemDto> GetItemByIdAsync(int id);

    /// <summary>
    /// Retrieves a collection of all items as simplified DTOs.
    /// </summary>
    /// <returns>A collection of <see cref="SimpleItemDto"/> representing all items.</returns>
    Task<IEnumerable<SimpleItemDto>> GetAllItemsAsync();

    /// <summary>
    /// Retrieves a collection of items belonging to a specific age category as simplified DTOs.
    /// </summary>
    /// <param name="ageCategoryId">The ID of the age category.</param>
    /// <returns>A collection of <see cref="SimpleItemDto"/> representing items in the specified age category.</returns>
    Task<IEnumerable<SimpleItemDto>> GetItemsByAgeCategoryIdAsync(int ageCategoryId);
    
    /// <summary>
    /// Retrieves a collection of items appropriate for a specific age as simplified DTOs.
    /// </summary>
    /// <param name="age">The age to filter items by.</param>
    /// <returns>
    /// A collection of <see cref="SimpleItemDto"/> representing items appropriate for the specified age.
    /// </returns>
    Task<IEnumerable<SimpleItemDto>> GetItemsAppropriateForAgeAsync(int age);

    /// <summary>
    /// Retrieves a collection of items published by a specific publisher as simplified DTOs.
    /// </summary>
    /// <param name="publisherId">The ID of the publisher.</param>
    /// <returns>A collection of <see cref="SimpleItemDto"/> representing items from the specified publisher.</returns>
    Task<IEnumerable<SimpleItemDto>> GetItemsByPublisherIdAsync(int publisherId);

    /// <summary>
    /// Deletes an item from the system by its ID.
    /// </summary>
    /// <param name="id">The ID of the item to delete.</param>
    Task DeleteItemAsync(int id);

    /// <summary>
    /// Retrieves a collection of simple order DTOs associated with a specific item.
    /// </summary>
    /// <param name="itemId">The ID of the item to retrieve orders for.</param>
    /// <returns>A collection of <see cref="SimpleOrderDto"/> associated with the item.</returns>
    Task<IEnumerable<SimpleOrderDto>> GetOrdersByItemIdAsync(int itemId);
}