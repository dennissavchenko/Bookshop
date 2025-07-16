namespace BookShopServer.Entities;

/// <summary>
/// Represents an item within a customer order in the bookshop system,
/// including its quantity and references to the associated order and item.
/// </summary>
public class OrderItem
{
    public int OrderId { get; set; }
    public int ItemId { get; set; }
    public int Quantity { get; set; }
    public virtual Order Order { get; set; }
    public virtual Item Item { get; set; }
}
