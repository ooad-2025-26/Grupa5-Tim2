using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ooadTim5.Models.Enums;

namespace ooadTim5.Models
{
    public class KorisnikSistema
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ime je obavezno!")]
        [StringLength(50, MinimumLength = 2,
            ErrorMessage = "Ime mora imati između 2 i 50 karaktera!")]
        [DisplayName("Ime:")]
        public string Ime { get; set; }

        [Required(ErrorMessage = "Prezime je obavezno!")]
        [StringLength(50, MinimumLength = 2,
            ErrorMessage = "Prezime mora imati između 2 i 50 karaktera!")]
        [DisplayName("Prezime:")]
        public string Prezime { get; set; }

        [Required(ErrorMessage = "Datum rođenja je obavezan!")]
        [DataType(DataType.Date)]
        [DisplayName("Datum rođenja:")]
        public DateTime DatumRodjenja { get; set; }

        [Required(ErrorMessage = "Email je obavezan!")]
        [EmailAddress(ErrorMessage = "Unesite ispravnu email adresu!")]
        [DisplayName("Email adresa:")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lozinka je obavezna!")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6,
            ErrorMessage = "Lozinka mora imati najmanje 6 karaktera!")]
        [DisplayName("Lozinka:")]
        public string Lozinka { get; set; }

        [Phone(ErrorMessage = "Unesite ispravan broj telefona!")]
        [DisplayName("Broj telefona:")]
        public string BrojTelefona { get; set; }

        [DisplayName("Adresa stanovanja:")]
        public string AdresaStanovanja { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Datum učlanjivanja:")]
        public DateTime DatumUclanjivanja { get; set; }

        [EnumDataType(typeof(UlogaKorisnika))]
        [DisplayName("Uloga:")]
        public UlogaKorisnika Uloga { get; set; }

        [DisplayName("Aktivan:")]
        public bool Aktivan { get; set; }
    }
}