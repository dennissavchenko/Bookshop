using BookShopServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookShopServer.Repositories;

/// <summary>
/// Provides data access methods for managing age category entities in the bookshop system.
/// </summary>
public class AgeCategoryRepository : IAgeCategoryRepository
{
    private readonly Context _context;
    
    /// <summary>
    /// Initializes a new instance of the <c>AgeCategoryRepository</c> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public AgeCategoryRepository(Context context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Retrieves an age category by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the age category to retrieve.</param>
    /// <returns>The age category entity if found.</returns>
    public async Task<AgeCategory?> GetAgeCategoryByIdAsync(int id)
    {
        return await _context.AgeCategories.FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <summary>
    /// Retrieves all age category entities.
    /// </summary>
    /// <returns>A collection of all age categories.</returns>
    public async Task<IEnumerable<AgeCategory>> GetAllAgeCategoriesAsync()
    {
        return await _context.AgeCategories.ToListAsync();
    }

    /// <summary>
    /// Adds a new age category to the database.
    /// </summary>
    /// <param name="ageCategory">The age category entity to add.</param>
    public async Task AddAgeCategoryAsync(AgeCategory ageCategory)
    {
        await _context.AgeCategories.AddAsync(ageCategory);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing age category in the database.
    /// </summary>
    /// <param name="ageCategory">The age category entity with updated information.</param>
    public async Task UpdateAgeCategoryAsync(AgeCategory ageCategory)
    {
        _context.AgeCategories.Update(ageCategory);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes an age category from the database by its ID.
    /// </summary>
    /// <param name="id">The ID of the age category to delete.</param>
    public async Task DeleteAgeCategoryAsync(int id)
    {
        var ageCategory = await _context.AgeCategories.FirstAsync(x => x.Id == id);
        _context.AgeCategories.Remove(ageCategory);
        await _context.SaveChangesAsync();
    }
    
    /// <summary>
    /// Checks if an age category with the specified ID exists.
    /// </summary>
    /// <param name="id">The ID of the age category to check.</param>
    /// <returns><c>true</c> if the age category exists; otherwise, <c>false</c>.</returns>
    public async Task<bool> AgeCategoryExistsAsync(int id)
    {
        return await _context.AgeCategories.AnyAsync(x => x.Id == id);
    }

    /// <summary>
    /// Checks if the specified age category has any associated items.
    /// </summary>
    /// <param name="id">The ID of the age category to check.</param>
    /// <returns><c>true</c> if the age category has associated items; otherwise, <c>false</c>.</returns>
    public async Task<bool> AgeCategoryHasItemsAsync(int id)
    {
        var ageCategory = await _context.AgeCategories.Include(x => x.Items).FirstAsync(x => x.Id == id);
        return ageCategory.Items.Any();
    }
}