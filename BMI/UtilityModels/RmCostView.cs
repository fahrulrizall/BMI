using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BMI.UtilityModels
{
    public class RmCostView
    {
        public RmCostView()
        {
            Material = new List<string>();
        }
        public List<string> Material  { get; set; }
        public string po { get; set; }
    }
}
