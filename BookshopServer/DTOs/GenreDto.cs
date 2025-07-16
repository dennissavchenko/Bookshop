using System.ComponentModel.DataAnnotations;

namespace BookShopServer.DTOs;

/// <summary>
/// Represents the data transfer object for displaying, adding and updating a genre.
/// </summary>
public class GenreDto
{
    public int Id { get; set; }
    [Required(AllowEmptyStrings = false)]
    [MaxLength(50)]
    public string Name { get; set; }
    [Required(AllowEmptyStrings = false)]
    [MaxLength(500)]
    public string Description { get; set; }
}