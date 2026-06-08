using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ooadTim5.Data;
using ooadTim5.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ooadTim5.Controllers
{

    public class KorisniciController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public KorisniciController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> Index()
        {
            var korisnici = _userManager.Users.ToList();
            var lista = new List<KorisnikViewModel>();

            foreach (var korisnik in korisnici)
            {
                var role = await _userManager.GetRolesAsync(korisnik);
                lista.Add(new KorisnikViewModel
                {
                    Id = korisnik.Id,
                    Email = korisnik.Email,
                    UserName = korisnik.UserName,
                    Rola = role.FirstOrDefault() ?? "Nema role"
                });
            }

            return View(lista);
        }

        // PROFIL - korisnik vidi svoj profil
        [Authorize(Roles = "administrator,bibliotekar,clan")]
        public async Task<IActionResult> Profil()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var kartica = await _context.ClanskeKartice
                .FirstOrDefaultAsync(k => k.KorisnikId == user.Id);

            ViewBag.Email = user.Email;
            ViewBag.Kartica = kartica;

            return View();
        }

        // DETALJI - admin vidi profil bilo kojeg korisnika
        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            var kartica = await _context.ClanskeKartice
                .FirstOrDefaultAsync(k => k.KorisnikId == id);

            ViewBag.Email = user.Email;
            ViewBag.UserName = user.UserName;
            ViewBag.Rola = roles.FirstOrDefault() ?? "Nema role";
            ViewBag.Kartica = kartica;

            return View();
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(DodajKorisnikaViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Lozinka);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, model.Rola);
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            var kartica = await _context.ClanskeKartice
                .FirstOrDefaultAsync(k => k.KorisnikId == id);

            var model = new IzmijeniKorisnikaViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Rola = roles.FirstOrDefault()
            };

            ViewBag.Kartica = kartica;

            return View(model);
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Edit(IzmijeniKorisnikaViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return NotFound();

            user.Email = model.Email;
            user.UserName = model.Email;

            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);
            await _userManager.AddToRoleAsync(user, model.Rola);

            // ---- KARTICA ----
            var kartica = await _context.ClanskeKartice
                .FirstOrDefaultAsync(k => k.KorisnikId == model.Id);

            if (kartica == null)
            {
                var nova = new ClanskaKartica
                {
                    KorisnikId = model.Id,
                    BrojKartice = Request.Form["BrojKartice"],
                    ClanstvoVaziDo = DateTime.Parse(Request.Form["ClanstvoVaziDo"]),
                    Aktivan = Request.Form["Aktivan"].Count > 0,
                    DatumIzdavanja = DateTime.Now
                };

                _context.ClanskeKartice.Add(nova);
            }
            else
            {
                kartica.BrojKartice = Request.Form["BrojKartice"];
                kartica.ClanstvoVaziDo = DateTime.Parse(Request.Form["ClanstvoVaziDo"]);
                kartica.Aktivan = Request.Form["Aktivan"].Count > 0;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            var model = new KorisnikViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Rola = roles.FirstOrDefault() ?? "Nema role"
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            await _userManager.DeleteAsync(user);
            return RedirectToAction("Index");
        }
    }
}