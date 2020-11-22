using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace BMI.Models
{
    public class RepackModel
    {
        [Key]
        public int id_repack { get; set; }
        public string po { get; set; }
        [Remote(action: "DateExist", controller: "Repack", HttpMethod = "POST", AdditionalFields = "date", ErrorMessage = "Repack date Not Available")]
        public DateTime date { get; set; }
        public string raw_source { get; set; }
        public float qty { get; set; }
        public DateTime production_date { get; set; }


        public PTModel fromPTModel { get; set; }
        [ForeignKey("fromPTModel")]
        public string from_pt { get; set; }


        public MasterBMIModel fromMasterBMIModel { get; set; }
        [ForeignKey("fromMasterBMIModel")]
        public string from_bmi_code { get; set; }

        public PTModel toPTModel { get; set; }
        [ForeignKey("toPTModel")]
        public string to_pt { get; set; }

        public MasterBMIModel toMasterBMIModel { get; set; }
        [ForeignKey("toMasterBMIModel")]
        public string to_bmi_code { get; set; }

        
    }
}
