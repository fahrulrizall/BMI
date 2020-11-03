using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BMI.Models
{
    public class ProductionInputModel
    {
        [Key]
        public int id_productioninput { get; set; }
        public BMIPO BMIPO { get; set; }
        [ForeignKey("BMIPO")]
        public string po { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime date { get; set; }
        public int pt { get; set; }
        public string reff { get; set; }

        public MasterBMIModel MasterBMIModel { get; set; }
        [ForeignKey("MasterBMIModel")]
        public string bmi_code { get; set; }
        public float qty { get; set; }
        public string landing_site { get; set; }
        
        
        #nullable enable
        public string? saved { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }

        
    }
}
