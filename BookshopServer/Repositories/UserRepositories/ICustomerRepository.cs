using BookShopServer.Entities;

namespace BookShopServer.Repositories.UserRepositories;

/// <summary>
/// Defines the contract for data access operations related to customer entities.
/// </summary>
public interface ICustomerRepository
{
    /// <summary>
    /// Retrieves a customer by their unique identifier.
    /// </summary>
    /// <param name="id">The ID of the customer to retrieve.</param>
    /// <returns>The customer entity if found, otherwise <c>null</c>.</returns>
    Task<Customer?> GetCustomerByIdAsync(int id);

    /// <summary>
    /// Retrieves all customer entities.
    /// </summary>
    /// <returns>A collection of all customers.</returns>
    Task<IEnumerable<Customer>> GetAllCustomersAsync();

    /// <summary>
    /// Adds a new customer to the database.
    /// </summary>
    /// <param name="customer">The customer entity to add.</param>
    Task AddCustomerAsync(Customer customer);

    /// <summary>
    /// Updates an existing customer in the database.
    /// </summary>
    /// <param name="customer">The customer entity with updated information.</param>
    Task UpdateCustomerAsync(Customer customer);

    /// <summary>
    /// Checks if a customer with the specified ID exists.
    /// </summary>
    /// <param name="id">The ID of the customer to check.</param>
    /// <returns><c>true</c> if the customer exists; otherwise, <c>false</c>.</returns>
    Task<bool> CustomerExistsAsync(int id);

    /// <summary>
    /// Retrieves a deleted customer entity.
    /// </summary>
    /// <returns>The deleted customer entity.</returns>
    Task<Customer> GetDeletedCustomerAsync();

    /// <summary>
    /// Deletes a customer from the database by their ID.
    /// </summary>
    /// <param name="id">The ID of the customer to delete.</param>
    Task DeleteCustomerAsync(int id);
    
    /// <summary>
    /// Determines if a customer is old enough to purchase a specific item based on their age and the item's age restriction.
    /// </summary>
    /// <param name="customerId">The ID of the customer.</param>
    /// <param name="itemId">The ID of the item to check.</param>
    /// <returns><c>true</c> if customer is eligible to buy the item; <c>false</c> - otherwise</returns>
    Task<bool> CustomerOldEnoughForPurchaseAsync(int customerId, int itemId);
}