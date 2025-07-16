namespace BookShopServer.Entities;

/// <summary>
/// Represents a payment in the bookshop system.
/// </summary>
public enum PaymentType
{
    Card,
    ApplePay,
    GooglePay,
    Blik
}

/// <summary>
/// Represents a payment made for an order in the bookshop system,
/// including the payment type, amount, timestamp, and associated order.
/// </summary>
public class Payment
{
    public int Id { get; set; }
    public PaymentType PaymentType { get; set; }
    public DateTime TimeStamp { get; set; }
    public double Amount { get; set; }
    public int OrderId { get; set; }
    public virtual Order Order { get; set; }
}
