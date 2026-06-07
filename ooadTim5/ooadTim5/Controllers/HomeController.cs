using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ooadTim5.Models;
using ooadTim5.Services;
using System.Diagnostics;

namespace ooadTim5.Controllers
{
    public class HomeController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly EmailService _emailService;

        public HomeController(SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            EmailService emailService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailService = emailService;
        }

        public IActionResult ONama() => View();

        [HttpGet]
        public IActionResult Prijava() => View();

        [HttpPost]
        public async Task<IActionResult> Prijava(string email, string lozinka)
        {
            var result = await _signInManager.PasswordSignInAsync(email, lozinka, false, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Greska = "Pogrešan email ili lozinka. Provjerite da li ste potvrdili email!";
            return View();
        }

        public async Task<IActionResult> Odjava()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Registracija() => View();

        [HttpPost]
        public async Task<IActionResult> Registracija(string email, string lozinka, string potvrda)
        {
            if (lozinka != potvrda)
            {
                ViewBag.Greska = "Lozinke se ne podudaraju.";
                return View();
            }

            var user = new IdentityUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, lozinka);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "clan");
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var link = Url.Action("PotvrdiEmail", "Home",
                    new { userId = user.Id, token = token }, Request.Scheme);

                await _emailService.PosaljiPotvrdu(email, link!);

                ViewBag.Poruka = "Registracija uspješna! Provjerite email i kliknite na link za potvrdu.";
                return View();
            }

            ViewBag.Greska = string.Join(", ", result.Errors.Select(e => e.Description));
            return View();
        }

        public async Task<IActionResult> PotvrdiEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Greska = "Greška pri potvrdi emaila.";
            return View("Registracija");
        }

        public IActionResult Index() => View();
        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}