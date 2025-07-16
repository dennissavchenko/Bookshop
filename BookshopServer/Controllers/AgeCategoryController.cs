using BookShopServer.DTOs;
using BookShopServer.Exceptions;
using BookShopServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShopServer.Controllers;

/// <summary>
/// Controller for managing age categories.
/// Provides endpoints for retrieving, creating, updating, and deleting age categories.
/// </summary>

[ApiController]
[Route("api/age-categories")]
public class AgeCategoryController : ControllerBase
{
    private readonly IAgeCategoryService _ageCategoryService;

    /// <summary>
    /// Initializes a new instance of the <c>AgeCategoryController</c> class.
    /// </summary>
    /// <param name="ageCategoryService">The service used for age category operations.</param>
    public AgeCategoryController(IAgeCategoryService ageCategoryService)
    {
        _ageCategoryService = ageCategoryService;
    }

    /// <summary>
    /// Retrieves an age category by its ID.
    /// </summary>
    /// <param name="id">The ID of the age category to retrieve.</param>
    /// <returns>
    /// A 200 OK response with the age category if found;
    /// otherwise, a 404 Not Found response.
    /// </returns>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAgeCategoryByIdAsync(int id)
    {
        try
        {
            var ageCategory = await _ageCategoryService.GetAgeCategoryByIdAsync(id);
            return Ok(ageCategory);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    /// Retrieves all age categories.
    /// </summary>
    /// <returns>
    /// A 200 OK response containing the list of all age categories.
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> GetAllAgeCategoriesAsync()
    {
        var ageCategories = await _ageCategoryService.GetAllAgeCategoriesAsync();
        return Ok(ageCategories);
    }

    /// <summary>
    /// Creates a new age category.
    /// </summary>
    /// <param name="ageCategoryDto">The age category to create.</param>
    /// <returns>
    /// A 201 Created response if successful.
    /// </returns>
    [HttpPost]
    public async Task<IActionResult> AddAgeCategoryAsync([FromBody] AgeCategoryDto ageCategoryDto)
    {
        await _ageCategoryService.AddAgeCategoryAsync(ageCategoryDto);
        return StatusCode(StatusCodes.Status201Created);
    }

    /// <summary>
    /// Updates an existing age category.
    /// </summary>
    /// <param name="ageCategoryDto">The updated age category data.</param>
    /// <returns>
    /// A 204 No Content response if the update is successful;
    /// a 404 Not Found response if age category was not found.
    /// </returns>
    [HttpPut]
    public async Task<IActionResult> UpdateAgeCategoryAsync([FromBody] AgeCategoryDto ageCategoryDto)
    {
        try
        {
            await _ageCategoryService.UpdateAgeCategoryAsync(ageCategoryDto);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    /// Deletes an age category by ID.
    /// </summary>
    /// <param name="id">The ID of the age category to delete.</param>
    /// <returns>
    /// A 204 No Content response if deletion is successful;
    /// a 404 Not Found response if the category does not exist;
    /// or a 409 Conflict response if deletion is not possible due to dependencies (there are items associated with the age category).
    /// </returns>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAgeCategoryAsync(int id)
    {
        try
        {
            await _ageCategoryService.DeleteAgeCategoryAsync(id);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ConflictException e)
        {
            return Conflict(e.Message);
        }
    }
}
