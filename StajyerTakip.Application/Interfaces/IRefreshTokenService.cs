using StajyerTakip.Domain.Entities;

namespace StajyerTakip.Application.Interfaces;

public interface IRefreshTokenService
{
    RefreshToken CreateRefreshToken(string username, string? ip);
    Task<RefreshToken?> GetAsync(string token);
    Task SaveAsync(RefreshToken token);
    Task RevokeAsync(RefreshToken token, string? ip, string? reason = null);
    Task RotateAsync(RefreshToken oldToken, RefreshToken newToken, string? ip);
}
