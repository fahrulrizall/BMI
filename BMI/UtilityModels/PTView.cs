using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMI.Models;
using BMI.UtilityModels;

namespace BMI.Models
{
    public class PTView
    {
        public int? pt { get; set; }
        public string po { get; set; }
        public string batch { get; set; }
        public string pt_status { get; set; }
        public DateTime date { get; set; }

    }
}
