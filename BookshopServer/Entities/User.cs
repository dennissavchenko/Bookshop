using BookShopServer.Services;

namespace BookShopServer.Entities;

/// <summary>
/// Represents a user account in the bookshop system,
/// including personal information, credentials, and links to either a customer or employee profile.
/// </summary>
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public virtual Customer? Customer { get; set; }
    public virtual Employee? Employee { get; set; }
    public virtual RefreshToken? RefreshToken { get; set; }
    public AccessLevel AccessLevel => Username == "admin" ? AccessLevel.Admin : Employee != null ? AccessLevel.Employee : AccessLevel.Customer;
}
