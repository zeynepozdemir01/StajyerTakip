using System.ComponentModel.DataAnnotations;

namespace StajyerTakip.Models.ViewModels
{
    public class LoginVm
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        public string Username { get; set; } = "";

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
