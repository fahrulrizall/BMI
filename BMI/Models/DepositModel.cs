using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BMI.Models
{
    public class DepositModel
    {
        [Key]
        public int id_deposit  { get; set; }
        public string deposit_detail  { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime paid_on  { get; set; }
        public float amount  { get; set; }
    }
}
