using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace BMI.Models
{
    public class AdjustmentRawModel
    {
        [Key]
        public int id_adjustmentRaw { get; set; }
        public string raw_source { get; set; }
        public Masterdatamodel Masterdatamodel { get; set; }
        [ForeignKey("Masterdatamodel")]
        public string sap_code { get; set; }
        public double qty { get; set; }
        #nullable enable
        public string? reason { get; set; }
        public string? landing_site { get; set; }
        public string? status { get; set; }
        public string? created_by { get; set; }
        public string? updated_by { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}
