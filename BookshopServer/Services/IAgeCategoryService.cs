using BookShopServer.DTOs;

namespace BookShopServer.Services;

/// <summary>
/// Defines the contract for services that manage age category operations.
/// Age categories are typically used to classify books or items based on suitable age ranges.
/// </summary>
public interface IAgeCategoryService
{
    /// <summary>
    /// Retrieves an age category by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the age category to retrieve.</param>
    /// <returns>A <see cref="AgeCategoryDto"/> representing the age category.</returns>
    Task<AgeCategoryDto> GetAgeCategoryByIdAsync(int id);

    /// <summary>
    /// Retrieves a collection of all age categories.
    /// </summary>
    /// <returns>A collection of <see cref="AgeCategoryDto"/> representing all age categories.</returns>
    Task<IEnumerable<AgeCategoryDto>> GetAllAgeCategoriesAsync();

    /// <summary>
    /// Adds a new age category to the system.
    /// </summary>
    /// <param name="ageCategory">The <see cref="AgeCategoryDto"/> containing the details of the age category to add.</param>
    Task AddAgeCategoryAsync(AgeCategoryDto ageCategory);

    /// <summary>
    /// Updates an existing age category's details.
    /// </summary>
    /// <param name="ageCategory">The <see cref="AgeCategoryDto"/> containing the updated details of the age category.</param>
    Task UpdateAgeCategoryAsync(AgeCategoryDto ageCategory);

    /// <summary>
    /// Deletes an age category from the system by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the age category to delete.</param>
    Task DeleteAgeCategoryAsync(int id);
}