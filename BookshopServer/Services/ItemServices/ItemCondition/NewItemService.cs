using BookShopServer.DTOs.ItemDTOs;
using BookShopServer.DTOs.ItemDTOs.AddOrUpdateItem;
using BookShopServer.Entities;
using BookShopServer.Exceptions;
using BookShopServer.Repositories;
using BookShopServer.Repositories.ItemRepositories.ItemCondition;
using BookShopServer.Repositories.ItemRepositories.ItemType;

namespace BookShopServer.Services.ItemServices.ItemCondition;

/// <summary>
/// Provides services for managing new item entities in the bookshop system, including books, magazines, and newspapers.
/// </summary>
public class NewItemService : INewItemService
{
    private readonly INewItemRepository _newItemRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IMagazineRepository _magazineRepository;
    private readonly INewspaperRepository _newspaperRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IAgeCategoryRepository _ageCategoryRepository;
    private readonly IPublisherRepository _publisherRepository;
    
    /// <summary>
    /// Initializes a new instance of the <c>NewItemService</c> class.
    /// </summary>
    /// <param name="newItemRepository">The repository for new items.</param>
    /// <param name="bookRepository">The repository for books.</param>
    /// <param name="magazineRepository">The repository for magazines.</param>
    /// <param name="newspaperRepository">The repository for newspapers.</param>
    /// <param name="authorRepository">The repository for authors.</param>
    /// <param name="genreRepository">The repository for genres.</param>
    /// <param name="ageCategoryRepository">The repository for age categories.</param>
    /// <param name="publisherRepository">The repository for publishers.</param>
    public NewItemService(INewItemRepository newItemRepository, IBookRepository bookRepository, 
                          IMagazineRepository magazineRepository, INewspaperRepository newspaperRepository, 
                          IAuthorRepository authorRepository, IGenreRepository genreRepository,
                          IAgeCategoryRepository ageCategoryRepository, IPublisherRepository publisherRepository)
    {
        _newItemRepository = newItemRepository;
        _bookRepository = bookRepository;
        _magazineRepository = magazineRepository;
        _newspaperRepository = newspaperRepository;
        _authorRepository = authorRepository;
        _genreRepository = genreRepository;
        _ageCategoryRepository = ageCategoryRepository;
        _publisherRepository = publisherRepository;
    }
    
    /// <summary>
    /// Adds a new generic item to the system.
    /// Performs validation for age category, publisher existence, and publishing date.
    /// </summary>
    /// <param name="newItemDto">The DTO containing the general details of the new item.</param>
    /// <returns>The ID of the newly added item.</returns>
    /// <exception cref="NotFoundException">Thrown if the specified age category or publisher does not exist.</exception>
    /// <exception cref="BadRequestException">Thrown if the publishing date is in the future.</exception>
    public async Task<int> AddNewItemAsync(NewItemDto newItemDto)
    {
        // Validate Age Category and Publisher existence
        if (!await _ageCategoryRepository.AgeCategoryExistsAsync(newItemDto.AgeCategoryId))
            throw new NotFoundException("Age category with the given ID does not exist.");
        if (!await _publisherRepository.PublisherExistsAsync(newItemDto.PublisherId))
            throw new NotFoundException("Publisher with the given ID does not exist.");
        
        // Validate Publishing Date
        if (newItemDto.PublishingDate > DateTime.Now)
            throw new BadRequestException("Publishing date cannot be in the future.");
        
        // Map DTO to entity and add to repository
        var newItem = MapToNewItem(newItemDto);
        return await _newItemRepository.AddNewItemAsync(newItem);
    }
    
    /// <summary>
    /// Updates an existing generic item in the system.
    /// Performs validation for item existence, age category, publisher existence, and publishing date.
    /// </summary>
    /// <param name="newItemDto">The DTO containing the updated general details of the item.</param>
    /// <exception cref="NotFoundException">Thrown if the new item, age category, or publisher does not exist.</exception>
    /// <exception cref="BadRequestException">Thrown if the publishing date is in the future.</exception>
    public async Task UpdateNewItemAsync(NewItemDto newItemDto)
    {
        // Validate New Item existence
        if(!await _newItemRepository.NewItemExistsAsync(newItemDto.Id))
            throw new NotFoundException("New item with the given ID does not exist.");
        
        // Validate Age Category and Publisher existence
        if (!await _ageCategoryRepository.AgeCategoryExistsAsync(newItemDto.AgeCategoryId))
            throw new NotFoundException("Age category with the given ID does not exist.");
        if (!await _publisherRepository.PublisherExistsAsync(newItemDto.PublisherId))
            throw new NotFoundException("Publisher with the given ID does not exist.");
        
        // Validate Publishing Date
        if (newItemDto.PublishingDate > DateTime.Now)
            throw new BadRequestException("Publishing date cannot be in the future.");
        
        // Map DTO to entity, set ID, and update in repository
        var newItem = MapToNewItem(newItemDto);
        newItem.Id = newItemDto.Id;
        await _newItemRepository.UpdateNewItemAsync(newItem);
    }
    
    /// <summary>
    /// Adds a new book to the system.
    /// Inherits base item validation from <see cref="AddNewItemAsync"/>.
    /// Validates the existence of authors and genres before associating them with the book.
    /// </summary>
    /// <param name="newBookDto">The DTO containing the details of the new book.</param>
    /// <exception cref="NotFoundException">Thrown if any specified author or genre does not exist.</exception>
    public async Task AddNewBookAsync(NewBookDto newBookDto)
    {
        // Validate if all the genres specified exist
        var existingGenres = await _genreRepository.GetExistingGenresAsync(newBookDto.Book.GenresIds);
        if (existingGenres.Count != newBookDto.Book.GenresIds.Count)
            throw new NotFoundException("One or more genres specified do not exist.");
        
        // Validate if all the authors specified exist
        var existingAuthors = await _authorRepository.GetExistingAuthorsAsync(newBookDto.Book.AuthorsIds);
        if (existingAuthors.Count != newBookDto.Book.AuthorsIds.Count)
            throw new NotFoundException("One or more authors specified do not exist.");
        
        // Add the base new item and get its ID
        var newBookId = await AddNewItemAsync(newBookDto);
        
        // Add book-specific details
        await _bookRepository.AddBookAsync(new Book
        {
            ItemId = newBookId,
            NumberOfPages = newBookDto.Book.NumberOfPages,
            CoverType = (CoverType) newBookDto.Book.CoverType
        });
        
        // Associate existing genres and authors with the new book
        await _bookRepository.AddGenresToBookAsync(newBookId, existingGenres);
        await _bookRepository.AddAuthorsToBookAsync(newBookId, existingAuthors);
    }
    
    /// <summary>
    /// Updates an existing book in the system.
    /// Inherits base item update validation from <see cref="UpdateNewItemAsync"/>.
    /// </summary>
    /// <param name="newBookDto">The DTO containing the updated details of the book.</param>
    public async Task UpdateNewBookAsync(UpdateNewBookDto newBookDto)
    {
        // Update the base new item
        await UpdateNewItemAsync(newBookDto);
        
        // Update book-specific details
        await _bookRepository.UpdateBookAsync(new Book
        {
            ItemId = newBookDto.Id,
            NumberOfPages = newBookDto.Book.NumberOfPages,
            CoverType = (CoverType) newBookDto.Book.CoverType
        });
    }
    
    /// <summary>
    /// Adds a new magazine to the system.
    /// Inherits base item validation from <see cref="AddNewItemAsync"/>.
    /// </summary>
    /// <param name="newMagazineDto">The DTO containing the details of the new magazine.</param>
    public async Task AddNewMagazineAsync(NewMagazineDto newMagazineDto)
    {
        // Add the base new item and get its ID
        var newMagazineId = await AddNewItemAsync(newMagazineDto);
        
        // Add magazine-specific details
        await _magazineRepository.AddMagazineAsync(new Magazine
        {
            ItemId = newMagazineId,
            IsSpecialEdition = newMagazineDto.Magazine.IsSpecialEdition
        });
    }
    
    /// <summary>
    /// Updates an existing magazine in the system.
    /// Inherits base item update validation from <see cref="UpdateNewItemAsync"/>.
    /// </summary>
    /// <param name="newMagazineDto">The DTO containing the updated details of the magazine.</param>
    public async Task UpdateNewMagazineAsync(NewMagazineDto newMagazineDto)
    {
        // Update the base new item
        await UpdateNewItemAsync(newMagazineDto);
        
        // Update magazine-specific details
        await _magazineRepository.UpdateMagazineAsync(new Magazine
        {
            ItemId = newMagazineDto.Id,
            IsSpecialEdition = newMagazineDto.Magazine.IsSpecialEdition
        });
    }
    
    /// <summary>
    /// Adds a new newspaper to the system.
    /// Inherits base item validation from <see cref="AddNewItemAsync"/>.
    /// </summary>
    /// <param name="newNewspaperDto">The DTO containing the details of the new newspaper.</param>
    public async Task AddNewNewspaperAsync(NewNewspaperDto newNewspaperDto)
    {
        // Add the base new item and get its ID
        var newNewspaperId = await AddNewItemAsync(newNewspaperDto);
        
        // Add newspaper-specific details
        await _newspaperRepository.AddNewspaperAsync(new Newspaper
        {
            ItemId = newNewspaperId,
            Headline = newNewspaperDto.Newspaper.Headline,
            Topics = newNewspaperDto.Newspaper.Topics
        });
    }
    
    /// <summary>
    /// Updates an existing newspaper in the system.
    /// Inherits base item update validation from <see cref="UpdateNewItemAsync"/>.
    /// </summary>
    /// <param name="newNewspaperDto">The DTO containing the updated details of the newspaper.</param>
    public async Task UpdateNewNewspaperAsync(NewNewspaperDto newNewspaperDto)
    {
        // Update the base new item
        await UpdateNewItemAsync(newNewspaperDto);
        
        // Update newspaper-specific details
        await _newspaperRepository.UpdateNewspaperAsync(new Newspaper
        {
            ItemId = newNewspaperDto.Id,
            Headline = newNewspaperDto.Newspaper.Headline,
            Topics = newNewspaperDto.Newspaper.Topics
        });
    }
    
    /// <summary>
    /// Retrieves a collection of all new items as simplified DTOs.
    /// Includes publisher name, average rating, and for books, authors and genres.
    /// </summary>
    /// <returns>A collection of <see cref="SimpleItemDto"/> representing all new items.</returns>
    public async Task<IEnumerable<SimpleItemDto>> GetAllNewItemsAsync()
    {
        var newItems = await _newItemRepository.GetAllNewItemsAsync();
        return newItems.Select(item => new SimpleItemDto
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
    /// Maps a <see cref="NewItemDto"/> to a <see cref="NewItem"/> entity.
    /// </summary>
    /// <param name="newItemDto">The DTO to map.</param>
    /// <returns>A new <see cref="NewItem"/> entity.</returns>
    private NewItem MapToNewItem(NewItemDto newItemDto)
    {
        return new NewItem
        {
            Name = newItemDto.Name,
            Description = newItemDto.Description,
            ImageUrl = newItemDto.ImageUrl,
            PublishingDate = newItemDto.PublishingDate,
            Language = newItemDto.Language,
            Price = newItemDto.Price,
            AmountInStock = newItemDto.AmountInStock,
            AgeCategoryId = newItemDto.AgeCategoryId,
            PublisherId = newItemDto.PublisherId,
            IsSealed = newItemDto.New.IsSealed
        };
    }
}