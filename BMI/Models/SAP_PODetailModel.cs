using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BMI.Models
{
    public class SAP_PODetailModel
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public SAP_POModel SAP_POModel { get; set; }
        [ForeignKey("SAP_POModel")]
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
