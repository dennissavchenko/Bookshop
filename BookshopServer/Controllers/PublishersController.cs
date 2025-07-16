using BookShopServer.DTOs;
using BookShopServer.Exceptions;
using BookShopServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookShopServer.Controllers;

/// <summary>
/// Controller for managing publishers.
/// Provides endpoints for retrieving, creating, updating, and deleting publishers.
/// </summary>
[ApiController]
[Route("api/publishers")]
public class PublishersController : ControllerBase
{
    private readonly IPublisherService _publisherService;

    /// <summary>
    /// Initializes a new instance of the <c>PublishersController</c> class.
    /// </summary>
    /// <param name="publisherService">Service for publisher-related operations.</param>
    public PublishersController(IPublisherService publisherService)
    {
        _publisherService = publisherService;
    }

    /// <summary>
    /// Retrieves a publisher by ID.
    /// </summary>
    /// <param name="id">The ID of the publisher.</param>
    /// <returns>
    /// A 200 OK response with the publisher data;
    /// or a 404 Not Found response if the publisher does not exist.
    /// </returns>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPublisherByIdAsync(int id)
    {
        try
        {
            var publisher = await _publisherService.GetPublisherByIdAsync(id);
            return Ok(publisher);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Retrieves all publishers.
    /// </summary>
    /// <returns>A 200 OK response containing the list of all publishers.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllPublishersAsync()
    {
        var publishers = await _publisherService.GetAllPublishersAsync();
        return Ok(publishers);
    }

    /// <summary>
    /// Creates a new publisher.
    /// </summary>
    /// <param name="publisher">The publisher data to create.</param>
    /// <returns>
    /// A 201 Created response if creation is successful;
    /// or a 409 Conflict response if email or phone number already exists.
    /// </returns>
    [HttpPost]
    public async Task<IActionResult> AddPublisherAsync([FromBody] PublisherDto publisher)
    {
        try
        {
            await _publisherService.AddPublisherAsync(publisher);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (ConflictException ex)
        {
            return Conflict(ex.Message);
        }
    }

    /// <summary>
    /// Updates an existing publisher.
    /// </summary>
    /// <param name="publisher">The updated publisher data.</param>
    /// <returns>
    /// A 204 No Content response if the update is successful;
    /// a 404 Not Found response if the publisher does not exist;
    /// or a 409 Conflict response if email or phone number already exists.
    /// </returns>
    [HttpPut]
    public async Task<IActionResult> UpdatePublisherAsync([FromBody] PublisherDto publisher)
    {
        try
        {
            await _publisherService.UpdatePublisherAsync(publisher);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ConflictException ex)
        {
            return Conflict(ex.Message);
        }
    }

    /// <summary>
    /// Deletes a publisher by ID.
    /// </summary>
    /// <param name="id">The ID of the publisher to delete.</param>
    /// <returns>
    /// A 204 No Content response if deletion is successful;
    /// a 404 Not Found response if the publisher does not exist;
    /// or a 409 Conflict response if deletion is not possible due to dependencies (the publisher has association with at least one item).
    /// </returns>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePublisherAsync(int id)
    {
        try
        {
            await _publisherService.DeletePublisherAsync(id);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ConflictException ex)
        {
            return Conflict(ex.Message);
        }
    }
    
    /// <summary>
    /// Retrieves filtered publishers.
    /// </summary>
    /// <returns>A 200 OK response containing the list of filtered publishers.</returns>
    [HttpGet("search")]
    public async Task<IActionResult> GetSmallAllPublishersAsync([FromQuery] string searchTerm)
    {
        var publishers = await _publisherService.GetFilteredPublishersAsync(searchTerm);
        return Ok(publishers);
    }
    
}
