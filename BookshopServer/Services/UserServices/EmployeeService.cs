using BookShopServer.DTOs.UserDTOs;
using BookShopServer.Entities;
using BookShopServer.Exceptions;
using BookShopServer.Repositories.UserRepositories;

namespace BookShopServer.Services.UserServices;

/// <summary>
/// Provides services for managing employee-related operations, including retrieving,
/// adding, updating employee details, and managing the minimum salary.
/// </summary>
public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUserRepository _userRepository;
    
    /// <summary>
    /// Initializes a new instance of the <c>EmployeeService</c> class.
    /// </summary>
    /// <param name="employeeRepository">The repository for employee-specific data.</param>
    /// <param name="userRepository">The repository for general user data.</param>
    public EmployeeService(IEmployeeRepository employeeRepository, IUserRepository userRepository)
    {
        _employeeRepository = employeeRepository;
        _userRepository = userRepository;
    }
    
    /// <summary>
    /// Retrieves an employee by their unique identifier and maps it to an <see cref="EmployeeDto"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the employee.</param>
    /// <returns>An <see cref="EmployeeDto"/> representing the employee.</returns>
    /// <exception cref="NotFoundException">Thrown if the employee with the given ID does not exist.</exception>
    public async Task<EmployeeDto> GetEmployeeByIdAsync(int id)
    {
        // Retrieve the employee entity from the repository
        var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
        
        // Check if the employee exists
        if (employee == null)
            throw new NotFoundException("Employee with the given ID does not exist.");

        // Map the employee entity and its associated user data to an EmployeeDto.
        return new EmployeeDto
        {
            Id = employee.UserId,
            Name = employee.User.Name,
            Surname = employee.User.Surname,
            Email = employee.User.Email,
            Username = employee.User.Username,
            Salary = employee.Salary,
            Experience = employee.Experience
        };
    }
    
    /// <summary>
    /// Retrieves a collection of all employees and maps them to <see cref="EmployeeDto"/>s.
    /// </summary>
    /// <returns>A collection of <see cref="EmployeeDto"/> representing all employees.</returns>
    public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
    {
        // Retrieve all employee entities from the repository
        var employees = await _employeeRepository.GetAllEmployeesAsync();

        // Map each employee entity and its associated user data to an EmployeeDto.
        return employees.Select(e => new EmployeeDto
        {
            Id = e.UserId,
            Name = e.User.Name,
            Surname = e.User.Surname,
            Email = e.User.Email,
            Username = e.User.Username,
            Salary = e.Salary,
            Experience = e.Experience
        });
    }
    
    /// <summary>
    /// Adds a new employee to the system. Performs validation for unique email, username,
    /// reserved usernames, and minimum salary.
    /// </summary>
    /// <param name="employeeDto">The DTO containing the details of the employee to add.</param>
    /// <exception cref="ConflictException">Thrown if a user with the provided email or username already exists.</exception>
    /// <exception cref="BadRequestException">Thrown if the username is 'DeletedUser' or the salary is below the minimum.</exception>
    public async Task AddEmployeeAsync(EmployeeDto employeeDto)
    {
        // Validate uniqueness of username and email
        if (!await _userRepository.UsernameUniqueAsync(employeeDto.Username))
            throw new ConflictException("Username already exists.");
        if (!await _userRepository.EmailUniqueAsync(employeeDto.Email))
            throw new ConflictException("Email already exists.");

        // Prevent use of reserved username
        if (employeeDto.Username.ToLower() == "deleted_user")
            throw new BadRequestException("Username 'deleted_user' is reserved and cannot be used.");

        // Validate minimum salary requirement
        if (employeeDto.Salary < Employee.MinSalary)
            throw new BadRequestException($"Salary cannot be less than {Employee.MinSalary}.");
        
        // Create a new User entity from the DTO
        var user = new User
        {
            Name = employeeDto.Name,
            Surname = employeeDto.Surname,
            Email = employeeDto.Email,
            Username = employeeDto.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(employeeDto.Password)
        };
        
        // Create a new Employee entity and link it to the User
        var employee = new Employee
        {
            Salary = employeeDto.Salary,
            Experience = employeeDto.Experience,
            User = user
        };
        
        // Add the new employee (which includes the user) to the repository
        await _employeeRepository.AddEmployeeAsync(employee);
    }
    
    /// <summary>
    /// Updates an existing employee's details. Performs validation for employee existence,
    /// unique email and username (if changed), reserved usernames, and minimum salary.
    /// </summary>
    /// <param name="employeeDto">The DTO containing the updated details of the employee.</param>
    /// <exception cref="NotFoundException">Thrown if the employee with the given ID does not exist.</exception>
    /// <exception cref="ConflictException">Thrown if the updated email or username conflicts with an existing user.</exception>
    /// <exception cref="BadRequestException">Thrown if the username is 'DeletedUser' or the salary is below the minimum.</exception>
    public async Task UpdateEmployeeAsync(EmployeeDto employeeDto)
    {
        // Retrieve the existing employee entity to compare current values
        var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeDto.Id);
        
        // Validate employee existence
        if (employee == null)
            throw new NotFoundException("Employee with the given ID does not exist.");

        // Validate uniqueness of username if it has changed
        if (!await _userRepository.UsernameUniqueAsync(employeeDto.Username) && employee.User.Username != employeeDto.Username)
            throw new ConflictException("Username already exists.");
        
        // Validate uniqueness of email if it has changed
        if (!await _userRepository.EmailUniqueAsync(employeeDto.Email) && employee.User.Email != employeeDto.Email)
            throw new ConflictException("Email already exists.");
        
        // Prevent use of reserved username
        if (employeeDto.Username.ToLower() == "deleted_user")
            throw new BadRequestException("Username 'deleted_user' is reserved and cannot be used.");
        
        // Validate minimum salary requirement
        if (employeeDto.Salary < Employee.MinSalary)
            throw new BadRequestException($"Salary cannot be less than {Employee.MinSalary}.");

        // Update User entity properties associated with the employee
        employee.User.Name = employeeDto.Name;
        employee.User.Surname = employeeDto.Surname;
        employee.User.Email = employeeDto.Email;
        employee.User.Username = employeeDto.Username;
        employee.User.Password = BCrypt.Net.BCrypt.HashPassword(employeeDto.Password);
        
        // Update Employee-specific properties
        employee.Salary = employeeDto.Salary;
        employee.Experience = employeeDto.Experience;

        // Update the employee (which cascades to the user) in the repository
        await _employeeRepository.UpdateEmployeeAsync(employee);
    }

    /// <summary>
    /// Updates the global minimum salary for employees. If an existing employee's salary
    /// falls below the new minimum, their salary will be adjusted up to the new minimum.
    /// </summary>
    /// <param name="newMinimumSalary">The new minimum salary to set.</param>
    /// <exception cref="BadRequestException">Thrown if the new minimum salary is less than the current minimum salary.</exception>
    public async Task UpdateMinimumSalary(double newMinimumSalary)
    {
        // Prevent setting a minimum salary lower than the current minimum
        if(newMinimumSalary < Employee.MinSalary)
            throw new BadRequestException("You cannot set a minimum salary lower than the current minimum salary.");
        
        // Retrieve all employees to check and adjust their salaries
        var employees = await _employeeRepository.GetAllEmployeesAsync();
        foreach (var employee in employees)
        {
            // If an employee's current salary is below the new minimum, update it
            if (employee.Salary < newMinimumSalary)
            {
                employee.Salary = newMinimumSalary;
                await _employeeRepository.UpdateEmployeeAsync(employee);
            }
        }

        // Update the static minimum salary property in the Employee entity
        Employee.MinSalary = newMinimumSalary;
        
        // Persist the updated minimum salary (assuming MinimumSalaryPersistence handles saving it)
        MinimumSalaryPersistence.Save();
    }
}