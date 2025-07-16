namespace BookShopServer.DTOs.UserDTOs;

/// <summary>
/// Represents the user roles in the system.
/// </summary>
public enum UserRole
{
    Customer,
    Employee,
    CustomerAndEmployee
}

/// <summary>
/// Represents the data transfer object for displaying user information.
/// </summary>
public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public DateTime? DOB { get; set; }
    public Address? Address { get; set; }
    public double? Salary { get; set; }
    public string? Experience { get; set; }
}