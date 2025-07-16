using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookShopServer.DTOs.ItemDTOs.AddOrUpdateItem;

/// <summary>
/// Represents abstract data transfer object for adding or updating an item.
/// </summary>
public class AddOrUpdateItemDto
{
    public int Id { get; set; }
    [Required(AllowEmptyStrings = false)]
    [MaxLength(100)]
    public string Name { get; set; }
    [Required(AllowEmptyStrings = false)]
    [MaxLength(300)]
    public string Description { get; set; }
    [Required(AllowEmptyStrings = false)]
    [MaxLength(300)]
    public string ImageUrl { get; set; }
    [Required]
    public DateTime PublishingDate { get; set; }
    [Required(AllowEmptyStrings = false)]
    [MaxLength(50)]
    public string Language { get; set; }
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    public double Price { get; set; }
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Amount in stock cannot be negative.")]
    public int AmountInStock { get; set; }
    public int PublisherId { get; set; }
    public int AgeCategoryId { get; set; }
}

/// <summary>
/// Represents the data transfer object for adding a new book item.
/// </summary>
public class NewBookDto : NewItemDto
{
    [Required]
    public BookDto Book { get; set; }
}

/// <summary>
/// Represents the data transfer object for updating a new book item.
/// </summary>
public class UpdateNewBookDto : NewItemDto
{
    [Required]
    public UpdateBookDto Book { get; set; }
}

/// <summary>
/// Represents the data transfer object for adding or updating a new magazine item.
/// </summary>
public class NewMagazineDto : NewItemDto
{
    [Required]
    public MagazineDto Magazine { get; set; }
}

/// <summary>
/// Represents the data transfer object for adding or updating a new newspaper item.
/// </summary>
public class NewNewspaperDto : NewItemDto
{
    [Required]
    public NewspaperDto Newspaper { get; set; }
}

/// <summary>
/// Represents the data transfer object for adding or updating a new item.
/// </summary>
public class NewItemDto : AddOrUpdateItemDto
{
    [Required]
    public NewDto New { get; set; }
}

/// <summary>
/// Represents the data transfer object for adding a used book item.
/// </summary>
public class UsedBookDto : UsedItemDto
{
    [Required]
    public BookDto Book { get; set; }
}

/// <summary>
/// Represents the data transfer object for updating a used book item.
/// </summary>
public class UpdateUsedBookDto : UsedItemDto
{
    [Required]
    public UpdateBookDto Book { get; set; }
}

/// <summary>
/// Represents the data transfer object for adding or updating a used magazine item.
/// </summary>
public class UsedMagazineDto : UsedItemDto
{
    [Required]
    public MagazineDto Magazine { get; set; }
}

/// <summary>
/// Represents the data transfer object for adding or updating a used newspaper item.
/// </summary>
public class UsedNewspaperDto : UsedItemDto
{
    [Required]
    public NewspaperDto Newspaper { get; set; }
}

/// <summary>
/// Represents the data transfer object for adding or updating a used item.
/// </summary>
public class UsedItemDto : AddOrUpdateItemDto
{
    [Required]
    public UsedDto Used { get; set; }
}




