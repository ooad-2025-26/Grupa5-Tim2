using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ooadTim5.Data;
using ooadTim5.Models;

namespace ooadTim5.Controllers
{
    [Authorize(Roles = "administrator,bibliotekar")]
    public class ClanskeKarticeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ClanskeKarticeController(ApplicationDbContext context,
                                         UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // 1. LISTA KORISNIKA BEZ KARTICE
        public async Task<IActionResult> NoviClanovi()
        {
            var korisnici = await _userManager.Users.ToListAsync();

            var bezKartice = new List<IdentityUser>();

            foreach (var user in korisnici)
            {
                var postoji = await _context.ClanskeKartice
                    .AnyAsync(x => x.KorisnikId == user.Id);

                if (!postoji)
                    bezKartice.Add(user);
            }

            return View(bezKartice);
        }
        // GET: Details
        public async Task<IActionResult> Details(int id)
        {
            var kartica = await _context.ClanskeKartice
                .FirstOrDefaultAsync(k => k.Id == id);

            if (kartica == null)
                return NotFound();

            return View(kartica);
        }
        // GET
        public IActionResult Create(string korisnikId)
        {
            var model = new ClanskaKartica
            {
                KorisnikId = korisnikId,
                DatumIzdavanja = DateTime.Now,
                ClanstvoVaziDo = DateTime.Now.AddYears(1),
                Aktivan = true
            };

            return View(model);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClanskaKartica model)
        {
            ModelState.Remove("BrojKartice");

            if (ModelState.IsValid)
            {
                // Generiši jedinstven broj kartice
                string brojKartice;
                do
                {
                    int broj = Random.Shared.Next(10000, 99999);
                    brojKartice = $"SSA{broj}";
                }
                while (await _context.ClanskeKartice.AnyAsync(k => k.BrojKartice == brojKartice));

                model.BrojKartice = brojKartice;
                model.Aktivan = true;
                model.DatumIzdavanja = DateTime.Now;

                if (model.ClanstvoVaziDo == default)
                    model.ClanstvoVaziDo = DateTime.Now.AddYears(1);

                _context.ClanskeKartice.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("NoviClanovi");
            }

            return View(model);
        }
    }
}