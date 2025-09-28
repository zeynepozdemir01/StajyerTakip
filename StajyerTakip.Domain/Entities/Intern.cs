using System.ComponentModel.DataAnnotations;

namespace StajyerTakip.Domain.Entities;

public class Intern
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ad alanı zorunludur.")]
    [StringLength(50, ErrorMessage = "Ad en fazla 50 karakter olabilir.")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Soyad alanı zorunludur.")]
    [StringLength(50, ErrorMessage = "Soyad en fazla 50 karakter olabilir.")]
    public string LastName  { get; set; } = null!;

    [Required(ErrorMessage = "TC Kimlik Numarası zorunludur.")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "TC Kimlik Numarası 11 haneli olmalıdır.")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "TC Kimlik sadece rakam içermelidir.")]
    public string NationalId { get; set; } = null!;

    [Required(ErrorMessage = "Email zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta giriniz.")]
    public string Email { get; set; } = null!;

    public string? Phone { get; set; }
    public string? School { get; set; }
    public string? Department { get; set; }

    [Required(ErrorMessage = "Başlangıç tarihi zorunludur.")]
    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    [Required]
    public string Status { get; set; } = "Aktif";

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
