using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BMI.UtilityModels
{
    public class RefferenceView
    {
        public string Id { get; set; }
        public string Refference { get; set; }
        public double Input { get; set; }
        public double Output { get; set; }
        public double Yield { get; set; }

        [Display(Name = "Estimated Time of Arrival")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        public string Vessel { get; set; }
    }
}
