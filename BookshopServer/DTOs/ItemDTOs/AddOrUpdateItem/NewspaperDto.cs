using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace BookShopServer.DTOs.ItemDTOs.AddOrUpdateItem;

/// <summary>
/// Represents the data transfer object for a newspaper item.
/// Usage: used as part of the AddOrUpdateItem DTO to add or update a newspaper item.
/// </summary>
public class NewspaperDto
{
    [Required]
    public string Headline { get; set; }
    [Required]
    [ValidStringList(2000)]
    public ICollection<string> Topics { get; set; }
}

/// <summary>
/// Validates a collection of strings to be non-empty, non-whitespace, and within a specified JSON length limit.
/// </summary>
public class ValidStringList : ValidationAttribute
{
    private readonly int _maxJsonLength;

    /// <summary>
    /// Initializes a new instance of the <c>ValidStringList</c> class with a specified maximum JSON length.
    /// </summary>
    /// <param name="maxJsonLength">Maximum JSON length after conversion</param>
    public ValidStringList(int maxJsonLength)
    {
        _maxJsonLength = maxJsonLength;
    }
    
    /// <summary>
    /// Validates the input collection of strings.
    /// </summary>
    /// <param name="value">The input collection to be validated</param>
    /// <returns>
    /// true if collection is valid;
    /// otherwise - false
    /// </returns>
    public override bool IsValid(object value)
    {
        // Check if the value is a list of strings and if number of items is within the range
        if (value is not List<string> list || list.Count < 1 || list.Count > 10)
            return false;

        // Check if all strings in the list are non-empty and non-whitespace
        if (!list.All(q => !string.IsNullOrWhiteSpace(q)))
            return false;

        string json = JsonSerializer.Serialize(list);
        return json.Length <= _maxJsonLength;
    }
}