using System.ComponentModel.DataAnnotations;
using BookShopServer.DTOs.UserDTOs;

namespace BookShopServer.DTOs;

/// <summary>
/// Represents the data transfer object for displaying, adding, and updating a publisher.
/// </summary>
public class PublisherDto
{
    public int Id { get; set; }
    [Required(AllowEmptyStrings = false)]
    [MaxLength(100)]
    public string Name { get; set; }
    [Required]
    public Address Address { get; set; }
    [Required(AllowEmptyStrings = false)]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; }
    [Required(AllowEmptyStrings = false)]
    [RegularExpression(@"^\d{9}$", ErrorMessage = "The number must contain exactly 9 digits.")]
    public string PhoneNumber { get; set; }
}