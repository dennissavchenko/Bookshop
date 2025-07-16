using BookShopServer.DTOs;
using BookShopServer.Entities;
using BookShopServer.Exceptions;
using BookShopServer.Repositories;

namespace BookShopServer.Services;

/// <summary>
/// Provides services for managing publisher-related operations, including retrieving,
/// adding, updating, and deleting publishers.
/// </summary>
public class PublisherService : IPublisherService
{
    private readonly IPublisherRepository _publisherRepository;
    
    /// <summary>
    /// Initializes a new instance of the <c>PublisherService</c> class.
    /// </summary>
    /// <param name="publisherRepository">The repository for publisher data.</param>
    public PublisherService(IPublisherRepository publisherRepository)
    {
        _publisherRepository = publisherRepository;
    }
    
    /// <summary>
    /// Retrieves a publisher by their unique identifier and maps it to a <see cref="PublisherDto"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the publisher.</param>
    /// <returns>A <see cref="PublisherDto"/> representing the publisher.</returns>
    /// <exception cref="NotFoundException">Thrown if the publisher with the given ID does not exist.</exception>
    public async Task<PublisherDto> GetPublisherByIdAsync(int id)
    {
        // Retrieve the publisher entity
        var publisher = await _publisherRepository.GetPublisherByIdAsync(id);
        
        // Check if the publisher exists
        if (publisher == null)
            throw new NotFoundException("Publisher with the given ID does not exist.");

        // Map the entity to a DTO and return
        return new PublisherDto
        {
            Id = publisher.Id,
            Name = publisher.Name,
            Address = publisher.Address,
            PhoneNumber = publisher.PhoneNumber,
            Email = publisher.Email
        };
    }
    
    /// <summary>
    /// Retrieves a collection of all publishers and maps them to <see cref="PublisherDto"/>s.
    /// </summary>
    /// <returns>A collection of <see cref="PublisherDto"/> representing all publishers.</returns>
    public async Task<IEnumerable<PublisherDto>> GetAllPublishersAsync()
    {
        // Retrieve all publisher entities from the repository
        var publishers = await _publisherRepository.GetAllPublishersAsync();

        // Map each entity to a DTO and convert the result to a list
        return publishers.Select(p => new PublisherDto
        {
            Id = p.Id,
            Name = p.Name,
            Address = p.Address,
            PhoneNumber = p.PhoneNumber,
            Email = p.Email
        }).ToList();
    }

    /// <summary>
    /// Adds a new publisher to the system. Performs validation for unique email and phone number.
    /// </summary>
    /// <param name="publisherDto">The DTO containing the details of the publisher to add.</param>
    /// <exception cref="ConflictException">Thrown if a publisher with the provided email or phone number already exists.</exception>
    public async Task AddPublisherAsync(PublisherDto publisherDto)
    {
        // Validate if the email is unique
        if(!await _publisherRepository.EmailUniqueAsync(publisherDto.Email))
            throw new ConflictException("Publisher with this email already exists.");
        
        // Validate if the phone number is unique
        if(!await _publisherRepository.PhoneNumberUniqueAsync(publisherDto.PhoneNumber))
            throw new ConflictException("Publisher with this phone number already exists.");

        // Create a new Publisher entity from the DTO
        var newPublisher = new Publisher
        {
            Name = publisherDto.Name,
            Address = publisherDto.Address,
            PhoneNumber = publisherDto.PhoneNumber,
            Email = publisherDto.Email
        };
        
        // Add the new publisher to the repository
        await _publisherRepository.AddPublisherAsync(newPublisher);
    }
    
    /// <summary>
    /// Updates an existing publisher's details. Performs validation for publisher existence
    /// and unique email/phone number (if changed).
    /// </summary>
    /// <param name="publisherDto">The DTO containing the updated details of the publisher.</param>
    /// <exception cref="NotFoundException">Thrown if the publisher with the given ID does not exist.</exception>
    /// <exception cref="ConflictException">Thrown if the updated email or phone number conflicts with an existing publisher.</exception>
    public async Task UpdatePublisherAsync(PublisherDto publisherDto)
    {
        // Retrieve the existing publisher entity to compare against for unique constraints
        var existingPublisher = await _publisherRepository.GetPublisherByIdAsync(publisherDto.Id);
        
        // Check if the publisher exists
        if(existingPublisher == null)
            throw new NotFoundException("Publisher with the given ID does not exist.");

        // Validate if the email is unique, considering the current publisher's email
        if(!await _publisherRepository.EmailUniqueAsync(publisherDto.Email) && existingPublisher.Email != publisherDto.Email)
            throw new ConflictException("Publisher with this email already exists.");
        
        // Validate if the phone number is unique, considering the current publisher's phone number
        if(!await _publisherRepository.PhoneNumberUniqueAsync(publisherDto.PhoneNumber) && existingPublisher.PhoneNumber != publisherDto.PhoneNumber)
            throw new ConflictException("Publisher with this phone number already exists.");
        
        // Update the properties of the existing publisher entity
        existingPublisher.Name = publisherDto.Name;
        existingPublisher.Address = publisherDto.Address;
        existingPublisher.PhoneNumber = publisherDto.PhoneNumber;
        existingPublisher.Email = publisherDto.Email;
        
        // Update the publisher in the repository
        await _publisherRepository.UpdatePublisherAsync(existingPublisher);
    }
    
    /// <summary>
    /// Deletes a publisher from the system by their unique identifier.
    /// A publisher cannot be deleted if there are items associated with them.
    /// </summary>
    /// <param name="id">The ID of the publisher to delete.</param>
    /// <exception cref="NotFoundException">Thrown if the publisher with the given ID does not exist.</exception>
    /// <exception cref="ConflictException">Thrown if the publisher has associated items and cannot be deleted.</exception>
    public async Task DeletePublisherAsync(int id)
    {
        // Check if the publisher exists
        if(!await _publisherRepository.PublisherExistsAsync(id))
            throw new NotFoundException("Publisher with the given ID does not exist.");

        // Prevent deletion if the publisher still has associated items
        if(await _publisherRepository.PublisherHasItemsAsync(id))
            throw new ConflictException("Publisher cannot be deleted because it has associated items.");

        // Delete the publisher from the repository
        await _publisherRepository.DeletePublisherAsync(id);
    }

    /// <inheridoc />
    public async Task<IEnumerable<BriefEntityDto>> GetFilteredPublishersAsync(string searchTerm)
    {
        // Retrieve filtered publisher entities from the repository
        var publishers = await _publisherRepository.GetFilteredPublishersAsync(searchTerm);

        // Map each entity to a DTO and convert the result to a list
        return publishers.Select(p => new BriefEntityDto
        {
            Id = p.Id,
            Name = p.Name
        }).ToList();
    }
}