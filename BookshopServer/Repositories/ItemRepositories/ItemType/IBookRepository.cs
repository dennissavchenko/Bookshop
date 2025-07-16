using BookShopServer.Entities;

namespace BookShopServer.Repositories.ItemRepositories.ItemType;

/// <summary>
/// Defines the contract for data access operations related to <see cref="Book"/> entities.
/// </summary>
public interface IBookRepository
{
    /// <summary>
    /// Retrieves a book by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the book.</param>
    /// <returns>The book if found; otherwise, null.</returns>
    Task<Book?> GetBookByIdAsync(int id);   

    /// <summary>
    /// Adds a new book to the repository asynchronously.
    /// </summary>
    /// <param name="book">The book entity to add.</param>
    Task AddBookAsync(Book book);

    /// <summary>
    /// Updates an existing book in the repository asynchronously.
    /// </summary>
    /// <param name="book">The book entity with updated information.</param>
    Task UpdateBookAsync(Book book);

    /// <summary>
    /// Adds an author to a specific book asynchronously.
    /// </summary>
    /// <param name="book">The book to which the author will be added.</param>
    /// <param name="author">The author to add to the book.</param>
    Task AddAuthorToBookAsync(Book book, Author author);

    /// <summary>
    /// Removes an author from a specific book asynchronously.
    /// </summary>
    /// <param name="book">The book from which the author will be removed.</param>
    /// <param name="author">The author to remove from the book.</param>
    Task RemoveAuthorFromBookAsync(Book book, Author author);

    /// <summary>
    /// Adds a genre to a specific book asynchronously.
    /// </summary>
    /// <param name="book">The book to which the genre will be added.</param>
    /// <param name="genre">The genre to add to the book.</param>
    Task AddGenreToBookAsync(Book book, Genre genre);

    /// <summary>
    /// Removes a genre from a specific book asynchronously.
    /// </summary>
    /// <param name="book">The book from which the genre will be removed.</param>
    /// <param name="genre">The genre to remove from the book.</param>
    Task RemoveGenreFromBookAsync(Book book, Genre genre);

    /// <summary>
    /// Checks if a book with the specified ID exists asynchronously.
    /// </summary>
    /// <param name="bookId">The unique identifier of the book to check.</param>
    /// <returns><c>true</c> if the book exists; otherwise, <c>false</c>.</returns>
    Task<bool> BookExistsAsync(int bookId);

    /// <summary>
    /// Retrieves all authors associated with a specific book asynchronously.
    /// </summary>
    /// <param name="bookId">The unique identifier of the book.</param>
    /// <returns>An enumerable collection of author entities.</returns>
    Task<IEnumerable<Author>> GetAuthorsByBookIdAsync(int bookId);

    /// <summary>
    /// Retrieves all genres associated with a specific book asynchronously.
    /// </summary>
    /// <param name="bookId">The unique identifier of the book.</param>
    /// <returns>An enumerable collection of genre entities.</returns>
    Task<IEnumerable<Genre>> GetGenresByBookIdAsync(int bookId);

    /// <summary>
    /// Adds a collection of genres to a specific book asynchronously.
    /// </summary>
    /// <param name="bookId">The unique identifier of the book.</param>
    /// <param name="genres">An enumerable collection of genre entities to add to the book.</param>
    Task AddGenresToBookAsync(int bookId, IEnumerable<Genre> genres);

    /// <summary>
    /// Adds a collection of authors to a specific book asynchronously.
    /// </summary>
    /// <param name="bookId">The unique identifier of the book.</param>
    /// <param name="authors">An enumerable collection of author entities to add to the book.</param>
    Task AddAuthorsToBookAsync(int bookId, IEnumerable<Author> authors);
}