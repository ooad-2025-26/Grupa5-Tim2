using System.ComponentModel.DataAnnotations;

namespace ooadTim5.Models
{
    public class ValidateClanstvoDate : ValidationAttribute
    {
        protected override ValidationResult IsValid(
            object date, ValidationContext validationContext)
        {
            return ((DateTime)date > DateTime.Now)
                ? ValidationResult.Success
                : new ValidationResult(
                    "Datum važenja članstva mora biti u budućnosti!");
        }
    }
}