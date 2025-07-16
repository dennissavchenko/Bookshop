using BookShopServer.Entities;

namespace BookShopServer.Repositories.ItemRepositories.ItemType;

/// <summary>
/// Provides data access methods for managing magazine entities in the bookshop system.
/// </summary>
public class MagazineRepository : IMagazineRepository
{
    private readonly Context _context;

    /// <summary>
    /// Initializes a new instance of the <c>MagazineRepository</c> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public MagazineRepository(Context context)
    {
        _context = context;
    }

    /// <summary>
    /// Adds a new magazine to the database.
    /// </summary>
    /// <param name="magazine">The magazine to add.</param>
    public async Task AddMagazineAsync(Magazine magazine)
    {
        await _context.Magazines.AddAsync(magazine);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing magazine in the database.
    /// </summary>
    /// <param name="magazine">The magazine with updated values.</param>
    public async Task UpdateMagazineAsync(Magazine magazine)
    {
        _context.Magazines.Update(magazine);
        await _context.SaveChangesAsync();
    }
}