using BookShopServer.DTOs.ItemDTOs;
using BookShopServer.Repositories.ItemRepositories;

namespace BookShopServer.Services.ItemServices.ItemType;

/// <summary>
/// Provides services for managing newspaper-specific operations within the bookshop system.
/// </summary>
public class NewspaperService : INewspaperService
{
    private readonly IItemRepository _itemRepository;
    
    /// <summary>
    /// Initializes a new instance of the <c>NewspaperService</c> class.
    /// </summary>
    /// <param name="itemRepository">The repository for accessing item data.</param>
    public NewspaperService(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }
    
    /// <summary>
    /// Retrieves a collection of all newspapers from the system, mapped to simplified DTOs.
    /// This method filters all items to include only those that are newspapers.
    /// </summary>
    /// <returns>A collection of <see cref="SimpleItemDto"/> representing all newspapers.</returns>
    public async Task<IEnumerable<SimpleItemDto>> GetAllNewspapersAsync()
    {
        // Retrieve all items from the repository
        var allItems = await _itemRepository.GetAllItemsAsync();
        
        // Filter the items to include only newspapers and map them to SimpleItemDto
        return SimpleItemDto.MapItems(allItems.Where(x => x.Newspaper != null));
    }
}