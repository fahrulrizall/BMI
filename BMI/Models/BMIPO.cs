using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BMI.Models
{
    public class BMIPO
    {
        [Key]
        public string po  { get; set; }
        public int pt   { get; set; }
    }
}
