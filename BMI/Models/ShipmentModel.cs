using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace BMI.Models
{
    public class Shipmentmodel
    {
        [Key]
        [Display(Name = "Shipment No")]
        [Remote(action: "IdExist",controller:"Shipment",HttpMethod ="POST", AdditionalFields ="id_ship", ErrorMessage = "No Shipment Already Exist")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_shipment { get; set; }
        
        [Display(Name = "Estimated Time of Departure")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime etd { get; set; }

        [Display(Name = "Estimated Time of Arrival")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime eta { get; set; }

        [Display(Name = "Destination")]
        [Required]
        public string destination { get; set; }

        [Display(Name = "Purchase Order")]
        //[Remote(action: "POExist", controller: "Shipment", HttpMethod = "POST", AdditionalFields = "po", ErrorMessage = "No Shipment Already Exist")]
        [Range(1111111111, 9999999999,ErrorMessage ="PO mush 10 Character")]
        [Required]
        public string po { get; set; }

        public string saved { get; set; }

        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }

    }
}
