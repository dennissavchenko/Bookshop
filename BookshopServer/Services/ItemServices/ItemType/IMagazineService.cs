using BookShopServer.DTOs.ItemDTOs;

namespace BookShopServer.Services.ItemServices.ItemType;

/// <summary>
/// Defines the contract for services that manage magazine-specific operations.
/// </summary>
public interface IMagazineService
{
    /// <summary>
    /// Retrieves a collection of all magazines as simplified DTOs.
    /// </summary>
    /// <returns>A collection of <see cref="SimpleItemDto"/> representing all magazines.</returns>
    Task<IEnumerable<SimpleItemDto>> GetAllMagazinesAsync();
}