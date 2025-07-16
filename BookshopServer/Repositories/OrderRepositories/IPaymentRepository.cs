using BookShopServer.Entities;

namespace BookShopServer.Repositories.OrderRepositories;

/// <summary>
/// Defines a contract for managing payment records in the system.
/// </summary>
public interface IPaymentRepository 
{
    /// <summary>
    /// Adds a new payment entry to the system.
    /// </summary>
    /// <param name="payment">The payment to be added.</param>
    Task AddPaymentAsync(Payment payment);
}