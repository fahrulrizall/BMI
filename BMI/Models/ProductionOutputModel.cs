﻿using System;
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
        public string po_bmi { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
       
        public DateTime date { get; set; }
        public POModel POModel { get; set; }
        [ForeignKey("POModel")]
        public string po { get; set; }
        public MasterBMIModel MasterBMIModel { get; set; }
        [ForeignKey("MasterBMIModel")]
        public string bmi_code { get; set; }
        public float qty { get; set; }
        
        #nullable enable
        public string? landing_site { get; set; }
        public string? raw_source { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public string? created_by { get; set; }
        public string? updated_by { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? gr_date { get; set; }
        public string? fairtrade_status { get; set; }
        public string? grade { get; set; }

        [NotMapped]
        public double qty_production { get; set; }
        [NotMapped]
        public double available { get; set; }
        [NotMapped]
        public double cases { get; set; }
        [NotMapped]
        public double amount { get; set; }
        [NotMapped]
        public double lbs { get; set; }
        [NotMapped]
        public double rm_cost { get; set; }

        [NotMapped]
        public int hour { get; set; }
        [NotMapped]
        public int minute { get; set; } 
        [NotMapped]
        public DateTime production_date { get; set; }
        [NotMapped]
        public string? status { get; set; }

    }
}
