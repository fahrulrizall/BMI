using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace BMI.Models
{
    public class Masterdatamodel
    {
        public Masterdatamodel()
        {
            Fgmodels = new List<Fgmodel>();
        }
        [Key]
        public string sap_code { get; set; }
        #nullable enable
        public string? description { get; set; }
        public float? lbs { get; set; }
        public string? category { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public ICollection<Fgmodel> Fgmodels { get; set; }
        public ICollection<RmDetailModel> Rmmodels { get; set; }

    }
}
