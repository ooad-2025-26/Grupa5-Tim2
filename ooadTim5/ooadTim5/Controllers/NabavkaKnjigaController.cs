using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ooadTim5.Data;
using ooadTim5.Models;
using ooadTim5.Models.Enums;
using System;
using System.Threading.Tasks;

namespace ooadTim5.Controllers
{
    [Authorize(Roles = "administrator,bibliotekar")]
    public class NabavkaKnjigaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NabavkaKnjigaController(ApplicationDbContext context)
        {
            _context = context;
        }

       /* public async Task<IActionResult> Index()
        {
            var data = await _context.Nabavke
                .Include(x => x.Knjiga)
                .Include(x => x.Dobavljac)
                .ToListAsync();

            return View(data);
        }*/

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var nabavka = await _context.Nabavke
                .Include(x => x.Knjiga)
                .Include(x => x.Dobavljac)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (nabavka == null) return NotFound();

            return View(nabavka);
        }

        public IActionResult Create()
        {
            ViewData["KnjigaId"] = new SelectList(_context.Knjige, "Id", "Naziv");
            ViewData["DobavljacId"] = new SelectList(_context.Dobavljaci, "Id", "Naziv");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NabavkaKnjiga model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["KnjigaId"] = new SelectList(_context.Knjige, "Id", "Naziv");
                ViewData["DobavljacId"] = new SelectList(_context.Dobavljaci, "Id", "Naziv");
                return View(model);
            }

            model.Status = StatusNabavke.u_obradi;

            _context.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var nabavka = await _context.Nabavke.FindAsync(id);
            if (nabavka == null) return NotFound();

            ViewData["KnjigaId"] = new SelectList(_context.Knjige, "Id", "Naziv", nabavka.KnjigaId);
            ViewData["DobavljacId"] = new SelectList(_context.Dobavljaci, "Id", "Naziv", nabavka.DobavljacId);

            return View(nabavka);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NabavkaKnjiga model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            _context.Update(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> PromijeniStatus(int id, StatusNabavke status)
        {
            var nabavka = await _context.Nabavke.FindAsync(id);
            if (nabavka == null) return NotFound();

            nabavka.Status = status;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}