using BookShopServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookShopServer.Repositories;

/// <summary>
/// Provides data access methods for managing review entities in the bookshop system.
/// </summary>
public class ReviewRepository : IReviewRepository
{
    private readonly Context _context;
    
    /// <summary>
    /// Initializes a new instance of the <c>ReviewRepository</c> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public ReviewRepository(Context context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Retrieves a collection of reviews associated with a specific item, including customer and user information.
    /// </summary>
    /// <param name="itemId">The ID of the item to retrieve reviews for.</param>
    /// <returns>A collection of reviews for the specified item.</returns>
    public async Task<IEnumerable<Review>> GetReviewsByItemIdAsync(int itemId)
    {
        return await _context.Reviews
            .Where(r => r.ItemId == itemId)
            .Include(x => x.Customer)
            .ThenInclude(x => x.User)
            .ToListAsync();
    }
    
    /// <summary>
    /// Retrieves a collection of reviews posted by a specific customer, including item information.
    /// </summary>
    /// <param name="customerId">The ID of the customer to retrieve reviews for.</param>
    /// <returns>A collection of reviews by the specified customer.</returns>
    public async Task<ICollection<Review>> GetReviewsByCustomerIdAsync(int customerId)
    {
        return await _context.Reviews
            .Where(r => r.CustomerId == customerId)
            .Include(r => r.Item)
            .ThenInclude(x => x.Reviews)
            .ToListAsync();
    }
    
    /// <summary>
    /// Retrieves a review by its unique identifier, including associated customer, user, and item information.
    /// </summary>
    /// <param name="id">The ID of the review to retrieve.</param>
    /// <returns>The review entity if found.</returns>
    public async Task<Review?> GetReviewByIdAsync(int id)
    {
        return await _context.Reviews
            .Include(r => r.Customer)
            .ThenInclude(x => x.User)
            .Include(r => r.Item)
            .FirstOrDefaultAsync(r => r.Id == id);
    }
    
    /// <summary>
    /// Adds a new review to the database.
    /// </summary>
    /// <param name="review">The review entity to add.</param>
    public async Task AddReviewAsync(Review review)
    {
        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();
    }
    
    /// <summary>
    /// Updates an existing review in the database.
    /// </summary>
    /// <param name="review">The review entity with updated information.</param>
    public async Task UpdateReviewAsync(Review review)
    {
        _context.Reviews.Update(review);
        await _context.SaveChangesAsync();
    }
    
    /// <summary>
    /// Deletes a review from the database by its ID.
    /// </summary>
    /// <param name="id">The ID of the review to delete.</param>
    public async Task DeleteReviewAsync(int id)
    {
        var review = await _context.Reviews.FirstAsync(x => x.Id == id);
        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();
    }
    
    /// <summary>
    /// Checks if a review with the specified ID exists.
    /// </summary>
    /// <param name="id">The ID of the review to check.</param>
    /// <returns><c>true</c> if the review exists; otherwise, <c>false</c>.</returns>
    public async Task<bool> ReviewExistsAsync(int id)
    {
        return await _context.Reviews.AnyAsync(r => r.Id == id);
    }
    
    /// <summary>
    /// Retrieves all review entities, including associated customer, user, and item information.
    /// </summary>
    /// <returns>A collection of all reviews.</returns>
    public async Task<IEnumerable<Review>> GetAllReviewsAsync()
    {
        return await _context.Reviews
            .Include(r => r.Customer)
            .ThenInclude(x => x.User)
            .Include(r => r.Item)
            .ToListAsync();
    }
    
    /// <summary>
    /// Checks if a review by a specific customer for a specific item already exists.
    /// </summary>
    /// <param name="itemId">The ID of the item.</param>
    /// <param name="customerId">The ID of the customer.</param>
    /// <returns><c>true</c> if a review by the customer for the item exists; otherwise, <c>false</c>.</returns>
    public async Task<bool> ReviewExistsForItemCustomerAsync(int itemId, int customerId)
    {
        return await _context.Reviews.AnyAsync(r => r.ItemId == itemId && r.CustomerId == customerId);
    }
    
    public async Task UpdateReviewsAsync(IEnumerable<Review> reviews)
    {
        foreach (var review in reviews)
        {
            _context.Reviews.Update(review);
        }
        await _context.SaveChangesAsync();
    }
}