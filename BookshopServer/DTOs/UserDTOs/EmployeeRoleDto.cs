using System.ComponentModel.DataAnnotations;
using BookShopServer.Entities;

namespace BookShopServer.DTOs.UserDTOs;

/// <summary>
/// Represents the data transfer object for adding or updating an employee role.
/// </summary>
public class EmployeeRoleDto
{
    public int UserId { get; set; }
    [Required]
    public double Salary { get; set; }
    [Required]
    [Range(0, 2)]
    public Experience Experience { get; set; }
}