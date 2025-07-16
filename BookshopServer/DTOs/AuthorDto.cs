using System.ComponentModel.DataAnnotations;

namespace BookShopServer.DTOs;

/// <summary>
/// Represents the data transfer object for displaying, adding and updating an author.
/// </summary>
public class AuthorDto
{
    public int Id { get; set; }
    [Required(AllowEmptyStrings = false)]
    [MaxLength(100)]
    public string Name { get; set; }
    [Required(AllowEmptyStrings = false)]
    [MaxLength(100)]
    public string Surname { get; set; }
    [Required]
    public DateTime DOB { get; set; }
    [MaxLength(200)]
    public string? Pseudonym { get; set; }
}