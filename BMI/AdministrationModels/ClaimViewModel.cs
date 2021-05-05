using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BMI.Models
{
    public class ClaimViewModel
    {
        public ClaimViewModel()
        {
            Claims = new List<ClaimList>();
        }
        public string UserId { get; set; }
        public List<ClaimList> Claims { get; set; }
    }
}
