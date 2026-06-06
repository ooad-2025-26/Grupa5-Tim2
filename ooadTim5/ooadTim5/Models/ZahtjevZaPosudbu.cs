using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ooadTim5.Models.Enums;

namespace ooadTim5.Models
{
    public class ZahtjevZaPosudbu
    {
        public ZahtjevZaPosudbu() { }

        [Key]
        public int Id { get; set; }

        [DisplayName("Korisnik:")]
        public string? KorisnikId { get; set; }

        [ForeignKey("Knjiga")]
        [DisplayName("Knjiga:")]
        public int KnjigaId { get; set; }
        public Knjiga? Knjiga { get; set; }

        [Required(ErrorMessage = "Datum zahtjeva je obavezan!")]
        [DataType(DataType.Date)]
        [DisplayName("Datum zahtjeva:")]
        public DateTime DatumZahtjeva { get; set; }

        [EnumDataType(typeof(StatusZahtjeva))]
        [DisplayName("Status:")]
        public StatusZahtjeva Status { get; set; }

        [StringLength(500, ErrorMessage = "Razlog ne može biti duži od 500 karaktera!")]
        [DisplayName("Razlog odbijanja:")]
        public string? RazlogOdbijanja { get; set; }
    }
}