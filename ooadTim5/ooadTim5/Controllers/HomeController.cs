using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ooadTim5.Services;

namespace ooadTim5.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly EmailService _emailService;

        public HomeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, EmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Registracija()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registracija(string email, string lozinka, string potvrda)
        {
            if (lozinka != potvrda)
            {
                ViewBag.Greska = "Lozinke se ne podudaraju.";
                return View();
            }

            var postojeciUser = await _userManager.FindByEmailAsync(email);
            if (postojeciUser != null && !postojeciUser.EmailConfirmed)
            {
                await _userManager.DeleteAsync(postojeciUser);
            }

            var user = new IdentityUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, lozinka);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "clan");
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var tokenEncoded = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(token));
                var link = Url.Action("PotvrdiEmail", "Home",
                    new { userId = user.Id, token = tokenEncoded },
                    protocol: "https",
                    host: "grupa5-tim2.onrender.com");

                try
                {
                    await _emailService.PosaljiPotvrdu(email, link!);
                    ViewBag.Poruka = "Registracija uspješna! Provjerite email i kliknite na link za potvrdu.";
                }
                catch
                {
                    ViewBag.Poruka = "Registracija uspješna, ali email nije poslan. Kontaktirajte administratora.";
                }

                return View();
            }

            ViewBag.Greska = string.Join(", ", result.Errors.Select(e => e.Code switch
            {
                "DuplicateUserName" => "Korisnik s ovim emailom već postoji.",
                "DuplicateEmail" => "Korisnik s ovim emailom već postoji.",
                "PasswordTooShort" => "Lozinka mora imati najmanje 6 karaktera.",
                "PasswordRequiresNonAlphanumeric" => "Lozinka mora sadržavati poseban karakter (npr. @, !, #).",
                "PasswordRequiresLower" => "Lozinka mora sadržavati malo slovo.",
                "PasswordRequiresUpper" => "Lozinka mora sadržavati veliko slovo.",
                "PasswordRequiresDigit" => "Lozinka mora sadržavati broj.",
                _ => e.Description
            }));

            return View();
        }

        public async Task<IActionResult> PotvrdiEmail(string userId, string token)
        {
            var tokenDecoded = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(token));
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.Greska = "Korisnik nije pronađen.";
                return View("Registracija");
            }

            var result = await _userManager.ConfirmEmailAsync(user, tokenDecoded);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Greska = "Greška pri potvrdi emaila. Link je možda istekao.";
            return View("Registracija");
        }
    }
}