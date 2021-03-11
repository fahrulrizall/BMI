using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace BMI.Models
{
    public class FundModel
    {
        [Key]
        public int id_fund { get; set; }
        public string vendor { get; set; }
        public float idr_amount { get; set; }
        public float ex_rate { get; set; }
        public float usd_amount { get; set; }
    }
}
