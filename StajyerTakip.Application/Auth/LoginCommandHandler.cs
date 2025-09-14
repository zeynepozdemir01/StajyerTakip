using MediatR;
using StajyerTakip.Application.Common;
using StajyerTakip.Application.Interfaces;
using StajyerTakip.Domain.Entities;

namespace StajyerTakip.Application.Auth;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, Result<string>>
{
    private readonly IUserRepository _users;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(IUserRepository users, ITokenService tokenService)
    {
        _users = users;
        _tokenService = tokenService;
    }

    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken ct)
    {
        var userTuple = await _users.GetByEmailAsync(request.Email);

        if (userTuple is null)
            return Result<string>.Fail("Email veya şifre hatalı.");

        var u = userTuple.Value;

        if (u.PasswordHash != request.Password)
            return Result<string>.Fail("Email veya şifre hatalı.");

        var user = new User
        {
            Id = u.Id,
            Email = u.Email,
            PasswordHash = u.PasswordHash,
            Role = u.Role
        };

        var token = _tokenService.CreateToken(user, DateTime.UtcNow);

        return Result<string>.Ok(token);
    }
}
