using System;
using System.ComponentModel.DataAnnotations;

namespace LoginController
{
    public class UserCredential
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
