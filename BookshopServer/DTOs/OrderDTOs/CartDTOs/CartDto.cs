namespace BookShopServer.DTOs.OrderDTOs.CartDTOs;

/// <summary>
/// Represents the data transfer object for displaying a shopping cart.
/// </summary>
public class CartDto
{
    public int Id { get; set; }
    public double TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<OrderItemDto> Items { get; set; }
    public int UserId { get; set; }
}