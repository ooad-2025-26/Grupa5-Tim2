using System.ComponentModel.DataAnnotations;

namespace ooadTim5.Models
{
    public class ValidatePosudbaDate : ValidationAttribute
    {
        protected override ValidationResult IsValid(
            object date, ValidationContext validationContext)
        {
            var posudba = (Posudba)validationContext.ObjectInstance;
            DateTime datumPosudbe = posudba.DatumPosudbe;
            DateTime ocekivani = (DateTime)date;

            return ocekivani > datumPosudbe
                ? ValidationResult.Success
                : new ValidationResult(
                    "Očekivani datum vraćanja mora biti nakon datuma posudbe!");
        }
    }
}