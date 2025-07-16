using BookShopServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookShopServer.Repositories.UserRepositories;

/// <summary>
/// Provides data access methods for managing employee entities in the bookshop system.
/// </summary>
public class EmployeeRepository : IEmployeeRepository
{
    private readonly Context _context;
    
    /// <summary>
    /// Initializes a new instance of the <c>EmployeeRepository</c> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public EmployeeRepository(Context context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Retrieves an employee by their unique identifier, including their user information.
    /// </summary>
    /// <param name="id">The ID of the employee to retrieve.</param>
    /// <returns>The employee entity if found.</returns>
    public async Task<Employee?> GetEmployeeByIdAsync(int id)
    {
        return await _context.Employees
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.UserId == id);
    }
    
    /// <summary>
    /// Retrieves all employee entities, including their associated user information.
    /// </summary>
    /// <returns>A collection of all employees.</returns>
    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
    {
        return await _context.Employees
            .Include(x => x.User)
            .ToListAsync();
    }
    
    /// <summary>
    /// Adds a new employee to the database.
    /// </summary>
    /// <param name="employee">The employee entity to add.</param>
    public async Task AddEmployeeAsync(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();
    }
    
    /// <summary>
    /// Updates an existing employee in the database.
    /// </summary>
    /// <param name="employee">The employee entity with updated information.</param>
    public async Task UpdateEmployeeAsync(Employee employee)
    {
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();
    }
    
    /// <summary>
    /// Checks if an employee with the specified ID exists.
    /// </summary>
    /// <param name="id">The ID of the employee to check.</param>
    /// <returns><c>true</c> if the employee exists; otherwise, <c>false</c>.</returns>
    public async Task<bool> EmployeeExistsAsync(int id)
    {
        return await _context.Employees.AnyAsync(x => x.UserId == id);
    }
    
    /// <summary>
    /// Deletes an employee from the database by their ID.
    /// </summary>
    /// <param name="id">The ID of the employee to delete.</param>
    public async Task DeleteEmployeeAsync(int id)
    {
        var employee = await _context.Employees.FirstAsync(x => x.UserId == id);
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
    }
    
}