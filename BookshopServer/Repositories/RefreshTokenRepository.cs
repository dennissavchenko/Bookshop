using BookShopServer.Entities;
using BookShopServer.Exceptions;
using Microsoft.EntityFrameworkCore;
using RefreshToken = BookShopServer.Entities.RefreshToken;

namespace BookShopServer.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly Context _context;

    public RefreshTokenRepository(Context context)
    {
        _context = context;
    }

    public async Task<bool> TokenForUserExistsAsync(int userId)
    {
        return await _context.RefreshTokens.AnyAsync(x => x.UserId == userId);
    }

    public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
    {
        try
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        } catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UNIQUE constraint failed") == true)
        {
            throw new UniqueConstraintException("Refresh token already exists.");
        }
    }

    public async Task UpdateRefreshTokenAsync(RefreshToken refreshToken)
    {
        try
        {
            _context.RefreshTokens.Update(refreshToken);
            await _context.SaveChangesAsync();
        } catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UNIQUE constraint failed") == true)
        {
            throw new UniqueConstraintException("Refresh token already exists.");
        }
    }

    public Task<RefreshToken?> GetRefreshTokenAsync(string token)
    {
        return _context.RefreshTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Token == token);
    }

    public Task<RefreshToken> GetRefreshTokenByUserIdAsync(int userId)
    {
        return _context.RefreshTokens
            .FirstAsync(x => x.UserId == userId);
    }
}