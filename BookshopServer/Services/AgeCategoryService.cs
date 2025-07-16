using BookShopServer.DTOs;
using BookShopServer.Entities;
using BookShopServer.Exceptions;
using BookShopServer.Repositories;

namespace BookShopServer.Services;

/// <summary>
/// Provides services for managing age category operations, including retrieving,
/// adding, updating, and deleting age categories for items.
/// </summary>
public class AgeCategoryService : IAgeCategoryService
{
    private readonly IAgeCategoryRepository _ageCategoryRepository;
    
    /// <summary>
    /// Initializes a new instance of the <c>AgeCategoryService</c> class.
    /// </summary>
    /// <param name="ageCategoryRepository">The repository for age category data.</param>
    public AgeCategoryService(IAgeCategoryRepository ageCategoryRepository)
    {
        _ageCategoryRepository = ageCategoryRepository;
    }
    
    /// <summary>
    /// Retrieves an age category by its unique identifier and maps it to an <see cref="AgeCategoryDto"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the age category.</param>
    /// <returns>An <see cref="AgeCategoryDto"/> representing the age category.</returns>
    /// <exception cref="NotFoundException">Thrown if the age category with the given ID does not exist.</exception>
    public async Task<AgeCategoryDto> GetAgeCategoryByIdAsync(int id)
    {
        // Retrieve the age category entity from the repository
        var ageCategory = await _ageCategoryRepository.GetAgeCategoryByIdAsync(id);
        
        // Check if the age category exists
        if (ageCategory == null)
            throw new NotFoundException("Age category with the given ID does not exist.");

        // Map the age category entity to an AgeCategoryDto
        return new AgeCategoryDto
        {
            Id = ageCategory.Id,
            Tag = ageCategory.Tag,
            Description = ageCategory.Description,
            MinimumAge = ageCategory.MinimumAge
        };
    }

    /// <summary>
    /// Retrieves a collection of all age categories and maps them to <see cref="AgeCategoryDto"/>s.
    /// </summary>
    /// <returns>A collection of <see cref="AgeCategoryDto"/> representing all age categories.</returns>
    public async Task<IEnumerable<AgeCategoryDto>> GetAllAgeCategoriesAsync()
    {
        // Retrieve all age category entities from the repository
        var ageCategories = await _ageCategoryRepository.GetAllAgeCategoriesAsync();
        
        // Map each age category entity to an AgeCategoryDto
        return ageCategories.Select(ageCategory => new AgeCategoryDto
        {
            Id = ageCategory.Id,
            Tag = ageCategory.Tag,
            Description = ageCategory.Description,
            MinimumAge = ageCategory.MinimumAge
        });
    }

    /// <summary>
    /// Adds a new age category to the system.
    /// </summary>
    /// <param name="ageCategoryDto">The <see cref="AgeCategoryDto"/> containing the details of the age category to add.</param>
    public async Task AddAgeCategoryAsync(AgeCategoryDto ageCategoryDto)
    {
        // Create a new AgeCategory entity from the DTO
        var newAgeCategory = new AgeCategory
        {
            Tag = ageCategoryDto.Tag,
            Description = ageCategoryDto.Description,
            MinimumAge = ageCategoryDto.MinimumAge
        };

        // Add the new age category to the repository
        await _ageCategoryRepository.AddAgeCategoryAsync(newAgeCategory);
    }

    /// <summary>
    /// Updates an existing age category's details.
    /// </summary>
    /// <param name="ageCategoryDto">The <see cref="AgeCategoryDto"/> containing the updated details of the age category.</param>
    /// <exception cref="NotFoundException">Thrown if the age category with the given ID does not exist.</exception>
    public async Task UpdateAgeCategoryAsync(AgeCategoryDto ageCategoryDto)
    {
        // Retrieve the existing age category entity from the repository
        var existingAgeCategory = await _ageCategoryRepository.GetAgeCategoryByIdAsync(ageCategoryDto.Id);
        
        // Check if the age category exists
        if (existingAgeCategory == null)
            throw new NotFoundException("Age category with the given ID does not exist.");

        // Update the properties of the existing age category entity with the new DTO values
        existingAgeCategory.Tag = ageCategoryDto.Tag;
        existingAgeCategory.Description = ageCategoryDto.Description;
        existingAgeCategory.MinimumAge = ageCategoryDto.MinimumAge;

        // Update the age category in the repository
        await _ageCategoryRepository.UpdateAgeCategoryAsync(existingAgeCategory);
    }

    /// <summary>
    /// Deletes an age category from the system by its unique identifier.
    /// An age category cannot be deleted if there are items associated with it.
    /// </summary>
    /// <param name="id">The unique identifier of the age category to delete.</param>
    /// <exception cref="NotFoundException">Thrown if the age category with the given ID does not exist.</exception>
    /// <exception cref="ConflictException">Thrown if the age category has associated items and cannot be deleted.</exception>
    public async Task DeleteAgeCategoryAsync(int id)
    {
        // Check if the age category exists before attempting deletion
        if(!await _ageCategoryRepository.AgeCategoryExistsAsync(id)) 
            throw new NotFoundException("Age category with the given ID does not exist.");

        // Prevent deletion if the age category is currently assigned to any items
        if (await _ageCategoryRepository.AgeCategoryHasItemsAsync(id))
            throw new ConflictException("Cannot delete age category because it has related items.");
        
        // Delete the age category from the repository
        await _ageCategoryRepository.DeleteAgeCategoryAsync(id);
    }
}