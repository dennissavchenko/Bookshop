namespace BookShopServer.Entities;

/// <summary>
/// Represents a new item in the bookshop system.
/// Includes a property indicating whether the item is sealed.
/// It inherits from the base Item class.
/// </summary>
public class NewItem : Item
{
    public bool IsSealed { get; set; }
}
