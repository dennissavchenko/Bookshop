using BookShopServer.DTOs.ReviewDTOs;
using BookShopServer.Entities;

namespace BookShopServer.DTOs.ItemDTOs;

/// <summary>
/// Represents a data transfer object for an item.
/// Usage: display detailed information about an item.
/// </summary>
public class ItemDto : SimpleItemDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public DateTime PublishingDate { get; set; }
    public string Language { get; set; }
    public double Price { get; set; }
    public int AmountInStock { get; set; }
    public int PublisherId { get; set; }
    public string PublisherName { get; set; }
    public int AgeCategoryId { get; set; }
    public int AgeCategory { get; set; }
    public double AverageRating { get; set; }
    public IEnumerable<ReviewForItemDto> Reviews { get; set; }
    public string? Type { get; set; } // "Book", "Magazine", "Newspaper" or null
    public ICollection<string>? Authors { get; set; }
    public ICollection<string>? Genres { get; set; }
    public int? NumberOfPages { get; set; }
    public string? CoverType { get; set; }
    public bool? IsSpecialEdition { get; set; }
    public string? Headline { get; set; }
    public ICollection<string>? Topics { get; set; }
    public bool IsUsed { get; set; } // "New" or "Used"
    public bool? IsSealed { get; set; }
    public string? Condition { get; set; }
    public bool? HasAnnotations { get; set; }

    /// <summary>
    /// Maps an Item entity to an ItemDto.
    /// </summary>
    /// <param name="item">Item entity</param>
    /// <returns>ItemDto object mapped from Item entity </returns>
    public static ItemDto MapToItemDto(Item item)
    {
        return new ItemDto
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            ImageUrl = item.ImageUrl,
            PublishingDate = item.PublishingDate,
            Language = item.Language,
            Price = item.Price,
            AmountInStock = item.AmountInStock,
            PublisherId = item.PublisherId,
            PublisherName = item.Publisher.Name,
            AgeCategoryId = item.AgeCategoryId,
            AgeCategory = item.AgeCategory.MinimumAge,
            AverageRating = item.GetAverageRating(),
            Reviews = item.Reviews.Select(x => new ReviewForItemDto
            {
                Id = x.Id,
                Username = x.Customer.User.Username,
                Rating = x.Rating,
                Text = x.Text,
                TimeStamp = x.TimeStamp,
                ItemId = x.ItemId,
                CustomerId = x.CustomerId
            }),
            Type = item.GetItemType().ToString(),
            NumberOfPages = item.Book?.NumberOfPages,
            CoverType = item.Book?.CoverType.ToString(),
            Authors = item.Book?.Authors.Select(x => x.Pseudonym ?? $"{x.Name} {x.Surname}").ToList(),
            Genres = item.Book?.Genres.Select(x => x.Name).ToList(),
            IsSpecialEdition = item.Magazine?.IsSpecialEdition,
            Headline = item.Newspaper?.Headline,
            Topics = item.Newspaper?.Topics.ToList(),
            IsUsed = item.GetIsUsed()
        };
    }
}

