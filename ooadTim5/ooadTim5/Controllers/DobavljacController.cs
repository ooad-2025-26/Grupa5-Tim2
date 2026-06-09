using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ooadTim5.Data;
using ooadTim5.Models;
using System.Threading.Tasks;

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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Dobavljac dobavljac)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dobavljac);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dobavljac);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var d = await _context.Dobavljaci.FindAsync(id);
            if (d == null) return NotFound();
            return View(d);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Dobavljac dobavljac)
        {
            _context.Update(dobavljac);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var d = await _context.Dobavljaci.FindAsync(id);
            if (d == null) return NotFound();
            return View(d);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var d = await _context.Dobavljaci.FindAsync(id);
            if (d != null)
            {
                _context.Dobavljaci.Remove(d);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}