using BMI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace BMI.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(RoleView roleView)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = roleView.RoleName
                };
                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    TempData["msg"] = "Item Succesfully Added";
                    TempData["result"] = "success";
                    return RedirectToAction("Index");
                }
                TempData["msg"] = "Item Failed to Add";
                TempData["result"] = "failed";
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new MultipleViewAuthorize();
            //model.roles = roleManager.Roles;
            //model.users = userManager.Users;
            var roles = roleManager.Roles;
            var user = userManager.Users;
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> Getrole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            return Json(role);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            var delete = await roleManager.DeleteAsync(role);

            if (delete.Succeeded)
            {
                TempData["msg"] = "Item Succesfully Deleted";
                TempData["result"] = "success";
                return RedirectToAction("Index");
            }
            TempData["msg"] = "Item Failed to Delete";
            TempData["result"] = "failed";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRole(RoleView roleView)
        {
            var role = await roleManager.FindByIdAsync(roleView.Id);

            role.Name = roleView.RoleName;
            var result = await roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                TempData["msg"] = "Item Succesfully Updated";
                TempData["result"] = "success";
                return RedirectToAction("Index");
            }
            TempData["msg"] = "Item Failed to Update";
            TempData["result"] = "failed";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetUserRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            var model = new List<UserRoleView>();
            foreach (var user in userManager.Users) 
            {
                var UserRoleView = new UserRoleView
                {
                    UserId = user.Id,
                    UserName = user.Email
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    UserRoleView.IsSelected = true;
                }
                else
                {
                    UserRoleView.IsSelected = false;
                }
                model.Add(UserRoleView);
            }
            ViewBag.id = id;
            return View (model);
        }

        public async Task<IActionResult> UpdateUserRole(List<UserRoleView> model, string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessages = $"Role with id {id} not available";
                return View("NotFound");
            }
            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);
                IdentityResult result = null; 
                if (model[i].IsSelected &&  !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }else 
                if (!model[i].IsSelected && (await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("GetUserRole",new { id = id });
                }
            }
            return RedirectToAction("GetUserRole", new { id = id });
        }


       



    }
}
