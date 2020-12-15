using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BMI.Models
{
    public class RegisterView
    {
        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "Username Required")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Email Required")]
        [Remote(action: "EmailExist", controller: "Account", HttpMethod = "POST", AdditionalFields = "Email", ErrorMessage = "Email Already in use")]
        public string Email { get; set; }

        [Required]
        public string Department { get; set; }
        [Required]
        public string Position { get; set; }


        [DataType(DataType.Password)]
        [Required(ErrorMessage = "please input password")]
        [StringLength(100,MinimumLength =10,ErrorMessage ="masukan sesuai ketentuan")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "password and confirm must match")]
        public string ConfirmPassword { get; set; }

    }

}
