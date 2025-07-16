using System.ComponentModel.DataAnnotations;

namespace BookShopServer.DTOs.ReviewDTOs;

/// <summary>
/// Represents the data transfer object for adding a review to an item.
/// </summary>
public class AddReviewDto
{
    public int ItemId { get; set; }
    public int CustomerId { get; set; }
    [Required(AllowEmptyStrings = false)]
    [MaxLength(500)]
    public string Text { get; set; }
    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }
}