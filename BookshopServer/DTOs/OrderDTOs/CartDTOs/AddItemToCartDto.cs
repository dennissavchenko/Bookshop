using System.ComponentModel.DataAnnotations;

namespace BookShopServer.DTOs.OrderDTOs.CartDTOs;

/// <summary>
/// Represents the data transfer object for adding or updating order-item in cart.
/// </summary>
public class AddItemToCartDto
{
    [Required]
    public int ItemId { get; set; }
    [Required]
    public int OrderId { get; set; }
    [Required]
    [Range(1, 1000)]
    public int Quantity { get; set; }
}