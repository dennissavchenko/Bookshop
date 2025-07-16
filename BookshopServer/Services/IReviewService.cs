using BookShopServer.DTOs;
using BookShopServer.DTOs.ReviewDTOs;

namespace BookShopServer.Services;

/// <summary>
/// Defines the contract for services that manage review operations.
/// </summary>
public interface IReviewService
{
    /// <summary>
    /// Retrieves a collection of reviews for a specific item.
    /// </summary>
    /// <param name="itemId">The ID of the item to retrieve reviews for.</param>
    /// <returns>A collection of <see cref="ReviewForItemDto"/> representing reviews for the item.</returns>
    Task<IEnumerable<ReviewForItemDto>> GetReviewsByItemIdAsync(int itemId);

    /// <summary>
    /// Retrieves a collection of reviews made by a specific customer.
    /// </summary>
    /// <param name="customerId">The ID of the customer to retrieve reviews from.</param>
    /// <returns>A collection of <see cref="ReviewFromCustomerDto"/> representing reviews made by the customer.</returns>
    Task<IEnumerable<ReviewFromCustomerDto>> GetReviewsByCustomerIdAsync(int customerId);

    /// <summary>
    /// Retrieves a review by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the review to retrieve.</param>
    /// <returns>A <see cref="ReviewDto"/> representing the review.</returns>
    Task<ReviewDto> GetReviewByIdAsync(int id);

    /// <summary>
    /// Retrieves a collection of all reviews in the system.
    /// </summary>
    /// <returns>A collection of <see cref="ReviewDto"/> representing all reviews.</returns>
    Task<IEnumerable<ReviewDto>> GetAllReviewsAsync();

    /// <summary>
    /// Adds a new review to the system.
    /// </summary>
    /// <param name="review">The <see cref="AddReviewDto"/> containing the details of the review to add.</param>
    Task AddReviewAsync(AddReviewDto review);

    /// <summary>
    /// Updates an existing review's details.
    /// </summary>
    /// <param name="review">The <see cref="UpdateReviewDto"/> containing the updated details of the review.</param>
    Task UpdateReviewAsync(UpdateReviewDto review);

    /// <summary>
    /// Deletes a review from the system by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the review to delete.</param>
    Task DeleteReviewAsync(int id);

    /// <summary>
    /// Assigns all reviews associated with a specific customer to a 'deleted customer' entity.
    /// This is typically used for data retention while anonymizing customer data upon deletion.
    /// </summary>
    /// <param name="customerId">The ID of the customer whose reviews are to be reassigned.</param>
    Task AssignReviewsOfCustomerToDeletedCustomerAsync(int customerId);
}