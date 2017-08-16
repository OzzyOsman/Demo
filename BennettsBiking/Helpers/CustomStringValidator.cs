using System.ComponentModel.DataAnnotations;

namespace BennettsBiking.Helpers
{
    /// <summary>
    /// An alternative might be to have a string Extension to strip out any unwanted characters 
    /// see commented out (requries static class) example below
    /// </summary>
    public class CustomStringValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string valueToCheck = value.ToString();

                if (valueToCheck.IndexOfAny(new char[] { '*', '!', '[', ']', '-' }) == -1)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult("Please enter a valid string without special characters: ],-,*,!,[");
        }
    }

    /// <summary>
    /// Usage Example: "teststring".ReplaceUnwantedChars()
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    //public static string ReplaceUnwantedChars(this string str)
    //{
    //    return str
    //        .Replace("!", "")
    //        .Replace("*", "");
    //}
}