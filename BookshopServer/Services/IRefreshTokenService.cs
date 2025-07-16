namespace BookShopServer.Services;

public interface IRefreshTokenService
{
    Task<bool> ValidateRefreshTokenAsync(int userId, string refreshToken);
}