using BookShopServer.Entities;
using BookShopServer.Exceptions;
using BookShopServer.Services;
using BookShopServer.Services.OrderServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShopServer.Controllers;

/// <summary>
/// Controller for managing customer orders.
/// Provides endpoints for retrieving, updating, and changing the status of orders.
/// </summary>
[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    /// <summary>
    /// Initializes a new instance of the <c>OrdersController</c> class.
    /// </summary>
    /// <param name="orderService">Service for order-related operations.</param>
    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Retrieves an order by its ID.
    /// </summary>
    /// <param name="id">The ID of the order.</param>
    /// <returns>
    /// A 200 OK response with the order data;
    /// or 404 Not Found if the order does not exist.
    /// </returns>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOrderByIdAsync(int id)
    {
        try
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            return Ok(order);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    /// Checks out an order by ID.
    /// </summary>
    /// <param name="id">The ID of the order to check out.</param>
    /// <returns>
    /// A 204 No Content response on success;
    /// 400 Bad Request if the order is not valid for checkout;
    /// or 404 Not Found if the order does not exist.
    /// </returns>
    [HttpPost("{id:int}/checkout")]
    public async Task<IActionResult> CheckoutOrderAsync(int id)
    {
        try
        {
            await _orderService.CheckoutOrderAsync(id);
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
    /// Confirms an order and registers a payment type.
    /// </summary>
    /// <param name="id">The ID of the order.</param>
    /// <param name="paymentType">The selected payment type.</param>
    /// <returns>
    /// A 204 No Content response if confirmation succeeds;
    /// 400 Bad Request if confirmation is not allowed;
    /// or 404 Not Found if the order does not exist.
    /// </returns>
    [HttpPost("{id:int}/confirm")]
    public async Task<IActionResult> ConfirmOrderAsync(int id, [FromQuery] PaymentType paymentType)
    {
        try
        {
            await _orderService.ConfirmOrderAsync(id, paymentType);
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
    /// Cancels an existing order.
    /// </summary>
    /// <param name="id">The ID of the order to cancel.</param>
    /// <returns>
    /// A 204 No Content response if cancellation succeeds;
    /// 400 Bad Request if the order cannot be cancelled;
    /// or 404 Not Found if the order does not exist.
    /// </returns>
    [HttpPost("{id:int}/cancel")]
    public async Task<IActionResult> CancelOrderAsync(int id)
    {
        try
        {
            await _orderService.CancelOrderAsync(id);
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
    /// Changes the status of an existing order.
    /// </summary>
    /// <param name="id">The ID of the order.</param>
    /// <param name="orderStatus">The new status to apply.</param>
    /// <returns>
    /// A 204 No Content response if the status is updated successfully;
    /// 400 Bad Request if the status change is not valid;
    /// or 404 Not Found if the order does not exist.
    /// </returns>
    [HttpPost("{id:int}/status")]
    [Authorize(Roles = "Admin, Employee")]
    public async Task<IActionResult> ChangeOrderStatusAsync(int id, [FromQuery] OrderStatus orderStatus)
    {
        try
        {
            await _orderService.ChangeStateAsync(id, orderStatus);
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
    /// Retrieves all orders with a specific status.
    /// </summary>
    /// <param name="orderStatus">The status to filter by.</param>
    /// <returns>A 200 OK response with the list of matching orders.</returns>
    [HttpGet("status/{orderStatus:int}")]
    public async Task<IActionResult> GetOrdersByStatusAsync(int orderStatus)
    {
        try
        {
            var orders = await _orderService.GetAllOrdersOfStatusAsync((OrderStatus) orderStatus);
            return Ok(orders);
        } catch (Exception e)
        {
            return BadRequest("Orders status code does not exist or is invalid: " + e.Message);
        }
    }

    /// <summary>
    /// Retrieves all items in a specific order.
    /// </summary>
    /// <param name="id">The ID of the order.</param>
    /// <returns>
    /// A 200 OK response with the list of items;
    /// or 404 Not Found if the order does not exist.
    /// </returns>
    [HttpGet("{id:int}/items")]
    public async Task<IActionResult> GetItemsInOrderAsync(int id)
    {
        try
        {
            var items = await _orderService.GetItemsInOrderAsync(id);
            return Ok(items);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    /// Retrieves all orders.
    /// </summary>
    /// <returns>A 200 OK response with the list of all orders.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllOrdersAsync()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return Ok(orders);
    }
}
