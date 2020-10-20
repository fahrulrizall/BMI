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
        public int id_shipment_detail { get; set; }

        [ForeignKey("Shipmentmodel")]
        public int id_ship { get; set; }

        [ForeignKey("MasterBMIModel")]
        public string bmi_code { get; set; }

        #nullable enable
        public string? reff { get; set; }
        public string? batch { get; set; }
        public int qty { get; set; }
        public float index { get; set; }
        public string? saved { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

        public Shipmentmodel? Shipmentmodel { get; set; }
        public MasterBMIModel? MasterBMIModel { get; set; }
    }
}
