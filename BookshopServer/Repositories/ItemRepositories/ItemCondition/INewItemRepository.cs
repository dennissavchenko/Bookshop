using BookShopServer.Entities;

namespace BookShopServer.Repositories.ItemRepositories.ItemCondition;

/// <summary>
/// Defines the contract for accessing and managing new items in the bookshop system.
/// </summary>
public interface INewItemRepository
{
    /// <summary>
    /// Retrieves a new item by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the new item.</param>
    /// <returns>The matching <c>NewItem</c> if found; otherwise, <c>null</c>.</returns>
    Task<NewItem?> GetNewItemByIdAsync(int id);

    /// <summary>
    /// Adds a new item to the database.
    /// </summary>
    /// <param name="newItem">The new item to be added.</param>
    /// <returns>The ID of the newly created item.</returns>
    Task<int> AddNewItemAsync(NewItem newItem);

    /// <summary>
    /// Updates an existing new item in the database.
    /// </summary>
    /// <param name="newItem">The new item with updated information.</param>
    Task UpdateNewItemAsync(NewItem newItem);

    /// <summary>
    /// Checks whether a new item with the specified ID exists.
    /// </summary>
    /// <param name="id">The ID of the new item to check.</param>
    /// <returns><c>true</c> if the item exists; otherwise, <c>false</c>.</returns>
    Task<bool> NewItemExistsAsync(int id);

    /// <summary>
    /// Retrieves all new items from the database.
    /// </summary>
    /// <returns>A collection of all <c>NewItem</c> entities.</returns>
    Task<IEnumerable<NewItem>> GetAllNewItemsAsync();
}
