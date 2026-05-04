using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ooadTim5.Models
{
    public class Posudba
    {
        public Posudba() { }

        [Key]
        public int Id { get; set; }

        [ForeignKey("Knjiga")]
        public int KnjigaId { get; set; }
        public Knjiga? Knjiga { get; set; }

        public string? ClanId { get; set; }
        public DateTime DatumPosudbe { get; set; }
        public DateTime OcekivaniDatumVracanja { get; set; }
        public DateTime? DatumVracanja { get; set; }
        public StatusPosudbe Status { get; set; }
        public string? Napomena { get; set; }
    }

    public enum StatusPosudbe
    {
        Aktivna,
        Vracena,
        Kasnjenje
    }
}