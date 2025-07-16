using System.Security.Claims;

namespace BookShopServer.Services;

public interface ITokenService
{
    string GenerateAccessToken(int userId, string userEmail, AccessLevel role);
    Task<string> GenerateRefreshToken(int userId);
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}