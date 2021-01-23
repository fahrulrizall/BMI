using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using BMI.UtilityModels;

namespace BMI.Models
{
    public class ProductionView
    {
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime date { get; set; }
        public DateTime production_date { get; set; }
        public int destination_pt { get;set; }
        public string po { get;set; }
        public string po_bmi { get;set; }
        public string bmi_code { get; set; }
        public string sap_code { get; set; }
        public string id_pt { get; set; }
        public double total { get; set; }
        public double qty { get; set; }
        public string batch { get; set; }
        public string landing_site { get; set; }
        public string raw_source { get; set; }
        public string to_bmi_code { get; set; }
        public string reason { get; set; }
        public MasterBMIModel MasterBMIModel { get; set; }
        public Masterdatamodel Masterdatamodel { get; set; }
        public POModel POModel { get; set; }
        public List<ProductionInputModel> ProductionInputModel { get; set; }
        public List<ProductionOutputModel> ProductionOutputModel { get; set; }
        public List<CategoryView> CategoryView { get; set; }
        public List<RepackModel> RepackModel { get; set; }

    }
}
