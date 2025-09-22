using System;
using System.Threading.Tasks;
using StajyerTakip.Application.Interfaces; // <-- Application arayüzünü referansla

namespace StajyerTakip.Infrastructure.Repositories
{
    public sealed class DemoUserRepository : IUserRepository
    {
        // Demo: admin@example.com / 123456   ve   user@example.com / 123456
        public Task<(int Id, string Email, string PasswordHash, string Role)?> GetByEmailAsync(string email)
        {
            // Demo amaçlı şifre hash (123456) — gerçek projede BCrypt/Argon2 kullan
            const string demoHash = "8d969eef6ecad3c29a3a629280e686cff8"; // kısaltılmış; sende gerçek hash varsa onu kullan

            if (email.Equals("admin@example.com", StringComparison.OrdinalIgnoreCase))
                return Task.FromResult<(int, string, string, string)?>((1, "admin@example.com", demoHash, "Admin"));

            if (email.Equals("user@example.com", StringComparison.OrdinalIgnoreCase))
                return Task.FromResult<(int, string, string, string)?>((2, "user@example.com", demoHash, "User"));

            return Task.FromResult<(int, string, string, string)?>(null);
        }
    }
}
