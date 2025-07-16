using BookShopServer.DTOs.ItemDTOs;
using BookShopServer.DTOs.ItemDTOs.AddOrUpdateItem;

namespace BookShopServer.Services.ItemServices.ItemCondition;

/// <summary>
/// Defines the contract for services that manage the creation and updating of various new item types.
/// </summary>
public interface INewItemService
{
    /// <summary>
    /// Adds a new book to the system.
    /// </summary>
    /// <param name="newBookDto">The DTO containing the details of the new book.</param>
    Task AddNewBookAsync(NewBookDto newBookDto);

    /// <summary>
    /// Adds a new magazine to the system.
    /// </summary>
    /// <param name="newMagazineDto">The DTO containing the details of the new magazine.</param>
    Task AddNewMagazineAsync(NewMagazineDto newMagazineDto);

    /// <summary>
    /// Adds a new newspaper to the system.
    /// </summary>
    /// <param name="newNewspaperDto">The DTO containing the details of the new newspaper.</param>
    Task AddNewNewspaperAsync(NewNewspaperDto newNewspaperDto);

    /// <summary>
    /// Adds a new generic item to the system.
    /// </summary>
    /// <param name="newItemDto">The DTO containing the general details of the new item.</param>
    /// <returns>The ID of the newly added item.</returns>
    Task<int> AddNewItemAsync(NewItemDto newItemDto);

    /// <summary>
    /// Updates an existing book in the system.
    /// </summary>
    /// <param name="newBookDto">The DTO containing the updated details of the book.</param>
    Task UpdateNewBookAsync(UpdateNewBookDto newBookDto);

    /// <summary>
    /// Updates an existing magazine in the system.
    /// </summary>
    /// <param name="newMagazineDto">The DTO containing the updated details of the magazine.</param>
    Task UpdateNewMagazineAsync(NewMagazineDto newMagazineDto);

    /// <summary>
    /// Updates an existing newspaper in the system.
    /// </summary>
    /// <param name="newNewspaperDto">The DTO containing the updated details of the newspaper.</param>
    Task UpdateNewNewspaperAsync(NewNewspaperDto newNewspaperDto);

    /// <summary>
    /// Updates an existing generic item in the system.
    /// </summary>
    /// <param name="newItemDto">The DTO containing the updated general details of the item.</param>
    Task UpdateNewItemAsync(NewItemDto newItemDto);

    /// <summary>
    /// Retrieves a collection of all new items as simplified DTOs.
    /// </summary>
    /// <returns>A collection of <see cref="SimpleItemDto"/> representing all new items.</returns>
    Task<IEnumerable<SimpleItemDto>> GetAllNewItemsAsync();
}