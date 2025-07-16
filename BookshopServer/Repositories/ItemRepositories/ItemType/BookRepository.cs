using BookShopServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookShopServer.Repositories.ItemRepositories.ItemType;

/// <summary>
/// Provides data access methods for managing book entities and their relationships with authors and genres.
/// </summary>
public class BookRepository : IBookRepository
{
    private readonly Context _context;

    /// <summary>
    /// Initializes a new instance of the <c>BookRepository</c> class with the specified database context.
    /// </summary>
    /// <param name="context">The database context.</param>
    public BookRepository(Context context)
    {
        _context = context;
    }
    
    public async Task<Book?> GetBookByIdAsync(int id)
    {
        return await _context.Books
            .Include(b => b.Authors)
            .Include(b => b.Genres)
            .FirstOrDefaultAsync(x => x.ItemId == id);
    }
    
    /// <summary>
    /// Adds a new book to the database.
    /// </summary>
    /// <param name="book">The book to add.</param>
    public async Task AddBookAsync(Book book)
    {
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing book in the database.
    /// </summary>
    /// <param name="book">The book with updated values.</param>
    public async Task UpdateBookAsync(Book book)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Adds an author to a book if the author is not already associated with it.
    /// </summary>
    /// <param name="book">The book.</param>
    /// <param name="author">The author to add.</param>
    public async Task AddAuthorToBookAsync(Book book, Author author)
    {
        book.Authors.Add(author);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Removes an author from a book if the association exists.
    /// </summary>
    /// <param name="book">The book.</param>
    /// <param name="author">The author to remove.</param>
    public async Task RemoveAuthorFromBookAsync(Book book, Author author)
    {
        book.Authors.Remove(author);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Adds a genre to a book if it is not already associated with the book.
    /// </summary>
    /// <param name="book">The book.</param>
    /// <param name="genre">The genre to add.</param>
    public async Task AddGenreToBookAsync(Book book, Genre genre)
    {
        book.Genres.Add(genre);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Removes a genre from a book if the association exists.
    /// </summary>
    /// <param name="book">The book.</param>
    /// <param name="genre">The genre to remove.</param>
    public async Task RemoveGenreFromBookAsync(Book book, Genre genre)
    {
        book.Genres.Remove(genre);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Checks if a book with the specified ID exists in the database.
    /// </summary>
    /// <param name="bookId">The ID of the book to check.</param>
    /// <returns><c>true</c> if the book exists; otherwise, <c>false</c>.</returns>
    public async Task<bool> BookExistsAsync(int bookId)
    {
        return await _context.Books.AnyAsync(x => x.ItemId == bookId);
    }

    /// <summary>
    /// Retrieves the list of authors associated with a specific book.
    /// </summary>
    /// <param name="bookId">The ID of the book.</param>
    /// <returns>A collection of associated authors.</returns>
    public async Task<IEnumerable<Author>> GetAuthorsByBookIdAsync(int bookId)
    {
        var book = await _context.Books
            .Include(b => b.Authors)
            .FirstAsync(x => x.ItemId == bookId);

        return book.Authors;
    }

    /// <summary>
    /// Retrieves the list of genres associated with a specific book.
    /// </summary>
    /// <param name="bookId">The ID of the book.</param>
    /// <returns>A collection of associated genres.</returns>
    public async Task<IEnumerable<Genre>> GetGenresByBookIdAsync(int bookId)
    {
        var book = await _context.Books
            .Include(b => b.Genres)
            .FirstAsync(x => x.ItemId == bookId);

        return book.Genres;
    }

    /// <summary>
    /// Adds a collection of genres to a specific book asynchronously.
    /// </summary>
    /// <param name="bookId">The unique identifier of the book.</param>
    /// <param name="genres">An enumerable collection of genre entities to add to the book.</param>
    public async Task AddGenresToBookAsync(int bookId, IEnumerable<Genre> genres)
    {
        var book = await _context.Books
            .Include(b => b.Genres)
            .FirstAsync(x => x.ItemId == bookId);
        foreach (var genre in genres)
        {
            book.Genres.Add(genre);
        }
        await _context.SaveChangesAsync();
    }
    
    /// <summary>
    /// Adds a collection of authors to a specific book asynchronously.
    /// </summary>
    /// <param name="bookId">The unique identifier of the book.</param>
    /// <param name="authors">An enumerable collection of author entities to add to the book.</param>
    public async Task AddAuthorsToBookAsync(int bookId, IEnumerable<Author> authors)
    {
        var book = await _context.Books
            .Include(b => b.Authors)
            .FirstAsync(x => x.ItemId == bookId);
        foreach (var author in authors)
        {
            book.Authors.Add(author);
        }
        await _context.SaveChangesAsync();
    }
    
}