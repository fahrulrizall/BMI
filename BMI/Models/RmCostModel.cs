using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BMI.Models
{
    public class RmCostModel
    {
        [Key]
        public int Id_Material { get; set; }
        public string Material { get; set; }
        public POModel POModel { get; set; }
        [ForeignKey("POModel")]
        public string PO { get; set; }
    }
}
