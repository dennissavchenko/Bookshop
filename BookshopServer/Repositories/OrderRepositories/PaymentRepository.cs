using BookShopServer.Entities;

namespace BookShopServer.Repositories.OrderRepositories;

/// <summary>
/// Repository class for managing payments, 
/// providing methods to persist payment data in the database.
/// </summary>
public class PaymentRepository : IPaymentRepository
{
    private readonly Context _context;

    /// <summary>
    /// Initializes a new instance of the <c>PaymentRepository</c> class with the specified database context.
    /// </summary>
    /// <param name="context">Database context.</param>
    public PaymentRepository(Context context)
    {
        _context = context;
    }

    /// <summary>
    /// Adds a new payment to the database.
    /// </summary>
    /// <param name="payment">Payment entity to be added.</param>
    public async Task AddPaymentAsync(Payment payment)
    {
        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();
    }
}