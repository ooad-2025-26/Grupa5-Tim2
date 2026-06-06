using System.ComponentModel.DataAnnotations;

namespace ooadTim5.Models.Enums
{
    public enum StatusNabavke
    {
        [Display(Name = "U obradi")]
        u_obradi,
        [Display(Name = "Poslano")]
        poslano,
        [Display(Name = "Primljeno")]
        primljeno,
        [Display(Name = "Otkazano")]
        otkazano
    }
}