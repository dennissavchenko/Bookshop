using BookShopServer.Entities;

namespace BookShopServer.Repositories;

public interface IRefreshTokenRepository
{
    Task<bool> TokenForUserExistsAsync(int userId);
    Task AddRefreshTokenAsync(RefreshToken refreshToken);
    Task UpdateRefreshTokenAsync(RefreshToken refreshToken);
    Task<RefreshToken?> GetRefreshTokenAsync(string token);
    Task<RefreshToken> GetRefreshTokenByUserIdAsync(int userId);
}