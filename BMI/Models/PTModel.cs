using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace BMI.Models
{
    public class PTModel
    {
        [Key]
        public string id_pt { get; set; }
        public int pt { get; set; }
        public string plant  { get; set; }
        public string batch  { get; set; }
        public string po  { get; set; }
        public string status  { get; set; }
        [NotMapped]
        [Remote(action: "DateExist", controller: "Production", HttpMethod = "POST", AdditionalFields = "date", ErrorMessage = "Production Date Not Available")]
        public DateTime date  { get; set; }
        
    }
}
