using BookShopServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookShopServer.Repositories;

/// <summary>
/// Provides data access methods for managing publisher entities in the bookshop system.
/// </summary>
public class PublisherRepository : IPublisherRepository
{
    private readonly Context _context;
    
    /// <summary>
    /// Initializes a new instance of the <c>PublisherRepository</c> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public PublisherRepository(Context context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Retrieves a publisher by their unique identifier.
    /// </summary>
    /// <param name="id">The ID of the publisher to retrieve.</param>
    /// <returns>The publisher entity if found.</returns>
    public async Task<Publisher?> GetPublisherByIdAsync(int id)
    {
        return await _context.Publishers.FirstOrDefaultAsync(x => x.Id == id);
    }
    
    /// <summary>
    /// Retrieves all publisher entities.
    /// </summary>
    /// <returns>A collection of all publishers.</returns>
    public async Task<IEnumerable<Publisher>> GetAllPublishersAsync()
    {
        return await _context.Publishers.ToListAsync();
    }
    
    /// <summary>
    /// Checks if a publisher with the specified ID exists.
    /// </summary>
    /// <param name="id">The ID of the publisher to check.</param>
    /// <returns><c>true</c> if the publisher exists; otherwise, <c>false</c>.</returns>
    public async Task<bool> PublisherExistsAsync(int id)
    {
        return await _context.Publishers.AnyAsync(x => x.Id == id);
    }
    
    /// <summary>
    /// Adds a new publisher to the database.
    /// </summary>
    /// <param name="publisher">The publisher entity to add.</param>
    public async Task AddPublisherAsync(Publisher publisher)
    {
        await _context.Publishers.AddAsync(publisher);
        await _context.SaveChangesAsync();
    }
    
    /// <summary>
    /// Updates an existing publisher in the database.
    /// </summary>
    /// <param name="publisher">The publisher entity with updated information.</param>
    public async Task UpdatePublisherAsync(Publisher publisher)
    {
        _context.Publishers.Update(publisher);
        await _context.SaveChangesAsync();
    }
    
    /// <summary>
    /// Deletes a publisher from the database by their ID.
    /// </summary>
    /// <param name="id">The ID of the publisher to delete.</param>
    public async Task DeletePublisherAsync(int id)
    {
        var publisher = await _context.Publishers.FirstAsync(x => x.Id == id);
        _context.Publishers.Remove(publisher);
        await _context.SaveChangesAsync();
    }
    
    /// <summary>
    /// Checks if the provided email address is unique among existing publishers (case-insensitive).
    /// </summary>
    /// <param name="email">The email address to check for uniqueness.</param>
    /// <returns><c>true</c> if the email is unique; otherwise, <c>false</c>.</returns>
    public async Task<bool> EmailUniqueAsync(string email)
    {
        return !await _context.Publishers.AnyAsync(x => x.Email.ToLower() == email.ToLower());
    }
    
    /// <summary>
    /// Checks if the provided phone number is unique among existing publishers.
    /// </summary>
    /// <param name="phoneNumber">The phone number to check for uniqueness.</param>
    /// <returns><c>true</c> if the phone number is unique; otherwise, <c>false</c>.</returns>
    public async Task<bool> PhoneNumberUniqueAsync(string phoneNumber)
    {
        return !await _context.Publishers.AnyAsync(x => x.PhoneNumber == phoneNumber);
    }
    
    /// <summary>
    /// Checks if the specified publisher has any associated items.
    /// This method eagerly loads related item data to perform the check.
    /// </summary>
    /// <param name="id">The ID of the publisher to check.</param>
    /// <returns><c>true</c> if the publisher has associated items; otherwise, <c>false</c>.</returns>
    public async Task<bool> PublisherHasItemsAsync(int id)
    {
        var publisher = await _context.Publishers.Include(x => x.Items).FirstAsync(x => x.Id == id);
        return publisher.Items.Any();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Publisher>> GetFilteredPublishersAsync(string searchTerm)
    {
        var startsWith = await _context.Publishers
            .Where(x => x.Name.ToLower().StartsWith(searchTerm.ToLower()))
            .OrderBy(x => x.Name)
            .ToListAsync();
        var contains = await _context.Publishers
            .Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()) && !x.Name.ToLower().StartsWith(searchTerm.ToLower()))
            .OrderBy(x => x.Name)
            .ToListAsync();
        return startsWith.Concat(contains);
    }
    
}