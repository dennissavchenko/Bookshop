namespace BookShopServer.Entities;

/// <summary>
/// Represents the condition of a used item in the bookshop system.
/// </summary>
public enum Condition
{
    Mint,
    Good,
    Fair,
    Poor
}


/// <summary>
/// Represents a used item in the bookshop system,
/// including its condition and whether it contains annotations.
/// It inherits from the base Item class.
/// </summary>
public class UsedItem : Item
{
    public Condition Condition { get; set; }
    public bool HasAnnotations { get; set; }
}
