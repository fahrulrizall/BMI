using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace BMI.Models
{
    public class DestroyRawModel
    {
        [Key]
        public int id_destroyRaw { get; set; }
        public string raw_source { get; set; }
        public Masterdatamodel Masterdatamodel { get; set; }
        [ForeignKey("Masterdatamodel")]
        public string sap_code { get; set; }
        public double qty { get; set; }
        #nullable enable
        public string? reason { get; set; }
        public string? status { get; set; }
    }
}
