using BookShopServer.DTOs.ItemDTOs;
using BookShopServer.DTOs.OrderDTOs;
using BookShopServer.Entities;
using BookShopServer.Exceptions;
using BookShopServer.Repositories;
using BookShopServer.Repositories.ItemRepositories;
using BookShopServer.Repositories.ItemRepositories.ItemCondition;
using BookShopServer.Repositories.OrderRepositories;

namespace BookShopServer.Services.ItemServices;

/// <summary>
/// Provides services for managing general item operations within the bookshop system,
/// including retrieving, filtering, and deleting items, as well as fetching related orders.
/// </summary>
public class ItemService : IItemService
{
    private readonly IItemRepository _itemRepository;
    private readonly IUsedItemRepository _usedItemRepository;
    private readonly INewItemRepository _newItemRepository;
    private readonly IAgeCategoryRepository _ageCategoryRepository;
    private readonly IPublisherRepository _publisherRepository;
    private readonly IOrderRepository _orderRepository;

    /// <summary>
    /// Initializes a new instance of the <c>ItemService</c> class.
    /// </summary>
    /// <param name="itemRepository">The general item repository.</param>
    /// <param name="usedItemRepository">The repository for used items.</param>
    /// <param name="newItemRepository">The repository for new items.</param>
    /// <param name="ageCategoryRepository">The repository for age categories.</param>
    /// <param name="publisherRepository">The repository for publishers.</param>
    /// <param name="orderRepository">The repository for orders.</param>
    public ItemService(IItemRepository itemRepository, IUsedItemRepository usedItemRepository, INewItemRepository newItemRepository,
        IAgeCategoryRepository ageCategoryRepository, IPublisherRepository publisherRepository, IOrderRepository orderRepository)
    {
        _itemRepository = itemRepository;
        _usedItemRepository = usedItemRepository;
        _newItemRepository = newItemRepository;
        _ageCategoryRepository = ageCategoryRepository;
        _publisherRepository = publisherRepository;
        _orderRepository = orderRepository;
    }

    /// <summary>
    /// Retrieves a single item by its ID, distinguishing between new and used items and mapping to an <see cref="ItemDto"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the item.</param>
    /// <returns>An <see cref="ItemDto"/> containing the item's details.</returns>
    /// <exception cref="NotFoundException">Thrown if no item with the given ID exists.</exception>
    public async Task<ItemDto> GetItemByIdAsync(int id)
    {
        // Attempt to retrieve the item as a UsedItem
        UsedItem? item = await _usedItemRepository.GetUsedItemByIdAsync(id);
        
        // If it's a UsedItem, map and return its details
        if (item != null)
        {
            var usedItemDto = ItemDto.MapToItemDto(item);
            usedItemDto.Condition = item.Condition.ToString();
            usedItemDto.HasAnnotations = item.HasAnnotations;
            return usedItemDto;
        }
        
        // If not a UsedItem, attempt to retrieve it as a NewItem
        NewItem? newItem = await _newItemRepository.GetNewItemByIdAsync(id);
        
        // If neither, throw NotFoundException
        if (newItem == null)
            throw new NotFoundException("Item with the given ID does not exist.");

        // If it's a NewItem, map and return its details
        var newItemDto = ItemDto.MapToItemDto(newItem);
        newItemDto.IsSealed = newItem.IsSealed;
        return newItemDto;
    }

    /// <summary>
    /// Retrieves a collection of all items in the system, mapped to simplified DTOs.
    /// </summary>
    /// <returns>A collection of <see cref="SimpleItemDto"/> representing all items.</returns>
    public async Task<IEnumerable<SimpleItemDto>> GetAllItemsAsync()
    {
        var items = await _itemRepository.GetAllItemsAsync();
        return SimpleItemDto.MapItems(items);
    }
    
    /// <summary>
    /// Retrieves a collection of items published by a specific publisher, mapped to simplified DTOs.
    /// </summary>
    /// <param name="publisherId">The ID of the publisher.</param>
    /// <returns>A collection of <see cref="SimpleItemDto"/> representing items from the specified publisher.</returns>
    /// <exception cref="NotFoundException">Thrown if the specified publisher does not exist.</exception>
    public async Task<IEnumerable<SimpleItemDto>> GetItemsByPublisherIdAsync(int publisherId)
    {
        // Check if the publisher exists
        if (!await _publisherRepository.PublisherExistsAsync(publisherId))
            throw new NotFoundException("Publisher with the given ID does not exist.");
        
        // Retrieve items by publisher and map to SimpleItemDto
        var items = await _itemRepository.GetItemsByPublisherAsync(publisherId);
        return SimpleItemDto.MapItems(items);
    }
    
    /// <summary>
    /// Retrieves a collection of items belonging to a specific age category, mapped to simplified DTOs.
    /// </summary>
    /// <param name="ageCategoryId">The ID of the age category.</param>
    /// <returns>A collection of <see cref="SimpleItemDto"/> representing items in the specified age category.</returns>
    /// <exception cref="NotFoundException">Thrown if the specified age category does not exist.</exception>
    public async Task<IEnumerable<SimpleItemDto>> GetItemsByAgeCategoryIdAsync(int ageCategoryId)
    {
        // Check if the age category exists
        if (!await _ageCategoryRepository.AgeCategoryExistsAsync(ageCategoryId))
            throw new NotFoundException("Age category with the given ID does not exist.");
        
        // Retrieve items by age category and map to SimpleItemDto
        var items = await _itemRepository.GetItemsByAgeCategoryAsync(ageCategoryId);
        return SimpleItemDto.MapItems(items);
    }

    /// <summary>
    /// Deletes an item from the system by its ID.
    /// </summary>
    /// <param name="id">The ID of the item to delete.</param>
    /// <exception cref="NotFoundException">Thrown if no item with the given ID exists.</exception>
    public async Task DeleteItemAsync(int id)
    {
        // Check if the item exists before attempting to delete
        if(!await _itemRepository.ItemExistsAsync(id)) 
            throw new NotFoundException("Item with the given ID does not exist.");
        
        // Delete the item from the repository
        await _itemRepository.DeleteItemAsync(id);
    }
    
    /// <summary>
    /// Retrieves a collection of simple order DTOs associated with a specific item.
    /// </summary>
    /// <param name="itemId">The ID of the item to retrieve orders for.</param>
    /// <returns>A collection of <see cref="SimpleOrderDto"/> associated with the item.</returns>
    /// <exception cref="NotFoundException">Thrown if the specified item does not exist.</exception>
    public async Task<IEnumerable<SimpleOrderDto>> GetOrdersByItemIdAsync(int itemId)
    {
        // Check if the item exists
        if (!await _itemRepository.ItemExistsAsync(itemId))
            throw new NotFoundException("Item with the given ID does not exist.");
        
        // Retrieve orders by item ID and map to SimpleOrderDto
        var orders = await _orderRepository.GetOrdersByItemIdAsync(itemId);
        return orders.Select(o => new SimpleOrderDto
        {
            Id = o.Id,
            TotalPrice = o.GetTotalPrice(),
            LastUpdatedAt = o.GetLastUpdatedAt(),
            Status = o.OrderStatus.ToString(),
            CustomerId = o.CustomerId,
        });
    }
    
    /// <summary>
    /// Retrieves a collection of items appropriate for a specific age as simplified DTOs.
    /// </summary>
    /// <param name="age">The age to filter items by.</param>
    /// <returns>
    /// A collection of <see cref="SimpleItemDto"/> representing items appropriate for the specified age.
    /// </returns>
    public async Task<IEnumerable<SimpleItemDto>> GetItemsAppropriateForAgeAsync(int age)
    {
        // Retrieve items appropriate for the specified age and map to SimpleItemDto
        if(age < 0)
            throw new BadRequestException("Age cannot be negative.");
        var items = await _itemRepository.GetItemsAppropriateForAgeAsync(age);
        return SimpleItemDto.MapItems(items);
    }
    
}