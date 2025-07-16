using System.ComponentModel.DataAnnotations;

namespace BookShopServer.DTOs.UserDTOs;

/// <summary>
/// Represents the data transfer object for a displaying, adding and updating a customer.
/// </summary>
public class CustomerDto
{
    public int Id { get; set; }
    [Required(AllowEmptyStrings = false)]
    [MaxLength(100)]
    public string Name { get; set; }
    [Required(AllowEmptyStrings = false)]
    [MaxLength(100)]
    public string Surname { get; set; }
    [Required(AllowEmptyStrings = false)]
    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; }
    [Required(AllowEmptyStrings = false)]
    [MaxLength(100)]
    public string Username { get; set; }
    [Required(AllowEmptyStrings = false)]
    [MaxLength(100)]
    public string Password { get; set; }
    [Required]
    public DateTime DOB { get; set; }
    public Address? Address { get; set; }
}