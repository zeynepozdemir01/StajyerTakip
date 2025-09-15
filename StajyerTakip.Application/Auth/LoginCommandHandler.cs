using MediatR;
using StajyerTakip.Application.Common;
using StajyerTakip.Application.Interfaces;
using StajyerTakip.Domain.Entities;

namespace StajyerTakip.Application.Auth;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, Result<string>>
{
    private readonly IUserRepository _users;
    private readonly ITokenService _tokens;

    public LoginCommandHandler(IUserRepository users, ITokenService tokens)
    {
        _users = users;
        _tokens = tokens;
    }

    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken ct)
    {
        var userTuple = await _users.GetByEmailAsync(request.Email);

        if (userTuple is null || userTuple.Value.PasswordHash != request.Password)
            return Result<string>.Fail("Email veya şifre hatalı.");

        var user = new User
        {
            Id = userTuple.Value.Id,
            Email = userTuple.Value.Email,
            PasswordHash = userTuple.Value.PasswordHash,
            Role = userTuple.Value.Role
        };

        var token = _tokens.CreateToken(user, DateTime.UtcNow);
        return Result<string>.Ok(token);
    }
}
