using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace BMI.Models
{
    public class Masterdatamodel
    {
        [Key]
        public string sap_code { get; set; }
        #nullable enable
        public string? description { get; set; }
        public float? lbs { get; set; }
        public string? category { get; set; }
        public string? hers_code { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public string? created_by { get; set; }
        public string? updated_by { get; set; }
        public float? standard_price { get; set; }
        public float? PF3770 { get; set; }
        public float? PF3710 { get; set; }

    }
}
