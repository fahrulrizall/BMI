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
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Remote(action: "DateExist", controller: "Repack", HttpMethod = "POST", AdditionalFields = "date", ErrorMessage = "Packing Date Not Available")]
        public DateTime date { get; set; }
        public string raw_source { get; set; }
        public double qty { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime production_date { get; set; }


        public POModel fromPOModel { get; set; }
        [ForeignKey("fromPOModel")]
        public string from_po { get; set; }


        public MasterBMIModel fromMasterBMIModel { get; set; }
        [ForeignKey("fromMasterBMIModel")]
        public string from_bmi_code { get; set; }

        public POModel toPOModel { get; set; }
        [ForeignKey("toPOModel")]
        public string to_po { get; set; }

        public MasterBMIModel toMasterBMIModel { get; set; }
        [ForeignKey("toMasterBMIModel")]
        public string to_bmi_code { get; set; }
        #nullable enable
        public string? landing_site { get; set; }
        public string? created_by { get; set; }
        public string? updated_by { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }

        [NotMapped]
        public  double cases { get; set; }
        [NotMapped]
        public  double available { get; set; }
        [NotMapped]
        public int? pt { get; set; }


    }
}
