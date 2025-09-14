using StajyerTakip.Application.Interfaces;

public sealed class DemoUserRepository : IUserRepository
{
    private sealed record DemoUser(int Id, string Email, string PasswordHash, string Role);

    private static readonly List<DemoUser> _users =
    [
        new(1, "admin@demo.com", "admin123", "Admin"),
        new(2, "user@demo.com",  "user123",  "User")
    ];

    public Task<(int Id, string Email, string PasswordHash, string Role)?> GetByEmailAsync(string email)
    {
        var u = _users.FirstOrDefault(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        if (u is null) return Task.FromResult<(int, string, string, string)?>(null);
        return Task.FromResult<(int, string, string, string)?>((u.Id, u.Email, u.PasswordHash, u.Role));
    }
}
