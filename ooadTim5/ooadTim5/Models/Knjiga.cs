using System.ComponentModel.DataAnnotations;
using ooadTim5.Models.Enums;

namespace ooadTim5.Models
{
    public class Knjiga
    {
        public Knjiga() { }

        [Key]
        public int Id { get; set; }

        public string? Naziv { get; set; }

        public string? Autor { get; set; }

        public string? ISBN { get; set; }

        public string? Kategorija { get; set; }

        public int GodinaIzdanja { get; set; }

        public int BrojStranica { get; set; }

        public string? Izdavac { get; set; }

        public string? Naslovnica { get; set; }

        public StatusKnjige Status { get; set; }
    }
}