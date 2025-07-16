namespace BookShopServer.Entities;

/// <summary>
/// Represents a customer review of an item in the bookshop system,
/// including rating, text content, timestamp, and references to the customer and item.
/// </summary>
public class Review
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int ItemId { get; set; }
    public int Rating { get; set; }
    public string Text { get; set; }
    public DateTime TimeStamp { get; set; }
    public virtual Customer Customer { get; set; }
    public virtual Item Item { get; set; }
}
