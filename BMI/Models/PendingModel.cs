using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BMI.Models
{
    public class PendingModel
    {
        [Key]
        public int id_pending { get; set; }
        [ForeignKey("RmModel")]
        public string raw_source { get; set; }
        public float qty { get; set; }

        public RmModel RmModel { get; set; }
    }
}
