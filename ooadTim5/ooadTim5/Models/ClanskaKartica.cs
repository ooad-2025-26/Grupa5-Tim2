using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ooadTim5.Models
{
    public class ClanskaKartica
    {
        public ClanskaKartica() { }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Broj kartice je obavezan!")]
        [RegularExpression(@"^SSA\d{5}$",
    ErrorMessage = "Broj kartice mora biti u formatu SSA12345!")]
        [StringLength(8, MinimumLength = 8,
    ErrorMessage = "Broj kartice mora imati tačno 8 karaktera!")]
        [DisplayName("Broj kartice:")]
        public string? BrojKartice { get; set; }

        [Required(ErrorMessage = "Datum izdavanja je obavezan!")]
        [DataType(DataType.Date)]
        [DisplayName("Datum izdavanja:")]
        public DateTime DatumIzdavanja { get; set; }

        [ValidateClanstvoDate]
        [DataType(DataType.Date)]
        [DisplayName("Članstvo važi do:")]
        public DateTime ClanstvoVaziDo { get; set; }

        [DisplayName("Aktivan:")]
        public bool Aktivan { get; set; }

        [DisplayName("ID korisnika:")]
        public string KorisnikId { get; set; }
    }
}