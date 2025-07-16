namespace BookShopServer.Entities;

/// <summary>
/// Represents a magazine in the bookshop system.
/// Includes a flag for special editions and a reference to its associated item.
/// </summary>
public class Magazine
{
    public int ItemId { get; set; }
    public bool IsSpecialEdition { get; set; }
    public virtual Item Item { get; set; }
}
