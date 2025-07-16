using BookShopServer.DTOs.UserDTOs;

namespace BookShopServer.Services.UserServices;

/// <summary>
/// Defines the contract for services that manage customer-related operations.
/// </summary>
public interface ICustomerService
{
    /// <summary>
    /// Retrieves a customer by their unique identifier.
    /// </summary>
    /// <param name="id">The ID of the customer to retrieve.</param>
    /// <returns>A <see cref="CustomerDto"/> representing the customer.</returns>
    Task<CustomerDto> GetCustomerByIdAsync(int id);

    /// <summary>
    /// Retrieves a collection of all customers.
    /// </summary>
    /// <returns>A collection of <see cref="CustomerDto"/> representing all customers.</returns>
    Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();

    /// <summary>
    /// Adds a new customer to the system.
    /// </summary>
    /// <param name="customer">The <see cref="CustomerDto"/> containing the details of the customer to add.</param>
    Task AddCustomerAsync(CustomerDto customer);

    /// <summary>
    /// Updates an existing customer's details.
    /// </summary>
    /// <param name="customer">The <see cref="CustomerDto"/> containing the updated details of the customer.</param>
    Task UpdateCustomerAsync(CustomerDto customer);

    /// <summary>
    /// Checks if a specific customer has received a particular item in any of their completed orders.
    /// </summary>
    /// <param name="customerId">The ID of the customer.</param>
    /// <param name="itemId">The ID of the item.</param>
    /// <returns><c>true</c> if the customer has received the item; otherwise, <c>false</c>.</returns>
    Task<bool> CustomerReceivedItemAsync(int customerId, int itemId);
}