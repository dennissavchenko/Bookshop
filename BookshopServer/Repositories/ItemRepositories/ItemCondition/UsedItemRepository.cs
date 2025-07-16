using BookShopServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookShopServer.Repositories.ItemRepositories.ItemCondition;

/// <summary>
/// Provides data access methods for managing used items in the bookshop system.
/// Supports retrieving, creating, updating, and checking for the existence of used items.
/// </summary>
public class UsedItemRepository : IUsedItemRepository
{
    private readonly Context _context;

    /// <summary>
    /// Initializes a new instance of the <c>UsedItemRepository</c> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public UsedItemRepository(Context context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a used item by its ID, including related data such as age category, publisher,
    /// reviews, customer, user, and associated content (book, magazine, newspaper).
    /// If the item is a book, its authors and genres are also loaded.
    /// </summary>
    /// <param name="id">The ID of the used item to retrieve.</param>
    /// <returns>The matching used item if found; otherwise, null.</returns>
    public async Task<UsedItem?> GetUsedItemByIdAsync(int id)
    {
        var newItem = await _context.UsedItems
            .Include(x => x.AgeCategory)
            .Include(x => x.Publisher)
            .Include(x => x.Reviews)
            .ThenInclude(x => x.Customer)
            .ThenInclude(x => x.User)
            .Include(x => x.Book)
            .Include(x => x.Magazine)
            .Include(x => x.Newspaper)
            .FirstOrDefaultAsync(x => x.Id == id);

        // If the used item is a book, load its authors and genres
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
    /// Adds a new used item to the database.
    /// </summary>
    /// <param name="usedItem">The used item to add.</param>
    /// <returns>The ID of the newly created item.</returns>
    public async Task<int> AddUsedItemAsync(UsedItem usedItem)
    {
        await _context.UsedItems.AddAsync(usedItem);
        await _context.SaveChangesAsync();
        return usedItem.Id;
    }

    /// <summary>
    /// Updates an existing used item in the database.
    /// </summary>
    /// <param name="usedItem">The used item with updated values.</param>
    public async Task UpdateUsedItemAsync(UsedItem usedItem)
    {
        _context.UsedItems.Update(usedItem);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Checks if a used item with the specified ID exists in the database.
    /// </summary>
    /// <param name="id">The ID of the item to check.</param>
    /// <returns><c>true</c> if the item exists; otherwise, <c>false</c>.</returns>
    public async Task<bool> UsedItemExistsAsync(int id)
    {
        return await _context.UsedItems.AnyAsync(x => x.Id == id);
    }

    /// <summary>
    /// Retrieves all used items from the database, including related data such as age category,
    /// publisher, reviews, and book associations.
    /// If the item is a book, authors and genres are also loaded.
    /// </summary>
    /// <returns>A collection of all used items.</returns>
    public async Task<IEnumerable<UsedItem>> GetAllUsedItemsAsync()
    {
        var item = await _context.UsedItems
            .Include(x => x.AgeCategory)
            .Include(x => x.Publisher)
            .Include(x => x.Reviews)
            .Include(x => x.Book)
            .ToListAsync();

        // Load related authors and genres for each book in the used items
        foreach (var i in item)
        {
            if (i.Book != null)
            {
                await _context.Entry(i.Book)
                    .Collection(b => b.Authors)
                    .LoadAsync();
                await _context.Entry(i.Book)
                    .Collection(g => g.Genres)
                    .LoadAsync();
            }
        }

        return item;
    }
}
