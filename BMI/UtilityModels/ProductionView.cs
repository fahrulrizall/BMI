﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BMI.Models
{
    public class ProductionView
    {
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime date { get; set; }
        public string code { get; set; }
        public string pt { get; set; }
        public double total { get; set; }
        public string batch { get; set; }
        public MasterBMIModel MasterBMIModel { get; set; }
        public PTModel PTModel { get; set; }
    }
}
