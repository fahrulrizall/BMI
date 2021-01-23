using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BMI.Models
{
    public class RmModel
    {
        [Key]
        public string raw_source { get; set; }
        [Display(Name = "Estimated Time of Departure")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        #nullable enable
        public DateTime etd { get; set; }

        [Display(Name = "Estimated Time of Arrival")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime eta { get; set; }

        [Display(Name = "Container")]
        public string? container { get; set; }

        [Display(Name = "Status")]
        public string? status { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public string? created_by { get; set; }
        public string? updated_by { get; set; }

        [NotMapped]
        public DateTime start_date { get; set; }
        [NotMapped]
        public DateTime finish_date { get; set; }
    }
}
