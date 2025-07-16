using BookShopServer.DTOs;

namespace BookShopServer.Services;

/// <summary>
/// Defines the contract for services that manage genre-related operations.
/// Genres are used to categorize books or other items within the shop.
/// </summary>
public interface IGenreService
{
    /// <summary>
    /// Retrieves a genre by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the genre to retrieve.</param>
    /// <returns>A <see cref="GenreDto"/> representing the genre.</returns>
    Task<GenreDto> GetGenreByIdAsync(int id);

    /// <summary>
    /// Retrieves a collection of all genres.
    /// </summary>
    /// <returns>A collection of <see cref="GenreDto"/> representing all genres.</returns>
    Task<IEnumerable<GenreDto>> GetAllGenresAsync();

    /// <summary>
    /// Adds a new genre to the system.
    /// </summary>
    /// <param name="genre">The <see cref="GenreDto"/> containing the details of the genre to add.</param>
    Task AddGenreAsync(GenreDto genre);

    /// <summary>
    /// Updates an existing genre's details.
    /// </summary>
    /// <param name="genre">The <see cref="GenreDto"/> containing the updated details of the genre.</param>
    Task UpdateGenreAsync(GenreDto genre);

    /// <summary>
    /// Deletes a genre from the system by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the genre to delete.</param>
    Task DeleteGenreAsync(int id);

    /// <summary>
    /// Retrieves a collection of filtered genres.
    /// </summary>
    /// <returns>A collection of <see cref="BriefEntityDto"/> filtered genres.</returns>
    Task<IEnumerable<BriefEntityDto>> GetFilteredGenresAsync(string searchTerm);
}