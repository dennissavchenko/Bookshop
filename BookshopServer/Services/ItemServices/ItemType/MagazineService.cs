using BookShopServer.DTOs;
using BookShopServer.DTOs.ItemDTOs;
using BookShopServer.Repositories.ItemRepositories;

namespace BookShopServer.Services.ItemServices.ItemType;

/// <summary>
/// Provides services for managing magazine-specific operations within the bookshop system.
/// </summary>
public class MagazineService : IMagazineService
{
    private readonly IItemRepository _itemRepository;
    
    /// <summary>
    /// Initializes a new instance of the <c>MagazineService</c> class.
    /// </summary>
    /// <param name="itemRepository">The repository for accessing item data.</param>
    public MagazineService(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }
    
    /// <summary>
    /// Retrieves a collection of all magazines from the system, mapped to simplified DTOs.
    /// This method filters all items to include only those that are magazines.
    /// </summary>
    /// <returns>A collection of <see cref="SimpleItemDto"/> representing all magazines.</returns>
    public async Task<IEnumerable<SimpleItemDto>> GetAllMagazinesAsync()
    {
        // Retrieve all items from the repository
        var allItems = await _itemRepository.GetAllItemsAsync();
        
        // Filter the items to include only magazines and map them to SimpleItemDto
        return SimpleItemDto.MapItems(allItems.Where(x => x.Magazine != null));
    }
}