using BookShopServer.DTOs.ItemDTOs;
using BookShopServer.DTOs.ItemDTOs.AddOrUpdateItem;
using BookShopServer.Entities;
using BookShopServer.Exceptions;
using BookShopServer.Repositories;
using BookShopServer.Repositories.ItemRepositories.ItemCondition;
using BookShopServer.Repositories.ItemRepositories.ItemType;

namespace BookShopServer.Services.ItemServices.ItemCondition;

/// <summary>
/// Provides services for managing used item entities in the bookshop system, including books, magazines, and newspapers.
/// </summary>
public class UsedItemService : IUsedItemService
{
    private readonly IUsedItemRepository _usedItemRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IMagazineRepository _magazineRepository;
    private readonly INewspaperRepository _newspaperRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IAgeCategoryRepository _ageCategoryRepository;
    private readonly IPublisherRepository _publisherRepository;
    
    /// <summary>
    /// Initializes a new instance of the <c>UsedItemService</c> class.
    /// </summary>
    /// <param name="usedItemRepository">The repository for used items.</param>
    /// <param name="bookRepository">The repository for books.</param>
    /// <param name="magazineRepository">The repository for magazines.</param>
    /// <param name="newspaperRepository">The repository for newspapers.</param>
    /// <param name="authorRepository">The repository for authors.</param>
    /// <param name="genreRepository">The repository for genres.</param>
    /// <param name="ageCategoryRepository">The repository for age categories.</param>
    /// <param name="publisherRepository">The repository for publishers.</param>
    public UsedItemService(IUsedItemRepository usedItemRepository, IBookRepository bookRepository, 
        IMagazineRepository magazineRepository, INewspaperRepository newspaperRepository,
        IAuthorRepository authorRepository, IGenreRepository genreRepository,
        IAgeCategoryRepository ageCategoryRepository, IPublisherRepository publisherRepository)
    {
        _usedItemRepository = usedItemRepository;
        _bookRepository = bookRepository;
        _magazineRepository = magazineRepository;
        _newspaperRepository = newspaperRepository;
        _authorRepository = authorRepository;
        _genreRepository = genreRepository;
        _ageCategoryRepository = ageCategoryRepository;
        _publisherRepository = publisherRepository;
    }
    
    /// <summary>
    /// Adds a new used generic item to the system.
    /// Performs validation for age category, publisher existence, and publishing date.
    /// </summary>
    /// <param name="usedItemDto">The DTO containing the general details of the used item.</param>
    /// <returns>The ID of the newly added item.</returns>
    /// <exception cref="NotFoundException">Thrown if the specified age category or publisher does not exist.</exception>
    /// <exception cref="BadRequestException">Thrown if the publishing date is in the future.</exception>
    public async Task<int> AddUsedItemAsync(UsedItemDto usedItemDto)
    {
        // Validate Age Category and Publisher existence
        if (!await _ageCategoryRepository.AgeCategoryExistsAsync(usedItemDto.AgeCategoryId))
            throw new NotFoundException("Age category with the given ID does not exist.");
        if (!await _publisherRepository.PublisherExistsAsync(usedItemDto.PublisherId))
            throw new NotFoundException("Publisher with the given ID does not exist.");
        
        // Validate Publishing Date
        if (usedItemDto.PublishingDate > DateTime.Now)
            throw new BadRequestException("Publishing date cannot be in the future.");
        
        // Map DTO to entity and add to repository
        var usedItem = MapToUsedItem(usedItemDto);
        return await _usedItemRepository.AddUsedItemAsync(usedItem);
    }

    /// <summary>
    /// Updates an existing used generic item in the system.
    /// Performs validation for item existence, age category, publisher existence, and publishing date.
    /// </summary>
    /// <param name="usedItemDto">The DTO containing the updated general details of the used item.</param>
    /// <exception cref="NotFoundException">Thrown if the used item, age category, or publisher does not exist.</exception>
    /// <exception cref="BadRequestException">Thrown if the publishing date is in the future.</exception>
    public async Task UpdateUsedItemAsync(UsedItemDto usedItemDto)
    {
        // Validate Used Item existence
        if (!await _usedItemRepository.UsedItemExistsAsync(usedItemDto.Id))
            throw new NotFoundException("Used item with the given ID does not exist.");
        
        // Validate Age Category and Publisher existence
        if (!await _ageCategoryRepository.AgeCategoryExistsAsync(usedItemDto.AgeCategoryId))
            throw new NotFoundException("Age category with the given ID does not exist.");
        if (!await _publisherRepository.PublisherExistsAsync(usedItemDto.PublisherId))
            throw new NotFoundException("Publisher with the given ID does not exist.");
        
        // Validate Publishing Date
        if (usedItemDto.PublishingDate > DateTime.Now)
            throw new BadRequestException("Publishing date cannot be in the future.");
        
        // Map DTO to entity, set ID, and update in repository
        var usedItem = MapToUsedItem(usedItemDto);
        usedItem.Id = usedItemDto.Id;
        await _usedItemRepository.UpdateUsedItemAsync(usedItem);
    }

    /// <summary>
    /// Adds a used book to the system.
    /// Inherits base item validation from <see cref="AddUsedItemAsync"/>.
    /// Validates the existence of authors and genres before associating them with the book.
    /// </summary>
    /// <param name="usedBookDto">The DTO containing the details of the used book.</param>
    /// <exception cref="NotFoundException">Thrown if any specified author or genre does not exist.</exception>
    public async Task AddUsedBookAsync(UsedBookDto usedBookDto)
    {
        // Validate the existence of genres
        var existingGenres = await _genreRepository.GetExistingGenresAsync(usedBookDto.Book.GenresIds);
        if (existingGenres.Count != usedBookDto.Book.GenresIds.Count)
            throw new NotFoundException("One or more genres specified do not exist.");
        
        // Validate the existence of authors
        var existingAuthors = await _authorRepository.GetExistingAuthorsAsync(usedBookDto.Book.AuthorsIds);
        if (existingAuthors.Count != usedBookDto.Book.AuthorsIds.Count)
            throw new NotFoundException("One or more authors specified do not exist.");
        
        // Add the base used item and get its ID
        var usedBookId = await AddUsedItemAsync(usedBookDto);
        
        // Add book-specific details
        await _bookRepository.AddBookAsync(new Book
        {
            ItemId = usedBookId,
            NumberOfPages = usedBookDto.Book.NumberOfPages,
            CoverType = (CoverType) usedBookDto.Book.CoverType
        });
        
        // Associate genres and authors with the book
        await _bookRepository.AddGenresToBookAsync(usedBookId, existingGenres);
        await _bookRepository.AddAuthorsToBookAsync(usedBookId, existingAuthors);
    }
    
    /// <summary>
    /// Updates an existing used book in the system.
    /// Inherits base item update validation from <see cref="UpdateUsedItemAsync"/>.
    /// </summary>
    /// <param name="usedBookDto">The DTO containing the updated details of the used book.</param>
    public async Task UpdateUsedBookAsync(UpdateUsedBookDto usedBookDto)
    {
        // Update the base used item
        await UpdateUsedItemAsync(usedBookDto);
        
        // Update book-specific details
        await _bookRepository.UpdateBookAsync(new Book
        {
            ItemId = usedBookDto.Id,
            NumberOfPages = usedBookDto.Book.NumberOfPages,
            CoverType = (CoverType) usedBookDto.Book.CoverType
        });
    }
    
    /// <summary>
    /// Adds a used magazine to the system.
    /// Inherits base item validation from <see cref="AddUsedItemAsync"/>.
    /// </summary>
    /// <param name="usedMagazineDto">The DTO containing the details of the used magazine.</param>
    public async Task AddUsedMagazineAsync(UsedMagazineDto usedMagazineDto)
    {
        // Add the base used item and get its ID
        var usedItemId = await AddUsedItemAsync(usedMagazineDto);
        
        // Add magazine-specific details
        await _magazineRepository.AddMagazineAsync(new Magazine
        {
            ItemId = usedItemId,
            IsSpecialEdition = usedMagazineDto.Magazine.IsSpecialEdition
        });
    }
    
    /// <summary>
    /// Updates an existing used magazine in the system.
    /// Inherits base item update validation from <see cref="UpdateUsedItemAsync"/>.
    /// </summary>
    /// <param name="usedMagazineDto">The DTO containing the updated details of the used magazine.</param>
    public async Task UpdateUsedMagazineAsync(UsedMagazineDto usedMagazineDto)
    {
        // Update the base used item
        await UpdateUsedItemAsync(usedMagazineDto);
        
        // Update magazine-specific details
        await _magazineRepository.UpdateMagazineAsync(new Magazine
        {
            ItemId = usedMagazineDto.Id,
            IsSpecialEdition = usedMagazineDto.Magazine.IsSpecialEdition
        });
    }
    
    /// <summary>
    /// Adds a used newspaper to the system.
    /// Inherits base item validation from <see cref="AddUsedItemAsync"/>.
    /// </summary>
    /// <param name="usedNewspaperDto">The DTO containing the details of the used newspaper.</param>
    public async Task AddUsedNewspaperAsync(UsedNewspaperDto usedNewspaperDto)
    {
        // Add the base used item and get its ID
        var usedItemId = await AddUsedItemAsync(usedNewspaperDto);
        
        // Add newspaper-specific details
        await _newspaperRepository.AddNewspaperAsync(new Newspaper
        {
            ItemId = usedItemId,
            Headline = usedNewspaperDto.Newspaper.Headline,
            Topics = usedNewspaperDto.Newspaper.Topics.ToList()
        });
    }
    
    /// <summary>
    /// Updates an existing used newspaper in the system.
    /// Inherits base item update validation from <see cref="UpdateUsedItemAsync"/>.
    /// </summary>
    /// <param name="usedNewspaperDto">The DTO containing the updated details of the used newspaper.</param>
    public async Task UpdateUsedNewspaperAsync(UsedNewspaperDto usedNewspaperDto)
    {
        // Update the base used item
        await UpdateUsedItemAsync(usedNewspaperDto);
        
        // Update newspaper-specific details
        await _newspaperRepository.UpdateNewspaperAsync(new Newspaper
        {
            ItemId = usedNewspaperDto.Id,
            Headline = usedNewspaperDto.Newspaper.Headline,
            Topics = usedNewspaperDto.Newspaper.Topics.ToList()
        });
    }
    
    /// <summary>
    /// Retrieves a collection of all used items as simplified DTOs.
    /// Includes publisher name, average rating, and for books, authors and genres.
    /// </summary>
    /// <returns>A collection of <see cref="SimpleItemDto"/> representing all used items.</returns>
    public async Task<IEnumerable<SimpleItemDto>> GetAllUsedItemsAsync()
    {
        var usedItems = await _usedItemRepository.GetAllUsedItemsAsync();
        return usedItems.Select(item => new SimpleItemDto
        {
            Id = item.Id,
            Name = item.Name,
            ImageUrl = item.ImageUrl,
            Price = item.Price,
            PublisherName = item.Publisher.Name,
            AverageRating = item.GetAverageRating(),
            Authors = item.Book?.Authors.Select(a => a.Pseudonym ?? $"{a.Name} {a.Surname}").ToList(),
            Genres = item.Book?.Genres.Select(g => g.Name).ToList(),
        });
    }

    /// <summary>
    /// Maps a <see cref="UsedItemDto"/> to a <see cref="UsedItem"/> entity.
    /// </summary>
    /// <param name="usedItemDto">The DTO to map.</param>
    /// <returns>A new <see cref="UsedItem"/> entity.</returns>
    private UsedItem MapToUsedItem(UsedItemDto usedItemDto)
    {
        return new UsedItem
        {
            Name = usedItemDto.Name,
            Description = usedItemDto.Description,
            ImageUrl = usedItemDto.ImageUrl,
            PublishingDate = usedItemDto.PublishingDate,
            Language = usedItemDto.Language,
            Price = usedItemDto.Price,
            AmountInStock = usedItemDto.AmountInStock,
            AgeCategoryId = usedItemDto.AgeCategoryId,
            PublisherId = usedItemDto.PublisherId,
            Condition = (Condition) usedItemDto.Used.Condition,
            HasAnnotations = usedItemDto.Used.HasAnnotations
        };
    }
}