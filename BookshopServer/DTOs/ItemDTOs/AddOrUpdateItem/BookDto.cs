using System.ComponentModel.DataAnnotations;

namespace BookShopServer.DTOs.ItemDTOs.AddOrUpdateItem;

/// <summary>
/// Represents a custom validation attribute to ensure that a collection of IDs is valid.
/// </summary>
public class ValidIdList : ValidationAttribute
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value">Integer collection to be validated</param>
    /// <returns>
    /// true if the collection is valid (contains at least one ID);
    /// false otherwise
    /// </returns>
    public override bool IsValid(object? value)
    {
        // Check if the value is a collection of integers and if it contains at least one item
        if (value is not ICollection<int> list || list.Count < 1)
            return false;

        return true;
    }
}

/// <summary>
/// Represents the data transfer object for a book item.
/// Usage: used as part of the AddOrUpdateItem DTO to add a new book.
/// </summary>
public class BookDto : UpdateBookDto
{
    [Required]
    [ValidIdList]
    public ICollection<int> AuthorsIds { get; set; }
    [Required]
    [ValidIdList]
    public ICollection<int> GenresIds { get; set; }
}

/// <summary>
/// Represents the data transfer object for updating a book item.
/// Usage: used as part of the AddOrUpdateItem DTO to update an existing book.
/// </summary>
public class UpdateBookDto
{
    [Required]
    [Range(1, 10000)]
    public int NumberOfPages { get; set; }
    [Required]
    [Range(0, 2)]
    public int CoverType { get; set; }
}