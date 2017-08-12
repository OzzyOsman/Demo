using System;
using System.ComponentModel.DataAnnotations;

namespace BennettsBiking.Helpers
{
    /// <summary>
    /// A better and more user friendly approch would be to include a DatePicker on the UI
    /// as date validation is not always easy due to expected format, min/max dates etc
    /// </summary>
    public class DateValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime convertedDate;
                bool result = DateTime.TryParse(value.ToString(), out convertedDate);

                return result? ValidationResult.Success : new ValidationResult("Please enter a valid date in the following format: dd MMM yyyy"); ;
            }

            return new ValidationResult("Please enter a valid date in the following format: dd MMM yyyy");
        }
    }
}