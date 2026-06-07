using System.ComponentModel.DataAnnotations;

namespace ooadTim5.Models
{
    public class IzmijeniKorisnikaViewModel
    {
        public string Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Rola { get; set; }
    }
}