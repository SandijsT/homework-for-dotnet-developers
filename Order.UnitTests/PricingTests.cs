using NUnit.Framework;
using OrderAPI.BusinessLogic;
using OrderAPI.DTOs;
using System.Reflection;
using System.Text.Json;

namespace Order.UnitTests
{
    [TestFixture]
    public class PricingTests
    {
        [Test]
        public void CalculatePrice_DiscountedAmount_HalfPrice()
        {
            var expectedPrice = 9899.00M;
            var orderAmount = 200;
            var discounts = new List<DiscountData> 
            { 
                new DiscountData 
                {
                    OrderAmount = orderAmount,
                    Discount = 50
                }
            };
            var filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            using (StreamWriter file = File.CreateText(filePath + @"/Data/DiscountData.json"))
            {
                file.Write(JsonSerializer.Serialize(discounts));
            }

            var result = Pricing.CalculatePrice(orderAmount);

            File.Delete(filePath + @"/Data/DiscountData.json");

            Assert.AreEqual(expectedPrice, result, $"Price should be {expectedPrice} but got {result}!");
        }

        [Test]
        public void GetDiscount_DiscountedAmount_EqualDiscount()
        {
            var expectedDiscount = 20;
            var orderAmount = 70;
            var discounts = new List<DiscountData> 
            {
                new DiscountData 
                {
                    OrderAmount = orderAmount,
                    Discount = 20
                }
            };

            var result = Pricing.GetDiscount(discounts, orderAmount);

            Assert.AreEqual(expectedDiscount, result, $"Discount should be {expectedDiscount} but got {result}!");
        }
    }
}
