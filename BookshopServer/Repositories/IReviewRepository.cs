using BookShopServer.Entities;

namespace BookShopServer.Repositories;

/// <summary>
/// Defines the contract for data access operations related to review entities.
/// </summary>
public interface IReviewRepository
{
    /// <summary>
    /// Retrieves a collection of reviews associated with a specific item.
    /// </summary>
    /// <param name="itemId">The ID of the item to retrieve reviews for.</param>
    /// <returns>A collection of reviews for the specified item.</returns>
    Task<IEnumerable<Review>> GetReviewsByItemIdAsync(int itemId);

    /// <summary>
    /// Retrieves a collection of reviews posted by a specific customer.
    /// </summary>
    /// <param name="customerId">The ID of the customer to retrieve reviews for.</param>
    /// <returns>A collection of reviews by the specified customer.</returns>
    Task<ICollection<Review>> GetReviewsByCustomerIdAsync(int customerId);

    /// <summary>
    /// Retrieves a review by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the review to retrieve.</param>
    /// <returns>The review entity if found, otherwise <c>null</c>.</returns>
    Task<Review?> GetReviewByIdAsync(int id);

    /// <summary>
    /// Adds a new review to the database.
    /// </summary>
    /// <param name="review">The review entity to add.</param>
    Task AddReviewAsync(Review review);

    /// <summary>
    /// Updates an existing review in the database.
    /// </summary>
    /// <param name="review">The review entity with updated information.</param>
    Task UpdateReviewAsync(Review review);

    /// <summary>
    /// Deletes a review from the database by its ID.
    /// </summary>
    /// <param name="id">The ID of the review to delete.</param>
    Task DeleteReviewAsync(int id);

    /// <summary>
    /// Checks if a review with the specified ID exists.
    /// </summary>
    /// <param name="id">The ID of the review to check.</param>
    /// <returns><c>true</c> if the review exists; otherwise, <c>false</c>.</returns>
    Task<bool> ReviewExistsAsync(int id);

    /// <summary>
    /// Retrieves all review entities.
    /// </summary>
    /// <returns>A collection of all reviews.</returns>
    Task<IEnumerable<Review>> GetAllReviewsAsync();

    /// <summary>
    /// Checks if a review by a specific customer for a specific item already exists.
    /// </summary>
    /// <param name="itemId">The ID of the item.</param>
    /// <param name="customerId">The ID of the customer.</param>
    /// <returns><c>true</c> if a review by the customer for the item exists; otherwise, <c>false</c>.</returns>
    Task<bool> ReviewExistsForItemCustomerAsync(int itemId, int customerId);
    Task UpdateReviewsAsync(IEnumerable<Review> reviews);
}