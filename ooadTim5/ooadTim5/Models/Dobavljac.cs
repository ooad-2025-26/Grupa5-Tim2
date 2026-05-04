using System.ComponentModel.DataAnnotations;

namespace ooadTim5.Models
{
    public class Dobavljac
    {
        public Dobavljac() { }

        [Key]
        public int Id { get; set; }
        public string? Naziv { get; set; }
        public string? KontaktEmail { get; set; }
        public string? KontaktTelefon { get; set; }
        public string? Adresa { get; set; }
    }
}