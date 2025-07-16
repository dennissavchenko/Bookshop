using BookShopServer.Repositories;

namespace BookShopServer.Services;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    
    public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
    }
    
    public async Task<bool> ValidateRefreshTokenAsync(int userId, string refreshToken)
    {
        var token = await _refreshTokenRepository.GetRefreshTokenAsync(refreshToken);
        if (token == null || token.UserId != userId || token.Expiration < DateTime.UtcNow) 
            return false;
        return true;
    }
    
}