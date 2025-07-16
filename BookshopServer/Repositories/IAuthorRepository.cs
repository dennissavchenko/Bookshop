using BookShopServer.Entities;

namespace BookShopServer.Repositories;

/// <summary>
/// Defines the contract for data access operations related to author entities.
/// </summary>
public interface IAuthorRepository
{
    /// <summary>
    /// Retrieves an author by their unique identifier.
    /// </summary>
    /// <param name="id">The ID of the author to retrieve.</param>
    /// <returns>The author entity if found, otherwise <c>null</c>.</returns>
    Task<Author?> GetAuthorByIdAsync(int id);

    /// <summary>
    /// Retrieves all author entities.
    /// </summary>
    /// <returns>A collection of all authors.</returns>
    Task<IEnumerable<Author>> GetAllAuthorsAsync();

    /// <summary>
    /// Checks if an author with the specified ID exists.
    /// </summary>
    /// <param name="id">The ID of the author to check.</param>
    /// <returns><c>true</c> if the author exists; otherwise, <c>false</c>.</returns>
    Task<bool> AuthorExistsAsync(int id);

    /// <summary>
    /// Adds a new author to the database.
    /// </summary>
    /// <param name="author">The author entity to add.</param>
    Task AddAuthorAsync(Author author);

    /// <summary>
    /// Updates an existing author in the database.
    /// </summary>
    /// <param name="author">The author entity with updated information.</param>
    Task UpdateAuthorAsync(Author author);

    /// <summary>
    /// Deletes an author from the database by their ID.
    /// </summary>
    /// <param name="id">The ID of the author to delete.</param>
    Task DeleteAuthorAsync(int id);

    /// <summary>
    /// Checks if the specified author is the sole author of any book in the system.
    /// </summary>
    /// <param name="id">The ID of the author to check.</param>
    /// <returns><c>true</c> if the author is the only author of at least one book; otherwise, <c>false</c>.</returns>
    Task<bool> IsOnlyAuthorOfAnyBookAsync(int id);
    /// <summary>
    /// Retrieves a collection of existing authors based on a list of authors IDs.
    /// <param name="authorsIds">Collection of authors IDs</param>
    /// <returns>Returns list of existing authors</returns>
    /// </summary>
    Task<ICollection<Author>> GetExistingAuthorsAsync(IEnumerable<int> authorsIds);
    
    /// <summary>
    /// Returns a collection of filtered authors.
    /// </summary>
    /// <returns>A collection of <see cref="Author"/> representing filtered authors.</returns>
    Task<IEnumerable<Author>> GetFilteredAuthorsAsync(string searchTerm);
}