using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BMI.Models
{
    public class ProductionOutputModel
    {
        [Key]
        public int id_productionoutput { get; set; }
        public string po { get; set; }
        public DateTime date { get; set; }
        public int pt { get; set; }
        public MasterBMIModel MasterBMIModel { get; set; }
        public MasterBMIModel MasterBMIModel1 { get; set; }
        [ForeignKey("MasterBMIModel")]
        public string bmi_code { get; set; }
        public float qty { get; set; }
        public string batch { get; set; }
        [ForeignKey("MasterBMIModel1")]
        #nullable enable
        public string? bmi_code_repack { get; set; }
        public string? batch_repack { get; set; }
        public string? saved { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }

        
    }
}
