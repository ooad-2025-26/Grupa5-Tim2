using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ooadTim5.Models
{
    public class NabavkaKnjiga
    {
        public NabavkaKnjiga() { }

        [Key]
        public int Id { get; set; }

        [ForeignKey("Knjiga")]
        public int KnjigaId { get; set; }
        public Knjiga? Knjiga { get; set; }

        [ForeignKey("Dobavljac")]
        public int DobavljacId { get; set; }
        public Dobavljac? Dobavljac { get; set; }

        public int Kolicina { get; set; }
        public DateTime DatumNarudzbe { get; set; }
        public DateTime OcekivaniDatumIsporuke { get; set; }
        public StatusNabavke Status { get; set; }
    }

    public enum StatusNabavke
    {
        UObradi,
        Poslano,
        Primljeno,
        Otkazano
    }
}