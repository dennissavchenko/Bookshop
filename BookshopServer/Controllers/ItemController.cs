using BookShopServer.Exceptions;
using BookShopServer.Services;
using BookShopServer.Services.ItemServices;
using Microsoft.AspNetCore.Mvc;

namespace BookShopServer.Controllers;

/// <summary>
/// Controller for managing general item-related operations.
/// Provides endpoints for retrieving items, their reviews, and related orders.
/// </summary>
[ApiController]
[Route("api/items")]
public class ItemController : ControllerBase
{
    private readonly IItemService _itemService;
    private readonly IReviewService _reviewService;

    /// <summary>
    /// Initializes a new instance of the <c>ItemController</c> class.
    /// </summary>
    /// <param name="itemService">Service for item-related operations.</param>
    /// <param name="reviewService">Service for review-related operations.</param>
    public ItemController(IItemService itemService, IReviewService reviewService)
    {
        _itemService = itemService;
        _reviewService = reviewService;
    }

    /// <summary>
    /// Retrieves an item by its ID.
    /// </summary>
    /// <param name="id">The ID of the item.</param>
    /// <returns>
    /// A 200 OK response with the item data;
    /// or a 404 Not Found response if the item is not found.
    /// </returns>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetItemByIdAsync(int id)
    {
        try
        {
            var item = await _itemService.GetItemByIdAsync(id);
            return Ok(item);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    /// Retrieves all items.
    /// </summary>
    /// <returns>
    /// A 200 OK response containing the list of all items.
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> GetAllItemsAsync()
    {
        var items = await _itemService.GetAllItemsAsync();
        return Ok(items);
    }

    /// <summary>
    /// Retrieves all orders that include a specific item.
    /// </summary>
    /// <param name="id">The ID of the item.</param>
    /// <returns>
    /// A 200 OK response with the list of related orders;
    /// or a 404 Not Found response if the item is not found.
    /// </returns>
    [HttpGet("{id:int}/orders")]
    public async Task<IActionResult> GetOrdersByItemIdAsync(int id)
    {
        try
        {
            var orders = await _itemService.GetOrdersByItemIdAsync(id);
            return Ok(orders);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    /// Retrieves all items by a specific publisher.
    /// </summary>
    /// <param name="publisherId">The ID of the publisher.</param>
    /// <returns>
    /// A 200 OK response with the list of items;
    /// or a 404 Not Found response if the publisher is not found.
    /// </returns>
    [HttpGet("publisher/{publisherId:int}")]
    public async Task<IActionResult> GetItemsByPublisherAsync(int publisherId)
    {
        try
        {
            var items = await _itemService.GetItemsByPublisherIdAsync(publisherId);
            return Ok(items);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpGet("age/{age:int}")]
    public async Task<IActionResult> GetItemsAppropriateForAgeAsync(int age)
    {
        try
        {
            var items = await _itemService.GetItemsAppropriateForAgeAsync(age);
            return Ok(items);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Retrieves all items by a specific age category.
    /// </summary>
    /// <param name="ageCategoryId">The ID of the age category.</param>
    /// <returns>
    /// A 200 OK response with the list of items;
    /// or a 404 Not Found response if the category is not found.
    /// </returns>
    [HttpGet("age-category/{ageCategoryId:int}")]
    public async Task<IActionResult> GetItemsByAgeCategoryAsync(int ageCategoryId)
    {
        try
        {
            var items = await _itemService.GetItemsByAgeCategoryIdAsync(ageCategoryId);
            return Ok(items);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    /// Retrieves all reviews for a specific item.
    /// </summary>
    /// <param name="id">The ID of the item.</param>
    /// <returns>
    /// A 200 OK response with the list of reviews;
    /// or a 404 Not Found response if the item is not found.
    /// </returns>
    [HttpGet("{id:int}/reviews")]
    public async Task<IActionResult> GetReviewsByItemIdAsync(int id)
    {
        try
        {
            var reviews = await _reviewService.GetReviewsByItemIdAsync(id);
            return Ok(reviews);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    /// Deletes an item by ID.
    /// </summary>
    /// <param name="id">The ID of the item to delete.</param>
    /// <returns>
    /// A 204 No Content response if the deletion is successful;
    /// or a 404 Not Found response if the item does not exist.
    /// </returns>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteItemAsync(int id)
    {
        try
        {
            await _itemService.DeleteItemAsync(id);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
}
