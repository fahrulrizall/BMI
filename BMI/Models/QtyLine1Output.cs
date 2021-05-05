using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BMI.Models
{
    public class QtyLine1Output
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string id { get; set; }

        public Masterdatamodel Masterdatamodel { get; set; }

        [ForeignKey("Masterdatamodel")]
        public string sap_code { get; set; }

        public DateVesselModel datevesselmodel { get; set; }
        [ForeignKey("DateVesselModel")]
        public string id_dateVessel { get; set; }
        public string refference { get; set; }
        public float qty { get; set; }
        public string batch { get; set; }
    }
}
