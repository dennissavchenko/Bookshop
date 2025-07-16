using BookShopServer.Exceptions;
using BookShopServer.Services.ItemServices.ItemType;
using Microsoft.AspNetCore.Mvc;

namespace BookShopServer.Controllers;

/// <summary>
/// Controller for managing book items.
/// Provides endpoints for retrieving books and managing their authors and genres.
/// </summary>
[ApiController]
[Route("api/items/books")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    /// <summary>
    /// Initializes a new instance of the <c>BooksController</c> class.
    /// </summary>
    /// <param name="bookService">Service for book-related operations.</param>
    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    /// <summary>
    /// Retrieves all books.
    /// </summary>
    /// <returns>
    /// A 200 OK response containing the list of books.
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> GetAllBooks()
    {
        var books = await _bookService.GetAllBooksAsync();
        return Ok(books);
    }
    
    /// <summary>
    /// Retrieves all authors of a specific book.
    /// </summary>
    /// <param name="id">The ID of the book.</param>
    /// <returns>
    /// A 200 OK response with authors of the book;
    /// or 404 Not Found if the book does not exist.
    /// </returns>
    [HttpGet("{id:int}/authors")]
    public async Task<IActionResult> GetAuthorsByBookIdAsync(int id)
    {
        try
        {
            var authors = await _bookService.GetAuthorsByBookIdAsync(id);
            return Ok(authors);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    /// <summary>
    /// Retrieves all genres of a specific book.
    /// </summary>
    /// <param name="id">The ID of the book.</param>
    /// <returns>
    /// A 200 OK response with genres of the book;
    /// or 404 Not Found if the book does not exist.
    /// </returns>
    [HttpGet("{id:int}/genres")]
    public async Task<IActionResult> GetGenresByBookIdAsync(int id)
    {
        try
        {
            var genres = await _bookService.GetGenresByBookIdAsync(id);
            return Ok(genres);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    /// Retrieves all books by a specific author.
    /// </summary>
    /// <param name="authorId">The ID of the author.</param>
    /// <returns>
    /// A 200 OK response with books by the author;
    /// or 404 Not Found if the author does not exist.
    /// </returns>
    [HttpGet("author/{authorId:int}")]
    public async Task<IActionResult> GetItemsByAuthorIdAsync(int authorId)
    {
        try
        {
            var items = await _bookService.GetAllBooksByAuthorIdAsync(authorId);
            return Ok(items);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    /// Retrieves all books in a specific genre.
    /// </summary>
    /// <param name="genreId">The ID of the genre.</param>
    /// <returns>
    /// A 200 OK response with books in the genre;
    /// or 404 Not Found if the genre does not exist.
    /// </returns>
    [HttpGet("genre/{genreId:int}")]
    public async Task<IActionResult> GetItemsByGenreIdAsync(int genreId)
    {
        try
        {
            var items = await _bookService.GetAllBooksByGenreIdAsync(genreId);
            return Ok(items);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    /// Adds an author to a book.
    /// </summary>
    /// <param name="bookId">The ID of the book.</param>
    /// <param name="authorId">The ID of the author to add.</param>
    /// <returns>
    /// A 204 No Content response on success;
    /// or 404 Not Found if the book or author does not exist.
    /// </returns>
    [HttpPost("{bookId:int}/authors/{authorId:int}")]
    public async Task<IActionResult> AddAuthorToBookAsync(int bookId, int authorId)
    {
        try
        {
            await _bookService.AddAuthorToBookAsync(bookId, authorId);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    /// Removes an author from a book.
    /// </summary>
    /// <param name="bookId">The ID of the book.</param>
    /// <param name="authorId">The ID of the author to remove.</param>
    /// <returns>
    /// A 204 No Content response on success;
    /// 404 Not Found if the book or author is not found;
    /// or 409 Conflict if the author cannot be removed due to constraints (trying to remove last author of a book).
    /// </returns>
    [HttpDelete("{bookId:int}/authors/{authorId:int}")]
    public async Task<IActionResult> RemoveAuthorFromBookAsync(int bookId, int authorId)
    {
        try
        {
            await _bookService.RemoveAuthorFromBookAsync(bookId, authorId);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ConflictException e)
        {
            return Conflict(e.Message);
        }
    }

    /// <summary>
    /// Adds a genre to a book.
    /// </summary>
    /// <param name="bookId">The ID of the book.</param>
    /// <param name="genreId">The ID of the genre to add.</param>
    /// <returns>
    /// A 204 No Content response on success;
    /// or 404 Not Found if the book or genre does not exist.
    /// </returns>
    [HttpPost("{bookId:int}/genres/{genreId:int}")]
    public async Task<IActionResult> AddGenreToBookAsync(int bookId, int genreId)
    {
        try
        {
            await _bookService.AddGenreToBookAsync(bookId, genreId);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    /// Removes a genre from a book.
    /// </summary>
    /// <param name="bookId">The ID of the book.</param>
    /// <param name="genreId">The ID of the genre to remove.</param>
    /// <returns>
    /// A 204 No Content response on success;
    /// 404 Not Found if the book or genre is not found,
    /// or 409 Conflict if the genre cannot be removed due to constraints (trying to remove last genre of a book).
    /// </returns>
    [HttpDelete("{bookId:int}/genres/{genreId:int}")]
    public async Task<IActionResult> RemoveGenreFromBookAsync(int bookId, int genreId)
    {
        try
        {
            await _bookService.RemoveGenreFromBookAsync(bookId, genreId);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ConflictException e)
        {
            return Conflict(e.Message);
        }
    }
    
}
