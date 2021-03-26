using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BMI.Models
{
    public class RmChecking
    {
        public string raw_source { get; set; }
        public string landing_site_rm { get; set; }
        public string sap_code_rm { get; set; }
        public Masterdatamodel masterdatamodel_rm { get; set; }
        public float? qty_rm { get; set; }

        public string landing_site_prod { get; set; }
        public string sap_code_prod { get; set; }
        public Masterdatamodel masterdatamodel_prod { get; set; }
        public double? qty_prod { get; set; }

        public decimal diffrence { get; set; }

        public List<RmDetailModel> RmDetailModel {get;set;}
        public List<ProductionInputModel> ProductionInputModel {get;set;}


    }
}
