using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ooadTim5.Models.Enums;

namespace ooadTim5.Models
{
    public class NabavkaKnjiga
    {
        public NabavkaKnjiga() { }

        [Key]
        public int Id { get; set; }

        [ForeignKey("Knjiga")]
        [DisplayName("Knjiga:")]
        public int KnjigaId { get; set; }
        public Knjiga? Knjiga { get; set; }

        [ForeignKey("Dobavljac")]
        [DisplayName("Dobavljač:")]
        public int DobavljacId { get; set; }
        public Dobavljac? Dobavljac { get; set; }

        [Required(ErrorMessage = "Količina je obavezna!")]
        [Range(1, 1000, ErrorMessage = "Količina mora biti između 1 i 1000!")]
        [DisplayName("Količina:")]
        public int Kolicina { get; set; }

        [Required(ErrorMessage = "Datum narudžbe je obavezan!")]
        [DataType(DataType.Date)]
        [DisplayName("Datum narudžbe:")]
        public DateTime DatumNarudzbe { get; set; }

        [Required(ErrorMessage = "Očekivani datum isporuke je obavezan!")]
        [DataType(DataType.Date)]
        [DisplayName("Očekivani datum isporuke:")]
        public DateTime OcekivaniDatumIsporuke { get; set; }

        [EnumDataType(typeof(StatusNabavke))]
        [DisplayName("Status:")]
        public StatusNabavke Status { get; set; }

        public string? NazivKnjige { get; set; }   // NOVO
        public string? AutorKnjige { get; set; }
    }
}