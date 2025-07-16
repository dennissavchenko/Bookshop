using BookShopServer.DTOs;
using BookShopServer.DTOs.ItemDTOs;
using BookShopServer.Exceptions;
using BookShopServer.Repositories;
using BookShopServer.Repositories.ItemRepositories;
using BookShopServer.Repositories.ItemRepositories.ItemType;

namespace BookShopServer.Services.ItemServices.ItemType;

/// <summary>
/// Provides services for managing book-specific operations within the bookshop system.
/// </summary>
public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IItemRepository _itemRepository;
    
    /// <summary>
    /// Initializes a new instance of the <c>BookService</c> class.
    /// </summary>
    /// <param name="bookRepository">The repository for book-specific data.</param>
    /// <param name="authorRepository">The repository for author data.</param>
    /// <param name="genreRepository">The repository for genre data.</param>
    /// <param name="itemRepository">The general item repository.</param>
    public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository, 
                       IGenreRepository genreRepository, IItemRepository itemRepository)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _genreRepository = genreRepository;
        _itemRepository = itemRepository;
    }
    
    /// <summary>
    /// Retrieves a collection of all books from the system, mapped to simplified DTOs.
    /// This method filters all items to include only those that are books.
    /// </summary>
    /// <returns>A collection of <see cref="SimpleItemDto"/> representing all books.</returns>
    public async Task<IEnumerable<SimpleItemDto>> GetAllBooksAsync()
    {
        var allItems = await _itemRepository.GetAllItemsAsync();
        // Filter items to only include books and map to SimpleItemDto
        return SimpleItemDto.MapItems(allItems.Where(x => x.Book != null));
    }
    
    /// <summary>
    /// Retrieves a collection of books written by a specific author, mapped to simplified DTOs.
    /// </summary>
    /// <param name="authorId">The ID of the author.</param>
    /// <returns>A collection of <see cref="SimpleItemDto"/> representing books by the specified author.</returns>
    /// <exception cref="NotFoundException">Thrown if the specified author does not exist.</exception>
    public async Task<IEnumerable<SimpleItemDto>> GetAllBooksByAuthorIdAsync(int authorId)
    {
        // Check if the author exists
        if (!await _authorRepository.AuthorExistsAsync(authorId))
            throw new NotFoundException("Author with the given ID does not exist.");
        
        var allItems = await _itemRepository.GetAllItemsAsync();
        // Filter items to include books associated with the given author and map to SimpleItemDto
        return SimpleItemDto.MapItems(allItems.Where(x => x.Book != null && x.Book.Authors.Any(a => a.Id == authorId)));
    }
    
    /// <summary>
    /// Retrieves a collection of books belonging to a specific genre, mapped to simplified DTOs.
    /// </summary>
    /// <param name="genreId">The ID of the genre.</param>
    /// <returns>A collection of <see cref="SimpleItemDto"/> representing books in the specified genre.</returns>
    /// <exception cref="NotFoundException">Thrown if the specified genre does not exist.</exception>
    public async Task<IEnumerable<SimpleItemDto>> GetAllBooksByGenreIdAsync(int genreId)
    {
        // Check if the genre exists
        if (!await _genreRepository.GenreExistsAsync(genreId))
            throw new NotFoundException("Genre with the given ID does not exist.");
        
        var allItems = await _itemRepository.GetAllItemsAsync();
        // Filter items to include books associated with the given genre and map to SimpleItemDto
        return SimpleItemDto.MapItems(allItems.Where(x => x.Book != null && x.Book.Genres.Any(g => g.Id == genreId)));
    }
    
    /// <summary>
    /// Associates an author with a book.
    /// </summary>
    /// <param name="bookId">The ID of the book.</param>
    /// <param name="authorId">The ID of the author to associate.</param>
    /// <exception cref="NotFoundException">Thrown if the book or author does not exist.</exception>
    public async Task AddAuthorToBookAsync(int bookId, int authorId)
    {
        var book = await _bookRepository.GetBookByIdAsync(bookId);
        // Check if the book exists
        if (book == null)
            throw new NotFoundException("Book with the given ID does not exist.");
        
        // Check if the author exists
        var author = await _authorRepository.GetAuthorByIdAsync(authorId);
        if (author == null)
            throw new NotFoundException("Author with the given ID does not exist.");
        
        // Add author to book in the repository
        await _bookRepository.AddAuthorToBookAsync(book, author);
    }
    
    /// <summary>
    /// Disassociates an author from a book.
    /// </summary>
    /// <param name="bookId">The ID of the book.</param>
    /// <param name="authorId">The ID of the author to disassociate.</param>
    /// <exception cref="NotFoundException">Thrown if the book or author does not exist.</exception>
    /// <exception cref="ConflictException">Thrown if attempting to remove the last author from a book.</exception>
    public async Task RemoveAuthorFromBookAsync(int bookId, int authorId)
    {
        // Fetch the book from the repository
        var book = await _bookRepository.GetBookByIdAsync(bookId);
        
        // Check if the book exists
        if (book == null)
            throw new NotFoundException("Book with the given ID does not exist.");
        
        // Prevent removing the last author from a book
        if (book.Authors.Count == 1)
            throw new ConflictException("Cannot remove the last author from a book.");

        var author = book.Authors.FirstOrDefault(a => a.Id == authorId);
        
        // Check if the author exists
        if (author == null)
            throw new NotFoundException("Author with the given ID does not exist in this book or in database.");
        
        // Remove author from book in the repository
        await _bookRepository.RemoveAuthorFromBookAsync(book, author);
    }
    
    /// <summary>
    /// Associates a genre with a book.
    /// </summary>
    /// <param name="bookId">The ID of the book.</param>
    /// <param name="genreId">The ID of the genre to associate.</param>
    /// <exception cref="NotFoundException">Thrown if the book or genre does not exist.</exception>
    public async Task AddGenreToBookAsync(int bookId, int genreId)
    {
        var book = await _bookRepository.GetBookByIdAsync(bookId);
        // Check if the book exists
        if (book == null)
            throw new NotFoundException("Book with the given ID does not exist.");
        
        var genre = await _genreRepository.GetGenreByIdAsync(genreId);
        // Check if the genre exists
        if (genre == null)
            throw new NotFoundException("Genre with the given ID does not exist.");
        
        // Add genre to book in the repository
        await _bookRepository.AddGenreToBookAsync(book, genre);
    }
    
    /// <summary>
    /// Disassociates a genre from a book.
    /// </summary>
    /// <param name="bookId">The ID of the book.</param>
    /// <param name="genreId">The ID of the genre to disassociate.</param>
    /// <exception cref="NotFoundException">Thrown if the book or genre does not exist.</exception>
    /// <exception cref="ConflictException">Thrown if attempting to remove the last genre from a book.</exception>
    public async Task RemoveGenreFromBookAsync(int bookId, int genreId)
    {
        var book = await _bookRepository.GetBookByIdAsync(bookId);
        // Check if the book exists
        if (book == null)
            throw new NotFoundException("Book with the given ID does not exist.");
        
        // Prevent removing the last genre from a book
        if (book.Genres.Count == 1)
            throw new ConflictException("Cannot remove the last genre from a book.");

        var genre = book.Genres.FirstOrDefault(g => g.Id == genreId);
        
        // Check if the genre exists
        if (genre == null)
            throw new NotFoundException("Genre with the given ID does not exist in this book or in database.");
        
        // Remove genre from book in the repository
        await _bookRepository.RemoveGenreFromBookAsync(book, genre);
    }
    
    /// <summary>
    /// Retrieves a collection of authors associated with a specific book, mapped to DTOs.
    /// </summary>
    /// <param name="bookId">The ID of the book.</param>
    /// <returns>A collection of <see cref="AuthorDto"/> representing the authors of the book.</returns>
    /// <exception cref="NotFoundException">Thrown if the book does not exist.</exception>
    public async Task<IEnumerable<AuthorDto>> GetAuthorsByBookIdAsync(int bookId)
    {
        // Check if the book exists
        if (!await _bookRepository.BookExistsAsync(bookId))
            throw new NotFoundException("Book with the given ID does not exist.");
        
        // Retrieve authors for the book and map to AuthorDto
        var authors = await _bookRepository.GetAuthorsByBookIdAsync(bookId);
        return authors.Select(a => new AuthorDto
        {
            Id = a.Id,
            Name = a.Name,
            Surname = a.Surname,
            DOB = a.DOB,
            Pseudonym = a.Pseudonym
        });
    }
    
    /// <summary>
    /// Retrieves a collection of genres associated with a specific book, mapped to DTOs.
    /// </summary>
    /// <param name="bookId">The ID of the book.</param>
    /// <returns>A collection of <see cref="GenreDto"/> representing the genres of the book.</returns>
    /// <exception cref="NotFoundException">Thrown if the book does not exist.</exception>
    public async Task<IEnumerable<GenreDto>> GetGenresByBookIdAsync(int bookId)
    {
        // Check if the book exists
        if (!await _bookRepository.BookExistsAsync(bookId))
            throw new NotFoundException("Book with the given ID does not exist.");
        
        // Retrieve genres for the book and map to GenreDto
        var genres = await _bookRepository.GetGenresByBookIdAsync(bookId);
        return genres.Select(g => new GenreDto
        {
            Id = g.Id,
            Name = g.Name,
            Description = g.Description
        });
    }
}