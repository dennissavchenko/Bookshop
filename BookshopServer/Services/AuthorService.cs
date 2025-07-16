using BookShopServer.DTOs;
using BookShopServer.Entities;
using BookShopServer.Exceptions;
using BookShopServer.Repositories;

namespace BookShopServer.Services;

/// <summary>
/// Provides services for managing author-related operations, including retrieving,
/// adding, updating, and deleting authors.
/// </summary>
public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;

    /// <summary>
    /// Initializes a new instance of the <c>AuthorService</c> class.
    /// </summary>
    /// <param name="authorRepository">The repository for author data.</param>
    public AuthorService(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    /// <summary>
    /// Retrieves an author by their unique identifier and maps it to an <see cref="AuthorDto"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the author.</param>
    /// <returns>An <see cref="AuthorDto"/> representing the author.</returns>
    /// <exception cref="NotFoundException">Thrown if the author with the given ID does not exist.</exception>
    public async Task<AuthorDto> GetAuthorByIdAsync(int id)
    {
        // Retrieve the author entity from the repository
        var author = await _authorRepository.GetAuthorByIdAsync(id);
        
        // Check if the author exists
        if (author == null)
            throw new NotFoundException("Author with the given ID does not exist.");

        // Map the author entity to an AuthorDto
        return new AuthorDto
        {
            Id = author.Id,
            Name = author.Name,
            Surname = author.Surname,
            DOB = author.DOB,
            Pseudonym = author.Pseudonym
        };
    }

    /// <summary>
    /// Retrieves a collection of all authors and maps them to <see cref="AuthorDto"/>s.
    /// </summary>
    /// <returns>A collection of <see cref="AuthorDto"/> representing all authors.</returns>
    public async Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync()
    {
        // Retrieve all author entities from the repository
        var authors = await _authorRepository.GetAllAuthorsAsync();

        // Map each author entity to an AuthorDto and convert the result to a list
        return authors.Select(a => new AuthorDto
        {
            Id = a.Id,
            Name = a.Name,
            Surname = a.Surname,
            DOB = a.DOB,
            Pseudonym = a.Pseudonym
        }).ToList();
    }

    /// <summary>
    /// Adds a new author to the system. Performs validation to ensure the Date of Birth is not in the future.
    /// </summary>
    /// <param name="authorDto">The <see cref="AuthorDto"/> containing the details of the author to add.</param>
    /// <exception cref="BadRequestException">Thrown if the Date of Birth is in the future.</exception>
    public async Task AddAuthorAsync(AuthorDto authorDto)
    {
        // Validate that the Date of Birth is not set in the future
        if (authorDto.DOB > DateTime.Now)
            throw new BadRequestException("Date of Birth cannot be in the future.");

        // Create a new Author entity from the DTO
        var newAuthor = new Author
        {
            Name = authorDto.Name,
            Surname = authorDto.Surname,
            DOB = authorDto.DOB,
            Pseudonym = authorDto.Pseudonym
        };

        // Add the new author to the repository
        await _authorRepository.AddAuthorAsync(newAuthor);
    }

    /// <summary>
    /// Updates an existing author's details. Performs validation for author existence
    /// and ensures the Date of Birth is not in the future.
    /// </summary>
    /// <param name="authorDto">The <see cref="AuthorDto"/> containing the updated details of the author.</param>
    /// <exception cref="NotFoundException">Thrown if the author with the given ID does not exist.</exception>
    /// <exception cref="BadRequestException">Thrown if the Date of Birth is in the future.</exception>
    public async Task UpdateAuthorAsync(AuthorDto authorDto)
    {
        // Validate that the Date of Birth is not set in the future
        if (authorDto.DOB > DateTime.Now)
            throw new BadRequestException("Date of Birth cannot be in the future.");

        // Create an Author entity from the DTO.
        var updatingAuthor = await _authorRepository.GetAuthorByIdAsync(authorDto.Id);
        
        // Check if the author exists before updating
        if (updatingAuthor == null)
            throw new NotFoundException("Author with the given ID does not exist.");
        
        updatingAuthor.Name = authorDto.Name;
        updatingAuthor.Surname = authorDto.Surname;
        updatingAuthor.DOB = authorDto.DOB;
        updatingAuthor.Pseudonym = authorDto.Pseudonym;

        // Update the author in the repository
        await _authorRepository.UpdateAuthorAsync(updatingAuthor);
    }

    /// <summary>
    /// Deletes an author from the system by their unique identifier.
    /// An author cannot be deleted if they are the only author assigned to one or more books.
    /// </summary>
    /// <param name="id">The unique identifier of the author to delete.</param>
    /// <exception cref="NotFoundException">Thrown if the author with the given ID does not exist.</exception>
    /// <exception cref="ConflictException">Thrown if the author is the only author of any associated books, preventing deletion.</exception>
    public async Task DeleteAuthorAsync(int id)
    {
        // Check if the author exists before attempting deletion
        if (!await _authorRepository.AuthorExistsAsync(id))
            throw new NotFoundException("Author with the given ID does not exist.");

        // Prevent deletion if this author is the only one assigned to any book.
        // This ensures books always have at least one author.
        if (await _authorRepository.IsOnlyAuthorOfAnyBookAsync(id))
            throw new ConflictException("Cannot delete the author as they are the only author of one or more books.");

        // Delete the author from the repository
        await _authorRepository.DeleteAuthorAsync(id);
    }
    
    /// <inheridoc />
    public async Task<IEnumerable<BriefEntityDto>> GetFilteredAuthorsAsync(string searchTerm)
    {
        // Retrieve filtered authors entities from the repository
        var authors = await _authorRepository.GetFilteredAuthorsAsync(searchTerm);

        // Map each entity to a DTO and convert the result to a list
        return authors.Select(a => new BriefEntityDto
        {
            Id = a.Id,
            Name = a.Pseudonym ?? $"{a.Name} {a.Surname}"
        }).ToList();
    }
    
}