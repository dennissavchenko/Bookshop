using BookShopServer.Entities;

namespace BookShopServer.Repositories;

/// <summary>
/// Defines the contract for data access operations related to age category entities.
/// </summary>
public interface IAgeCategoryRepository
{
    /// <summary>
    /// Retrieves an age category by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the age category to retrieve.</param>
    /// <returns>The age category entity if found, otherwise <c>null</c>.</returns>
    Task<AgeCategory?> GetAgeCategoryByIdAsync(int id);

    /// <summary>
    /// Retrieves all age category entities.
    /// </summary>
    /// <returns>A collection of all age categories.</returns>
    Task<IEnumerable<AgeCategory>> GetAllAgeCategoriesAsync();

    /// <summary>
    /// Adds a new age category to the database.
    /// </summary>
    /// <param name="ageCategory">The age category entity to add.</param>
    Task AddAgeCategoryAsync(AgeCategory ageCategory);

    /// <summary>
    /// Updates an existing age category in the database.
    /// </summary>
    /// <param name="ageCategory">The age category entity with updated information.</param>
    Task UpdateAgeCategoryAsync(AgeCategory ageCategory);

    /// <summary>
    /// Deletes an age category from the database by its ID.
    /// </summary>
    /// <param name="id">The ID of the age category to delete.</param>
    Task DeleteAgeCategoryAsync(int id);

    /// <summary>
    /// Checks if an age category with the specified ID exists.
    /// </summary>
    /// <param name="id">The ID of the age category to check.</param>
    /// <returns><c>true</c> if the age category exists; otherwise, <c>false</c>.</returns>
    Task<bool> AgeCategoryExistsAsync(int id);

    /// <summary>
    /// Checks if the specified age category has any associated items.
    /// </summary>
    /// <param name="id">The ID of the age category to check.</param>
    /// <returns><c>true</c> if the age category has associated items; otherwise, <c>false</c>.</returns>
    Task<bool> AgeCategoryHasItemsAsync(int id);
}