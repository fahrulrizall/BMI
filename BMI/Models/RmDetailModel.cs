using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BMI.Models
{
    public class RmDetailModel
    {
        [Key]
        public int id_raw { get; set; }

        public Masterdatamodel Masterdatamodel { get; set; }
        public RmModel RmModel { get; set; }

        
        [Display(Name = "Refference")]
        [ForeignKey("RmModel")]
        public string raw_source { get; set; }

        #nullable enable
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

        public string? CS_location { get; set; }

        [Display(Name = "Qty")]
        public float? qty_pl { get; set; }
        [Display(Name = "Received")]
        public float? qty_received { get; set; }
        public string? landing_site_received { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public string? created_by { get; set; }
        public string? updated_by { get; set; }


        [NotMapped]
        public double total_qty { get; set; }
        [NotMapped]
        public float qty { get; set; }
        [NotMapped]
        public double? amount_pl { get; set; }
        [NotMapped]
        public double? amount_received { get; set; }
        [NotMapped]
        public string? reason { get; set; }

    }
}
