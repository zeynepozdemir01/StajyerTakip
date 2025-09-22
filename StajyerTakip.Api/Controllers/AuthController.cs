using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StajyerTakip.Application.Interfaces;

namespace StajyerTakip.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IJwtTokenService _jwt;
    private readonly IRefreshTokenService _refresh;
    public AuthController(IJwtTokenService jwt, IRefreshTokenService refresh)
    { _jwt = jwt; _refresh = refresh; }

    public sealed record LoginRequest(string Username, string Password);
    public sealed record LoginResponse(string AccessToken, string RefreshToken, int ExpiresInSeconds);

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest req)
    {
        if (req.Username != "admin" || req.Password != "123456") return Unauthorized("Kullanıcı veya şifre hatalı");

        var access = _jwt.GenerateAccessToken(req.Username, new[] { "Admin" });
        var rt = _refresh.CreateRefreshToken(req.Username, HttpContext.Connection.RemoteIpAddress?.ToString());
        await _refresh.SaveAsync(rt);

        return Ok(new LoginResponse(access, rt.Token, _jwt.AccessTokenMinutes * 60));
    }

    public sealed record RefreshRequest(string RefreshToken);
    public sealed record RefreshResponse(string AccessToken, string RefreshToken, int ExpiresInSeconds);

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<ActionResult<RefreshResponse>> Refresh([FromBody] RefreshRequest req)
    {
        var token = await _refresh.GetAsync(req.RefreshToken);
        if (token is null || !token.IsActive) return Unauthorized("Refresh token geçersiz");

        var newAccess = _jwt.GenerateAccessToken(token.Username);
        var newRt = _refresh.CreateRefreshToken(token.Username, HttpContext.Connection.RemoteIpAddress?.ToString());
        await _refresh.RotateAsync(token, newRt, HttpContext.Connection.RemoteIpAddress?.ToString());

        return Ok(new RefreshResponse(newAccess, newRt.Token, _jwt.AccessTokenMinutes * 60));
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] RefreshRequest req)
    {
        var token = await _refresh.GetAsync(req.RefreshToken);
        if (token != null && token.IsActive)
            await _refresh.RevokeAsync(token, HttpContext.Connection.RemoteIpAddress?.ToString(), "Logout");
        return Ok(new { message = "ok" });
    }
}
