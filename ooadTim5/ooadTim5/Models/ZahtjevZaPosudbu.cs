using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ooadTim5.Models
{
    public class ZahtjevZaPosudbu
    {
        public ZahtjevZaPosudbu() { }

        [Key]
        public int Id { get; set; }

        public string? KorisnikId { get; set; }

        [ForeignKey("Knjiga")]
        public int KnjigaId { get; set; }
        public Knjiga? Knjiga { get; set; }

        public DateTime DatumZahtjeva { get; set; }
        public StatusZahtjeva Status { get; set; }
        public string? RazlogOdbijanja { get; set; }
    }

    public enum StatusZahtjeva
    {
        NaCekanju,
        Odobren,
        Odbijen
    }
}