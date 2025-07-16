using BookShopServer.Entities;

namespace BookShopServer.Repositories.UserRepositories;

/// <summary>
/// Defines the contract for data access operations related to employee entities.
/// </summary>
public interface IEmployeeRepository
{
    /// <summary>
    /// Retrieves an employee by their unique identifier.
    /// </summary>
    /// <param name="id">The ID of the employee to retrieve.</param>
    /// <returns>The employee entity if found, otherwise <c>null</c>.</returns>
    Task<Employee?> GetEmployeeByIdAsync(int id);

    /// <summary>
    /// Retrieves all employee entities.
    /// </summary>
    /// <returns>A collection of all employees.</returns>
    Task<IEnumerable<Employee>> GetAllEmployeesAsync();

    /// <summary>
    /// Adds a new employee to the database.
    /// </summary>
    /// <param name="employee">The employee entity to add.</param>
    Task AddEmployeeAsync(Employee employee);

    /// <summary>
    /// Updates an existing employee in the database.
    /// </summary>
    /// <param name="employee">The employee entity with updated information.</param>
    Task UpdateEmployeeAsync(Employee employee);

    /// <summary>
    /// Checks if an employee with the specified ID exists.
    /// </summary>
    /// <param name="id">The ID of the employee to check.</param>
    /// <returns><c>true</c> if the employee exists; otherwise, <c>false</c>.</returns>
    Task<bool> EmployeeExistsAsync(int id);

    /// <summary>
    /// Deletes an employee from the database by their ID.
    /// </summary>
    /// <param name="id">The ID of the employee to delete.</param>
    Task DeleteEmployeeAsync(int id);
}