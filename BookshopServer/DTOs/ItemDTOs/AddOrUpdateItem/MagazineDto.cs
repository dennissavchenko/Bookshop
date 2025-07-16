using System.ComponentModel.DataAnnotations;

namespace BookShopServer.DTOs.ItemDTOs.AddOrUpdateItem;

/// <summary>
/// Represents the data transfer object for a magazine item.
/// Usage: used as part of the AddOrUpdateItem DTO to add or update a magazine item.
/// </summary>
public class MagazineDto
{
    [Required]
    public bool IsSpecialEdition { get; set; }
}