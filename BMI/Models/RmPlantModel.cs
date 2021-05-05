using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BMI.Models
{
    public class RmPlantModel
    {
        [Key]
        public string refference { get; set; }
        public VendorModel VendorModel { get; set; }
        [ForeignKey("VendorModel")]
        public string vendor { get; set; }
        public string sap_po { get; set; }
        public string pgi { get; set; }
        public string pgr { get; set; }
        public string return_no { get; set; }
        public string plant { get; set; }
        [Display(Name = "Delivery Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime delivery_date { get; set; }
        [Display(Name = "Estimated Time of Departure")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime etd { get; set; }
        [Display(Name = "Estimated Time of Arrival")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime eta { get; set; }
        public string invoice { get; set; }
        public string container { get; set; }
        public string bl_no { get; set; }
        public string shipping_line { get; set; }
        public string loading_port { get; set; }
        public string destination { get; set; }
        [Display(Name = "PGR Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime pgr_date { get; set; }
        public string status { get; set; }
    }
}
