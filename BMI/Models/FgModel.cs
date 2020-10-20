using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace BMI.Models
{
    public class Fgmodel
    {

        [Key]

        public int id_fg { get; set; }

        [Display(Name = "SAP Code")]
        [Required(ErrorMessage = "This Field Required")]
        [ForeignKey("Masterdatamodel")]
        
        
        public string sap_code { get; set; }

        [Display(Name = "Plant")]
        public int plant { get; set; }

        [Display(Name = "Price LBS")]
        [Required(ErrorMessage = "This Field Required")]
        public float price_lbs { get; set; }

        [Display(Name = "Standard Price")]
        [Required(ErrorMessage = "This Field Required")]
        public float std_price { get; set; }

        [Display(Name = "Processing Fee")]
        [Required(ErrorMessage = "This Field Required")]
        public float processing_fee { get; set; }
    
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

        public Masterdatamodel Masterdatamodel { get; set; }

        
    }
}
