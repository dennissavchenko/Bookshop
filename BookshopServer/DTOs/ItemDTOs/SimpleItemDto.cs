using BookShopServer.Entities;

namespace BookShopServer.DTOs.ItemDTOs;

/// <summary>
/// Represents a simplified version of an item, suitable for listing or basic display.
/// Usage: display a list of items with essential information.
/// </summary>
public class SimpleItemDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public double Price { get; set; }
    public string PublisherName { get; set; }
    public double AverageRating { get; set; }
    public ICollection<string>? Authors { get; set; }
    public ICollection<string>? Genres { get; set; }
    
    /// <summary>
    /// Maps a collection of Item entities to a collection of SimpleItemDto.
    /// </summary>
    /// <param name="items">An enumerable collection of Item</param>
    /// <returns>
    /// An enumerable collection of SimpleItemDto objects, each representing an item with essential information.
    /// </returns>
    public static IEnumerable<SimpleItemDto> MapItems(IEnumerable<Item> items)
    {
        return items.Select(x => new SimpleItemDto
        {
            Id = x.Id,
            Name = x.Name,
            ImageUrl = x.ImageUrl,
            Price = x.Price,
            PublisherName = x.Publisher.Name,
            AverageRating = x.GetAverageRating(),
            Authors = x.Book?.Authors.Select(a => a.Pseudonym ?? $"{a.Name} {a.Surname}").ToList(),
            Genres = x.Book?.Genres.Select(g => g.Name).ToList(),
        });
    }
    
    /// <summary>
    /// Maps a single Item entity to a SimpleItemDto.
    /// </summary>
    /// <param name="item">Item to map</param>
    /// <returns>
    /// A SimpleItemDto object representing the item with essential information.
    /// </returns>
    public static SimpleItemDto MapItem(Item item)
    {
        return new SimpleItemDto
        {
            Id = item.Id,
            Name = item.Name,
            ImageUrl = item.ImageUrl,
            Price = item.Price,
            PublisherName = item.Publisher.Name,
            AverageRating = item.GetAverageRating(),
            Authors = item.Book?.Authors.Select(a => a.Pseudonym ?? $"{a.Name} {a.Surname}").ToList(),
            Genres = item.Book?.Genres.Select(g => g.Name).ToList(),
        };
    }
    
}