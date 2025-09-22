namespace StajyerTakip.Application.Interfaces;

public interface IJwtTokenService
{
    string GenerateAccessToken(string username, IEnumerable<string>? roles = null);
    int AccessTokenMinutes { get; }
}
