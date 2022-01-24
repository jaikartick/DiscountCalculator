using System;
using System.ComponentModel.DataAnnotations;

namespace DiscountCalculator
{
    public class UserCredential
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
