using Microsoft.EntityFrameworkCore;
using StajyerTakip.Application.Interfaces;
using StajyerTakip.Domain.Entities;
using StajyerTakip.Infrastructure.Data;

namespace StajyerTakip.Infrastructure.Auth;

public sealed class RefreshTokenService : IRefreshTokenService
{
    private readonly AppDbContext _db;
    private readonly JwtSettings _opt;
    public RefreshTokenService(AppDbContext db, Microsoft.Extensions.Options.IOptions<JwtSettings> opt)
    { _db = db; _opt = opt.Value; }

    public RefreshToken CreateRefreshToken(string username, string? ip)
        => new()
        {
            Username = username,
            Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
            CreatedAt = DateTime.UtcNow,
            CreatedByIp = ip,
            ExpiresAt = DateTime.UtcNow.AddDays(_opt.RefreshTokenDays)
        };

    public Task<RefreshToken?> GetAsync(string token)
        => _db.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);

    public async Task SaveAsync(RefreshToken token)
    {
        _db.RefreshTokens.Add(token);
        await _db.SaveChangesAsync();
    }

    public async Task RevokeAsync(RefreshToken token, string? ip, string? reason = null)
    {
        token.RevokedAt = DateTime.UtcNow;
        token.RevokedByIp = ip;
        await _db.SaveChangesAsync();
    }

    public async Task RotateAsync(RefreshToken oldToken, RefreshToken newToken, string? ip)
    {
        oldToken.RevokedAt = DateTime.UtcNow;
        oldToken.RevokedByIp = ip;
        oldToken.ReplacedByToken = newToken.Token;
        _db.RefreshTokens.Add(newToken);
        await _db.SaveChangesAsync();
    }
}
