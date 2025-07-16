using BookShopServer.Entities;
using BookShopServer.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BookShopServer.Repositories.ItemRepositories;

/// <summary>
/// Provides data access methods for managing item entities in the bookshop system,
/// including filtering by author, genre, age category, and publisher, as well as stock operations.
/// </summary>
public class ItemRepository : IItemRepository
{
    private readonly Context _context;

    /// <summary>
    /// Initializes a new instance of the <c>ItemRepository</c> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public ItemRepository(Context context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all items from the database, including related data and loading book-specific associations.
    /// </summary>
    /// <returns>A collection of all items.</returns>
    public async Task<IEnumerable<Item>> GetAllItemsAsync()
    {
        var item = await _context.Items
            .Include(x => x.AgeCategory)
            .Include(x => x.Publisher)
            .Include(x => x.Reviews)
            .Include(x => x.Book)
            .Include(x => x.Magazine)
            .Include(x => x.Newspaper)
            .ToListAsync();

        foreach (var i in item)
        {
            if (i.Book != null)
            {
                await _context.Entry(i.Book).Collection(b => b.Authors).LoadAsync();
                await _context.Entry(i.Book).Collection(g => g.Genres).LoadAsync();
            }
        }

        return item;
    }

    /// <summary>
    /// Retrieves items that are associated with a specific genre.
    /// </summary>
    /// <param name="genreId">The ID of the genre to filter by.</param>
    /// <returns>A collection of matching items.</returns>
    public async Task<IEnumerable<Item>> GetItemsByGenreAsync(int genreId)
    {
        var item = await _context.Items
            .Include(x => x.AgeCategory)
            .Include(x => x.Publisher)
            .Include(x => x.Reviews)
            .Include(x => x.Book)
            .Where(x => x.Book != null && x.Book.Genres.Any(g => g.Id == genreId))
            .ToListAsync();

        foreach (var i in item)
        {
            if (i.Book != null)
            {
                await _context.Entry(i.Book).Collection(b => b.Authors).LoadAsync();
                await _context.Entry(i.Book).Collection(g => g.Genres).LoadAsync();
            }
        }

        return item;
    }

    /// <summary>
    /// Retrieves items that are associated with a specific author.
    /// </summary>
    /// <param name="authorId">The ID of the author to filter by.</param>
    /// <returns>A collection of matching items.</returns>
    public async Task<IEnumerable<Item>> GetItemsByAuthorAsync(int authorId)
    {
        var item = await _context.Items
            .Include(x => x.AgeCategory)
            .Include(x => x.Publisher)
            .Include(x => x.Reviews)
            .Include(x => x.Book)
            .Where(x => x.Book != null && x.Book.Authors.Any(a => a.Id == authorId))
            .ToListAsync();

        foreach (var i in item)
        {
            if (i.Book != null)
            {
                await _context.Entry(i.Book).Collection(b => b.Authors).LoadAsync();
                await _context.Entry(i.Book).Collection(g => g.Genres).LoadAsync();
            }
        }

        return item;
    }

    /// <summary>
    /// Retrieves items that belong to a specific age category.
    /// </summary>
    /// <param name="ageCategoryId">The ID of the age category.</param>
    /// <returns>A collection of matching items.</returns>
    public async Task<IEnumerable<Item>> GetItemsByAgeCategoryAsync(int ageCategoryId)
    {
        var item = await _context.Items
            .Include(x => x.AgeCategory)
            .Include(x => x.Publisher)
            .Include(x => x.Reviews)
            .Include(x => x.Book)
            .Where(x => x.AgeCategory.Id == ageCategoryId)
            .ToListAsync();

        foreach (var i in item)
        {
            if (i.Book != null)
            {
                await _context.Entry(i.Book).Collection(b => b.Authors).LoadAsync();
                await _context.Entry(i.Book).Collection(g => g.Genres).LoadAsync();
            }
        }

        return item;
    }

    /// <summary>
    /// Retrieves items filtered by a specific age category.
    /// </summary>
    /// <param name="age">The age to filter items by.</param>
    /// <returns>A collection of items appropriate for the age.</returns>
    public async Task<IEnumerable<Item>> GetItemsAppropriateForAgeAsync(int age)
    {
        var item = await _context.Items
            .Include(x => x.AgeCategory)
            .Include(x => x.Publisher)
            .Include(x => x.Reviews)
            .Include(x => x.Book)
            .Where(x => x.AgeCategory.MinimumAge <= age)
            .ToListAsync();

        foreach (var i in item)
        {
            if (i.Book != null)
            {
                await _context.Entry(i.Book).Collection(b => b.Authors).LoadAsync();
                await _context.Entry(i.Book).Collection(g => g.Genres).LoadAsync();
            }
        }

        return item;
    }

    /// <summary>
    /// Retrieves items that are published by a specific publisher.
    /// </summary>
    /// <param name="publisherId">The ID of the publisher.</param>
    /// <returns>A collection of matching items.</returns>
    public async Task<IEnumerable<Item>> GetItemsByPublisherAsync(int publisherId)
    {
        var item = await _context.Items
            .Include(x => x.AgeCategory)
            .Include(x => x.Publisher)
            .Include(x => x.Reviews)
            .Include(x => x.Book)
            .Where(x => x.Publisher.Id == publisherId)
            .ToListAsync();

        foreach (var i in item)
        {
            if (i.Book != null)
            {
                await _context.Entry(i.Book).Collection(b => b.Authors).LoadAsync();
                await _context.Entry(i.Book).Collection(g => g.Genres).LoadAsync();
            }
        }

        return item;
    }

    /// <summary>
    /// Deletes an item from the database by its ID.
    /// </summary>
    /// <param name="id">The ID of the item to delete.</param>
    public async Task DeleteItemAsync(int id)
    {
        var item = await _context.Items
            // .Include(i => i.Book)
            // .Include(i => i.Magazine)
            // .Include(i => i.Newspaper)
            .FirstAsync(i => i.Id == id);

        // if (item.Book != null)
        //     _context.Books.Remove(item.Book);
        //
        // if (item.Magazine != null)
        //     _context.Magazines.Remove(item.Magazine);
        //
        // if (item.Newspaper != null)
        //     _context.Newspapers.Remove(item.Newspaper);

        _context.Items.Remove(item);
        await _context.SaveChangesAsync();
    }


    /// <summary>
    /// Checks whether an item with the specified ID exists.
    /// </summary>
    /// <param name="id">The ID of the item to check.</param>
    /// <returns><c>true</c> if the item exists; otherwise, <c>false</c>.</returns>
    public Task<bool> ItemExistsAsync(int id)
    {
        return _context.Items.AnyAsync(x => x.Id == id);
    }

    /// <summary>
    /// Retrieves the current quantity in stock for a given item.
    /// </summary>
    /// <param name="id">The ID of the item.</param>
    /// <returns>The quantity in stock.</returns>
    public async Task<int> GetItemAmountInStockAsync(int id)
    {
        var item = await _context.Items.FirstAsync(x => x.Id == id);
        return item.AmountInStock;
    }

    /// <summary>
    /// Increases the stock quantity of an item.
    /// </summary>
    /// <param name="id">The ID of the item.</param>
    /// <param name="amount">The amount to increase.</param>
    public async Task IncreaseItemAmountInStockAsync(int id, int amount)
    {
        var item = await _context.Items.FirstAsync(x => x.Id == id);
        item.AmountInStock += amount;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Decreases the stock quantity of an item.
    /// </summary>
    /// <param name="id">The ID of the item.</param>
    /// <param name="amount">The amount to decrease.</param>
    public async Task DecreaseItemAmountInStockAsync(int id, int amount)
    {
        var item = await _context.Items.FirstAsync(x => x.Id == id);
        item.AmountInStock -= amount;
        await _context.SaveChangesAsync();
    }
}
