using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ooadTim5.Data;
using ooadTim5.Models;
using QRCoder;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ooadTim5.Controllers
{
    [Authorize(Roles = "administrator,bibliotekar")]
    public class ClanskeKarticeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ClanskeKarticeController(ApplicationDbContext context,
                                         UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> NoviClanovi()
        {
            var korisnici = await _userManager.Users
                 .Where(u => u.EmailConfirmed)
                 .ToListAsync();
            var bezKartice = new List<IdentityUser>();

            foreach (var user in korisnici)
            {
                var postoji = await _context.ClanskeKartice
                    .AnyAsync(x => x.KorisnikId == user.Id);
                if (!postoji)
                    bezKartice.Add(user);
            }

            return View(bezKartice);
        }

        public async Task<IActionResult> Details(int id)
        {
            var kartica = await _context.ClanskeKartice
                .FirstOrDefaultAsync(k => k.Id == id);

            if (kartica == null)
                return NotFound();

            return View(kartica);
        }

        public IActionResult Create(string korisnikId)
        {
            var model = new ClanskaKartica
            {
                KorisnikId = korisnikId,
                DatumIzdavanja = DateTime.Now,
                ClanstvoVaziDo = DateTime.Now.AddYears(1),
                Aktivan = true
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClanskaKartica model)
        {
            ModelState.Remove("BrojKartice");

            if (ModelState.IsValid)
            {
                string brojKartice;
                do
                {
                    int broj = Random.Shared.Next(10000, 99999);
                    brojKartice = $"SSA{broj}";
                }
                while (await _context.ClanskeKartice.AnyAsync(k => k.BrojKartice == brojKartice));

                model.BrojKartice = brojKartice;
                model.Aktivan = true;
                model.DatumIzdavanja = DateTime.Now;

                if (model.ClanstvoVaziDo == default)
                    model.ClanstvoVaziDo = DateTime.Now.AddYears(1);

                _context.ClanskeKartice.Add(model);
                await _context.SaveChangesAsync();

                // Redirect na PDF nakon kreiranja
                return RedirectToAction("DownloadPDF", new { id = model.Id });
            }

            return View(model);
        }

        public async Task<IActionResult> DownloadPDF(int id)
        {
            var kartica = await _context.ClanskeKartice
                .FirstOrDefaultAsync(k => k.Id == id);

            if (kartica == null)
                return NotFound();

            // Dohvati korisnika
            var korisnik = await _userManager.FindByIdAsync(kartica.KorisnikId);
            string imeKorisnika = korisnik?.Email ?? "Nepoznat korisnik";

            // Generiši QR kod
            var qrUrl = $"{Request.Scheme}://{Request.Host}/Korisnici/Profil";
            using var qrGenerator = new QRCodeGenerator();
            using var qrData = qrGenerator.CreateQrCode(qrUrl, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrData);
            var qrBytes = qrCode.GetGraphic(10);

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A6.Landscape());
                    page.Margin(0);

                    page.Content().Row(row =>
                    {
                        // Lijeva strana - zelena
                        row.RelativeItem(2).Background(Color.FromHex("#1a4a3a"))
                            .Padding(24).Column(col =>
                            {
                                col.Item().Text("📚 LitHub")
                                    .FontSize(24).Bold()
                                    .FontColor(Colors.White);

                                col.Item().Text("Članska kartica")
                                    .FontSize(10)
                                    .FontColor(Color.FromHex("#a8d5c2"));

                                // IME KORISNIKA
                                col.Item().PaddingTop(12).Text(imeKorisnika)
                                    .FontSize(11).Bold()
                                    .FontColor(Colors.White);

                                col.Item().PaddingTop(12).Text("BROJ KARTICE")
                                    .FontSize(8).Bold()
                                    .FontColor(Color.FromHex("#c8621a"));

                                col.Item().Text(kartica.BrojKartice)
                                    .FontSize(18).Bold()
                                    .FontColor(Colors.White);

                                col.Item().PaddingTop(12).Text("VAŽI DO")
                                    .FontSize(8).Bold()
                                    .FontColor(Color.FromHex("#c8621a"));

                                col.Item().Text(kartica.ClanstvoVaziDo.ToString("dd.MM.yyyy"))
                                    .FontSize(14).Bold()
                                    .FontColor(Colors.White);

                                col.Item().PaddingTop(12).Text("DATUM IZDAVANJA")
                                    .FontSize(8).Bold()
                                    .FontColor(Color.FromHex("#c8621a"));

                                col.Item().Text(kartica.DatumIzdavanja.ToString("dd.MM.yyyy"))
                                    .FontSize(14).Bold()
                                    .FontColor(Colors.White);

                                col.Item().PaddingTop(20);

                                if (kartica.Aktivan)
                                    col.Item().Text("● AKTIVAN")
                                        .FontSize(9).Bold()
                                        .FontColor(Color.FromHex("#4caf50"));
                                else
                                    col.Item().Text("● NEAKTIVAN")
                                        .FontSize(9).Bold()
                                        .FontColor(Color.FromHex("#f44336"));
                            });

                        // Desna strana - bijela sa QR
                        row.RelativeItem(1).Background(Colors.White)
                            .AlignCenter().AlignMiddle()
                            .Padding(16).Column(col =>
                            {
                                col.Item().AlignCenter()
                                    .Image(qrBytes).FitArea();

                                col.Item().PaddingTop(8).AlignCenter()
                                    .Text("Skeniraj za profil")
                                    .FontSize(8)
                                    .FontColor(Color.FromHex("#999999"));
                            });
                    });
                });
            });

            var pdfBytes = pdf.GeneratePdf();
            return File(pdfBytes, "application/pdf", $"kartica_{kartica.BrojKartice}.pdf");
        }
    }
}