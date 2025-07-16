using BookShopServer.DTOs;
using BookShopServer.Entities;
using BookShopServer.Exceptions;
using BookShopServer.Repositories;

namespace BookShopServer.Services;

/// <summary>
/// Provides services for managing genre-related operations, including retrieving,
/// adding, updating, and deleting genres for books.
/// </summary>
public class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;
    
    /// <summary>
    /// Initializes a new instance of the <c>GenreService</c> class.
    /// </summary>
    /// <param name="genreRepository">The repository for genre data.</param>
    public GenreService(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }
    
    /// <summary>
    /// Retrieves a genre by its unique identifier and maps it to a <see cref="GenreDto"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the genre.</param>
    /// <returns>A <see cref="GenreDto"/> representing the genre.</returns>
    /// <exception cref="NotFoundException">Thrown if the genre with the given ID does not exist.</exception>
    public async Task<GenreDto> GetGenreByIdAsync(int id)
    {
        // Retrieve the genre entity from the repository
        var genre = await _genreRepository.GetGenreByIdAsync(id);
        
        // Check if the genre exists
        if (genre == null)
            throw new NotFoundException("Genre with the given ID does not exist.");

        // Map the genre entity to a GenreDto
        return new GenreDto
        {
            Id = genre.Id,
            Name = genre.Name,
            Description = genre.Description
        };
    }
    
    /// <summary>
    /// Retrieves a collection of all genres and maps them to <see cref="GenreDto"/>s.
    /// </summary>
    /// <returns>A collection of <see cref="GenreDto"/> representing all genres.</returns>
    public async Task<IEnumerable<GenreDto>> GetAllGenresAsync()
    {
        // Retrieve all genre entities from the repository
        var genres = await _genreRepository.GetAllGenresAsync();

        // Map each genre entity to a GenreDto and convert the result to a list
        return genres.Select(g => new GenreDto
        {
            Id = g.Id,
            Name = g.Name,
            Description = g.Description
        }).ToList();
    }
    
    /// <summary>
    /// Adds a new genre to the system.
    /// </summary>
    /// <param name="genreDto">The <see cref="GenreDto"/> containing the details of the genre to add.</param>
    public async Task AddGenreAsync(GenreDto genreDto)
    {
        // Create a new Genre entity from the DTO
        var newGenre = new Genre
        {
            Name = genreDto.Name,
            Description = genreDto.Description
        };
        
        // Add the new genre to the repository
        await _genreRepository.AddGenreAsync(newGenre);
    }
    
    /// <summary>
    /// Updates an existing genre's details.
    /// </summary>
    /// <param name="genreDto">The <see cref="GenreDto"/> containing the updated details of the genre.</param>
    /// <exception cref="NotFoundException">Thrown if the genre with the given ID does not exist.</exception>
    public async Task UpdateGenreAsync(GenreDto genreDto)
    {
        // Create a Genre entity from the DTO.
        var updatingGenre = await _genreRepository.GetGenreByIdAsync(genreDto.Id);
        
        // Check if the genre exists
        if (updatingGenre == null)
            throw new NotFoundException("Genre with the given ID does not exist.");

        updatingGenre.Name = genreDto.Name;
        updatingGenre.Description = genreDto.Description;
        
        // Update the genre in the repository
        await _genreRepository.UpdateGenreAsync(updatingGenre);
    }
    
    /// <summary>
    /// Deletes a genre from the system by its unique identifier.
    /// A genre cannot be deleted if it is the sole genre assigned to one or more books.
    /// </summary>
    /// <param name="id">The unique identifier of the genre to delete.</param>
    /// <exception cref="NotFoundException">Thrown if the genre with the given ID does not exist.</exception>
    /// <exception cref="ConflictException">Thrown if the genre is the only genre of any associated books, preventing deletion.</exception>
    public async Task DeleteGenreAsync(int id)
    {
        // Check if the genre exists before attempting deletion
        if (!await _genreRepository.GenreExistsAsync(id))
            throw new NotFoundException("Genre with the given ID does not exist.");
        
        // Prevent deletion if this genre is the only one assigned to any book.
        // This ensures books always have at least one genre.
        if (await _genreRepository.IsOnlyGenreOfAnyBookAsync(id))
            throw new ConflictException("This genre is the only genre of one or more books and cannot be deleted.");

        // Delete the genre from the repository
        await _genreRepository.DeleteGenreAsync(id);
    }

    /// <inheridoc />
    public async Task<IEnumerable<BriefEntityDto>> GetFilteredGenresAsync(string searchTerm)
    {
        // Retrieve filtered genres entities from the repository
        var genres = await _genreRepository.GetFilteredGenresAsync(searchTerm);

        // Map each entity to a DTO and convert the result to a list
        return genres.Select(p => new BriefEntityDto
        {
            Id = p.Id,
            Name = p.Name
        }).ToList();
    }
}