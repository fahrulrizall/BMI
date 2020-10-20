using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BMI.Models
{
    public class Usermodel
    {
        [Key]
        [Required(ErrorMessage ="please input valid ID")]
        public int UserId { get; set; }


        [Required(ErrorMessage = "please input fisrt name")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "please input last name")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "please input email address")]
        public string EmailAddress { get; set; }
        
        
        [Compare("EmailAddress",ErrorMessage ="email and confirm must match")]
        public string ConfirmEmail { get; set; }
        
        
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "please input password")]
        [StringLength(100,MinimumLength =10,ErrorMessage ="masukan sesuai ketentuan")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "password and confirm must match")]
        public string ConfirmPassword { get; set; }

    }

}
