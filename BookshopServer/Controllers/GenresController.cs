using BookShopServer.DTOs;
using BookShopServer.Exceptions;
using BookShopServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookShopServer.Controllers;

/// <summary>
/// Controller for managing book genres.
/// Provides endpoints for retrieving, creating, updating, and deleting genres.
/// </summary>
[ApiController]
[Route("api/genres")]
public class GenresController : ControllerBase
{
    private readonly IGenreService _genreService;

    /// <summary>
    /// Initializes a new instance of the <c>GenresController</c> class.
    /// </summary>
    /// <param name="genreService">Service for genre-related operations.</param>
    public GenresController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    /// <summary>
    /// Retrieves a genre by its ID.
    /// </summary>
    /// <param name="id">The ID of the genre.</param>
    /// <returns>
    /// A 200 OK response with the genre data;
    /// or a 404 Not Found response if the genre does not exist.
    /// </returns>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetGenreById(int id)
    {
        try
        {
            var genre = await _genreService.GetGenreByIdAsync(id);
            return Ok(genre);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    /// Retrieves all available genres.
    /// </summary>
    /// <returns>
    /// A 200 OK response containing the list of all genres.
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> GetAllGenres()
    {
        var genres = await _genreService.GetAllGenresAsync();
        return Ok(genres);
    }

    /// <summary>
    /// Creates a new genre.
    /// </summary>
    /// <param name="genre">The genre data to create.</param>
    /// <returns>
    /// A 201 Created response if the genre was successfully created.
    /// </returns>
    [HttpPost]
    public async Task<IActionResult> AddGenre([FromBody] GenreDto genre)
    {
        await _genreService.AddGenreAsync(genre);
        return StatusCode(StatusCodes.Status201Created);
    }

    /// <summary>
    /// Updates an existing genre.
    /// </summary>
    /// <param name="genre">The updated genre data.</param>
    /// <returns>
    /// A 204 No Content response on success;
    /// or 404 Not Found if the genre does not exist.
    /// </returns>
    [HttpPut]
    public async Task<IActionResult> UpdateGenre([FromBody] GenreDto genre)
    {
        try
        {
            await _genreService.UpdateGenreAsync(genre);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    /// Deletes a genre by ID.
    /// </summary>
    /// <param name="id">The ID of the genre to delete.</param>
    /// <returns>
    /// A 204 No Content response if the deletion is successful;
    /// a 404 Not Found response if the genre does not exist;
    /// or a 409 Conflict response if the deletion is not possible due to dependencies (trying to delete only genre of a book).
    /// </returns>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteGenre(int id)
    {
        try
        {
            await _genreService.DeleteGenreAsync(id);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ConflictException e)
        {
            return Conflict(e.Message);
        }
    }
    
    /// <summary>
    /// Retrieves filtered genres.
    /// </summary>
    /// <returns>A 200 OK response containing the list of filtered genres.</returns>
    [HttpGet("search")]
    public async Task<IActionResult> GetFilteredGenresAsync([FromQuery] string searchTerm)
    {
        var genres = await _genreService.GetFilteredGenresAsync(searchTerm);
        return Ok(genres);
    }
    
}
