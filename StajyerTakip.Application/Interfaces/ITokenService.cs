using StajyerTakip.Domain.Entities;

namespace StajyerTakip.Application.Interfaces;

public interface ITokenService
{
    string CreateToken(User user, DateTime now);
}
