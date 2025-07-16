using BookShopServer.DTOs;

namespace BookShopServer.Services;

/// <summary>
/// Defines the contract for services that manage author-related operations.
/// </summary>
public interface IAuthorService
{
    /// <summary>
    /// Retrieves an author by their unique identifier.
    /// </summary>
    /// <param name="id">The ID of the author to retrieve.</param>
    /// <returns>A <see cref="AuthorDto"/> representing the author.</returns>
    Task<AuthorDto> GetAuthorByIdAsync(int id);

    /// <summary>
    /// Retrieves a collection of all authors.
    /// </summary>
    /// <returns>A collection of <see cref="AuthorDto"/> representing all authors.</returns>
    Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync();

    /// <summary>
    /// Adds a new author to the system.
    /// </summary>
    /// <param name="author">The <see cref="AuthorDto"/> containing the details of the author to add.</param>
    Task AddAuthorAsync(AuthorDto author);

    /// <summary>
    /// Updates an existing author's details.
    /// </summary>
    /// <param name="author">The <see cref="AuthorDto"/> containing the updated details of the author.</param>
    Task UpdateAuthorAsync(AuthorDto author);

    /// <summary>
    /// Deletes an author from the system by their unique identifier.
    /// </summary>
    /// <param name="id">The ID of the author to delete.</param>
    Task DeleteAuthorAsync(int id);
    
    /// <summary>
    /// Retrieves a collection of filtered authors.
    /// </summary>
    /// <returns>A collection of <see cref="BriefEntityDto"/> filtered authors.</returns>
    Task<IEnumerable<BriefEntityDto>> GetFilteredAuthorsAsync(string searchTerm);
}