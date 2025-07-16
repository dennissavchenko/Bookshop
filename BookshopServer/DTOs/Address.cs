using System.ComponentModel.DataAnnotations;

namespace BookShopServer.DTOs;

/// <summary>
/// Represents an address data transfer object used for input or output operations,
/// including street, house number, city, postal code, and country information.
/// </summary>
public class Address
{
    [Required(AllowEmptyStrings = false)]
    [MaxLength(100)]
    public string Street { get; set; }

    public int HouseNumber { get; set; }

    [Required(AllowEmptyStrings = false)]
    [MaxLength(50)]
    public string City { get; set; }

    [Required(AllowEmptyStrings = false)]
    [MaxLength(20)]
    public string PostalCode { get; set; }

    [Required(AllowEmptyStrings = false)]
    [MaxLength(50)]
    public string Country { get; set; }
}