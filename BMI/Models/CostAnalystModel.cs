using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BMI.Models
{
    public class CostAnalystModel
    {
        public int Id { get; set; }
        public POModel POModel { get; set; }
        [ForeignKey("POModel")]
        public string PO { get; set; }
        public Masterdatamodel masterdatamodel { get; set; }
        [ForeignKey("masterdatamodel")]
        public string SAP_Code { get; set; }
        public float Target_Lbs { get; set; }
        //public string Material { get; set; }
        //public float Price { get; set; }
        //public string Version { get; set; }

        [NotMapped]
        public double? Price { get; set; }
        [NotMapped]
        public double Yield { get; set; }
        [NotMapped]
        public double? Result { get; set; }

    }
}
