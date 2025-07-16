namespace BookShopServer.Entities;

/// <summary>
/// Represents the type of cover for a book.
/// </summary>
public enum CoverType
{
    Hard,
    Soft,
    SpiralBound
}

/// <summary>
/// Represents a book in the bookshop system, including its physical attributes and associated genres and authors.
/// </summary>
public class Book
{
    public int ItemId { get; set; }
    public int NumberOfPages { get; set; }
    public CoverType CoverType { get; set; }
    public virtual ICollection<Genre> Genres { get; set; }
    public virtual ICollection<Author> Authors { get; set; }
    public virtual Item Item { get; set; }
}
