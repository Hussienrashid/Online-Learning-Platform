using HW.Data;
using HW.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class TokenService
{
    private readonly IConfiguration _config;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppDbContext _db;

    public TokenService(IConfiguration config, UserManager<ApplicationUser> userManager, AppDbContext db)
    {
        _config = config;
        _userManager = userManager;
        _db = db;
    }

    public async Task<AuthResponse> CreateAuthTokensAsync(ApplicationUser user)
    {
        var jwt = _config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<System.Security.Claims.Claim>
{
            new System.Security.Claims.Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new System.Security.Claims.Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new System.Security.Claims.Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var minutes = int.Parse(jwt["AccessTokenMinutes"] ?? "15");
        var expiresUtc = DateTime.UtcNow.AddMinutes(minutes);

        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: expiresUtc,
            signingCredentials: creds
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        var refreshDays = int.Parse(jwt["RefreshTokenDays"] ?? "7");
        var refreshToken = GenerateSecureToken();
        var refreshExpires = DateTime.UtcNow.AddDays(refreshDays);

        // optional: one active refresh token per user (clean old)
        var old = await _db.RefreshTokens.Where(x => x.UserId == user.Id && x.RevokedUtc == null).ToListAsync();
        foreach (var t in old) t.RevokedUtc = DateTime.UtcNow;

        _db.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiresUtc = refreshExpires
        });

        await _db.SaveChangesAsync();

        return new AuthResponse(accessToken, expiresUtc, refreshToken, refreshExpires);
    }

    public async Task<AuthResponse?> RefreshAsync(string refreshToken)
    {
        var stored = await _db.RefreshTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Token == refreshToken);

        if (stored == null || !stored.IsActive || stored.User == null)
            return null;

        // rotate refresh token: revoke old and create new
        stored.RevokedUtc = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return await CreateAuthTokensAsync(stored.User);
    }

    public async Task<bool> RevokeAsync(string refreshToken)
    {
        var stored = await _db.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken);
        if (stored == null || stored.RevokedUtc != null) return false;

        stored.RevokedUtc = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }

    private static string GenerateSecureToken()
    {
        var bytes = new byte[64];
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToBase64String(bytes);
    }
}
