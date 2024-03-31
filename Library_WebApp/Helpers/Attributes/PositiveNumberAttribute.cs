using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Globalization;

namespace Library_WebApp.Helpers.Attributes
{
    public class PositiveNumberAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            int? num = (int?)value;

            if (num.HasValue)
            {
                if (num.Value > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            string errMessage = "Number should be greater than zero.";
            return String.Format(CultureInfo.CurrentCulture, errMessage, name);
        }
    }
}
