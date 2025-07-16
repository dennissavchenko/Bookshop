using BookShopServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookShopServer.Repositories;

/// <summary>
/// Provides data access methods for managing genre entities in the bookshop system.
/// </summary>
public class GenreRepository : IGenreRepository
{
    private readonly Context _context;
    
    /// <summary>
    /// Initializes a new instance of the <c>GenreRepository</c> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public GenreRepository(Context context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Retrieves a genre by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the genre to retrieve.</param>
    /// <returns>The genre entity if found.</returns>
    public async Task<Genre?> GetGenreByIdAsync(int id)
    {
        return await _context.Genres.FirstOrDefaultAsync(x => x.Id == id);
    }
    
    /// <summary>
    /// Retrieves all genre entities.
    /// </summary>
    /// <returns>A collection of all genres.</returns>
    public async Task<IEnumerable<Genre>> GetAllGenresAsync()
    {
        return await _context.Genres.ToListAsync();
    }
    
    /// <summary>
    /// Checks if a genre with the specified ID exists.
    /// </summary>
    /// <param name="id">The ID of the genre to check.</param>
    /// <returns><c>true</c> if the genre exists; otherwise, <c>false</c>.</returns>
    public async Task<bool> GenreExistsAsync(int id)
    {
        return await _context.Genres.AnyAsync(x => x.Id == id);
    }
    
    /// <summary>
    /// Adds a new genre to the database.
    /// </summary>
    /// <param name="genre">The genre entity to add.</param>
    public async Task AddGenreAsync(Genre genre)
    {
        await _context.Genres.AddAsync(genre);
        await _context.SaveChangesAsync();
    }
    
    /// <summary>
    /// Updates an existing genre in the database.
    /// </summary>
    /// <param name="genre">The genre entity with updated information.</param>
    public async Task UpdateGenreAsync(Genre genre)
    {
        _context.Genres.Update(genre);
        await _context.SaveChangesAsync();
    }   
    
    /// <summary>
    /// Deletes a genre from the database by its ID.
    /// </summary>
    /// <param name="id">The ID of the genre to delete.</param>
    public async Task DeleteGenreAsync(int id)
    {
        var genre = await _context.Genres.FirstAsync(x => x.Id == id);
        _context.Genres.Remove(genre);
        await _context.SaveChangesAsync();
    }
    
    /// <summary>
    /// Checks if the specified genre is the only genre of any book in the system.
    /// This method eagerly loads related book and genre data to perform the check.
    /// </summary>
    /// <param name="id">The ID of the genre to check.</param>
    /// <returns><c>true</c> if the genre is the only genre of at least one book; otherwise, <c>false</c>.</returns>
    public async Task<bool> IsOnlyGenreOfAnyBookAsync(int id)
    {
        var genre = await _context.Genres.Include(x => x.Books).ThenInclude(x => x.Genres).FirstAsync(x => x.Id == id);
        return genre.Books.Any(x => x.Genres.Count == 1);
    }

    /// <summary>
    /// Retrieves a collection of existing genres based on a list of genre IDs.
    /// <param name="genresIds">Collection of genres IDs</param>
    /// <returns>Returns collection of existing genres</returns>
    /// </summary>
    public async Task<ICollection<Genre>> GetExistingGenresAsync(IEnumerable<int> genresIds)
    {
        return await _context.Genres.Where(x => genresIds.Contains(x.Id)).ToListAsync();
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<Genre>> GetFilteredGenresAsync(string searchTerm)
    {
        var startsWith = await _context.Genres
            .Where(x => x.Name.ToLower().StartsWith(searchTerm.ToLower()))
            .OrderBy(x => x.Name)
            .ToListAsync();
        var contains = await _context.Genres
            .Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()) && !x.Name.ToLower().StartsWith(searchTerm.ToLower()))
            .OrderBy(x => x.Name)
            .ToListAsync();
        return startsWith.Concat(contains);
    }
}