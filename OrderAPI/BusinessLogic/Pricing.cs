using OrderAPI.DTOs;
using System.Reflection;
using System.Text.Json;

namespace OrderAPI.BusinessLogic
{
    public static class Pricing
    {
        private const decimal BasePrice = 98.99M;

        public static decimal CalculatePrice(int orderAmount)
        {
            var filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var discountData = File.ReadAllText(filePath + @"/Data/DiscountData.json");
            var discounts = JsonSerializer.Deserialize<List<DiscountData>>(discountData);

            if (discounts == null)
            {
                throw new Exception("Failed to find discount data file, the file might not exist!");
            }

            var appliedDiscount = GetDiscount(discounts, orderAmount);
            var totalPrice = BasePrice * orderAmount;
            var actualPrice = Math.Round(totalPrice - (totalPrice * (appliedDiscount / 100M)), 2);

            return actualPrice;
        }

        public static int GetDiscount(List<DiscountData> discounts, int orderAmount)
        {
            foreach (var discount in discounts)
            {
                if (discount.OrderAmount <= orderAmount)
                {
                    return discount.Discount;
                }
            }

            return 0;
        }
    }
}
