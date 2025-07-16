namespace BookShopServer.Entities;

/// <summary>
/// Represents a literary genre in the bookshop system, including its name,
/// description, and associated books.
/// </summary>
public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public virtual ICollection<Book> Books { get; set; }
}
