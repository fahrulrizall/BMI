using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BMI.Models
{
    public class Rmmodel
    {
        [Key]
        public int id_raw { get; set; }
        [Display(Name = "Estimated Time of Departure")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        #nullable enable
        public DateTime etd { get; set; }

        [Display(Name = "Estimated Time of Arrival")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime eta { get; set; }

        [Display(Name = "Container")]
        public string? container { get; set; }


        [Display(Name = "Refference")]
        public string? reff { get; set; }


        [Display(Name = "Status")]
        public string? status { get; set; }


        [Display(Name = "Landing Site")]
        public string? landing_site { get; set; }

        [ForeignKey("Masterdatamodel")]
        [Display(Name = "SAP Code")]
        public string? sap_code { get; set; }

        [Display(Name = "Cases")]
        public int? cases { get; set; }

        public string? uom { get; set; }

        [Display(Name = "USD Price")]
        public float? usd_price { get; set; }


        [Display(Name = "IDR Rate")]
        public float? ex_rate { get; set; }

        [Display(Name = "Packing List")]
        public float? qty_pl { get; set; }
        [Display(Name = "Received")]
        public float? qty_received { get; set; }
        public string? saved { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        
        public Masterdatamodel? Masterdatamodel { get; set; }

    }
}
