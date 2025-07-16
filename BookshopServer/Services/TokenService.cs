using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BookShopServer.Entities;
using BookShopServer.Exceptions;
using BookShopServer.Repositories;
using BookShopServer.Repositories.UserRepositories;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace BookShopServer.Services;

public enum AccessLevel {
    Customer,
    Employee,
    Admin,
}

public class TokenService : ITokenService
{
    private readonly int _accessTokenExpirationMinutes;
    private readonly int _refreshTokenExpirationDays;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration,  IRefreshTokenRepository refreshTokenRepository, IUserRepository userRepository, int accessTokenMinutes = 60, int refreshTokenDays = 30)
    {
        _configuration = configuration;
        _accessTokenExpirationMinutes = accessTokenMinutes;
        _refreshTokenExpirationDays = refreshTokenDays;
        _refreshTokenRepository = refreshTokenRepository;
        _userRepository = userRepository;
    }

    public string GenerateAccessToken(int userId, string email, AccessLevel role)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? ""));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"] ?? "",
            audience: _configuration["Jwt:Audience"] ?? "",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> GenerateRefreshToken(int userId)
    {
        if (!await _userRepository.UserExistsAsync(userId))
            throw new NotFoundException("User with the given ID does not exist.");
        
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        var token = Convert.ToBase64String(randomBytes);

        if (await _refreshTokenRepository.TokenForUserExistsAsync(userId))
        {
            var refreshTokenEntity = await _refreshTokenRepository.GetRefreshTokenByUserIdAsync(userId);
            refreshTokenEntity.Token = token;
            refreshTokenEntity.Expiration = DateTime.UtcNow.AddDays(_refreshTokenExpirationDays);
            await _refreshTokenRepository.UpdateRefreshTokenAsync(refreshTokenEntity);
        }
        else
            await _refreshTokenRepository.AddRefreshTokenAsync(new RefreshToken
            {
                Token = token,
                UserId = userId,
                Expiration = DateTime.UtcNow.AddDays(_refreshTokenExpirationDays)
            });

        return token;
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "")),
            ValidateLifetime = false // Important: ignore expiration
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtToken ||
                !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;
        }
        catch
        {
            return null;
        }
    }
}