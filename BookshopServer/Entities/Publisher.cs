using BookShopServer.DTOs;
using BookShopServer.DTOs.UserDTOs;

namespace BookShopServer.Entities;

/// <summary>
/// Represents a publisher in the bookshop system,
/// including contact information, address, and associated items.
/// </summary>
public class Publisher
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Address Address { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public virtual ICollection<Item> Items { get; set; }
}
