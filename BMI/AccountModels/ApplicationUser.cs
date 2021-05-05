using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Security.Principal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BMI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Department { get; set; }
        public string Position { get; set; }

        [NotMapped]
        public RegisterView RegisterView { get; set; }

        //[DataType(DataType.Password)]
        //[Required(ErrorMessage = "please input password")]
        //[StringLength(100, MinimumLength = 10, ErrorMessage = "masukan sesuai ketentuan")]
        //public string Password { get; set; }

        //[Compare("Password", ErrorMessage = "Password and Confirm Password not Match")]
        //public string ConfirmPassword { get; set; }
    }
}
