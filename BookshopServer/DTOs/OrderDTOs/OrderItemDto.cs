using BookShopServer.DTOs.ItemDTOs;

namespace BookShopServer.DTOs.OrderDTOs;

/// <summary>
/// Represents the data transfer object for displaying an item in an order.
/// </summary>
public class OrderItemDto
{
    public SimpleItemDto Item { get; set; }
    public int Quantity { get; set; }
}