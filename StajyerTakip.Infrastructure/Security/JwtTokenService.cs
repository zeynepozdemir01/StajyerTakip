using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StajyerTakip.Application.Interfaces;
using StajyerTakip.Domain.Entities;

namespace StajyerTakip.Infrastructure.Security;

public class JwtTokenService : ITokenService
{
    private readonly IConfiguration _config;

    public JwtTokenService(IConfiguration config)
    {
        _config = config;
    }

    public string CreateToken(User user, DateTime now)
    {
        // appsettings: "Jwt": { "Key": "...","Issuer":"...","Audience":"...", "ExpiresMinutes": "60" }
        var jwtSection = _config.GetSection("Jwt");
        var key = jwtSection["Key"] ?? throw new InvalidOperationException("Jwt:Key missing.");
        var issuer = jwtSection["Issuer"] ?? "StajyerTakip";
        var audience = jwtSection["Audience"] ?? "StajyerTakip.Client";
        var expiresMinutesStr = jwtSection["ExpiresMinutes"];
        var expiresMinutes = string.IsNullOrWhiteSpace(expiresMinutesStr) ? 60 : int.Parse(expiresMinutesStr);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Role, user.Role ?? "User")
        };

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: now,
            expires: now.AddMinutes(expiresMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
