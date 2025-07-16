using BookShopServer.DTOs;
using BookShopServer.DTOs.UserDTOs;
using BookShopServer.Entities;
using BookShopServer.Exceptions;
using BookShopServer.Services;
using BookShopServer.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShopServer.Controllers;

/// <summary>
/// Controller for managing employee-related operations.
/// Provides endpoints for retrieving, creating, updating employees,
/// and managing the minimum salary policy.
/// </summary>
[ApiController]
[Route("api/users/employees")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    /// <summary>
    /// Initializes a new instance of the <c>EmployeesController</c> class.
    /// </summary>
    /// <param name="employeeService">Service for employee operations.</param>
    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    /// <summary>
    /// Retrieves an employee by ID.
    /// </summary>
    /// <param name="id">The ID of the employee.</param>
    /// <returns>
    /// A 200 OK response with the employee data;
    /// or 404 Not Found if the employee is not found.
    /// </returns>
    [Authorize(Roles = "Admin, Employee")]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetEmployeeById(int id)
    {
        try
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            return Ok(employee);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    /// Retrieves all employees.
    /// </summary>
    /// <returns>A 200 OK response with a list of all employees.</returns>
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllEmployees()
    {
        var employees = await _employeeService.GetAllEmployeesAsync();
        return Ok(employees);
    }

    /// <summary>
    /// Creates a new employee.
    /// </summary>
    /// <param name="employeeDto">DTO containing the employee data.</param>
    /// <returns>
    /// A 201 Created response on success;
    /// 400 Bad Request if the input is invalid (salary below minimum);
    /// or 409 Conflict if username or email already exists.
    /// </returns>
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddEmployee([FromBody] EmployeeDto employeeDto)
    {
        try
        {
            await _employeeService.AddEmployeeAsync(employeeDto);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
        catch (ConflictException e)
        {
            return Conflict(e.Message);
        }
    }

    /// <summary>
    /// Updates an existing employee.
    /// </summary>
    /// <param name="employeeDto">DTO containing the updated employee data.</param>
    /// <returns>
    /// A 204 No Content response on success;
    /// 404 Not Found if the employee does not exist;
    /// 400 Bad Request if the input is invalid (salary below minimum);
    /// or 409 Conflict if username or email already exists.
    /// </returns>
    [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeDto employeeDto)
    {
        try
        {
            await _employeeService.UpdateEmployeeAsync(employeeDto);
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
        catch (ConflictException e)
        {
            return Conflict(e.Message);
        }
    }

    /// <summary>
    /// Updates the minimum salary for employees.
    /// </summary>
    /// <param name="newMinimumSalary">The new minimum salary value.</param>
    /// <returns>
    /// A 204 No Content response on success;
    /// or 400 Bad Request if the value is invalid (if new salary is smaller than the current one).
    /// </returns>
    [Authorize(Roles = "Admin")]
    [HttpPut("minimum-salary")]
    public async Task<IActionResult> UpdateMinimumSalary([FromBody] double newMinimumSalary)
    {
        try
        {
            await _employeeService.UpdateMinimumSalary(newMinimumSalary);
            return NoContent();
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Retrieves the current minimum salary for employees.
    /// </summary>
    /// <returns>A 200 OK response with the minimum salary value.</returns>
    [Authorize(Roles = "Admin")]
    [HttpGet("minimum-salary")]
    public IActionResult GetMinimumSalary()
    {
        return Ok(Employee.MinSalary);
    }
}
