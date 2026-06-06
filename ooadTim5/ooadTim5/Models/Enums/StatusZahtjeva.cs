using System.ComponentModel.DataAnnotations;

namespace ooadTim5.Models.Enums
{
    public enum StatusZahtjeva
    {
        [Display(Name = "Na čekanju")]
        Na_cekanju,
        [Display(Name = "Odobren")]
        odobren,
        [Display(Name = "Odbijen")]
        odbijen
    }
}