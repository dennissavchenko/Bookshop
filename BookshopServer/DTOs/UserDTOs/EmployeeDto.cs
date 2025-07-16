using System.ComponentModel.DataAnnotations;
using BookShopServer.Entities;

namespace BookShopServer.DTOs.UserDTOs;

/// <summary>
/// Represents the data transfer object for displaying, adding or updating an employee.
/// </summary>
public class EmployeeDto
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
    [Range(1, Double.MaxValue)]
    public double Salary { get; set; }
    [Required]
    [Range(0, 2)]
    public Experience Experience { get; set; }
}