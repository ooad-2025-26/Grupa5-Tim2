using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ooadTim5.Data;
using ooadTim5.Models;
using ooadTim5.Models.Enums;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ooadTim5.Controllers
{
    public class ZahtjevZaPosudbuController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ZahtjevZaPosudbuController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "administrator,bibliotekar,clan")]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            IQueryable<ZahtjevZaPosudbu> query = _context.Zahtjevi
                .Include(z => z.Knjiga);

            if (User.IsInRole("administrator") || User.IsInRole("bibliotekar"))
            {
                var svi = await query.ToListAsync();

                var korisnici = await _context.Users
                    .ToDictionaryAsync(x => x.Id, x => x.UserName ?? x.Email);

                ViewBag.Korisnici = korisnici;

                return View(svi);
            }

            var moji = await query
                .Where(z => z.KorisnikId == userId)
                .ToListAsync();

            var korisniciUser = await _context.Users
                .ToDictionaryAsync(x => x.Id, x => x.UserName ?? x.Email);

            ViewBag.Korisnici = korisniciUser;

            return View(moji);
        }

        [Authorize(Roles = "administrator,bibliotekar,clan")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var zahtjev = await _context.Zahtjevi
                .Include(z => z.Knjiga)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (zahtjev == null) return NotFound();

            var user = await _userManager.FindByIdAsync(zahtjev.KorisnikId);

            ViewBag.KorisnikIme = user?.UserName ?? user?.Email ?? "Nepoznat korisnik";

            var aktivnePosudbe = await _context.Posudbe
                .Include(p => p.Knjiga)
                .Where(p => p.ClanId == zahtjev.KorisnikId && p.DatumVracanja == null)
                .ToListAsync();

            ViewBag.AktivnePosudbe = aktivnePosudbe;

            var kartica = await _context.ClanskeKartice
                .FirstOrDefaultAsync(x => x.KorisnikId == zahtjev.KorisnikId);

            ViewBag.Kartica = kartica;

            ViewBag.ValidnaKartica =
                kartica != null &&
                kartica.Aktivan &&
                kartica.ClanstvoVaziDo >= DateTime.Today;

            return View(zahtjev);
        }

        // CREATE GET
        [Authorize(Roles = "administrator,bibliotekar,clan")]
        public async Task<IActionResult> Create(int? knjigaId)
        {
            if (knjigaId == null) return RedirectToAction("Index", "Knjiga");

            var knjiga = await _context.Knjige.FindAsync(knjigaId);
            if (knjiga == null) return NotFound();

            ViewBag.NazivKnjige = knjiga.Naziv;
            ViewData["KnjigaId"] = knjigaId;

            var model = new ZahtjevZaPosudbu { KnjigaId = knjigaId.Value };
            return View(model);
        }

        // CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrator,bibliotekar,clan")]
        public async Task<IActionResult> Create(ZahtjevZaPosudbu zahtjevZaPosudbu)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);

                // Provjera članske kartice samo za clan ulogu
                if (User.IsInRole("clan"))
                {
                    var kartica = await _context.ClanskeKartice
                        .FirstOrDefaultAsync(k => k.KorisnikId == userId &&
                                                  k.Aktivan &&
                                                  k.ClanstvoVaziDo >= DateTime.Today);

                    if (kartica == null)
                    {
                        TempData["Greska"] = "Nemate aktivnu člansku karticu. Molimo posjetite biblioteku da dobijete karticu.";
                        return RedirectToAction("Index", "Knjiga");
                    }
                }

                zahtjevZaPosudbu.KorisnikId = userId;
                zahtjevZaPosudbu.DatumZahtjeva = DateTime.Now;
                zahtjevZaPosudbu.Status = StatusZahtjeva.Na_cekanju;
                zahtjevZaPosudbu.RazlogOdbijanja = "";

                _context.Add(zahtjevZaPosudbu);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            var knjiga = await _context.Knjige.FindAsync(zahtjevZaPosudbu.KnjigaId);
            ViewBag.NazivKnjige = knjiga?.Naziv;
            ViewData["KnjigaId"] = zahtjevZaPosudbu.KnjigaId;
            return View(zahtjevZaPosudbu);
        }

        // EDIT
        [Authorize(Roles = "administrator,bibliotekar")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var zahtjev = await _context.Zahtjevi.FindAsync(id);
            if (zahtjev == null) return NotFound();

            ViewData["KnjigaId"] = new SelectList(_context.Knjige, "Id", "Naziv", zahtjev.KnjigaId);
            return View(zahtjev);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrator,bibliotekar")]
        public async Task<IActionResult> Edit(int id, ZahtjevZaPosudbu zahtjevZaPosudbu)
        {
            if (id != zahtjevZaPosudbu.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(zahtjevZaPosudbu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(zahtjevZaPosudbu);
        }

        [HttpPost]
        [Authorize(Roles = "administrator,bibliotekar")]
        public async Task<IActionResult> Odobri(int id)
        {
            var zahtjev = await _context.Zahtjevi.FindAsync(id);

            if (zahtjev == null)
                return NotFound();

            zahtjev.Status = StatusZahtjeva.odobren;

            var posudba = new Posudba
            {
                KnjigaId = zahtjev.KnjigaId,
                ClanId = zahtjev.KorisnikId,

                DatumPosudbe = DateTime.Now,

                OcekivaniDatumVracanja = DateTime.Now.AddDays(14),

                Status = StatusPosudbe.aktivna,
                Napomena = ""

            };

            var knjiga = await _context.Knjige
    .FindAsync(zahtjev.KnjigaId);

            if (knjiga != null)
            {
                knjiga.Status = StatusKnjige.nedostupna;
            }

            _context.Posudbe.Add(posudba);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "administrator,bibliotekar")]
        public async Task<IActionResult> Odbij(int id, string razlog)
        {
            var zahtjev = await _context.Zahtjevi.FindAsync(id);
            if (zahtjev == null) return NotFound();

            zahtjev.Status = StatusZahtjeva.odbijen;
            zahtjev.RazlogOdbijanja = razlog ?? "";
            _context.Update(zahtjev);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // DELETE ZAHTJEV (ADMIN)
        [Authorize(Roles = "administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var zahtjev = await _context.Zahtjevi.FindAsync(id);

            if (zahtjev == null)
                return NotFound();

            _context.Zahtjevi.Remove(zahtjev);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        
    }
}