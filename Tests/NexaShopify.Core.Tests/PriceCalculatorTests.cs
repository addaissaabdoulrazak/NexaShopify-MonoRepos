using Xunit;
using NexaShopify.Core.Services;
using System;

namespace NexaShopify.Core.Tests
{
    public class PriceCalculatorTests
    {
        [Fact]
        public void CalculateDiscount_ShouldReturnCorrectPrice_WhenValidInputs()
        {
            // Arrange
            var calculator = new PriceCalculator();
            decimal price = 100m;
            decimal discount = 20m;

            // Act
            var result = calculator.CalculateDiscount(price, discount);

            // Assert
            Assert.Equal(80m, result);
        }

        [Fact]
        public void CalculateDiscount_ShouldThrowException_WhenPriceIsNegative()
        {
            // Arrange
            var calculator = new PriceCalculator();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => calculator.CalculateDiscount(-10, 10));
        }
    }
}