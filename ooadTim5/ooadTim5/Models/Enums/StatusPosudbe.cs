using System.ComponentModel.DataAnnotations;

namespace ooadTim5.Models.Enums
{
    public enum StatusPosudbe
    {
        [Display(Name = "Aktivna")]
        aktivna,
        [Display(Name = "Vraćena")]
        vracena,
        [Display(Name = "Kašnjenje")]
        kasnjenje
    }
}