using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BMI.Models
{
    public class PTModel
    {
        [Key]
        public int id_pt { get; set; }
        public int pt { get; set; }
        public string plant  { get; set; }
        public string batch  { get; set; }
        public string po  { get; set; }
        
    }
}
