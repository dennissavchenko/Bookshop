using BookShopServer.Services.ItemServices.ItemType;
using Microsoft.AspNetCore.Mvc;

namespace BookShopServer.Controllers;

/// <summary>
/// Controller for managing magazine items.
/// Provides an endpoint for retrieving all magazines.
/// </summary>
[ApiController]
[Route("api/items/magazines")]
public class MagazinesController : ControllerBase
{
    private readonly IMagazineService _magazineService;

    /// <summary>
    /// Initializes a new instance of the <c>MagazinesController</c> class.
    /// </summary>
    /// <param name="magazineService">Service for magazine-related operations.</param>
    public MagazinesController(IMagazineService magazineService)
    {
        _magazineService = magazineService;
    }

    /// <summary>
    /// Retrieves all magazines.
    /// </summary>
    /// <returns>
    /// A 200 OK response containing the list of all magazines.
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> GetAllMagazines()
    {
        var magazines = await _magazineService.GetAllMagazinesAsync();
        return Ok(magazines);
    }
}