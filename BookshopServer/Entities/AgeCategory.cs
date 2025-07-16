namespace BookShopServer.Entities;

/// <summary>
/// Represents an age category used to classify items in the bookshop system.
/// Contains a tag, description, and minimum required age for accessing related items.
/// </summary>
public class AgeCategory
{
    public int Id { get; set; }
    public string Tag { get; set; }
    public string Description { get; set; }
    public int MinimumAge { get; set; }
    public virtual ICollection<Item> Items { get; set; }
}
