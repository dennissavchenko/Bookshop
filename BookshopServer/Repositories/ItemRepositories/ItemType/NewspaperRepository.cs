using BookShopServer.Entities;

namespace BookShopServer.Repositories.ItemRepositories.ItemType;

/// <summary>
/// Provides data access methods for managing newspaper entities in the bookshop system.
/// </summary>
public class NewspaperRepository : INewspaperRepository
{
    private readonly Context _context;

    /// <summary>
    /// Initializes a new instance of the <c>NewspaperRepository</c> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public NewspaperRepository(Context context)
    {
        _context = context;
    }

    /// <summary>
    /// Adds a new newspaper to the database.
    /// </summary>
    /// <param name="newspaper">The newspaper to add.</param>
    public async Task AddNewspaperAsync(Newspaper newspaper)
    {
        await _context.Newspapers.AddAsync(newspaper);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing newspaper in the database.
    /// </summary>
    /// <param name="newspaper">The newspaper with updated values.</param>
    public async Task UpdateNewspaperAsync(Newspaper newspaper)
    {
        _context.Newspapers.Update(newspaper);
        await _context.SaveChangesAsync();
    }
}