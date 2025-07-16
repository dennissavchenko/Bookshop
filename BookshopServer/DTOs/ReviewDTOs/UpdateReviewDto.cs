using System.ComponentModel.DataAnnotations;

namespace BookShopServer.DTOs.ReviewDTOs;

/// <summary>
/// Represents the data transfer object for updating a review.
/// </summary>
public class UpdateReviewDto
{
    public int Id { get; set; }
    [Required(AllowEmptyStrings = false)]
    [MaxLength(500)]
    public string Text { get; set; }
    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }
}