using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BMI.Models
{
    public class ShipmentDetailModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string id_shipment_detail { get; set; }

        [ForeignKey("ShipmentModel")]
        public string id_shipment { get; set; }
        public DateTime pdc { get; set; }
        public string raw_source { get; set; }
        public string landing_site { get; set; }
        public int qty { get; set; }

        public ShipmentModel ShipmentModel { get; set; }
        #nullable enable
        public string? CS_location { get; set; }
        public string? created_by { get; set; }
        public string? updated_by { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }

        
    }
}
