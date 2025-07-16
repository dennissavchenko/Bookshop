namespace BookShopServer.Entities;

/// <summary>
/// Defines the available item types in the bookshop.
/// </summary>
public enum ItemType
{
    Book,
    Magazine,
    Newspaper
}

/// <summary>
/// Represents a general item in the bookshop system.
/// Serves as a base class for books, magazines, and newspapers, and contains shared properties and logic.
/// </summary>
public abstract class Item
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public DateTime PublishingDate { get; set; }
    public string Language { get; set; }
    public double Price { get; set; }
    public int AmountInStock { get; set; }
    
    // Navigation properties to specific item types
    public virtual Book? Book { get; set; }
    public virtual Magazine? Magazine { get; set; }
    public virtual Newspaper? Newspaper { get; set; }

    // Navigation properties to related entities
    public int PublisherId { get; set; }
    public virtual Publisher Publisher { get; set; }

    // Navigation property to the age category
    public int AgeCategoryId { get; set; }
    public virtual AgeCategory AgeCategory { get; set; }

    // Navigation properties to related entities
    public virtual ICollection<OrderItem> OrderItems { get; set; }
    
    // Navigation property to reviews associated with the item
    public virtual ICollection<Review> Reviews { get; set; }

    /// <summary>
    /// Calculates the average rating of the item based on associated reviews.
    /// </summary>
    /// <returns>
    /// The average rating of the item, calculated from the ratings of all associated reviews.
    /// Returns 0 if there are no reviews.
    /// </returns>
    public double GetAverageRating()
    {
        if (Reviews.Count == 0)
            return 0;

        return Reviews.Average(r => r.Rating);
    }

    /// <summary>
    /// Determines whether the item is a used item.
    /// </summary>
    /// <returns>
    /// True if the item is of type UsedItem;
    /// otherwise, false.
    /// </returns>
    public bool GetIsUsed()
    {
        return this is UsedItem;
    }

    /// <summary>
    /// Identifies the specific type of the item (Book, Magazine, or Newspaper).
    /// </summary>
    /// <returns>
    /// The item type if identified;
    /// otherwise, null.
    /// </returns>
    public ItemType? GetItemType()
    {
        return Book != null ? ItemType.Book :
               Magazine != null ? ItemType.Magazine :
               Newspaper != null ? ItemType.Newspaper :
               null;
    }
}
