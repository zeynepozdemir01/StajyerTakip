using MediatR;
using StajyerTakip.Application.Common;

namespace StajyerTakip.Application.Auth;

public sealed record LoginCommand(string Email, string Password)
    : IRequest<Result<string>>;
