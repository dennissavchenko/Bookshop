using System.ComponentModel.DataAnnotations;

namespace BookShopServer.DTOs;

/// <summary>
/// Represents the data transfer object for displaying, adding and updating age category.
/// </summary>
public class AgeCategoryDto
{
    public int Id { get; set; }
    [Required(AllowEmptyStrings = false)]
    [MaxLength(20)]
    public string Tag { get; set; }
    [Required(AllowEmptyStrings = false)]
    [MaxLength(200)]
    public string Description { get; set; }
    [Required]
    [Range(0, 100)]
    public int MinimumAge { get; set; }
}