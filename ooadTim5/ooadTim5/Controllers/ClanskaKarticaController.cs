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
    public class ClanskaKarticaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClanskaKarticaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ClanskaKartica
        public async Task<IActionResult> Index()
        {
            return View(await _context.ClanskeKartice.ToListAsync());
        }

        // GET: ClanskaKartica/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clanskaKartica = await _context.ClanskeKartice
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clanskaKartica == null)
            {
                return NotFound();
            }

            return View(clanskaKartica);
        }

        // GET: ClanskaKartica/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ClanskaKartica/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BrojKartice,DatumIzdavanja,ClanstvoVaziDo,Aktivan,KorisnikId")] ClanskaKartica clanskaKartica)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clanskaKartica);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clanskaKartica);
        }

        // GET: ClanskaKartica/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clanskaKartica = await _context.ClanskeKartice.FindAsync(id);
            if (clanskaKartica == null)
            {
                return NotFound();
            }
            return View(clanskaKartica);
        }

        // POST: ClanskaKartica/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BrojKartice,DatumIzdavanja,ClanstvoVaziDo,Aktivan,KorisnikId")] ClanskaKartica clanskaKartica)
        {
            if (id != clanskaKartica.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clanskaKartica);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClanskaKarticaExists(clanskaKartica.Id))
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
            return View(clanskaKartica);
        }

        // GET: ClanskaKartica/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clanskaKartica = await _context.ClanskeKartice
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clanskaKartica == null)
            {
                return NotFound();
            }

            return View(clanskaKartica);
        }

        // POST: ClanskaKartica/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clanskaKartica = await _context.ClanskeKartice.FindAsync(id);
            if (clanskaKartica != null)
            {
                _context.ClanskeKartice.Remove(clanskaKartica);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClanskaKarticaExists(int id)
        {
            return _context.ClanskeKartice.Any(e => e.Id == id);
        }
    }
}
