namespace BookShopServer.DTOs.OrderDTOs;

/// <summary>
/// Represents the data transfer object for displaying a confirmed order.
/// </summary>
public class ConfirmedOrderDto
{
    public int Id { get; set; }
    public string Status { get; set; }
    public double TotalPrice { get; set; }
    public DateTime ConfirmedAt { get; set; }
    public DateTime? PreparationStartedAt { get; set; }
    public DateTime? ShippedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public int CustomerId { get; set; }
    public ICollection<OrderItemDto> Items { get; set; }
}