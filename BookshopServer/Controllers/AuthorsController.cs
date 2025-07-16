using BookShopServer.DTOs;
using BookShopServer.Exceptions;
using BookShopServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookShopServer.Controllers;

/// <summary>
/// Controller for managing authors.
/// Provides endpoints for retrieving, creating, updating, and deleting authors.
/// </summary>
[ApiController]
[Route("api/authors")]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorService _authorService;

    /// <summary>
    /// Initializes a new instance of the <c>AuthorsController</c> class.
    /// </summary>
    /// <param name="authorService">The service used for author operations.</param>
    public AuthorsController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    /// <summary>
    /// Retrieves an author by ID.
    /// </summary>
    /// <param name="id">The ID of the author to retrieve.</param>
    /// <returns>
    /// A 200 OK response with the author if found;
    /// otherwise, a 404 Not Found response.
    /// </returns>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAuthorById(int id)
    {
        try
        {
            var author = await _authorService.GetAuthorByIdAsync(id);
            return Ok(author);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    /// Retrieves all authors.
    /// </summary>
    /// <returns>
    /// A 200 OK response containing the list of all authors.
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> GetAllAuthors()
    {
        var authors = await _authorService.GetAllAuthorsAsync();
        return Ok(authors);
    }

    /// <summary>
    /// Creates a new author.
    /// </summary>
    /// <param name="author">The author to create.</param>
    /// <returns>
    /// A 201 Created response if successful;
    /// or a 400 Bad Request response if the input is invalid (DOB in the future).
    /// </returns>
    [HttpPost]
    public async Task<IActionResult> AddAuthor([FromBody] AuthorDto author)
    {
        try
        {
            await _authorService.AddAuthorAsync(author);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Updates an existing author.
    /// </summary>
    /// <param name="author">The updated author data.</param>
    /// <returns>
    /// A 204 No Content response if the update is successful;
    /// a 404 Not Found response if the author does not exist;
    /// or a 400 Bad Request response if the input is invalid (DOB in the future).
    /// </returns>
    [HttpPut]
    public async Task<IActionResult> UpdateAuthor([FromBody] AuthorDto author)
    {
        try
        {
            await _authorService.UpdateAuthorAsync(author);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Deletes an author by ID.
    /// </summary>
    /// <param name="id">The ID of the author to delete.</param>
    /// <returns>
    /// A 204 No Content response if deletion is successful;
    /// a 404 Not Found response if the author does not exist;
    /// or a 409 Conflict response if deletion is not possible due to dependencies (the author is the only author of at least one book).
    /// </returns>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        try
        {
            await _authorService.DeleteAuthorAsync(id);
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
    /// Retrieves filtered authors.
    /// </summary>
    /// <returns>A 200 OK response containing the list of filtered authors.</returns>
    [HttpGet("search")]
    public async Task<IActionResult> GetFilteredAuthorsAsync([FromQuery] string searchTerm)
    {
        var authors = await _authorService.GetFilteredAuthorsAsync(searchTerm);
        return Ok(authors);
    }
    
}
