using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ooadTim5.Data;
using ooadTim5.Models;
using ooadTim5.Models.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ooadTim5.Controllers
{
    [Authorize(Roles = "administrator,bibliotekar,clan")]
    public class PosudbaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public PosudbaController(ApplicationDbContext context,
                          UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // LISTA POSUDBI
        public async Task<IActionResult> Index()
        {
            var posudbe = await _context.Posudbe
                .Include(p => p.Knjiga)
                .ToListAsync();

            var users = await _userManager.Users
                .ToDictionaryAsync(x => x.Id, x => x.UserName ?? x.Email);

            ViewBag.Korisnici = users;
            foreach (var p in posudbe)
            {
                // 1. Ako nije vraćena
                if (p.DatumVracanja == null)
                {
                    // 2. Ako je rok prošao → kasni
                    if (p.OcekivaniDatumVracanja < DateTime.Today)
                    {
                        p.Status = StatusPosudbe.kasnjenje;
                    }
                    else
                    {
                        p.Status = StatusPosudbe.aktivna;
                    }
                }
            }

            return View(posudbe);
        }

        // DETAILS
        public async Task<IActionResult> Details(int id)
        {
            var posudba = await _context.Posudbe
                .Include(p => p.Knjiga)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (posudba == null)
                return NotFound();

            var user = await _userManager.FindByIdAsync(posudba.ClanId);

            ViewBag.ClanIme = user?.UserName ?? user?.Email ?? posudba.ClanId;

            return View(posudba);
        }

        // VRACANJE KNJIGE
        [HttpPost]
        public async Task<IActionResult> Vrati(int id)
        {
            var posudba = await _context.Posudbe
                .Include(p => p.Knjiga)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (posudba == null)
                return NotFound();

            posudba.DatumVracanja = DateTime.Now;
            posudba.Status = StatusPosudbe.vracena;

            // optional: vrati knjigu dostupnu
            if (posudba.Knjiga != null)
                posudba.Knjiga.Status = StatusKnjige.dostupna;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}