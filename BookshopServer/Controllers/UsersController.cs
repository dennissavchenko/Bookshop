using BookShopServer.DTOs;
using BookShopServer.DTOs.UserDTOs;
using BookShopServer.Exceptions;
using BookShopServer.Services;
using BookShopServer.Services.UserServices;
using Microsoft.AspNetCore.Mvc;

namespace BookShopServer.Controllers;

/// <summary>
/// Controller for managing users and their roles.
/// </summary>
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    /// <summary>
    /// Initializes a new instance of the <c>UsersController</c> class.
    /// </summary>
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Deletes a user by ID.
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    /// <returns>
    /// A 204 No Content response if deletion is successful;  
    /// a 404 Not Found response if the user does not exist.
    /// </returns>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Retrieves a user by ID.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <returns>
    /// A 200 OK response with the user data;  
    /// a 404 Not Found response if the user does not exist.
    /// </returns>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUserByIdAsync(int id)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Retrieves all users.
    /// </summary>
    /// <returns>
    /// A 200 OK response with the list of all users.
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    /// <summary>
    /// Assigns a customer role to a user.
    /// </summary>
    /// <param name="customerRoleDto">The customer role data.</param>
    /// <returns>
    /// A 201 Created response if successful;  
    /// a 400 Bad Request response if the input is invalid;  
    /// a 404 Not Found response if the user does not exist.
    /// </returns>
    [HttpPost("customer-role")]
    public async Task<IActionResult> AddCustomerRole([FromBody] CustomerRoleDto customerRoleDto)
    {
        try
        {
            await _userService.AddCustomerRoleAsync(customerRoleDto);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (BadRequestException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Assigns an employee role to a user.
    /// </summary>
    /// <param name="employeeRoleDto">The employee role data.</param>
    /// <returns>
    /// A 201 Created response if successful;  
    /// a 400 Bad Request response if the input is invalid;  
    /// a 404 Not Found response if the user does not exist.
    /// </returns>
    [HttpPost("employee-role")]
    public async Task<IActionResult> AddEmployeeRole([FromBody] EmployeeRoleDto employeeRoleDto)
    {
        try
        {
            await _userService.AddEmployeeRoleAsync(employeeRoleDto);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (BadRequestException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Removes the customer role from a user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>
    /// A 204 No Content response if successful;  
    /// a 404 Not Found response if the user or role does not exist;  
    /// a 409 Conflict response if the role cannot be removed due to related constraints (user has to have at least one role).
    /// </returns>
    [HttpDelete("{userId:int}/customer-role")]
    public async Task<IActionResult> DeleteCustomerRole(int userId)
    {
        try
        {
            await _userService.DeleteCustomerRoleAsync(userId);
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
    /// Removes the employee role from a user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>
    /// A 204 No Content response if successful;  
    /// a 404 Not Found response if the user or role does not exist;  
    /// a 409 Conflict response if the role cannot be removed due to related constraints (user has to have at least one role).
    /// </returns>
    [HttpDelete("{userId:int}/employee-role")]
    public async Task<IActionResult> DeleteEmployeeRole(int userId)
    {
        try
        {
            await _userService.DeleteEmployeeRoleAsync(userId);
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
}
