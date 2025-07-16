using BookShopServer.DTOs.UserDTOs;
using BookShopServer.Entities;
using BookShopServer.Exceptions;
using BookShopServer.Repositories.UserRepositories;
using BookShopServer.Services.OrderServices;

// Assuming this namespace is for user-related services, possibly for ReviewService

namespace BookShopServer.Services.UserServices;

/// <summary>
/// Provides services for managing user accounts, including deletion, retrieval, and role management.
/// This service acts as a central point for actions affecting both general user data and their specific roles (customer/employee).
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IOrderService _orderService;
    private readonly ICustomerRepository _customerRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IReviewService _reviewService;

    /// <summary>
    /// Initializes a new instance of the <c>UserService</c> class with necessary repositories and services.
    /// </summary>
    /// <param name="userRepository">The repository for general user data.</param>
    /// <param name="orderService">The service for managing order-related operations.</param>
    /// <param name="customerRepository">The repository for customer-specific data.</param>
    /// <param name="employeeRepository">The repository for employee-specific data.</param>
    /// <param name="reviewService">The service for managing review-related operations.</param>
    public UserService(IUserRepository userRepository, IOrderService orderService, ICustomerRepository customerRepository, IEmployeeRepository employeeRepository, IReviewService reviewService)
    {
        _userRepository = userRepository;
        _orderService = orderService;
        _customerRepository = customerRepository;
        _employeeRepository = employeeRepository;
        _reviewService = reviewService;
    }

    /// <summary>
    /// Deletes a user from the system. This operation includes handling associated data like orders and reviews
    /// by reassigning them to a 'deleted user' entity before the user record is removed.
    /// </summary>
    /// <param name="id">The unique identifier of the user to delete.</param>
    /// <exception cref="NotFoundException">Thrown if the user with the given ID does not exist.</exception>
    public async Task DeleteUserAsync(int id)
    {
        // Check if the user exists before attempting deletion
        if (!await _userRepository.UserExistsAsync(id))
            throw new NotFoundException("User with the given ID does not exist.");

        // Reassign customer's orders to a 'deleted user' to retain historical data without personal link
        await _orderService.AssignOrdersOfCustomerToDeletedCustomerAsync(id);
        // Reassign customer's reviews to a 'deleted user'
        await _reviewService.AssignReviewsOfCustomerToDeletedCustomerAsync(id);

        // Finally, delete the user record from the repository
        await _userRepository.DeleteUserAsync(id);
    }
    
    /// <summary>
    /// Retrieves a user by their unique identifier and maps their details, including role-specific information,
    /// to a <see cref="UserDto"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>A <see cref="UserDto"/> representing the user.</returns>
    /// <exception cref="NotFoundException">Thrown if the user with the given ID does not exist.</exception>
    public async Task<UserDto> GetUserByIdAsync(int id)
    {
        // Retrieve the user entity, which should include associated Customer and Employee entities if they exist
        var user = await _userRepository.GetUserByIdAsync(id);
        
        // Check if the user exists
        if (user == null)
            throw new NotFoundException("User with the given ID does not exist.");

        // Map the user entity to a UserDto
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email,
            Username = user.Username,
            // Determine and set the user's role based on existing Customer and Employee navigations
            Role = user.Customer != null && user.Employee != null ? UserRole.CustomerAndEmployee.ToString() :
                   user.Customer != null ? UserRole.Customer.ToString() :
                   user.Employee != null ? UserRole.Employee.ToString() : "Unknown",
            // Include customer-specific details if the user has a customer role
            DOB = user.Customer?.DOB,
            Address = user.Customer?.Address,
            // Include employee-specific details if the user has an employee role
            Salary = user.Employee?.Salary,
            Experience = user.Employee?.Experience.ToString()
        };
    }
    
    /// <summary>
    /// Retrieves a collection of all users in the system and maps their details, including role-specific information,
    /// to <see cref="UserDto"/>s.
    /// </summary>
    /// <returns>A collection of <see cref="UserDto"/> representing all users.</returns>
    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        // Retrieve all user entities
        var users = await _userRepository.GetAllUsersAsync();

        // Map each user entity to a UserDto, determining their role and including relevant details
        return users.Select(u => new UserDto
        {
            Id = u.Id,
            Name = u.Name,
            Surname = u.Surname,
            Email = u.Email,
            Username = u.Username,
            Role = u.Customer != null && u.Employee != null ? UserRole.CustomerAndEmployee.ToString() :
                   u.Customer != null ? UserRole.Customer.ToString() :
                   u.Employee != null ? UserRole.Employee.ToString() : "Unknown",
            DOB = u.Customer?.DOB,
            Address = u.Customer?.Address,
            Salary = u.Employee?.Salary,
            Experience = u.Employee?.Experience.ToString(),
        }).ToList();
    }

    /// <summary>
    /// Adds a customer role to an existing user. If the user already has a customer role, no action is taken.
    /// Performs validation for user existence and date of birth.
    /// </summary>
    /// <param name="customerRoleDto">The DTO containing the user ID and customer-specific details.</param>
    /// <exception cref="NotFoundException">Thrown if the user with the given ID does not exist.</exception>
    /// <exception cref="BadRequestException">Thrown if the provided Date of Birth is in the future.</exception>
    public async Task AddCustomerRoleAsync(CustomerRoleDto customerRoleDto)
    {
        // Validate user existence
        if(!await _userRepository.UserExistsAsync(customerRoleDto.UserId))
            throw new NotFoundException("User with the given ID does not exist.");
        
        // Validate Date of Birth
        if(customerRoleDto.DOB > DateTime.Now)
            throw new BadRequestException("Date of Birth cannot be in the future.");

        // Check if the user already has a customer role to avoid duplicate entries
        if (!await _customerRepository.CustomerExistsAsync(customerRoleDto.UserId))
        {
            // If not, add the new customer role with provided details
            await _customerRepository.AddCustomerAsync(new Customer
            {
                UserId = customerRoleDto.UserId,
                Address = customerRoleDto.Address,
                DOB = customerRoleDto.DOB
            });
        }
    }
    
    /// <summary>
    /// Adds an employee role to an existing user. If the user already has an employee role, no action is taken.
    /// Performs validation for user existence and minimum salary.
    /// </summary>
    /// <param name="employeeRoleDto">The DTO containing the user ID and employee-specific details.</param>
    /// <exception cref="NotFoundException">Thrown if the user with the given ID does not exist.</exception>
    /// <exception cref="BadRequestException">Thrown if the provided salary is less than the minimum required salary.</exception>
    public async Task AddEmployeeRoleAsync(EmployeeRoleDto employeeRoleDto)
    {
        // Validate user existence
        if(!await _userRepository.UserExistsAsync(employeeRoleDto.UserId))
            throw new NotFoundException("User with the given ID does not exist.");
        
        // Validate minimum salary
        if (employeeRoleDto.Salary < Employee.MinSalary)
            throw new BadRequestException($"Salary cannot be less than {Employee.MinSalary}.");

        // Check if the user already has an employee role to avoid duplicate entries
        if (!await _employeeRepository.EmployeeExistsAsync(employeeRoleDto.UserId))
        {
            // If not, add the new employee role with provided details
            await _employeeRepository.AddEmployeeAsync(new Employee
            {
                UserId = employeeRoleDto.UserId,
                Salary = employeeRoleDto.Salary,
                Experience = employeeRoleDto.Experience
            });
        }
        // If the employee role already exists, silently do nothing.
    }
    
    /// <summary>
    /// Deletes the customer role from a user. This operation is only allowed if the user also has an employee role.
    /// Associated customer data (orders, reviews) are reassigned to a 'deleted user' entity.
    /// </summary>
    /// <param name="userId">The unique identifier of the user from whom to remove the customer role.</param>
    /// <exception cref="NotFoundException">Thrown if the user or their customer role does not exist.</exception>
    /// <exception cref="ConflictException">Thrown if the customer role is the user's only role.</exception>
    public async Task DeleteCustomerRoleAsync(int userId)
    {
        // Retrieve the user to check their roles
        var user = await _userRepository.GetUserByIdAsync(userId);
        
        // Validate user existence
        if (user == null)
            throw new NotFoundException("User with the given ID does not exist.");
        
        // Prevent deleting the customer role if it's the user's only role (to avoid a user with no roles)
        if(user.Employee == null)
            throw new ConflictException("Customer is user's only role, cannot delete.");
        
        // Check if the customer role actually exists for this user
        if (!await _customerRepository.CustomerExistsAsync(userId))
            throw new NotFoundException("Customer with the given ID does not exist.");
        
        // Reassign associated customer data before deleting the role
        await _orderService.AssignOrdersOfCustomerToDeletedCustomerAsync(userId);
        await _reviewService.AssignReviewsOfCustomerToDeletedCustomerAsync(userId);
        
        // Delete the customer role from the repository
        await _customerRepository.DeleteCustomerAsync(userId);
    }
    
    /// <summary>
    /// Deletes the employee role from a user. This operation is only allowed if the user also has a customer role.
    /// </summary>
    /// <param name="userId">The unique identifier of the user from whom to remove the employee role.</param>
    /// <exception cref="NotFoundException">Thrown if the user or their employee role does not exist.</exception>
    /// <exception cref="ConflictException">Thrown if the employee role is the user's only role.</exception>
    public async Task DeleteEmployeeRoleAsync(int userId)
    {
        // Retrieve the user to check their roles
        var user = await _userRepository.GetUserByIdAsync(userId);
        
        // Validate user existence
        if (user == null)
            throw new NotFoundException("User with the given ID does not exist.");
        
        // Prevent deleting the employee role if it's the user's only role (to avoid a user with no roles)
        if(user.Customer == null)
            throw new ConflictException("Employee is user's only role, cannot delete.");
        
        // Check if the employee role actually exists for this user
        if (!await _employeeRepository.EmployeeExistsAsync(userId))
            throw new NotFoundException("Employee with the given ID does not exist.");
        
        // Delete the employee role from the repository
        await _employeeRepository.DeleteEmployeeAsync(userId);
    }

    public async Task<User?> ValidateUserAsync(string usernameOrEmail, string password)
    {
        var user = await _userRepository.GetUserByEmailAsync(usernameOrEmail);
        if (user == null)
            user = await _userRepository.GetUserByUsernameAsync(usernameOrEmail);
        if (user == null || user.Username == "deleted_user")
            throw new NotFoundException("User with the given username or email does not exist.");
        if (BCrypt.Net.BCrypt.Verify(password, user.Password))
            return user;
        return null;
    }
}