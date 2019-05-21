using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Globomatics.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Wrong email format!")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password has to be confirmed")]
        [Display(Name ="Confirmed password")]
        public string ConfirmedPassword { get; set; }
    }
}
