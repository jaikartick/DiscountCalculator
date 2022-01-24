using System;
namespace DiscountCalculator
{
    public class DiscountResult
    {
        public double TotalPrice { get; set; }

        public DiscountResult(double totalPrice)
        {
            TotalPrice = totalPrice;
        }
    }
}
