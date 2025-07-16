using BookShopServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookShopServer.Repositories;

/// <summary>
/// Provides data access methods for managing author entities in the bookshop system.
/// </summary>
public class AuthorRepository : IAuthorRepository
{
    private readonly Context _context;
    
    /// <summary>
    /// Initializes a new instance of the <c>AuthorRepository</c> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public AuthorRepository(Context context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Retrieves an author by their unique identifier.
    /// </summary>
    /// <param name="id">The ID of the author to retrieve.</param>
    /// <returns>The author entity if found.</returns>
    public async Task<Author?> GetAuthorByIdAsync(int id)
    {
        return await _context.Authors.FirstOrDefaultAsync(x => x.Id == id);
    }
    
    /// <summary>
    /// Retrieves all author entities.
    /// </summary>
    /// <returns>A collection of all authors.</returns>
    public async Task<IEnumerable<Author>> GetAllAuthorsAsync()
    {
        return await _context.Authors.ToListAsync();
    }
    
    /// <summary>
    /// Checks if an author with the specified ID exists.
    /// </summary>
    /// <param name="id">The ID of the author to check.</param>
    /// <returns><c>true</c> if the author exists; otherwise, <c>false</c>.</returns>
    public async Task<bool> AuthorExistsAsync(int id)
    {
        return await _context.Authors.AnyAsync(x => x.Id == id);
    }
    
    /// <summary>
    /// Adds a new author to the database.
    /// </summary>
    /// <param name="author">The author entity to add.</param>
    public async Task AddAuthorAsync(Author author)
    {
        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();
    }
    
    /// <summary>
    /// Updates an existing author in the database.
    /// </summary>
    /// <param name="author">The author entity with updated information.</param>
    public async Task UpdateAuthorAsync(Author author)
    {
        _context.Authors.Update(author);
        await _context.SaveChangesAsync();
    }
    
    /// <summary>
    /// Deletes an author from the database by their ID.
    /// </summary>
    /// <param name="id">The ID of the author to delete.</param>
    public async Task DeleteAuthorAsync(int id)
    {
        var author = await _context.Authors.FirstAsync(x => x.Id == id);
        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();
    }
    
    /// <summary>
    /// Checks if the specified author is the sole author of any book in the system.
    /// This method eagerly loads related book and author data to perform the check.
    /// </summary>
    /// <param name="id">The ID of the author to check.</param>
    /// <returns><c>true</c> if the author is the only author of at least one book; otherwise, <c>false</c>.</returns>
    public async Task<bool> IsOnlyAuthorOfAnyBookAsync(int id)
    {
        var author = await _context.Authors.Include(x => x.Books).ThenInclude(x => x.Authors).FirstAsync(x => x.Id == id);
        return author.Books.Any(x => x.Authors.Count == 1);
    }

    /// <summary>
    /// Retrieves a collection of existing authors based on a list of authors IDs.
    /// <param name="authorsIds">Collection of authors IDs</param>
    /// <returns>Returns list of existing authors</returns>
    /// </summary>
    public async Task<ICollection<Author>> GetExistingAuthorsAsync(IEnumerable<int> authorsIds)
    {
        return await _context.Authors.Where(x => authorsIds.Contains(x.Id)).ToListAsync();
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<Author>> GetFilteredAuthorsAsync(string searchTerm)
    {
        searchTerm = searchTerm.Trim().ToLower();
        var startsWith = await _context.Authors
            .Where(x => (x.Pseudonym ?? string.Concat(x.Name, x.Surname)).ToLower().StartsWith(searchTerm))
            .OrderBy(x => x.Name)
            .ToListAsync();
        var contains = await _context.Authors
            .Where(x => (x.Pseudonym ?? string.Concat(x.Name, x.Surname)).ToLower().Contains(searchTerm) && !(x.Pseudonym ?? string.Concat(x.Name, x.Surname)).ToLower().StartsWith(searchTerm))
            .OrderBy(x => x.Name)
            .ToListAsync();
        return startsWith.Concat(contains);
    }
}