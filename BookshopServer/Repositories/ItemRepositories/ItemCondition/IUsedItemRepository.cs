using BookShopServer.Entities;

namespace BookShopServer.Repositories.ItemRepositories.ItemCondition;

/// <summary>
/// Defines the contract for accessing and managing used items in the bookshop system.
/// </summary>
public interface IUsedItemRepository
{
    /// <summary>
    /// Retrieves a used item by its ID, including related data such as publisher, reviews, and associated content.
    /// </summary>
    /// <param name="id">The ID of the used item to retrieve.</param>
    /// <returns>The matching used item if found; otherwise, null.</returns>
    Task<UsedItem?> GetUsedItemByIdAsync(int id);

    /// <summary>
    /// Adds a new used item to the database.
    /// </summary>
    /// <param name="usedItem">The used item to add.</param>
    /// <returns>The ID of the newly added used item.</returns>
    Task<int> AddUsedItemAsync(UsedItem usedItem);

    /// <summary>
    /// Updates an existing used item in the database.
    /// </summary>
    /// <param name="usedItem">The used item with updated values.</param>
    Task UpdateUsedItemAsync(UsedItem usedItem);

    /// <summary>
    /// Checks whether a used item with the specified ID exists.
    /// </summary>
    /// <param name="id">The ID of the item to check.</param>
    /// <returns><c>true</c> if the item exists; otherwise, <c>false</c>.</returns>
    Task<bool> UsedItemExistsAsync(int id);

    /// <summary>
    /// Retrieves all used items from the database, including their related data.
    /// </summary>
    /// <returns>A collection of all used items.</returns>
    Task<IEnumerable<UsedItem>> GetAllUsedItemsAsync();
}
