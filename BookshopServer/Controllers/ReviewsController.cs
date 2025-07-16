using BookShopServer.DTOs;
using BookShopServer.DTOs.ReviewDTOs;
using BookShopServer.Exceptions;
using BookShopServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookShopServer.Controllers;

/// <summary>
/// Controller for managing user reviews.
/// Provides endpoints to create, retrieve, update, and delete reviews.
/// </summary>
[ApiController]
[Route("api/reviews")]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;

    /// <summary>
    /// Initializes a new instance of the <c>ReviewsController</c> class.
    /// </summary>
    /// <param name="reviewService">Service for review-related operations.</param>
    public ReviewsController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    /// <summary>
    /// Retrieves a review by its ID.
    /// </summary>
    /// <param name="id">The ID of the review.</param>
    /// <returns>
    /// A 200 OK response with the review;
    /// or 404 Not Found if the review does not exist.
    /// </returns>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetReviewByIdAsync(int id)
    {
        try
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            return Ok(review);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    /// Retrieves all reviews.
    /// </summary>
    /// <returns>
    /// A 200 OK response containing a list of all reviews.
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> GetAllReviewsAsync()
    {
        var reviews = await _reviewService.GetAllReviewsAsync();
        return Ok(reviews);
    }

    /// <summary>
    /// Creates a new review.
    /// </summary>
    /// <param name="reviewDto">The review data to add.</param>
    /// <returns>
    /// A 201 Created response if successful;
    /// 404 Not Found if the referenced customer or item does not exist;
    /// 400 Bad Request if the input is invalid.
    /// </returns>
    [HttpPost]
    public async Task<IActionResult> AddReviewAsync([FromBody] AddReviewDto reviewDto)
    {
        try
        {
            await _reviewService.AddReviewAsync(reviewDto);
            return StatusCode(StatusCodes.Status201Created);
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
    /// Updates an existing review.
    /// </summary>
    /// <param name="reviewDto">The updated review data.</param>
    /// <returns>
    /// A 204 No Content response if successful;
    /// or 404 Not Found if the review does not exist.
    /// </returns>
    [HttpPut]
    public async Task<IActionResult> UpdateReviewAsync([FromBody] UpdateReviewDto reviewDto)
    {
        try
        {
            await _reviewService.UpdateReviewAsync(reviewDto);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    /// Deletes a review by ID.
    /// </summary>
    /// <param name="id">The ID of the review to delete.</param>
    /// <returns>
    /// A 204 No Content response if successful;
    /// or 404 Not Found if the review does not exist.
    /// </returns>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteReviewAsync(int id)
    {
        try
        {
            await _reviewService.DeleteReviewAsync(id);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}
