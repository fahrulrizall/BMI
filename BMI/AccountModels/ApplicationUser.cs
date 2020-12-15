using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BMI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Department { get; set; }
        public string Position { get; set; }
    }
}
