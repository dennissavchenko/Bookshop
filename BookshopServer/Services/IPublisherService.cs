using BookShopServer.DTOs;

namespace BookShopServer.Services;

/// <summary>
/// Defines the contract for services that manage publisher-related operations.
/// Publishers are entities responsible for producing and distributing books or other items.
/// </summary>
public interface IPublisherService
{
    /// <summary>
    /// Retrieves a publisher by their unique identifier.
    /// </summary>
    /// <param name="id">The ID of the publisher to retrieve.</param>
    /// <returns>A <see cref="PublisherDto"/> representing the publisher.</returns>
    Task<PublisherDto> GetPublisherByIdAsync(int id);

    /// <summary>
    /// Retrieves a collection of all publishers.
    /// </summary>
    /// <returns>A collection of <see cref="PublisherDto"/> representing all publishers.</returns>
    Task<IEnumerable<PublisherDto>> GetAllPublishersAsync();

    /// <summary>
    /// Adds a new publisher to the system.
    /// </summary>
    /// <param name="publisher">The <see cref="PublisherDto"/> containing the details of the publisher to add.</param>
    Task AddPublisherAsync(PublisherDto publisher);

    /// <summary>
    /// Updates an existing publisher's details.
    /// </summary>
    /// <param name="publisher">The <see cref="PublisherDto"/> containing the updated details of the publisher.</param>
    Task UpdatePublisherAsync(PublisherDto publisher);

    /// <summary>
    /// Deletes a publisher from the system by their unique identifier.
    /// </summary>
    /// <param name="id">The ID of the publisher to delete.</param>
    Task DeletePublisherAsync(int id);
    
    /// <summary>
    /// Returns a collection of filtered publishers.
    /// </summary>
    /// <returns>A collection of <see cref="BriefEntityDto"/> representing filtered publishers.</returns>
    Task<IEnumerable<BriefEntityDto>> GetFilteredPublishersAsync(string searchTerm);
}