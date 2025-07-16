using BookShopServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookShopServer.Repositories.UserRepositories;

/// <summary>
/// Provides data access methods for managing customer entities in the bookshop system.
/// </summary>
public class CustomerRepository : ICustomerRepository
{
    private readonly Context _context;
    
    /// <summary>
    /// Initializes a new instance of the <c>CustomerRepository</c> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public CustomerRepository(Context context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Retrieves a customer by their unique identifier, including their user information, orders, and reviews.
    /// </summary>
    /// <param name="id">The ID of the customer to retrieve.</param>
    /// <returns>The customer entity if found.</returns>
    public async Task<Customer?> GetCustomerByIdAsync(int id)
    {
        return await _context.Customers
            .Include(x => x.User)
            .Include(x => x.Orders)
            .ThenInclude(x => x.OrderItems)
            .ThenInclude(x => x.Item)
            .Include(r => r.Reviews)
            .ThenInclude(x => x.Item)
            .FirstOrDefaultAsync(x => x.UserId == id);
    }
    
    /// <summary>
    /// Retrieves all customer entities, including their associated user information.
    /// </summary>
    /// <returns>A collection of all customers.</returns>
    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        return await _context.Customers
            .Include(x => x.User)
            .ToListAsync();
    }
    
    /// <summary>
    /// Adds a new customer to the database.
    /// </summary>
    /// <param name="customer">The customer entity to add.</param>
    public async Task AddCustomerAsync(Customer customer)
    {
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
    }
    
    /// <summary>
    /// Updates an existing customer in the database.
    /// </summary>
    /// <param name="customer">The customer entity with updated information.</param>
    public async Task UpdateCustomerAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
    }
    
    /// <summary>
    /// Checks if a customer with the specified ID exists.
    /// </summary>
    /// <param name="id">The ID of the customer to check.</param>
    /// <returns><c>true</c> if the customer exists; otherwise, <c>false</c>.</returns>
    public async Task<bool> CustomerExistsAsync(int id)
    {
        return await _context.Customers.AnyAsync(x => x.UserId == id);
    }
    
    /// <summary>
    /// Retrieves a special "deleted" customer entity, or creates one if it doesn't exist.
    /// This is typically used to associate records with a placeholder customer when a user is deleted.
    /// </summary>
    /// <returns>The "deleted" customer entity.</returns>
    public async Task<Customer> GetDeletedCustomerAsync()
    {
        var deletedCustomer = await _context.Customers
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.User.Username == "deleted_user");

        if (deletedCustomer != null)
            return deletedCustomer;
        
        var user = new User
        {
            Name = "System",
            Surname = "Account",
            Email = BCrypt.Net.BCrypt.HashPassword("blank"),
            Username = "deleted_user",
            Password = BCrypt.Net.BCrypt.HashPassword("WK21QQQQSSS!!@A")
        };

        
        var customer = new Customer
        {
            User = user,
            DOB = DateTime.Now
        };

        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();

        return customer;
    }

    /// <summary>
    /// Deletes a customer from the database by their ID.
    /// </summary>
    /// <param name="id">The ID of the customer to delete.</param>
    public async Task DeleteCustomerAsync(int id)
    {
        var customer = await _context.Customers.FirstAsync(x => x.UserId == id);
        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Determines if a customer is old enough to purchase a specific item based on their age and the item's age restriction.
    /// </summary>
    /// <param name="customerId">The ID of the customer.</param>
    /// <param name="itemId">The ID of the item to check.</param>
    /// <returns><c>true</c> if customer is eligible to buy the item; <c>false</c> - otherwise</returns>
    public async Task<bool> CustomerOldEnoughForPurchaseAsync(int customerId, int itemId)
    {
        var customer = await _context.Customers.FirstAsync(x => x.UserId == customerId);
        var item = await _context.Items
            .Include(x => x.AgeCategory)
            .FirstAsync(x => x.Id == itemId);
        return customer.Age >= item.AgeCategory.MinimumAge;
    }
    
}