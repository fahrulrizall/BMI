using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace BMI.Models
{
    public class POModel
    {
        [Key]
        public string po { get; set; }
        #nullable enable
        public int? pt { get; set; }
        
        public string? plant  { get; set; }
        public string? batch  { get; set; }
        public string? pt_status  { get; set; }

        [Display(Name = "Estimated Time of Departure")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? etd { get; set; }

        [Display(Name = "Estimated Time of Arrival")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? eta { get; set; }

        [Display(Name = "Destination")]
        public string? destination { get; set; }
        
        public string? container { get; set; }
        public string? inv_no { get; set; }
        public string? fda_no { get; set; }
        public string? seal_no { get; set; }
        public string? ocean_carrier { get; set; }
        public DateTime? document_date { get; set; }
        public string? vessel_name { get; set; }
        public string? master_bol { get; set; }
        public string? house_bol { get; set; }
        public string? voyage_no { get; set; }
        public string? port_loading { get; set; }
        public string? port_receipt { get; set; }
        public int? shipment_no { get; set; }
        public string? po_status { get; set; }

        public string? saved { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public string? created_by { get; set; }
        public string? updated_by { get; set; }

        [NotMapped]
        [Remote(action: "DateExist", controller: "Production", HttpMethod = "POST", AdditionalFields = "date", ErrorMessage = "Production Date Not Available")]
        public DateTime date  { get; set; }
        [NotMapped]
        public double yield { get; set; }
    }
}
