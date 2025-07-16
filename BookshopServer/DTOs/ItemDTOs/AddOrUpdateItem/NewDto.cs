using System.ComponentModel.DataAnnotations;

namespace BookShopServer.DTOs.ItemDTOs.AddOrUpdateItem;

/// <summary>
/// Represents the data transfer object for a new item.
/// Usage: used as part of the AddOrUpdateItem DTO to add or update a used item.
/// </summary>
public class NewDto
{
    [Required]
    public bool IsSealed { get; set; }
}