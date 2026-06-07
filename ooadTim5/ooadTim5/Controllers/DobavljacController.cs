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
    [Authorize(Roles = "administrator")]
    public class DobavljacController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DobavljacController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Dobavljaci.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var dobavljac = await _context.Dobavljaci
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dobavljac == null) return NotFound();

            return View(dobavljac);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naziv,KontaktEmail,KontaktTelefon,Adresa")] Dobavljac dobavljac)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dobavljac);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dobavljac);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var dobavljac = await _context.Dobavljaci.FindAsync(id);
            if (dobavljac == null) return NotFound();

            return View(dobavljac);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,KontaktEmail,KontaktTelefon,Adresa")] Dobavljac dobavljac)
        {
            if (id != dobavljac.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dobavljac);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DobavljacExists(dobavljac.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dobavljac);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var dobavljac = await _context.Dobavljaci
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dobavljac == null) return NotFound();

            return View(dobavljac);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dobavljac = await _context.Dobavljaci.FindAsync(id);
            if (dobavljac != null)
            {
                _context.Dobavljaci.Remove(dobavljac);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DobavljacExists(int id)
        {
            return _context.Dobavljaci.Any(e => e.Id == id);
        }
    }
}