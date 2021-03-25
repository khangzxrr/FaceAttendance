using System;
using System.ComponentModel.DataAnnotations;

namespace SpeedyAPI.Validations
{
    public class MustBeInPresent : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime dateTime = (DateTime)value;

            if (dateTime >= DateTime.Now)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage ?? "Make sure your date is >= than today");
        }
    }
}
