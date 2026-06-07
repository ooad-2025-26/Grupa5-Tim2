using System.ComponentModel.DataAnnotations;

namespace ooadTim5.Models
{
    public class DodajKorisnikaViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Lozinka { get; set; }

        [Required]
        public string Rola { get; set; }
    }
}