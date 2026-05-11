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
    public class PosudbaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PosudbaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Posudba
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Posudbe.Include(p => p.Knjiga);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Posudba/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posudba = await _context.Posudbe
                .Include(p => p.Knjiga)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (posudba == null)
            {
                return NotFound();
            }

            return View(posudba);
        }

        // GET: Posudba/Create
        public IActionResult Create()
        {
            ViewData["KnjigaId"] = new SelectList(_context.Knjige, "Id", "Id");
            return View();
        }

        // POST: Posudba/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,KnjigaId,ClanId,DatumPosudbe,OcekivaniDatumVracanja,DatumVracanja,Status,Napomena")] Posudba posudba)
        {
            if (ModelState.IsValid)
            {
                _context.Add(posudba);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KnjigaId"] = new SelectList(_context.Knjige, "Id", "Id", posudba.KnjigaId);
            return View(posudba);
        }

        // GET: Posudba/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posudba = await _context.Posudbe.FindAsync(id);
            if (posudba == null)
            {
                return NotFound();
            }
            ViewData["KnjigaId"] = new SelectList(_context.Knjige, "Id", "Id", posudba.KnjigaId);
            return View(posudba);
        }

        // POST: Posudba/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KnjigaId,ClanId,DatumPosudbe,OcekivaniDatumVracanja,DatumVracanja,Status,Napomena")] Posudba posudba)
        {
            if (id != posudba.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(posudba);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PosudbaExists(posudba.Id))
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
            ViewData["KnjigaId"] = new SelectList(_context.Knjige, "Id", "Id", posudba.KnjigaId);
            return View(posudba);
        }

        // GET: Posudba/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posudba = await _context.Posudbe
                .Include(p => p.Knjiga)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (posudba == null)
            {
                return NotFound();
            }

            return View(posudba);
        }

        // POST: Posudba/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var posudba = await _context.Posudbe.FindAsync(id);
            if (posudba != null)
            {
                _context.Posudbe.Remove(posudba);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PosudbaExists(int id)
        {
            return _context.Posudbe.Any(e => e.Id == id);
        }
    }
}
