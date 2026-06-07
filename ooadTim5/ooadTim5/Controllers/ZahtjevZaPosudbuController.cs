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
            var data = await _context.Zahtjevi
                .Include(z => z.Knjiga)
                .ToListAsync();

            // Dohvati emailove za sve korisnike
            var korisnici = new Dictionary<string, string>();
            foreach (var z in data)
            {
                if (z.KorisnikId != null && !korisnici.ContainsKey(z.KorisnikId))
                {
                    var user = await _userManager.FindByIdAsync(z.KorisnikId);
                    korisnici[z.KorisnikId] = user?.Email ?? z.KorisnikId;
                }
            }

            ViewBag.Korisnici = korisnici;
            return View(data);
        }

        // DETAILS
        [Authorize(Roles = "administrator,bibliotekar,clan")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var zahtjev = await _context.Zahtjevi
                .Include(z => z.Knjiga)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (zahtjev == null) return NotFound();

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
                zahtjevZaPosudbu.KorisnikId = _userManager.GetUserId(User);
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

        // DELETE
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var zahtjev = await _context.Zahtjevi
                .Include(z => z.Knjiga)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (zahtjev == null) return NotFound();

            return View(zahtjev);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zahtjev = await _context.Zahtjevi.FindAsync(id);

            if (zahtjev != null)
            {
                _context.Zahtjevi.Remove(zahtjev);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}