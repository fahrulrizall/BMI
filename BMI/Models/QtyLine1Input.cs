using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BMI.Models
{
    public class QtyLine1Input
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public DateVesselModel datevesselmodel { get; set; }
        [ForeignKey("datevesselmodel")]
        public int id_dateVessel { get; set; }
        public string refference { get; set; }
        public float qty { get; set; }
    }
}
