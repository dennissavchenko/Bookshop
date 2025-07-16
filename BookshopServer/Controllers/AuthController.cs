using System.Security.Claims;
using BookShopServer.DTOs;
using BookShopServer.Exceptions;
using BookShopServer.Services;
using BookShopServer.Services.UserServices;
using Microsoft.AspNetCore.Mvc;
using RefreshRequest = BookShopServer.DTOs.RefreshRequest;

namespace BookShopServer.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;
    private readonly IRefreshTokenService _refreshTokenService;
    
    public AuthController(ITokenService tokenService, IUserService userService, IRefreshTokenService refreshTokenService)
    {
        _tokenService = tokenService;
        _userService = userService;
        _refreshTokenService = refreshTokenService;
    }

    [HttpPost("employees/login")]
    public async Task<IActionResult> EmployeeLogin([FromBody] LogInRequest logInRequest)
    {
        try
        {
            var user = await _userService.ValidateUserAsync(logInRequest.UsernameOrEmail, logInRequest.Password);
            
            if (user == null)
                return Unauthorized("Invalid username or password.");
            
            if (user.Employee == null)
                return Unauthorized("User does not have employee access level.");
            
            var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Email, user.AccessLevel);
            var refreshToken = await _tokenService.GenerateRefreshToken(user.Id);
            
            return Ok(new RefreshRequest
            {
                Id = user.Id,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
            
        } catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpPost("customers/login")]
    public async Task<IActionResult> CustomerLogin([FromBody] LogInRequest logInRequest)
    {
        try
        {
            var user = await _userService.ValidateUserAsync(logInRequest.UsernameOrEmail, logInRequest.Password);
            
            if (user == null)
                return Unauthorized("Invalid username or password.");
            
            if (user.Customer == null)
                return Unauthorized("Access denied. Only customers can log in here.");
            
            var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Email, user.AccessLevel);
            var refreshToken = await _tokenService.GenerateRefreshToken(user.Id);
            
            return Ok(new RefreshRequest
            {
                Id = user.Id,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
            
        } catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
    {
        var principal = _tokenService.GetPrincipalFromExpiredToken(refreshRequest.AccessToken);
        
        if (principal == null)
            return Unauthorized("Invalid access token.");

        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = principal.FindFirst(ClaimTypes.Email)?.Value;
        var accessLevel = principal.FindFirst(ClaimTypes.Role)?.Value;
        if (userId == null || email == null || accessLevel == null)
            return Unauthorized("Invalid access token claims.");

        if (await _refreshTokenService.ValidateRefreshTokenAsync(int.Parse(userId), refreshRequest.RefreshToken))
        {
            var newAccessToken = _tokenService.GenerateAccessToken(int.Parse(userId), email, (AccessLevel) Enum.Parse(typeof(AccessLevel), accessLevel));
            var newRefreshToken = await _tokenService.GenerateRefreshToken(int.Parse(userId));
            return Ok(new RefreshRequest
            {
                Id = int.Parse(userId),
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
        return Unauthorized("Invalid or expired refresh token.");
    }
}
