using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ooadTim5.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ooadTim5.Controllers
{
    [Authorize(Roles = "administrator")]
    public class KorisniciController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public KorisniciController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var korisnici = _userManager.Users.ToList();

            var lista = new List<KorisnikViewModel>();

            foreach (var korisnik in korisnici)
            {
                var role = await _userManager.GetRolesAsync(korisnik);

                lista.Add(new KorisnikViewModel
                {
                    Id = korisnik.Id,
                    Email = korisnik.Email,
                    UserName = korisnik.UserName,
                    Rola = role.FirstOrDefault() ?? "Nema role"
                });
            }

            return View(lista);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DodajKorisnikaViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Lozinka);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, model.Rola);
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            var model = new IzmijeniKorisnikaViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Rola = roles.FirstOrDefault()
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(IzmijeniKorisnikaViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return NotFound();

            // update email
            user.Email = model.Email;
            user.UserName = model.Email;

            await _userManager.UpdateAsync(user);

            // update role
            var roles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, roles);

            await _userManager.AddToRoleAsync(user, model.Rola);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            var model = new KorisnikViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Rola = roles.FirstOrDefault() ?? "Nema role"
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            await _userManager.DeleteAsync(user);

            return RedirectToAction("Index");
        }
    }


}