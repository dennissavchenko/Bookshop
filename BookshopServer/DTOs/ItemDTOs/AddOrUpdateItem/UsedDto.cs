using System.ComponentModel.DataAnnotations;

namespace BookShopServer.DTOs.ItemDTOs.AddOrUpdateItem;

/// <summary>
/// Represents the data transfer object for a used item.
/// Usage: used as part of the AddOrUpdateItem DTO to add or update a used item.
/// </summary>
public class UsedDto
{
    [Required]
    [Range(0, 3)]
    public int Condition { get; set; }
    [Required]
    public bool HasAnnotations { get; set; }
}