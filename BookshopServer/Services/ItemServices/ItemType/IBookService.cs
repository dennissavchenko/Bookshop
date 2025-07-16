using BookShopServer.DTOs;
using BookShopServer.DTOs.ItemDTOs;

namespace BookShopServer.Services.ItemServices.ItemType;

/// <summary>
/// Defines the contract for services that manage book-specific operations.
/// </summary>
public interface IBookService
{
    /// <summary>
    /// Retrieves a collection of all books as simplified DTOs.
    /// </summary>
    /// <returns>A collection of <see cref="SimpleItemDto"/> representing all books.</returns>
    Task<IEnumerable<SimpleItemDto>> GetAllBooksAsync();

    /// <summary>
    /// Retrieves a collection of books written by a specific author as simplified DTOs.
    /// </summary>
    /// <param name="authorId">The ID of the author.</param>
    /// <returns>A collection of <see cref="SimpleItemDto"/> representing books by the specified author.</returns>
    Task<IEnumerable<SimpleItemDto>> GetAllBooksByAuthorIdAsync(int authorId);

    /// <summary>
    /// Retrieves a collection of books belonging to a specific genre as simplified DTOs.
    /// </summary>
    /// <param name="genreId">The ID of the genre.</param>
    /// <returns>A collection of <see cref="SimpleItemDto"/> representing books in the specified genre.</returns>
    Task<IEnumerable<SimpleItemDto>> GetAllBooksByGenreIdAsync(int genreId);

    /// <summary>
    /// Associates an author with a book.
    /// </summary>
    /// <param name="bookId">The ID of the book.</param>
    /// <param name="authorId">The ID of the author to associate.</param>
    Task AddAuthorToBookAsync(int bookId, int authorId);

    /// <summary>
    /// Disassociates an author from a book.
    /// </summary>
    /// <param name="bookId">The ID of the book.</param>
    /// <param name="authorId">The ID of the author to disassociate.</param>
    Task RemoveAuthorFromBookAsync(int bookId, int authorId);

    /// <summary>
    /// Associates a genre with a book.
    /// </summary>
    /// <param name="bookId">The ID of the book.</param>
    /// <param name="genreId">The ID of the genre to associate.</param>
    Task AddGenreToBookAsync(int bookId, int genreId);

    /// <summary>
    /// Disassociates a genre from a book.
    /// </summary>
    /// <param name="bookId">The ID of the book.</param>
    /// <param name="genreId">The ID of the genre to disassociate.</param>
    Task RemoveGenreFromBookAsync(int bookId, int genreId);

    /// <summary>
    /// Retrieves a collection of authors associated with a specific book.
    /// </summary>
    /// <param name="bookId">The ID of the book.</param>
    /// <returns>A collection of <see cref="AuthorDto"/> representing the authors of the book.</returns>
    Task<IEnumerable<AuthorDto>> GetAuthorsByBookIdAsync(int bookId);

    /// <summary>
    /// Retrieves a collection of genres associated with a specific book.
    /// </summary>
    /// <param name="bookId">The ID of the book.</param>
    /// <returns>A collection of <see cref="GenreDto"/> representing the genres of the book.</returns>
    Task<IEnumerable<GenreDto>> GetGenresByBookIdAsync(int bookId);
}