using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DiscountCalculator
{
    public class Gold
    {
        [Required]
        [Range(0, 1000000, ErrorMessage = "Invalid Price")]
        public double? Price { get; set; }

        [Required]
        [Range(0, 10000, ErrorMessage = "Invalid Weight")]
        public double? Weight { get; set; }

        [Range(0,100, ErrorMessage = "Invalid Discount")]
        public double Discount { get; set; }
    }
}
