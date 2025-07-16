using BookShopServer.DTOs.ItemDTOs;

namespace BookShopServer.Services.ItemServices.ItemType;

/// <summary>
/// Defines the contract for services that manage newspaper-specific operations.
/// </summary>
public interface INewspaperService
{
    /// <summary>
    /// Retrieves a collection of all newspapers as simplified DTOs.
    /// </summary>
    /// <returns>A collection of <see cref="SimpleItemDto"/> representing all newspapers.</returns>
    Task<IEnumerable<SimpleItemDto>> GetAllNewspapersAsync();
}