using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Validation
{
    public class OrderAmountAttribute : ValidationAttribute
    {
        public int MinimumOrderAmount { get; }
        public int MaximumOrderAmount { get; }

        public OrderAmountAttribute(int minimumOrderAmount, int maximumOrderAmount)
        {
            MinimumOrderAmount = minimumOrderAmount;
            MaximumOrderAmount = maximumOrderAmount;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                int amount;
                var isRound = int.TryParse(value.ToString(), out amount);

                if (!isRound)
                {
                    return new ValidationResult("Order amount must be a round number!");
                }
                if(amount < MinimumOrderAmount || amount > MaximumOrderAmount)
                {
                    return new ValidationResult($"Order amount must be a between {MinimumOrderAmount} and {MaximumOrderAmount}!");
                }
            }

            return ValidationResult.Success;
        }
    }
}
