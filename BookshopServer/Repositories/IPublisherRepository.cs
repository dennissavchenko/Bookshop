using BookShopServer.Entities;

namespace BookShopServer.Repositories;

/// <summary>
/// Defines the contract for data access operations related to publisher entities.
/// </summary>
public interface IPublisherRepository
{
    /// <summary>
    /// Retrieves a publisher by their unique identifier.
    /// </summary>
    /// <param name="id">The ID of the publisher to retrieve.</param>
    /// <returns>The publisher entity if found, otherwise <c>null</c>.</returns>
    Task<Publisher?> GetPublisherByIdAsync(int id);

    /// <summary>
    /// Retrieves all publisher entities.
    /// </summary>
    /// <returns>A collection of all publishers.</returns>
    Task<IEnumerable<Publisher>> GetAllPublishersAsync();

    /// <summary>
    /// Checks if a publisher with the specified ID exists.
    /// </summary>
    /// <param name="id">The ID of the publisher to check.</param>
    /// <returns><c>true</c> if the publisher exists; otherwise, <c>false</c>.</returns>
    Task<bool> PublisherExistsAsync(int id);

    /// <summary>
    /// Adds a new publisher to the database.
    /// </summary>
    /// <param name="publisher">The publisher entity to add.</param>
    Task AddPublisherAsync(Publisher publisher);

    /// <summary>
    /// Updates an existing publisher in the database.
    /// </summary>
    /// <param name="publisher">The publisher entity with updated information.</param>
    Task UpdatePublisherAsync(Publisher publisher);

    /// <summary>
    /// Deletes a publisher from the database by their ID.
    /// </summary>
    /// <param name="id">The ID of the publisher to delete.</param>
    Task DeletePublisherAsync(int id);

    /// <summary>
    /// Checks if the provided email address is unique among existing publishers.
    /// </summary>
    /// <param name="email">The email address to check for uniqueness.</param>
    /// <returns><c>true</c> if the email is unique; otherwise, <c>false</c>.</returns>
    Task<bool> EmailUniqueAsync(string email);

    /// <summary>
    /// Checks if the provided phone number is unique among existing publishers.
    /// </summary>
    /// <param name="phoneNumber">The phone number to check for uniqueness.</param>
    /// <returns><c>true</c> if the phone number is unique; otherwise, <c>false</c>.</returns>
    Task<bool> PhoneNumberUniqueAsync(string phoneNumber);

    /// <summary>
    /// Checks if the specified publisher has any associated items.
    /// </summary>
    /// <param name="id">The ID of the publisher to check.</param>
    /// <returns><c>true</c> if the publisher has associated items; otherwise, <c>false</c>.</returns>
    Task<bool> PublisherHasItemsAsync(int id);
    
    /// <summary>
    /// Returns a collection of filtered publishers.
    /// </summary>
    /// <returns>A collection of <see cref="Publisher"/> representing filtered publishers.</returns>
    Task<IEnumerable<Publisher>> GetFilteredPublishersAsync(string searchTerm);
}