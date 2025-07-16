using BookShopServer.Entities;

namespace BookShopServer.Repositories;

/// <summary>
/// Defines the contract for data access operations related to genre entities.
/// </summary>
public interface IGenreRepository
{
    /// <summary>
    /// Retrieves a genre by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the genre to retrieve.</param>
    /// <returns>The genre entity if found, otherwise <c>null</c>.</returns>
    Task<Genre?> GetGenreByIdAsync(int id);

    /// <summary>
    /// Retrieves all genre entities.
    /// </summary>
    /// <returns>A collection of all genres.</returns>
    Task<IEnumerable<Genre>> GetAllGenresAsync();

    /// <summary>
    /// Checks if a genre with the specified ID exists.
    /// </summary>
    /// <param name="id">The ID of the genre to check.</param>
    /// <returns><c>true</c> if the genre exists; otherwise, <c>false</c>.</returns>
    Task<bool> GenreExistsAsync(int id);

    /// <summary>
    /// Adds a new genre to the database.
    /// </summary>
    /// <param name="genre">The genre entity to add.</param>
    Task AddGenreAsync(Genre genre);

    /// <summary>
    /// Updates an existing genre in the database.
    /// </summary>
    /// <param name="genre">The genre entity with updated information.</param>
    Task UpdateGenreAsync(Genre genre);

    /// <summary>
    /// Deletes a genre from the database by its ID.
    /// </summary>
    /// <param name="id">The ID of the genre to delete.</param>
    Task DeleteGenreAsync(int id);

    /// <summary>
    /// Checks if the specified genre is the only genre of any book in the system.
    /// </summary>
    /// <param name="id">The ID of the genre to check.</param>
    /// <returns><c>true</c> if the genre is the only genre of at least one book; otherwise, <c>false</c>.</returns>
    Task<bool> IsOnlyGenreOfAnyBookAsync(int id);
    /// <summary>
    /// Retrieves a collection of existing genres based on a list of genre IDs.
    /// <param name="genresIds">Collection of genres IDs</param>
    /// <returns>Returns collection of existing genres</returns>
    /// </summary>
    Task<ICollection<Genre>> GetExistingGenresAsync(IEnumerable<int> genresIds);
    
    /// <summary>
    /// Returns a collection of filtered genres.
    /// </summary>
    /// <returns>A collection of <see cref="Genre"/> representing filtered genres.</returns>
    Task<IEnumerable<Genre>> GetFilteredGenresAsync(string searchTerm);
}