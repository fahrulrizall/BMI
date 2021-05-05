using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BMI.Models
{
    public class VendorModel
    {
        [Key]
        public string code { get; set; }
        public string description { get; set; }
    }
}
