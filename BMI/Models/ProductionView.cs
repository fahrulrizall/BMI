using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BMI.Models
{
    public class ProductionView
    {
        public string code { get; set; }
        public float total { get; set; }
        public MasterBMIModel MasterBMIModels { get; set; }
    }
}
