using System.ComponentModel.DataAnnotations;

namespace ooadTim5.Models
{
    public class ClanskaKartica
    {
        public ClanskaKartica() { }

        [Key]
        public int Id { get; set; }
        public string? BrojKartice { get; set; }
        public DateTime DatumIzdavanja { get; set; }
        public DateTime ClanstvoVaziDo { get; set; }
        public bool Aktivan { get; set; }
        public string? KorisnikId { get; set; }
    }
}