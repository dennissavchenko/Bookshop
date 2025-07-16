namespace BookShopServer.Entities;

/// <summary>
/// Represents the status of an order in the bookshop system.
/// </summary>
public enum OrderStatus
{
    Cart,
    Pending,
    Confirmed,
    Preparation,
    Shipped,
    Delivered,
    Cancelled
}

/// <summary>
/// Represents a customer order in the bookshop system,
/// including its status, timestamps, associated items, and payment details.
/// </summary>
public class Order
{
    public int Id { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? PreparationStartedAt { get; set; }
    public DateTime? ShippedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public int CustomerId { get; set; }
    public virtual Payment? Payment { get; set; }
    public virtual ICollection<OrderItem> OrderItems { get; set; }
    public virtual Customer Customer { get; set; }

    /// <summary>
    /// Returns the most recent timestamp that reflects the last known progress in the order's lifecycle.
    /// </summary>
    /// <returns>
    /// The most recent non-null status timestamp, 
    /// or the creation date if no updates have occurred.
    /// </returns>
    public DateTime GetLastUpdatedAt()
    {
        return CancelledAt ?? DeliveredAt ?? ShippedAt ?? PreparationStartedAt ?? ConfirmedAt ?? CreatedAt;
    }

    /// <summary>
    /// Calculates the total cost of the order by summing the price and quantity of each item.
    /// </summary>
    /// <returns>
    /// Total order price.
    /// </returns>
    public double GetTotalPrice()
    {
        return OrderItems.Sum(x => x.Item.Price * x.Quantity);
    }
}
