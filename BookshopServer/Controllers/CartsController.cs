using BookShopServer.DTOs.OrderDTOs.CartDTOs;
using BookShopServer.Exceptions;
using BookShopServer.Services;
using BookShopServer.Services.OrderServices;
using Microsoft.AspNetCore.Mvc;

namespace BookShopServer.Controllers;

/// <summary>
/// Controller for managing shopping carts and their items.
/// Provides endpoints for creating carts, adding, updating, and removing items.
/// </summary>
[ApiController]
[Route("api/carts")]
public class CartsController : ControllerBase
{
    private readonly ICartService _cartService;

    /// <summary>
    /// Initializes a new instance of the <c>CartsController</c> class.
    /// </summary>
    /// <param name="cartService">The service used for cart operations.</param>
    public CartsController(ICartService cartService)
    {
        _cartService = cartService;
    }

    /// <summary>
    /// Creates a new shopping cart.
    /// </summary>
    /// <param name="newCartDto">DTO containing data for the new cart.</param>
    /// <returns>
    /// A 201 Created response on success;
    /// a 400 Bad Request if the input is invalid (trying to add more items than available);
    /// or a 404 Not Found if related resources are missing.
    /// </returns>
    [HttpPost]
    public async Task<IActionResult> CreateCartAsync([FromBody] NewCartDto newCartDto)
    {
        try
        {
            await _cartService.CreateCartAsync(newCartDto);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (BadRequestException x)
        {
            return BadRequest(x.Message);
        }
        catch (NotFoundException x)
        {
            return NotFound(x.Message);
        }
    }

    /// <summary>
    /// Adds an item to an existing cart.
    /// </summary>
    /// <param name="addItemToCartDto">DTO with cart ID, item ID, and quantity to add.</param>
    /// <returns>
    /// A 201 Created response on success;
    /// a 400 Bad Request if the input is invalid (trying to add more items than available);
    /// or a 404 Not Found if the cart or item does not exist.
    /// </returns>
    [HttpPost("items")]
    public async Task<IActionResult> AddItemToCartAsync([FromBody] AddItemToCartDto addItemToCartDto)
    {
        try
        {
            await _cartService.AddItemToCartAsync(addItemToCartDto);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (BadRequestException x)
        {
            return BadRequest(x.Message);
        }
        catch (NotFoundException x)
        {
            return NotFound(x.Message);
        }
    }

    /// <summary>
    /// Removes a specific item from a cart.
    /// </summary>
    /// <param name="orderId">The ID of the cart.</param>
    /// <param name="itemId">The ID of the item to remove.</param>
    /// <returns>
    /// A 204 No Content response on success;
    /// or a 404 Not Found if the cart or item is not found.
    /// </returns>
    [HttpDelete("{orderId:int}/items/{itemId:int}")]
    public async Task<IActionResult> RemoveItemFromCartAsync(int orderId, int itemId)
    {
        try
        {
            await _cartService.RemoveItemFromCartAsync(orderId, itemId);
            return NoContent();
        }
        catch (NotFoundException x)
        {
            return NotFound(x.Message);
        }
    }

    /// <summary>
    /// Updates the quantity of an item in the cart.
    /// </summary>
    /// <param name="addItemToCartDto">DTO containing cart ID, item ID, and new quantity.</param>
    /// <returns>
    /// A 204 No Content response on success;
    /// a 400 Bad Request if the input is invalid (trying to set a quantity greater than available stock);
    /// or a 404 Not Found if the cart or item does not exist.
    /// </returns>
    [HttpPut("items")]
    public async Task<IActionResult> UpdateItemQuantityInCartAsync([FromBody] AddItemToCartDto addItemToCartDto)
    {
        try
        {
            await _cartService.UpdateItemQuantityInCartAsync(addItemToCartDto);
            return NoContent();
        }
        catch (BadRequestException x)
        {
            return BadRequest(x.Message);
        }
        catch (NotFoundException x)
        {
            return NotFound(x.Message);
        }
    }
}
