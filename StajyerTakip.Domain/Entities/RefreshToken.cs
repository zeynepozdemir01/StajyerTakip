namespace StajyerTakip.Domain.Entities;

public class RefreshToken
{
    public int Id { get; set; }
    public string Username { get; set; } = default!;
    public string Token { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }  
    public DateTime CreatedAt { get; set; }  
    public string? CreatedByIp { get; set; }

    public DateTime? RevokedAt { get; set; } 
    public string? RevokedByIp { get; set; }
    public string? ReplacedByToken { get; set; }

    public bool IsActive => RevokedAt == null && DateTime.UtcNow < ExpiresAt;
}
