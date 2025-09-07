namespace StajyerTakip.Domain.Entities;

public class Intern
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName  { get; set; } = null!;
    public string NationalId { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string? School { get; set; }
    public string? Department { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string Status { get; set; } = "Aktif";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
