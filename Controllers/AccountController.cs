using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StajyerTakip.Models.ViewModels;
using System.Security.Claims;

namespace StajyerTakip.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _cfg;
        public AccountController(IConfiguration cfg) => _cfg = cfg;

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
            => View(new LoginVm { ReturnUrl = returnUrl });

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVm vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var inputUser = (vm.Username ?? string.Empty).Trim();
            var inputPass = vm.Password ?? string.Empty;

            var cfgUser    = _cfg["Auth:Username"]    ?? string.Empty;
            var cfgPass    = _cfg["Auth:Password"]    ?? string.Empty;
            var cfgDisplay = _cfg["Auth:DisplayName"] ?? cfgUser;

            string? role = null;
            string? displayName = null;

            if (!string.IsNullOrEmpty(cfgUser) &&
                !string.IsNullOrEmpty(cfgPass) &&
                inputUser == cfgUser && inputPass == cfgPass)
            {
                role = "Admin";
                displayName = string.IsNullOrWhiteSpace(cfgDisplay) ? cfgUser : cfgDisplay;
            }
            else if (inputUser == "admin" && inputPass == "12345")
            {
                role = "Admin";
                displayName = "admin";
            }
            else if (inputUser == "editor" && inputPass == "12345")
            {
                role = "User";
                displayName = "editor";
            }

            if (role is null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
                return View(vm);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, inputUser),
                new Claim(ClaimTypes.Name, displayName!),
                new Claim(ClaimTypes.Role, role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = vm.RememberMe,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                });

            if (!string.IsNullOrWhiteSpace(vm.ReturnUrl) && Url.IsLocalUrl(vm.ReturnUrl))
                return Redirect(vm.ReturnUrl);

            return RedirectToAction("Index", "Interns");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied() => View();
    }
}
