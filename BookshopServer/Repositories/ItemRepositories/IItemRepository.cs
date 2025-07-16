using BookShopServer.Entities;

namespace BookShopServer.Repositories.ItemRepositories;

/// <summary>
/// Defines the contract for accessing and managing item entities in the bookshop system,
/// including stock control and filtering by publisher or age category.
/// </summary>
public interface IItemRepository
{
    /// <summary>
    /// Retrieves all items from the database.
    /// </summary>
    /// <returns>A collection of all items.</returns>
    Task<IEnumerable<Item>> GetAllItemsAsync();

    /// <summary>
    /// Retrieves items filtered by a specific age category.
    /// </summary>
    /// <param name="ageCategoryId">The ID of the age category.</param>
    /// <returns>A collection of items in the specified age category.</returns>
    Task<IEnumerable<Item>> GetItemsByAgeCategoryAsync(int ageCategoryId);
    
    /// <summary>
    /// Retrieves items filtered by a specific age category.
    /// </summary>
    /// <param name="age">The age to filter items by.</param>
    /// <returns>A collection of items appropriate for the age.</returns>
    Task<IEnumerable<Item>> GetItemsAppropriateForAgeAsync(int age);

    /// <summary>
    /// Retrieves items filtered by a specific publisher.
    /// </summary>
    /// <param name="publisherId">The ID of the publisher.</param>
    /// <returns>A collection of items published by the specified publisher.</returns>
    Task<IEnumerable<Item>> GetItemsByPublisherAsync(int publisherId);

    /// <summary>
    /// Deletes an item from the database by its ID.
    /// </summary>
    /// <param name="id">The ID of the item to delete.</param>
    Task DeleteItemAsync(int id);

    /// <summary>
    /// Checks whether an item with the specified ID exists.
    /// </summary>
    /// <param name="id">The ID of the item to check.</param>
    /// <returns><c>true</c> if the item exists; otherwise, <c>false</c>.</returns>
    Task<bool> ItemExistsAsync(int id);

    /// <summary>
    /// Retrieves the quantity of the specified item currently in stock.
    /// </summary>
    /// <param name="id">The ID of the item.</param>
    /// <returns>The amount in stock.</returns>
    Task<int> GetItemAmountInStockAsync(int id);

    /// <summary>
    /// Increases the amount in stock for a specified item.
    /// </summary>
    /// <param name="id">The ID of the item.</param>
    /// <param name="amount">The amount to increase.</param>
    Task IncreaseItemAmountInStockAsync(int id, int amount);

    /// <summary>
    /// Decreases the amount in stock for a specified item.
    /// </summary>
    /// <param name="id">The ID of the item.</param>
    /// <param name="amount">The amount to decrease.</param>
    Task DecreaseItemAmountInStockAsync(int id, int amount);
}
