using System;
using System.Linq;
using System.Threading.Tasks;
using StajyerTakip.Application.Interfaces;

namespace StajyerTakip.Infrastructure.Security
{
    public class DemoUserRepository : IUserRepository
    {
        private static readonly (int Id, string Email, string PasswordHash, string Role)[] Users =
            new (int, string, string, string)[]
            {
                (1, "demo@stajyer.local", "Password123!", "Admin"),
            };

        public Task<(int Id, string Email, string PasswordHash, string Role)?> GetByEmailAsync(string email)
        {
            var match = Users.FirstOrDefault(
                u => string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrEmpty(match.Email))
                return Task.FromResult<(int, string, string, string)?>(null);

            return Task.FromResult<(int, string, string, string)?>(match);
        }
    }
}
