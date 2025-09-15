namespace StajyerTakip.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public string Role { get; set; } = "User";
    public string PasswordHash { get; set; } = default!; 
}
