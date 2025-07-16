using BookShopServer.DTOs;
using BookShopServer.DTOs.UserDTOs;

namespace BookShopServer.Entities;

/// <summary>
/// Represents a customer in the bookshop system, including personal details,
/// related user account, address, orders, and reviews.
/// </summary>
public class Customer
{
    public int UserId { get; set; }
    public DateTime DOB { get; set; }
    public Address? Address { get; set; }
    public virtual User User { get; set; }
    public virtual ICollection<Order> Orders { get; set; }
    public virtual ICollection<Review> Reviews { get; set; }
    
    public int Age => DateTime.Now.Year - DOB.Year - (DateTime.Now.DayOfYear < DOB.DayOfYear ? 1 : 0);
    
}
