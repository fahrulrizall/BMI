using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BMI.Models
{
    public class RmPlantDetailModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string id_rmmowidetail { get; set; }
        public RmPlantModel RMMOWIModel { get; set; }
        [ForeignKey("RMMOWIModel")]
        public string refference { get; set; }
        public Masterdatamodel Masterdatamodel { get; set; }
        [ForeignKey("Masterdatamodel")]
        public string sap_code { get; set; }
        public string style { get; set; }
        public float unit_price { get; set; }
        public float qty_pl { get; set; }
        public float qty_received { get; set; }
        public string vessel { get; set; }

        [NotMapped]
        public double amount_pl { get; set; }
        [NotMapped]
        public double amount_received { get; set; }
    }

    
}
