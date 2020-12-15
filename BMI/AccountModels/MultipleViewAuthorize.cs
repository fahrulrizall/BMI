using BMI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BMI.Models
{

    //private readonly RoleManager<IdentityRole> roleManager;
    //private readonly UserManager<ApplicationUser> userManager;

    //public MultipleViewAuthorize(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    //{
    //    this.roleManager = roleManager;
    //    this.userManager = userManager;
    //}



    public class MultipleViewAuthorize
    {
        public IQueryableRoleStore<IdentityRole> roles  { get; set; }
        public IQueryableUserStore<ApplicationUser> users { get; set; }
    }
}
