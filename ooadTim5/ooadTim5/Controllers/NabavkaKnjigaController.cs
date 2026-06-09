using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ooadTim5.Data;
using ooadTim5.Models;
using ooadTim5.Models.Enums;

namespace ooadTim5.Controllers
{
    [Authorize(Roles = "administrator")]
    public class NabavkaKnjigaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NabavkaKnjigaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // INDEX
        public async Task<IActionResult> Index()
        {
            return View(await _context.Nabavke.ToListAsync());
        }

        // DETAILS
        public async Task<IActionResult> Details(int id)
        {
            var nabavka = await _context.Nabavke.FirstOrDefaultAsync(x => x.Id == id);
            if (nabavka == null) return NotFound();

            return View(nabavka);
        }

        // CREATE GET
        public IActionResult Create()
        {
            ViewData["DobavljacId"] = new SelectList(_context.Dobavljaci, "Id", "Naziv");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NabavkaKnjiga model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["DobavljacId"] = new SelectList(_context.Dobavljaci, "Id", "Naziv");
                return View(model);
            }

            model.Status = StatusNabavke.u_obradi;
            model.DatumNarudzbe = DateTime.Now;

            _context.Nabavke.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var nabavka = await _context.Nabavke.FirstOrDefaultAsync(x => x.Id == id);
            if (nabavka == null) return NotFound();

            return View(nabavka);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nabavka = await _context.Nabavke.FindAsync(id);

            if (nabavka != null)
            {
                _context.Nabavke.Remove(nabavka);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // PROMJENA STATUSA (JEDNO DUGME LOGIKA)
        [HttpPost]
        public async Task<IActionResult> PromijeniStatus(int id)
        {
            var nabavka = await _context.Nabavke.FirstOrDefaultAsync(x => x.Id == id);
            if (nabavka == null) return NotFound();

            if (nabavka.Status == StatusNabavke.u_obradi)
                nabavka.Status = StatusNabavke.poslano;

            else if (nabavka.Status == StatusNabavke.poslano)
            {
                nabavka.Status = StatusNabavke.primljeno;

                // 👉 KREIRAJ KNJIGU
                var knjiga = new Knjiga
                {
                    Naziv = nabavka.NazivKnjige,
                    Autor = nabavka.AutorKnjige,
                    ISBN = nabavka.ISBN,
                    Kategorija = nabavka.Kategorija,
                    GodinaIzdanja = nabavka.GodinaIzdanja ?? 0,
                    BrojStranica = nabavka.BrojStranica ?? 1,
                    Izdavac = nabavka.Izdavac,
                    Naslovnica = nabavka.Naslovnica,
                    Status = StatusKnjige.dostupna
                };

                _context.Knjige.Add(knjiga);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}