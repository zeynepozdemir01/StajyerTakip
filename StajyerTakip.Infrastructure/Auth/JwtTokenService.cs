using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StajyerTakip.Application.Interfaces;

namespace StajyerTakip.Infrastructure.Auth;

public sealed class JwtSettings
{
    public string Issuer { get; set; } = "StajyerTakip";
    public string Audience { get; set; } = "StajyerTakipClient";
    public string Secret { get; set; } = "change-me";
    public int AccessTokenMinutes { get; set; } = 20;
    public int RefreshTokenDays { get; set; } = 7;
}

public sealed class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _opt;
    public int AccessTokenMinutes => _opt.AccessTokenMinutes;
    public JwtTokenService(IOptions<JwtSettings> opt) => _opt = opt.Value;

    public string GenerateAccessToken(string username, IEnumerable<string>? roles = null)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, username),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
            new(ClaimTypes.Name, username)
        };
        if (roles != null)
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opt.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            issuer: _opt.Issuer,
            audience: _opt.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(_opt.AccessTokenMinutes),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}
