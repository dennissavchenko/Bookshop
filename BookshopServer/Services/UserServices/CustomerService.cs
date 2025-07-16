using BookShopServer.DTOs.UserDTOs;
using BookShopServer.Entities;
using BookShopServer.Exceptions;
using BookShopServer.Repositories.ItemRepositories;
using BookShopServer.Repositories.UserRepositories;

namespace BookShopServer.Services.UserServices;

/// <summary>
/// Provides services for managing customer-related operations, including retrieving,
/// adding, updating customer details, and checking if a customer has received a specific item.
/// </summary>
public class CustomerService : ICustomerService
{
    private readonly IUserRepository _userRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IItemRepository _itemRepository;
    
    /// <summary>
    /// Initializes a new instance of the <c>CustomerService</c> class.
    /// </summary>
    /// <param name="userRepository">The repository for user data.</param>
    /// <param name="customerRepository">The repository for customer-specific data.</param>
    /// <param name="itemRepository">The repository for item data.</param>
    public CustomerService(IUserRepository userRepository, ICustomerRepository customerRepository, IItemRepository itemRepository)
    {
        _userRepository = userRepository;
        _customerRepository = customerRepository;
        _itemRepository = itemRepository;
    }
    
    /// <summary>
    /// Retrieves a customer by their unique identifier and maps it to a <see cref="CustomerDto"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the customer.</param>
    /// <returns>A <see cref="CustomerDto"/> representing the customer.</returns>
    /// <exception cref="NotFoundException">Thrown if the customer with the given ID does not exist.</exception>
    public async Task<CustomerDto> GetCustomerByIdAsync(int id)
    {
        // Retrieve the customer entity
        var customer = await _customerRepository.GetCustomerByIdAsync(id);
        
        // Check if the customer exists
        if (customer == null)
            throw new NotFoundException("Customer with the given ID does not exist.");
        
        // Map the customer entity to a CustomerDto
        return new CustomerDto
        {
            Id = customer.UserId,
            Name = customer.User.Name,
            Surname = customer.User.Surname,
            Email = customer.User.Email,
            Username = customer.User.Username,
            Address = customer.Address,
            DOB = customer.DOB,
        };
    }
    
    /// <summary>
    /// Retrieves a collection of all customers and maps them to <see cref="CustomerDto"/>s.
    /// </summary>
    /// <returns>A collection of <see cref="CustomerDto"/> representing all customers.</returns>
    public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
    {
        // Retrieve all customer entities
        var customers = await _customerRepository.GetAllCustomersAsync();
        
        // Map each customer entity to a CustomerDto and return as a list
        return customers.Select(c => new CustomerDto
        {
            Id = c.UserId,
            Name = c.User.Name,
            Surname = c.User.Surname,
            Email = c.User.Email,
            Username = c.User.Username,
            Address = c.Address,
            DOB = c.DOB
        }).ToList();
    }
    
    /// <summary>
    /// Adds a new customer to the system. Performs validation for unique email and username,
    /// and ensures date of birth is not in the future.
    /// </summary>
    /// <param name="customerDto">The DTO containing the details of the customer to add.</param>
    /// <exception cref="ConflictException">Thrown if a user with the provided email or username already exists.</exception>
    /// <exception cref="BadRequestException">Thrown if the username is 'DeletedUser' or DOB is in the future.</exception>
    public async Task AddCustomerAsync(CustomerDto customerDto)
    {
        // Validate uniqueness of email and username
        if (!await _userRepository.EmailUniqueAsync(customerDto.Email))
            throw new ConflictException("User with this email already exists.");
        if (!await _userRepository.UsernameUniqueAsync(customerDto.Username))
            throw new ConflictException("User with this username already exists.");
        
        // Disallow reserved username
        if(customerDto.Username.ToLower() == "deleted_user")
            throw new BadRequestException("Username 'deleted_user' is reserved and cannot be used.");
        
        // Validate Date of Birth
        if (customerDto.DOB > DateTime.Now)
            throw new BadRequestException("Date of Birth cannot be in the future.");
        
        // Create User entity from DTO
        var user = new User
        {
            Name = customerDto.Name,
            Surname = customerDto.Surname,
            Email = customerDto.Email,
            Username = customerDto.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(customerDto.Password)
        };
        
        // Create Customer entity and link to User
        var customer = new Customer
        {
            User = user,
            Address = customerDto.Address,
            DOB = customerDto.DOB
        };
        
        // Add the new customer (which includes the user) to the repository
        await _customerRepository.AddCustomerAsync(customer);
    }
    
    /// <summary>
    /// Updates an existing customer's details. Performs validation for customer existence,
    /// unique email and username (if changed), and ensures date of birth is not in the future.
    /// </summary>
    /// <param name="customerDto">The DTO containing the updated details of the customer.</param>
    /// <exception cref="NotFoundException">Thrown if the customer with the given ID does not exist.</exception>
    /// <exception cref="ConflictException">Thrown if the updated email or username conflicts with an existing user.</exception>
    /// <exception cref="BadRequestException">Thrown if the username is 'DeletedUser' or DOB is in the future.</exception>
    public async Task UpdateCustomerAsync(CustomerDto customerDto)
    {
        var customer = await _customerRepository.GetCustomerByIdAsync(customerDto.Id);
        
        // Validate customer existence
        if (customer == null)
            throw new NotFoundException("Customer with the given ID does not exist.");
        
        // Retrieve the existing user associated with the customer
        var user = await _userRepository.GetUserByIdAsync(customerDto.Id);
        
        if (user == null)
            throw new NotFoundException("User with the given ID does not exist.");

        // Validate uniqueness of email if it has changed
        if (!await _userRepository.EmailUniqueAsync(customerDto.Email) && user.Email != customerDto.Email)
            throw new ConflictException("User with this email already exists.");
        
        // Validate uniqueness of username if it has changed
        if (!await _userRepository.UsernameUniqueAsync(customerDto.Username) && user.Username != customerDto.Username)
            throw new ConflictException("User with this username already exists.");
        
        // Disallow reserved username
        if (customerDto.Username.ToLower() == "deleted_user")
            throw new BadRequestException("Username 'deleted_user' is reserved and cannot be used.");
        
        // Validate Date of Birth
        if (customerDto.DOB > DateTime.Now)
            throw new BadRequestException("Date of Birth cannot be in the future.");
        
        // Update User entity properties
        user.Name = customerDto.Name;
        user.Surname = customerDto.Surname;
        user.Email = customerDto.Email;
        user.Username = customerDto.Username;
        user.Password = BCrypt.Net.BCrypt.HashPassword(customerDto.Password);

        // Update User in the repository
        await _userRepository.UpdateUserAsync(user);
        
        // Update Customer entity properties
        customer.Address = customerDto.Address;
        customer.DOB = customerDto.DOB;

        // Update Customer in the repository
        await _customerRepository.UpdateCustomerAsync(customer);
    }

    /// <summary>
    /// Checks if a specific customer has received a particular item.
    /// This is determined by checking if the customer has any delivered orders that contain the specified item.
    /// </summary>
    /// <param name="customerId">The unique identifier of the customer.</param>
    /// <param name="itemId">The unique identifier of the item.</param>
    /// <returns><c>true</c> if the customer has received the item; otherwise, <c>false</c>.</returns>
    /// <exception cref="NotFoundException">Thrown if the customer or item does not exist.</exception>
    public async Task<bool> CustomerReceivedItemAsync(int customerId, int itemId)
    {
        // Validate item existence
        if(!await _itemRepository.ItemExistsAsync(itemId))
            throw new NotFoundException("Item with the given ID does not exist.");
        
        // Retrieve the customer with their orders and order items
        var customer = await _customerRepository.GetCustomerByIdAsync(customerId);
        
        // Validate customer existence
        if (customer == null)
            throw new NotFoundException("Customer with the given ID does not exist.");
        
        // Check if any of the customer's delivered orders contain the specified item
        return customer.Orders.Any(x => x.OrderStatus == OrderStatus.Delivered && 
                                 x.OrderItems.Any(oi => oi.ItemId == itemId));
    }
}