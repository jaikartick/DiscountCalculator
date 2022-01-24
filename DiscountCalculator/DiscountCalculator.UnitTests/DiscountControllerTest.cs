using System;
using DiscountCalculator.Controllers;
using NUnit.Framework;

namespace DiscountCalculator.UnitTests
{
    [TestFixture]
    public class DiscountControllerTest
    {
        private DiscountController _discountController;

        [SetUp]
        public void CreateConverter()
        {
            _discountController = new DiscountController(null);
        }

        [Test]
        [TestCase(1000, 10, null, 10000)]
        [TestCase(1000, 10, 5, 9500)]
        public void GetDiscountPrice(double price, double weight, double discount, double expectedTotal)
        {
            var actual = _discountController.GetTotalPrice(new Gold() { Price = price, Weight = weight, Discount = discount });
            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedTotal, actual.TotalPrice);
        }
    }
}
