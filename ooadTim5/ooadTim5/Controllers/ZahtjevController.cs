using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ooadTim5.Data;
using ooadTim5.Models;

namespace ooadTim5.Controllers
{
    public class ZahtjevController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ZahtjevController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ZahtjevZaPosudbu
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Zahtjevi.Include(z => z.Knjiga);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ZahtjevZaPosudbu/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zahtjevZaPosudbu = await _context.Zahtjevi
                .Include(z => z.Knjiga)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zahtjevZaPosudbu == null)
            {
                return NotFound();
            }

            return View(zahtjevZaPosudbu);
        }

        // GET: ZahtjevZaPosudbu/Create
        public IActionResult Create()
        {
            ViewData["KnjigaId"] = new SelectList(_context.Knjige, "Id", "Id");
            return View();
        }

        // POST: ZahtjevZaPosudbu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,KorisnikId,KnjigaId,DatumZahtjeva,Status,RazlogOdbijanja")] ZahtjevZaPosudbu zahtjevZaPosudbu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zahtjevZaPosudbu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KnjigaId"] = new SelectList(_context.Knjige, "Id", "Id", zahtjevZaPosudbu.KnjigaId);
            return View(zahtjevZaPosudbu);
        }

        // GET: ZahtjevZaPosudbu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zahtjevZaPosudbu = await _context.Zahtjevi.FindAsync(id);
            if (zahtjevZaPosudbu == null)
            {
                return NotFound();
            }
            ViewData["KnjigaId"] = new SelectList(_context.Knjige, "Id", "Id", zahtjevZaPosudbu.KnjigaId);
            return View(zahtjevZaPosudbu);
        }

        // POST: ZahtjevZaPosudbu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KorisnikId,KnjigaId,DatumZahtjeva,Status,RazlogOdbijanja")] ZahtjevZaPosudbu zahtjevZaPosudbu)
        {
            if (id != zahtjevZaPosudbu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zahtjevZaPosudbu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZahtjevZaPosudbuExists(zahtjevZaPosudbu.Id))
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
            ViewData["KnjigaId"] = new SelectList(_context.Knjige, "Id", "Id", zahtjevZaPosudbu.KnjigaId);
            return View(zahtjevZaPosudbu);
        }

        // GET: ZahtjevZaPosudbu/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zahtjevZaPosudbu = await _context.Zahtjevi
                .Include(z => z.Knjiga)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zahtjevZaPosudbu == null)
            {
                return NotFound();
            }

            return View(zahtjevZaPosudbu);
        }

        // POST: ZahtjevZaPosudbu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zahtjevZaPosudbu = await _context.Zahtjevi.FindAsync(id);
            if (zahtjevZaPosudbu != null)
            {
                _context.Zahtjevi.Remove(zahtjevZaPosudbu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZahtjevZaPosudbuExists(int id)
        {
            return _context.Zahtjevi.Any(e => e.Id == id);
        }
    }
}
