using NUnit.Framework;
using OrderAPI.Validation;

namespace Order.UnitTests
{
    [TestFixture]
    public class ValidationTests
    {
        [Test]
        public void IsValid_MaximumAmount_Valid() => IsValid(10, 100, "100", true);
        [Test]
        public void IsValid_MinimumAmount_Valid() => IsValid(10, 100, "10", true);
        [Test]
        public void IsValid_AmountOutsideOfRange_Invalid() => IsValid(10, 100, "150", false);
        [Test]
        public void IsValid_AmountNotWholeNumber_Invalid() => IsValid(10, 100, "0.5", false);

        private void IsValid(int minOrder, int maxOrder, string order, bool isValid)
        {
            var orderAmountAttribute = new OrderAmountAttribute(minOrder, maxOrder);
            var result = orderAmountAttribute.IsValid(order);

            Assert.AreEqual(isValid, result);
        }

        [Test]
        public void IsValid_DeliveryDateInFuture_Valid() => IsValid(DateTime.Now.AddDays(20).ToString(), true);
        [Test]
        public void IsValid_DeliveryDateInPast_Invalid() => IsValid(DateTime.Now.AddDays(-20).ToString(), false);
        [Test]
        public void IsValid_DeliveryInvalidFormat_Invalid() => IsValid("05/18/2023", false);

        private void IsValid(string expectedDeliveryDate, bool isValid)
        {
            var expectedDeliveryDateAttribute = new ExpectedDeliveryDateAttribute();
            var result = expectedDeliveryDateAttribute.IsValid(expectedDeliveryDate);

            Assert.AreEqual(isValid, result);
        }
    }
}
