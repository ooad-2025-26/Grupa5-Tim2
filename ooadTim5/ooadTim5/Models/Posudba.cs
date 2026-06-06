using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ooadTim5.Models.Enums;

namespace ooadTim5.Models
{
    public class Posudba
    {
        public Posudba() { }

        [Key]
        public int Id { get; set; }

        [ForeignKey("Knjiga")]
        [DisplayName("Knjiga:")]
        public int KnjigaId { get; set; }
        public Knjiga? Knjiga { get; set; }

        [DisplayName("Član:")]
        public string? ClanId { get; set; }

        [Required(ErrorMessage = "Datum posudbe je obavezan!")]
        [DataType(DataType.Date)]
        [DisplayName("Datum posudbe:")]
        public DateTime DatumPosudbe { get; set; }

        [Required(ErrorMessage = "Očekivani datum vraćanja je obavezan!")]
        [ValidatePosudbaDate]
        [DataType(DataType.Date)]
        [DisplayName("Očekivani datum vraćanja:")]
        public DateTime OcekivaniDatumVracanja { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Datum vraćanja:")]
        public DateTime? DatumVracanja { get; set; }

        [EnumDataType(typeof(StatusPosudbe))]
        [DisplayName("Status:")]
        public StatusPosudbe Status { get; set; }

        [StringLength(500, ErrorMessage = "Napomena ne može biti duža od 500 karaktera!")]
        [DisplayName("Napomena:")]
        public string? Napomena { get; set; }
    }
}