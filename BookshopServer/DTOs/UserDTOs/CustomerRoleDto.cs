using System.ComponentModel.DataAnnotations;

namespace BookShopServer.DTOs.UserDTOs;

/// <summary>
/// Represents the data transfer object for adding or updating a customer role.
/// </summary>
public class CustomerRoleDto
{
    public int UserId { get; set; }
    [Required]
    public DateTime DOB { get; set; }
    public Address? Address { get; set; }
}