using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace OrderAPI.Validation
{
    public class ExpectedDeliveryDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value != null)
            {
                DateTime expectedDeliveryDate;
                var isCorrectFormat = DateTime.TryParse(value.ToString(), new CultureInfo("lv-LV"), DateTimeStyles.None, out expectedDeliveryDate);

                if (!isCorrectFormat)
                {
                    return new ValidationResult("Expected delivery date was not in the correct format of a date, use the Latvian standart for dates!");
                }
                if (expectedDeliveryDate < DateTime.Now)
                {
                    return new ValidationResult($"Expected delivery date in {expectedDeliveryDate}, is not in the future!");
                }
            }

            return ValidationResult.Success;
        }
    }
}
