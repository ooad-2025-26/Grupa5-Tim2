using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ooadTim5.Models
{
    public class Dobavljac
    {
        public Dobavljac() { }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv je obavezan!")]
        [StringLength(200, MinimumLength = 2,
            ErrorMessage = "Naziv mora imati između 2 i 200 karaktera!")]
        [DisplayName("Naziv:")]
        public string? Naziv { get; set; }

        [Required(ErrorMessage = "Email je obavezan!")]
        [EmailAddress(ErrorMessage = "Unesite ispravnu email adresu!")]
        [DisplayName("Kontakt email:")]
        public string? KontaktEmail { get; set; }

        [Phone(ErrorMessage = "Unesite ispravan broj telefona!")]
        [DisplayName("Kontakt telefon:")]
        public string? KontaktTelefon { get; set; }

        [StringLength(300, ErrorMessage = "Adresa ne može biti duža od 300 karaktera!")]
        [DisplayName("Adresa:")]
        public string? Adresa { get; set; }
    }
}