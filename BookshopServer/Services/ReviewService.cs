using BookShopServer.DTOs;
using BookShopServer.DTOs.ReviewDTOs;
using BookShopServer.Entities;
using BookShopServer.Exceptions;
using BookShopServer.Repositories;
using BookShopServer.Repositories.ItemRepositories;
using BookShopServer.Repositories.UserRepositories;

namespace BookShopServer.Services;

/// <summary>
/// Provides services for managing customer reviews of items, including retrieval, addition,
/// update, deletion, and reassignment of reviews from deleted customers.
/// </summary>
public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IItemRepository _itemRepository;
    
    /// <summary>
    /// Initializes a new instance of the <c>ReviewService</c> class.
    /// </summary>
    /// <param name="reviewRepository">The repository for review data.</param>
    /// <param name="itemRepository">The repository for item data.</param>
    /// <param name="customerRepository">The repository for customer data.</param>
    public ReviewService(IReviewRepository reviewRepository, IItemRepository itemRepository, ICustomerRepository customerRepository)
    {
        _reviewRepository = reviewRepository;
        _itemRepository = itemRepository;
        _customerRepository = customerRepository;
    }
    
    /// <summary>
    /// Retrieves a collection of reviews for a specific item, mapped to <see cref="ReviewForItemDto"/>.
    /// </summary>
    /// <param name="itemId">The unique identifier of the item.</param>
    /// <returns>A collection of <see cref="ReviewForItemDto"/> representing reviews for the specified item.</returns>
    public async Task<IEnumerable<ReviewForItemDto>> GetReviewsByItemIdAsync(int itemId)
    {
        // Check if the item exists before retrieving reviews
        if (!await _itemRepository.ItemExistsAsync(itemId))
            throw new NotFoundException("Item with given ID does not exist.");
        // Retrieve reviews associated with the given item ID
        var reviews = await _reviewRepository.GetReviewsByItemIdAsync(itemId);
        
        // Map the review entities to ReviewForItemDto, including username and item/customer IDs
        return reviews.Select(review => new ReviewForItemDto
        {
            Id = review.Id,
            Text = review.Text,
            Rating = review.Rating,
            TimeStamp = review.TimeStamp,
            Username = review.Customer.User.Username,
            ItemId = review.ItemId,
            CustomerId = review.CustomerId
        });
    }
    
    /// <summary>
    /// Retrieves a collection of reviews made by a specific customer, mapped to <see cref="ReviewFromCustomerDto"/>.
    /// </summary>
    /// <param name="customerId">The unique identifier of the customer.</param>
    /// <returns>A collection of <see cref="ReviewFromCustomerDto"/> representing reviews from the specified customer.</returns>
    public async Task<IEnumerable<ReviewFromCustomerDto>> GetReviewsByCustomerIdAsync(int customerId)
    {
        // Check if the customer exists before retrieving reviews
        if (!await _customerRepository.CustomerExistsAsync(customerId))
            throw new NotFoundException("Customer with given ID does not exist.");
        // Retrieve reviews associated with the given customer ID
        var reviews = await _reviewRepository.GetReviewsByCustomerIdAsync(customerId);
        
        // Map the review entities to ReviewFromCustomerDto, including item name and item/customer IDs
        return reviews.Select(review => new ReviewFromCustomerDto
        {
            Id = review.Id,
            Text = review.Text,
            Rating = review.Rating,
            TimeStamp = review.TimeStamp,
            ItemId = review.ItemId,
            CustomerId = review.CustomerId,
            ItemName = review.Item.Name
        });
    }
    
    /// <summary>
    /// Retrieves a collection of all reviews in the system, mapped to <see cref="ReviewDto"/>.
    /// </summary>
    /// <returns>A collection of <see cref="ReviewDto"/> representing all reviews.</returns>
    public async Task<IEnumerable<ReviewDto>> GetAllReviewsAsync()
    {
        // Retrieve all review entities
        var reviews = await _reviewRepository.GetAllReviewsAsync();
        
        // Map all review entities to ReviewDto, including both username and item name
        return reviews.Select(review => new ReviewDto
        {
            Id = review.Id,
            Text = review.Text,
            Rating = review.Rating,
            TimeStamp = review.TimeStamp,
            Username = review.Customer.User.Username, 
            ItemName = review.Item.Name,
            ItemId = review.ItemId,
            CustomerId = review.CustomerId
        });
    }
    
    /// <summary>
    /// Retrieves a single review by its unique identifier, mapped to <see cref="ReviewDto"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the review.</param>
    /// <returns>A <see cref="ReviewDto"/> representing the found review.</returns>
    /// <exception cref="NotFoundException">Thrown if the review with the given ID does not exist.</exception>
    public async Task<ReviewDto> GetReviewByIdAsync(int id)
    {
        // Retrieve the review entity
        var review = await _reviewRepository.GetReviewByIdAsync(id);
        
        // Check if the review exists
        if (review == null)
            throw new NotFoundException("Review with given ID does not exist.");
        
        // Map the review entity to ReviewDto
        return new ReviewDto
        {
            Id = review.Id,
            Text = review.Text,
            Rating = review.Rating,
            TimeStamp = review.TimeStamp,
            Username = review.Customer.User.Username, 
            ItemName = review.Item.Name,
            ItemId = review.ItemId,
            CustomerId = review.CustomerId
        };
    }
    
    /// <summary>
    /// Adds a new review to the system. Performs validation to ensure the item and customer exist,
    /// and that the customer is not a "DeletedUser" and has not already reviewed the item.
    /// </summary>
    /// <param name="reviewDto">The <see cref="AddReviewDto"/> containing the details for the new review.</param>
    /// <exception cref="NotFoundException">Thrown if the item or customer does not exist.</exception>
    /// <exception cref="BadRequestException">Thrown if the customer is a "DeletedUser" or has already reviewed the item.</exception>
    public async Task AddReviewAsync(AddReviewDto reviewDto)
    {
        // Validate if the item exists
        if (!await _itemRepository.ItemExistsAsync(reviewDto.ItemId))
            throw new NotFoundException("Item with given ID does not exist.");
        
        // Retrieve customer
        var customer = await _customerRepository.GetCustomerByIdAsync(reviewDto.CustomerId);
        
        // Check if the customer exists
        if (customer == null)
            throw new NotFoundException("Customer with given ID does not exist.");
        
        // Check if the customer is a "DeletedUser"
        if (customer.User.Username == "DeletedUser")
            throw new BadRequestException("Deleted user cannot review items.");
        
        // Prevent a customer from submitting multiple reviews for the same item
        if (await _reviewRepository.ReviewExistsForItemCustomerAsync(reviewDto.ItemId, reviewDto.CustomerId))
            throw new BadRequestException("Customer has already reviewed this item.");
        
        // Create a new Review entity from the DTO
        var review = new Review
        {
            Text = reviewDto.Text,
            Rating = reviewDto.Rating,
            TimeStamp = DateTime.Now, 
            ItemId = reviewDto.ItemId,
            CustomerId = reviewDto.CustomerId
        };
        
        // Add the new review to the repository
        await _reviewRepository.AddReviewAsync(review);
    }
    
    /// <summary>
    /// Updates an existing review's text and rating. The timestamp of the review is updated to the current time.
    /// </summary>
    /// <param name="reviewDto">The <see cref="UpdateReviewDto"/> containing the ID and updated details of the review.</param>
    /// <exception cref="NotFoundException">Thrown if the review with the given ID does not exist.</exception>
    public async Task UpdateReviewAsync(UpdateReviewDto reviewDto)
    {
        // Retrieve the existing review entity
        var review = await _reviewRepository.GetReviewByIdAsync(reviewDto.Id);
        
        // Check if the review exists
        if (review == null)
            throw new NotFoundException("Review with given ID does not exist.");
        
        // Update the review's properties
        review.Text = reviewDto.Text;
        review.Rating = reviewDto.Rating;
        review.TimeStamp = DateTime.Now;
        
        // Persist the updated review to the repository
        await _reviewRepository.UpdateReviewAsync(review);
    }
    
    /// <summary>
    /// Deletes a review from the system by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the review to delete.</param>
    /// <exception cref="NotFoundException">Thrown if the review with the given ID does not exist.</exception>
    public async Task DeleteReviewAsync(int id)
    {
        // Check if the review exists before attempting deletion
        if (!await _reviewRepository.ReviewExistsAsync(id))
            throw new NotFoundException("Review with given ID does not exist.");
        
        // Delete the review from the repository
        await _reviewRepository.DeleteReviewAsync(id);
    }

    /// <summary>
    /// Assigns all reviews associated with a specific customer to a special 'deleted customer' entity.
    /// This is typically used when a customer account is deleted, allowing reviews to be retained
    /// for item display purposes without linking them to the original, now-deleted customer.
    /// </summary>
    /// <param name="customerId">The unique identifier of the customer whose reviews are to be reassigned.</param>
    /// <exception cref="NotFoundException">Thrown if the customer with the given ID does not exist.</exception>
    public async Task AssignReviewsOfCustomerToDeletedCustomerAsync(int customerId)
    {
        // Validate customer existence
        if (!await _customerRepository.CustomerExistsAsync(customerId))
            throw new NotFoundException("Customer with given ID does not exist.");

        // Get the special 'DeletedUser' customer entity once, before the loop.
        // This customer ID will be used for all reassignments.
        var deletedCustomer = await _customerRepository.GetDeletedCustomerAsync();
    
        // Retrieve all reviews made by the specified customer.
        var reviews = await _reviewRepository.GetReviewsByCustomerIdAsync(customerId);

        // Iterate through each review and reassign its CustomerId to the 'deleted customer'
        foreach (var review in reviews)
        {
            // Update the CustomerId of the current review object in the collection
            review.CustomerId = deletedCustomer.UserId;
        }

        await _reviewRepository.UpdateReviewsAsync(reviews);

    }
}