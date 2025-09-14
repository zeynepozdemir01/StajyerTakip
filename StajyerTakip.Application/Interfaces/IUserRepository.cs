public interface IUserRepository
{
    Task<(int Id, string Email, string PasswordHash, string Role)?> GetByEmailAsync(string email);
}
