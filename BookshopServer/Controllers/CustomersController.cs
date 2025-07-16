using BookShopServer.DTOs.UserDTOs;
using BookShopServer.Exceptions;
using BookShopServer.Services;
using BookShopServer.Services.OrderServices;
using BookShopServer.Services.UserServices;
using Microsoft.AspNetCore.Mvc;

namespace BookShopServer.Controllers;

/// <summary>
/// Controller for managing customer-related operations.
/// Provides endpoints for retrieving, creating, and updating customers,
/// as well as accessing their orders, reviews, and shopping carts.
/// </summary>
[ApiController]
[Route("api/users/customers")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly IOrderService _orderService;
    private readonly ICartService _cartService;
    private readonly IReviewService _reviewService;

    /// <summary>
    /// Initializes a new instance of the <c>CustomersController</c> class.
    /// </summary>
    /// <param name="customerService">Service for customer operations.</param>
    /// <param name="orderService">Service for order operations.</param>
    /// <param name="cartService">Service for cart operations.</param>
    /// <param name="reviewService">Service for review operations.</param>
    public CustomersController(ICustomerService customerService, IOrderService orderService, ICartService cartService, IReviewService reviewService)
    {
        _orderService = orderService;
        _customerService = customerService;
        _cartService = cartService;
        _reviewService = reviewService;
    }

    /// <summary>
    /// Retrieves a customer by ID.
    /// </summary>
    /// <param name="id">The ID of the customer.</param>
    /// <returns>
    /// A 200 OK response with the customer data;
    /// or 404 Not Found if not found.
    /// </returns>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCustomerByIdAsync(int id)
    {
        try
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            return Ok(customer);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    /// Retrieves all orders placed by a specific customer.
    /// </summary>
    /// <param name="customerId">The ID of the customer.</param>
    /// <returns>
    /// A 200 OK response with a list of orders;
    /// or 404 Not Found if customer is not found.
    /// </returns>
    [HttpGet("{customerId:int}/orders")]
    public async Task<IActionResult> GetOrdersByCustomerIdAsync(int customerId)
    {
        try
        {
            var orders = await _orderService.GetOrdersByCustomerIdOrderedByLastUpdateAsync(customerId);
            return Ok(orders);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    /// Retrieves all reviews submitted by a specific customer.
    /// </summary>
    /// <param name="customerId">The ID of the customer.</param>
    /// <returns>
    /// A 200 OK response with a list of reviews;
    /// or 404 Not Found if customer is not found.
    /// </returns>
    [HttpGet("{customerId:int}/reviews")]
    public async Task<IActionResult> GetReviewsByCustomerIdAsync(int customerId)
    {
        try
        {
            var reviews = await _reviewService.GetReviewsByCustomerIdAsync(customerId);
            return Ok(reviews);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    /// Retrieves the shopping cart of a specific customer.
    /// </summary>
    /// <param name="customerId">The ID of the customer.</param>
    /// <returns>
    /// A 200 OK response with the cart;
    /// or 404 Not Found if customer or cart is not found.
    /// </returns>
    [HttpGet("{customerId:int}/cart")]
    public async Task<IActionResult> GetCartByCustomerIdAsync(int customerId)
    {
        try
        {
            var cart = await _cartService.GetCartByCustomerIdAsync(customerId);
            return Ok(cart);
        }
        catch (NotFoundException x)
        {
            return NotFound(x.Message);
        }
    }

    /// <summary>
    /// Retrieves all customers.
    /// </summary>
    /// <returns>
    /// A 200 OK response with a list of all customers.
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> GetAllCustomersAsync()
    {
        var customers = await _customerService.GetAllCustomersAsync();
        return Ok(customers);
    }

    /// <summary>
    /// Creates a new customer.
    /// </summary>
    /// <param name="customer">The customer data to create.</param>
    /// <returns>
    /// A 201 Created response on success;
    /// 409 Conflict if username or email already exists;
    /// or 400 Bad Request if the data is invalid (DOB in the future or system reserved username).
    /// </returns>
    [HttpPost]
    public async Task<IActionResult> AddCustomerAsync([FromBody] CustomerDto customer)
    {
        try
        {
            await _customerService.AddCustomerAsync(customer);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (ConflictException e)
        {
            return Conflict(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Updates an existing customer.
    /// </summary>
    /// <param name="customer">The updated customer data.</param>
    /// <returns>
    /// A 204 No Content response on success;
    /// 404 Not Found if the customer does not exist;
    /// 409 Conflict if username or email already exists;
    /// or 400 Bad Request if the input is invalid (DOB in the future or system reserved username).
    /// </returns>
    [HttpPut]
    public async Task<IActionResult> UpdateCustomer([FromBody] CustomerDto customer)
    {
        try
        {
            await _customerService.UpdateCustomerAsync(customer);
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
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Checks if a customer has received a specific item.
    /// </summary>
    /// <param name="id">The ID of the customer.</param>
    /// <param name="itemId">The ID of the item.</param>
    /// <returns>
    /// A 200 OK response with a boolean indicating receipt;
    /// or 404 Not Found if the customer or item is not found.
    /// </returns>
    [HttpGet("{id:int}/received-item/{itemId:int}")]
    public async Task<IActionResult> CustomerReceivedItem(int id, int itemId)
    {
        try
        {
            var received = await _customerService.CustomerReceivedItemAsync(id, itemId);
            return Ok(received);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}
