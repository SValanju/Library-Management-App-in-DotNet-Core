using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Library_WebApp.Helpers.Attributes
{
    public class CustomEmailAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            var email = value as string;
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Basic email format check
            if (!new EmailAddressAttribute().IsValid(email))
                return false;

            // Checking top-level domain in email
            var regex = new Regex(@"\.[a-zA-Z]{2,}$");
            return regex.IsMatch(email);
        }
    }
}
