using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BMI.UtilityModels;
using BMI.Models;

namespace BMI.Models
{
    public class DepositView
    {
        public List<RmDetailModel> otw { get; set; }
        public List<RmDetailModel> in_plant { get; set; }
        public List<ProductionOutputModel> fg { get; set; }
    }
}
