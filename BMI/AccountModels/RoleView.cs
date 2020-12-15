using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BMI.Models
{
    public class RoleView
    {
        public string Id { get; set; }
        [Required]
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
}
