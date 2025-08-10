using BookShopServer.DTOs;
using BookShopServer.DTOs.ItemDTOs.AddOrUpdateItem;
using BookShopServer.Exceptions;
using BookShopServer.Services;
using BookShopServer.Services.ItemServices.ItemCondition;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShopServer.Controllers;

/// <summary>
/// Controller for managing used items (books, magazines, newspapers).
/// </summary>
[ApiController]
[Route("api/items/used")]
public class UsedItemsController : ControllerBase
{
    private readonly IUsedItemService _usedItemService;

    /// <summary>
    /// Initializes a new instance of the <c>UsedItemsController</c> class.
    /// </summary>
    public UsedItemsController(IUsedItemService usedItemService)
    {
        _usedItemService = usedItemService;
    }

    /// <summary>
    /// Retrieves all used items.
    /// </summary>
    /// <returns>
    /// A 200 OK response with the list of used items;
    /// or a 404 Not Found response if none exist.
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> GetAllUsedItemsAsync()
    {
        try
        {
            var usedItems = await _usedItemService.GetAllUsedItemsAsync();
            return Ok(usedItems);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    /// Adds a generic used item.
    /// </summary>
    /// <returns>
    /// A 201 Created response if successful;
    /// a 400 Bad Request response if the input is invalid;
    /// or a 404 Not Found response if a related entity does not exist.
    /// </returns>
    [HttpPost]
    [Authorize(Roles = "Admin, Employee")]
    public async Task<IActionResult> AddUsedItemAsync([FromBody] UsedItemDto usedItemDto)
    {
        try
        {
            await _usedItemService.AddUsedItemAsync(usedItemDto);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Adds a used book.
    /// </summary>
    /// <returns>
    /// A 201 Created response if successful;
    /// a 400 Bad Request response if the input is invalid;
    /// or a 404 Not Found response if a related entity does not exist.
    /// </returns>
    [HttpPost("book")]
    [Authorize(Roles = "Admin, Employee")]
    public async Task<IActionResult> AddUsedBookAsync([FromBody] UsedBookDto usedBookDto)
    {
        try
        {
            await _usedItemService.AddUsedBookAsync(usedBookDto);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Adds a used magazine.
    /// </summary>
    /// <returns>
    /// A 201 Created response if successful;
    /// a 400 Bad Request response if the input is invalid;
    /// or a 404 Not Found response if a related entity does not exist.
    /// </returns>
    [HttpPost("magazine")]
    [Authorize(Roles = "Admin, Employee")]
    public async Task<IActionResult> AddUsedMagazineAsync([FromBody] UsedMagazineDto usedMagazineDto)
    {
        try
        {
            await _usedItemService.AddUsedMagazineAsync(usedMagazineDto);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Adds a used newspaper.
    /// </summary>
    /// <returns>
    /// A 201 Created response if successful;
    /// a 400 Bad Request response if the input is invalid;
    /// or a 404 Not Found response if a related entity does not exist.
    /// </returns>
    [HttpPost("newspaper")]
    [Authorize(Roles = "Admin, Employee")]
    public async Task<IActionResult> AddUsedNewspaperAsync([FromBody] UsedNewspaperDto usedNewspaperDto)
    {
        try
        {
            await _usedItemService.AddUsedNewspaperAsync(usedNewspaperDto);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Updates a generic used item.
    /// </summary>
    /// <returns>
    /// A 204 No Content response if successful;
    /// a 400 Bad Request response if the input is invalid;
    /// or a 404 Not Found response if the item does not exist.
    /// </returns>
    [HttpPut]
    [Authorize(Roles = "Admin, Employee")]
    public async Task<IActionResult> UpdateUsedItemAsync([FromBody] UsedItemDto usedItemDto)
    {
        try
        {
            await _usedItemService.UpdateUsedItemAsync(usedItemDto);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Updates a used book.
    /// </summary>
    /// <returns>
    /// A 204 No Content response if successful;
    /// a 400 Bad Request response if the input is invalid;
    /// or a 404 Not Found response if the book does not exist.
    /// </returns>
    [HttpPut("book")]
    [Authorize(Roles = "Admin, Employee")]
    public async Task<IActionResult> UpdateUsedBookAsync([FromBody] UpdateUsedBookDto usedBookDto)
    {
        try
        {
            await _usedItemService.UpdateUsedBookAsync(usedBookDto);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Updates a used magazine.
    /// </summary>
    /// <returns>
    /// A 204 No Content response if successful;
    /// a 400 Bad Request response if the input is invalid;
    /// or a 404 Not Found response if the magazine does not exist.
    /// </returns>
    [HttpPut("magazine")]
    [Authorize(Roles = "Admin, Employee")]
    public async Task<IActionResult> UpdateUsedMagazineAsync([FromBody] UsedMagazineDto usedMagazineDto)
    {
        try
        {
            await _usedItemService.UpdateUsedMagazineAsync(usedMagazineDto);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Updates a used newspaper.
    /// </summary>
    /// <returns>
    /// A 204 No Content response if successful;
    /// a 400 Bad Request response if the input is invalid;
    /// or a 404 Not Found response if the newspaper does not exist.
    /// </returns>
    [HttpPut("newspaper")]
    [Authorize(Roles = "Admin, Employee")]
    public async Task<IActionResult> UpdateUsedNewspaperAsync([FromBody] UsedNewspaperDto usedNewspaperDto)
    {
        try
        {
            await _usedItemService.UpdateUsedNewspaperAsync(usedNewspaperDto);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }
}
