using System;

namespace NexaShopify.Core.Services
{
    public class PriceCalculator
    {
        public decimal CalculateDiscount(decimal price, decimal percentage)
        {
            if (price < 0)
                throw new ArgumentException("Price cannot be negative");
                
            return price - (price * (percentage / 100));
        }
    }
}