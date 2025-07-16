using BookShopServer.DTOs.UserDTOs;

namespace BookShopServer.Services.UserServices;

/// <summary>
/// Defines the contract for services that manage employee-related operations.
/// </summary>
public interface IEmployeeService
{
    /// <summary>
    /// Retrieves an employee by their unique identifier.
    /// </summary>
    /// <param name="id">The ID of the employee to retrieve.</param>
    /// <returns>An <see cref="EmployeeDto"/> representing the employee.</returns>
    Task<EmployeeDto> GetEmployeeByIdAsync(int id);

    /// <summary>
    /// Retrieves a collection of all employees.
    /// </summary>
    /// <returns>A collection of <see cref="EmployeeDto"/> representing all employees.</returns>
    Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();

    /// <summary>
    /// Adds a new employee to the system.
    /// </summary>
    /// <param name="employeeDto">The <see cref="EmployeeDto"/> containing the details of the employee to add.</param>
    Task AddEmployeeAsync(EmployeeDto employeeDto);

    /// <summary>
    /// Updates an existing employee's details.
    /// </summary>
    /// <param name="employeeDto">The <see cref="EmployeeDto"/> containing the updated details of the employee.</param>
    Task UpdateEmployeeAsync(EmployeeDto employeeDto);

    /// <summary>
    /// Updates the minimum salary requirement for employees.
    /// </summary>
    /// <param name="newMinimumSalary">The new minimum salary to set.</param>
    Task UpdateMinimumSalary(double newMinimumSalary);
}