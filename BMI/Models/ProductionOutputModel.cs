using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BMI.Models
{
    public class ProductionOutputModel
    {
        [Key]
        public int id_productionoutput { get; set; }
        public string po { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Remote(action: "DateExist", controller: "Production", HttpMethod = "POST", AdditionalFields = "date", ErrorMessage = "Production Not Available")]
        public DateTime date { get; set; }
        public PTModel PTModel { get; set; }
        [ForeignKey("PTModel")]
        public string id_pt { get; set; }
        public MasterBMIModel MasterBMIModel { get; set; }
        [ForeignKey("MasterBMIModel")]
        public string bmi_code { get; set; }
        public float qty { get; set; }
        
        #nullable enable
        public string? raw_source { get; set; }
        public string? saved { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        [NotMapped]
        public double qty_production { get; set; }
        [NotMapped]
        public double available { get; set; }
    }
}
