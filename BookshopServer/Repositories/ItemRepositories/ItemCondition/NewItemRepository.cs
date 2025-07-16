using BookShopServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookShopServer.Repositories.ItemRepositories.ItemCondition;

/// <summary>
/// Provides data access methods for managing new items in the bookshop system.
/// Supports retrieving, creating, updating, and checking for the existence of new items.
/// </summary>
public class NewItemRepository : INewItemRepository
{
    private readonly Context _context;

    /// <summary>
    /// Initializes a new instance of the NewItemRepository class with the specified database context.
    /// </summary>
    /// <param name="context">The database context.</param>
    public NewItemRepository(Context context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a NewItem by its ID, including related entities such as AgeCategory, Publisher, Reviews,
    /// Customer, User, and associated content (Book, Magazine, Newspaper).
    /// Loads author and genre collections if the item is a book.
    /// </summary>
    /// <param name="id">The ID of the new item to retrieve.</param>
    /// <returns>The matching NewItem if found; otherwise, <c>null</c>.</returns>
    public async Task<NewItem?> GetNewItemByIdAsync(int id)
    {
        var newItem = await _context.NewItems
            .Include(x => x.AgeCategory)
            .Include(x => x.Publisher)
            .Include(x => x.Reviews)
            .ThenInclude(x => x.Customer)
            .ThenInclude(x => x.User)
            .Include(x => x.Book)
            .Include(x => x.Magazine)
            .Include(x => x.Newspaper)
            .FirstOrDefaultAsync(x => x.Id == id);

        // If the new item is a book, load its authors and genres
        if (newItem?.Book != null)
        {
            await _context.Entry(newItem.Book)
                .Collection(b => b.Authors)
                .LoadAsync();
            await _context.Entry(newItem.Book)
                .Collection(b => b.Genres)
                .LoadAsync();
        }

        return newItem;
    }

    /// <summary>
    /// Adds a new NewItem to the database.
    /// </summary>
    /// <param name="newItem">The new item to add.</param>
    /// <returns>The ID of the newly added item.</returns>
    public async Task<int> AddNewItemAsync(NewItem newItem)
    {
        await _context.NewItems.AddAsync(newItem);
        await _context.SaveChangesAsync();
        return newItem.Id;
    }

    /// <summary>
    /// Updates an existing NewItem in the database.
    /// </summary>
    /// <param name="newItem">The new item entity with updated values.</param>
    public async Task UpdateNewItemAsync(NewItem newItem)
    {
        _context.NewItems.Update(newItem);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Checks whether a NewItem with the specified ID exists in the database.
    /// </summary>
    /// <param name="id">The ID of the item to check.</param>
    /// <returns><c>true</c> if the item exists; otherwise, <c>false</c>.</returns>
    public async Task<bool> NewItemExistsAsync(int id)
    {
        return await _context.NewItems.AnyAsync(x => x.Id == id);
    }

    /// <summary>
    /// Retrieves all NewItem entities from the database,
    /// including their related AgeCategory, Publisher, Reviews, and Book details.
    /// If a new item is a book, loads its authors and genres.
    /// </summary>
    /// <returns>A collection of all NewItem entities.</returns>
    public async Task<IEnumerable<NewItem>> GetAllNewItemsAsync()
    {
        var newItems = await _context.NewItems
            .Include(x => x.AgeCategory)
            .Include(x => x.Publisher)
            .Include(x => x.Reviews)
            .Include(x => x.Book)
            .ToListAsync();

        // If the new item is a book, load its authors and genres
        foreach (var item in newItems)
        {
            if (item.Book != null)
            {
                await _context.Entry(item.Book)
                    .Collection(b => b.Authors)
                    .LoadAsync();
                await _context.Entry(item.Book)
                    .Collection(g => g.Genres)
                    .LoadAsync();
            }
        }

        return newItems;
    }
}
