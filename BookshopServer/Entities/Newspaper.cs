namespace BookShopServer.Entities;

/// <summary>
/// Represents a newspaper in the bookshop system.
/// Includes a headline, a list of topics, and a reference to its associated item.
/// </summary>
public class Newspaper
{
    public int ItemId { get; set; }
    public string Headline { get; set; }
    public ICollection<string> Topics { get; set; }
    public virtual Item Item { get; set; }
}
