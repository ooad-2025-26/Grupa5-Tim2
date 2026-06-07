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
    public class KnjigaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KnjigaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Knjige.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var knjiga = await _context.Knjige
                .FirstOrDefaultAsync(m => m.Id == id);
            if (knjiga == null) return NotFound();

            return View(knjiga);
        }

        [Authorize(Roles = "administrator, bibliotekar")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "administrator, bibliotekar")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naziv,Autor,ISBN,Kategorija,GodinaIzdanja,BrojStranica,Izdavac,Naslovnica,Status")] Knjiga knjiga)
        {
            if (ModelState.IsValid)
            {
                _context.Add(knjiga);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(knjiga);
        }

        [Authorize(Roles = "administrator, bibliotekar")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var knjiga = await _context.Knjige.FindAsync(id);
            if (knjiga == null) return NotFound();

            return View(knjiga);
        }

        [Authorize(Roles = "administrator, bibliotekar")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Autor,ISBN,Kategorija,GodinaIzdanja,BrojStranica,Izdavac,Naslovnica,Status")] Knjiga knjiga)
        {
            if (id != knjiga.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(knjiga);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KnjigaExists(knjiga.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(knjiga);
        }

        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var knjiga = await _context.Knjige
                .FirstOrDefaultAsync(m => m.Id == id);
            if (knjiga == null) return NotFound();

            return View(knjiga);
        }

        [Authorize(Roles = "administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var knjiga = await _context.Knjige.FindAsync(id);
            if (knjiga != null)
            {
                _context.Knjige.Remove(knjiga);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KnjigaExists(int id)
        {
            return _context.Knjige.Any(e => e.Id == id);
        }
    }
}