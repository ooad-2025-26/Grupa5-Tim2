using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ooadTim5.Models.Enums;

namespace ooadTim5.Models
{
    public class Knjiga
    {
        public Knjiga() { }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv je obavezan!")]
        [StringLength(200, MinimumLength = 2,
            ErrorMessage = "Naziv mora imati između 2 i 200 karaktera!")]
        [DisplayName("Naziv knjige:")]
        public string? Naziv { get; set; }

        [Required(ErrorMessage = "Autor je obavezan!")]
        [DisplayName("Autor:")]
        public string? Autor { get; set; }

        [Required(ErrorMessage = "ISBN je obavezan!")]
        [RegularExpression(@"^\d{3}-\d{10}$",
      ErrorMessage = "ISBN mora biti u formatu 978-XXXXXXXXXX (13 cifara)!")]
        [DisplayName("ISBN:")]
        public string? ISBN { get; set; }

        [DisplayName("Kategorija:")]
        public string? Kategorija { get; set; }

        [Range(1000, 2025,
            ErrorMessage = "Godina mora biti između 1000 i 2025!")]
        [DisplayName("Godina izdanja:")]
        public int GodinaIzdanja { get; set; }

        [Range(1, 5000,
            ErrorMessage = "Broj stranica mora biti između 1 i 5000!")]
        [DisplayName("Broj stranica:")]
        public int BrojStranica { get; set; }

        [DisplayName("Izdavač:")]
        public string? Izdavac { get; set; }

        [DisplayName("Naslovnica (URL):")]
        public string? Naslovnica { get; set; }

        [EnumDataType(typeof(StatusKnjige))]
        [DisplayName("Status knjige:")]
        public StatusKnjige Status { get; set; }
    }
}