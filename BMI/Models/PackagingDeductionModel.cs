using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BMI.Models
{
    public class PackagingDeductionModel
    {
        public int id { get; set; }
        public string po_shipment { get; set; }
        public string po_pt { get; set; }
        public string sap_code { get; set; }
        public int qty { get; set; }
    }
}
