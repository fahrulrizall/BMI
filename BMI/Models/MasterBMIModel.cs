using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BMI.Models
{
    public class MasterBMIModel
    {

        [Key]
        public string bmi_code { get; set; }

        #nullable enable
        public string? sap_code { get; set; }
        public string? description { get; set; }
        public float? lbs { get; set; }
        public string? index_category { get; set; }
        public string? daily_category { get; set; }
        public string? weekly_category { get; set; }
        public float? index { get; set; }
        public float? index_lb { get; set; }
        public float? index_cs { get; set; }
        public float? zafc_kg { get; set; }
        public float? zafc_cs { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}
