using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ooadTim5.Data;
using ooadTim5.Models;

namespace ooadTim5.Controllers
{
    [Authorize(Roles = "administrator, bibliotekar")]
    public class NabavkaKnjigaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NabavkaKnjigaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: NabavkaKnjiga
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Nabavke.Include(n => n.Dobavljac).Include(n => n.Knjiga);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: NabavkaKnjiga/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nabavkaKnjiga = await _context.Nabavke
                .Include(n => n.Dobavljac)
                .Include(n => n.Knjiga)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nabavkaKnjiga == null)
            {
                return NotFound();
            }

            return View(nabavkaKnjiga);
        }

        // GET: NabavkaKnjiga/Create
        public IActionResult Create()
        {
            ViewData["DobavljacId"] = new SelectList(_context.Dobavljaci, "Id", "KontaktEmail");
            ViewData["KnjigaId"] = new SelectList(_context.Knjige, "Id", "Autor");
            return View();
        }

        // POST: NabavkaKnjiga/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,KnjigaId,DobavljacId,Kolicina,DatumNarudzbe,OcekivaniDatumIsporuke,Status")] NabavkaKnjiga nabavkaKnjiga)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nabavkaKnjiga);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DobavljacId"] = new SelectList(_context.Dobavljaci, "Id", "KontaktEmail", nabavkaKnjiga.DobavljacId);
            ViewData["KnjigaId"] = new SelectList(_context.Knjige, "Id", "Autor", nabavkaKnjiga.KnjigaId);
            return View(nabavkaKnjiga);
        }

        // GET: NabavkaKnjiga/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nabavkaKnjiga = await _context.Nabavke.FindAsync(id);
            if (nabavkaKnjiga == null)
            {
                return NotFound();
            }
            ViewData["DobavljacId"] = new SelectList(_context.Dobavljaci, "Id", "KontaktEmail", nabavkaKnjiga.DobavljacId);
            ViewData["KnjigaId"] = new SelectList(_context.Knjige, "Id", "Autor", nabavkaKnjiga.KnjigaId);
            return View(nabavkaKnjiga);
        }

        // POST: NabavkaKnjiga/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KnjigaId,DobavljacId,Kolicina,DatumNarudzbe,OcekivaniDatumIsporuke,Status")] NabavkaKnjiga nabavkaKnjiga)
        {
            if (id != nabavkaKnjiga.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nabavkaKnjiga);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NabavkaKnjigaExists(nabavkaKnjiga.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DobavljacId"] = new SelectList(_context.Dobavljaci, "Id", "KontaktEmail", nabavkaKnjiga.DobavljacId);
            ViewData["KnjigaId"] = new SelectList(_context.Knjige, "Id", "Autor", nabavkaKnjiga.KnjigaId);
            return View(nabavkaKnjiga);
        }

        // GET: NabavkaKnjiga/Delete/5
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nabavkaKnjiga = await _context.Nabavke
                .Include(n => n.Dobavljac)
                .Include(n => n.Knjiga)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nabavkaKnjiga == null)
            {
                return NotFound();
            }

            return View(nabavkaKnjiga);
        }

        // POST: NabavkaKnjiga/Delete/5
        [Authorize(Roles = "administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nabavkaKnjiga = await _context.Nabavke.FindAsync(id);
            if (nabavkaKnjiga != null)
            {
                _context.Nabavke.Remove(nabavkaKnjiga);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NabavkaKnjigaExists(int id)
        {
            return _context.Nabavke.Any(e => e.Id == id);
        }
    }
}