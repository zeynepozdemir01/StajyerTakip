using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using StajyerTakip.Validation;

namespace StajyerTakip.Models
{
    [Index(nameof(Status), Name = "IX_Interns_Status")]
    [Index(nameof(Email), Name = "UX_Interns_Email", IsUnique = true)]
    [Index(nameof(NationalId), Name = "UX_Interns_NationalId", IsUnique = true)]
    public partial class Intern
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad zorunludur.")]
        [StringLength(50, ErrorMessage = "Ad en fazla 50 karakter olabilir.")]
        [Display(Name = "Ad")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Soyad zorunludur.")]
        [StringLength(50, ErrorMessage = "Soyad en fazla 50 karakter olabilir.")]
        [Display(Name = "Soyad")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "TC Kimlik No zorunludur.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "TC Kimlik No 11 haneli rakamlardan oluşmalıdır.")]
        [TcKimlikNo(ErrorMessage = "Geçersiz TC Kimlik Numarası.")]
        [Display(Name = "TC Kimlik No")]
        public string NationalId { get; set; } = null!;

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [StringLength(255)]
        [Display(Name = "E-posta")]
        public string Email { get; set; } = null!;

        [StringLength(30)]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        [Display(Name = "Telefon")]
        public string? Phone { get; set; }

        [StringLength(100)]
        [Display(Name = "Okul")]
        public string? School { get; set; }

        [StringLength(100)]
        [Display(Name = "Bölüm")]
        public string? Department { get; set; }

        [Required(ErrorMessage = "Başlangıç Tarihi zorunludur.")]
        [DataType(DataType.Date)]
        [Display(Name = "Başlangıç Tarihi")]
        public DateOnly StartDate { get; set; }

        [DataType(DataType.Date)]
        [DateOnlyGreaterOrEqual(nameof(StartDate), ErrorMessage = "Bitiş tarihi başlangıçtan önce olamaz.")]
        [Display(Name = "Bitiş Tarihi")]
        public DateOnly? EndDate { get; set; }

        [Required(ErrorMessage = "Durum zorunludur.")]
        [StringLength(10)]
        [Display(Name = "Durum")]
        public string Status { get; set; } = "Aktif";

        // DB tarafında default verebilirsin; burada da başlatıyoruz.
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
