using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BMI.Models
{
    public class ShipmentModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string id_shipment { get; set; }

        [ForeignKey("POModel")]
        public string po { get; set; }

        public DateTime pdc { get; set; }

        [ForeignKey("MasterBMIModel")]
        public string bmi_code { get; set; }
        public int qty { get; set; }

        [ForeignKey("POModelBatch")]
        public string batch { get; set; }
        public string raw_source { get; set; }
        public float index { get; set; }
        public string created_by { get; set; }
        public string updated_by { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

        public POModel POModel { get; set; }
        public POModel POModelBatch { get; set; }
        public MasterBMIModel MasterBMIModel { get; set; }

        [NotMapped]
        public double cases { get; set; }
    }
}
