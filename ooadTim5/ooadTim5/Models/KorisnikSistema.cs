using ooadTim5.Models.Enums;
using ooadTim5.Models.Enums;

namespace ooadTim5.Models
{
    public class KorisnikSistema
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public string Email { get; set; }
        public string Lozinka { get; set; }
        public string BrojTelefona { get; set; }
        public string AdresaStanovanja { get; set; }
        public DateTime DatumUclanjivanja { get; set; }
        public UlogaKorisnika Uloga { get; set; }
        public bool Aktivan { get; set; }
    }
}