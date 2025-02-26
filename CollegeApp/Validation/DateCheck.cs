using System.ComponentModel.DataAnnotations;

namespace CollegeApp.Validation
{
    public class DateCheck : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var date = (DateTime?)value;
            if (date < DateTime.Today)
                return new ValidationResult("Date of Addmission should be Today's Date");
            else
                return ValidationResult.Success;
        }
    }
}
