using BookShopServer.DTOs;
using BookShopServer.DTOs.ItemDTOs.AddOrUpdateItem;
using BookShopServer.Exceptions;
using BookShopServer.Services;
using BookShopServer.Services.ItemServices.ItemCondition;
using Microsoft.AspNetCore.Mvc;

namespace BookShopServer.Controllers;

/// <summary>
/// Controller for managing the creation and updates of new items (books, magazines, newspapers).
/// </summary>
[ApiController]
[Route("api/items/new")]
public class NewItemsController : ControllerBase
{
    private readonly INewItemService _newItemService;

    /// <summary>
    /// Initializes a new instance of the <c>NewItemsController</c> class.
    /// </summary>
    /// <param name="newItemService">Service for new item operations.</param>
    public NewItemsController(INewItemService newItemService)
    {
        _newItemService = newItemService;
    }

    /// <summary>
    /// Retrieves all new items.
    /// </summary>
    /// <returns>
    /// A 200 OK response with the list of new items;
    /// or a 404 Not Found response if none exist.
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> GetAllNewItemsAsync()
    {
        try
        {
            var newItems = await _newItemService.GetAllNewItemsAsync();
            return Ok(newItems);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    /// Adds a new generic item.
    /// </summary>
    /// <param name="newItemDto">The item data.</param>
    /// <returns>
    /// A 201 Created response if successful;
    /// 404 Not Found if referenced resources (e.g., publisher) are missing;
    /// 400 Bad Request if validation fails.
    /// </returns>
    [HttpPost]
    public async Task<IActionResult> AddNewItemAsync([FromBody] NewItemDto newItemDto)
    {
        try
        {
            await _newItemService.AddNewItemAsync(newItemDto);
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
    /// Updates a generic item.
    /// </summary>
    /// <param name="newItemDto">The updated item data.</param>
    /// <returns>
    /// A 204 No Content response if successful;
    /// 404 Not Found if the item does not exist;
    /// 400 Bad Request if validation fails.
    /// </returns>
    [HttpPut]
    public async Task<IActionResult> UpdateNewItemAsync([FromBody] NewItemDto newItemDto)
    {
        try
        {
            await _newItemService.UpdateNewItemAsync(newItemDto);
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
    /// Adds a new book.
    /// </summary>
    /// <param name="newBookDto">The new book data.</param>
    /// <returns>
    /// A 201 Created response if successful;
    /// 404 Not Found if required references are missing;
    /// 400 Bad Request if validation fails.
    /// </returns>
    [HttpPost("book")]
    public async Task<IActionResult> AddNewBookAsync([FromBody] NewBookDto newBookDto)
    {
        try
        {
            await _newItemService.AddNewBookAsync(newBookDto);
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
    /// Adds a new magazine.
    /// </summary>
    /// <param name="newMagazineDto">The new magazine data.</param>
    /// <returns>
    /// A 201 Created response if successful;
    /// 404 Not Found if required references are missing;
    /// 400 Bad Request if validation fails.
    /// </returns>
    [HttpPost("magazine")]
    public async Task<IActionResult> AddNewMagazineAsync([FromBody] NewMagazineDto newMagazineDto)
    {
        try
        {
            await _newItemService.AddNewMagazineAsync(newMagazineDto);
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
    /// Adds a new newspaper.
    /// </summary>
    /// <param name="newNewspaperDto">The new newspaper data.</param>
    /// <returns>
    /// A 201 Created response if successful;
    /// 404 Not Found if required references are missing;
    /// 400 Bad Request if validation fails.
    /// </returns>
    [HttpPost("newspaper")]
    public async Task<IActionResult> AddNewNewspaperAsync([FromBody] NewNewspaperDto newNewspaperDto)
    {
        try
        {
            await _newItemService.AddNewNewspaperAsync(newNewspaperDto);
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
    /// Updates an existing book.
    /// </summary>
    /// <param name="newBookDto">The updated book data.</param>
    /// <returns>
    /// A 204 No Content response if successful;
    /// 404 Not Found if the book does not exist;
    /// 400 Bad Request if validation fails.
    /// </returns>
    [HttpPut("book")]
    public async Task<IActionResult> UpdateNewBookAsync([FromBody] UpdateNewBookDto newBookDto)
    {
        try
        {
            await _newItemService.UpdateNewBookAsync(newBookDto);
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
    /// Updates an existing magazine.
    /// </summary>
    /// <param name="newMagazineDto">The updated magazine data.</param>
    /// <returns>
    /// A 204 No Content response if successful;
    /// 404 Not Found if the magazine does not exist;
    /// 400 Bad Request if validation fails.
    /// </returns>
    [HttpPut("magazine")]
    public async Task<IActionResult> UpdateNewMagazineAsync([FromBody] NewMagazineDto newMagazineDto)
    {
        try
        {
            await _newItemService.UpdateNewMagazineAsync(newMagazineDto);
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
    /// Updates an existing newspaper.
    /// </summary>
    /// <param name="newNewspaperDto">The updated newspaper data.</param>
    /// <returns>
    /// A 204 No Content response if successful;
    /// 404 Not Found if the newspaper does not exist;
    /// 400 Bad Request if validation fails.
    /// </returns>
    [HttpPut("newspaper")]
    public async Task<IActionResult> UpdateNewNewspaperAsync([FromBody] NewNewspaperDto newNewspaperDto)
    {
        try
        {
            await _newItemService.UpdateNewNewspaperAsync(newNewspaperDto);
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
