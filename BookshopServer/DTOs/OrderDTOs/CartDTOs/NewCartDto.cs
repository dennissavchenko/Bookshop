using System.ComponentModel.DataAnnotations;

namespace BookShopServer.DTOs.OrderDTOs.CartDTOs;

/// <summary>
/// Represents the data transfer object for creating a new cart.
/// </summary>
public class NewCartDto
{
    [Required]
    public int CustomerId { get; set; }
    [Required]
    public int ItemId { get; set; }
    [Required]
    [Range(1, 1000)]
    public int Quantity { get; set; }
}