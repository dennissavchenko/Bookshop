using BookShopServer.Services.ItemServices.ItemType;
using Microsoft.AspNetCore.Mvc;

namespace BookShopServer.Controllers;

/// <summary>
/// Controller for managing newspaper items.
/// Provides an endpoint for retrieving all newspapers.
/// </summary>
[ApiController]
[Route("api/items/newspapers")]
public class NewspapersController : ControllerBase
{
    private readonly INewspaperService _newspaperService;

    /// <summary>
    /// Initializes a new instance of the <c>NewspapersController</c> class.
    /// </summary>
    /// <param name="newspaperService">Service for newspaper-related operations.</param>
    public NewspapersController(INewspaperService newspaperService)
    {
        _newspaperService = newspaperService;
    }

    /// <summary>
    /// Retrieves all newspapers.
    /// </summary>
    /// <returns>
    /// A 200 OK response containing the list of all newspapers.
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> GetAllNewspapers()
    {
        var newspapers = await _newspaperService.GetAllNewspapersAsync();
        return Ok(newspapers);
    }
}