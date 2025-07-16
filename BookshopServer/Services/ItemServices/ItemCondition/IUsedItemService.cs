using BookShopServer.DTOs.ItemDTOs;
using BookShopServer.DTOs.ItemDTOs.AddOrUpdateItem;

namespace BookShopServer.Services.ItemServices.ItemCondition;

/// <summary>
/// Defines the contract for services that manage the creation and updating of various used item types.
/// </summary>
public interface IUsedItemService
{
    /// <summary>
    /// Adds a used book to the system.
    /// </summary>
    /// <param name="usedBookDto">The DTO containing the details of the used book.</param>
    Task AddUsedBookAsync(UsedBookDto usedBookDto);

    /// <summary>
    /// Adds a used magazine to the system.
    /// </summary>
    /// <param name="usedMagazineDto">The DTO containing the details of the used magazine.</param>
    Task AddUsedMagazineAsync(UsedMagazineDto usedMagazineDto);

    /// <summary>
    /// Adds a used newspaper to the system.
    /// </summary>
    /// <param name="usedNewspaperDto">The DTO containing the details of the used newspaper.</param>
    Task AddUsedNewspaperAsync(UsedNewspaperDto usedNewspaperDto);

    /// <summary>
    /// Adds a used generic item to the system.
    /// </summary>
    /// <param name="usedItemDto">The DTO containing the general details of the used item.</param>
    /// <returns>The ID of the newly added item.</returns>
    Task<int> AddUsedItemAsync(UsedItemDto usedItemDto);

    /// <summary>
    /// Updates an existing used book in the system.
    /// </summary>
    /// <param name="usedBookDto">The DTO containing the updated details of the used book.</param>
    Task UpdateUsedBookAsync(UpdateUsedBookDto usedBookDto);

    /// <summary>
    /// Updates an existing used magazine in the system.
    /// </summary>
    /// <param name="usedMagazineDto">The DTO containing the updated details of the used magazine.</param>
    Task UpdateUsedMagazineAsync(UsedMagazineDto usedMagazineDto);

    /// <summary>
    /// Updates an existing used newspaper in the system.
    /// </summary>
    /// <param name="usedNewspaperDto">The DTO containing the updated details of the used newspaper.</param>
    Task UpdateUsedNewspaperAsync(UsedNewspaperDto usedNewspaperDto);

    /// <summary>
    /// Updates an existing generic used item in the system.
    /// </summary>
    /// <param name="usedItemDto">The DTO containing the updated general details of the used item.</param>
    Task UpdateUsedItemAsync(UsedItemDto usedItemDto);

    /// <summary>
    /// Retrieves a collection of all used items as simplified DTOs.
    /// </summary>
    /// <returns>A collection of <see cref="SimpleItemDto"/> representing all used items.</returns>
    Task<IEnumerable<SimpleItemDto>> GetAllUsedItemsAsync();
}