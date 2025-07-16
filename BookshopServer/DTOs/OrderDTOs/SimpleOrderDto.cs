namespace BookShopServer.DTOs.OrderDTOs;

/// <summary>
/// Represents the data transfer object for displaying a simple order.
/// </summary>
public class SimpleOrderDto
{
    public int Id { get; set; }
    public string Status { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public double TotalPrice { get; set; }
    public int CustomerId { get; set; }
}