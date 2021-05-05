using BMI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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
                    return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
                }
                TempData["msg"] = "Item Failed to Add";
                TempData["result"] = "failed";
                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));

            }
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }

        [HttpGet]
        public IActionResult UserList()
        {
            var users = userManager.Users;
            return View(users);
        }

        [HttpGet]
        public IActionResult RoleList()
        {
            var roles = roleManager.Roles;
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

            if (role == null)
            {                ViewBag.ErrorMessage = $"Role with ID= {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    TempData["msg"] = "Item Succesfully Deleted";
                    TempData["result"] = "success";
                    return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
                }

                foreach (var error in result.Errors)
                {
                    TempData["msg"] = "Item Failed to Delete";
                    TempData["result"] = "failed";
                    ModelState.AddModelError("", error.Description);
                }
            }
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));

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
                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
            }
            TempData["msg"] = "Item Failed to Update";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }

        public async Task<IActionResult> GetUserRole(string id,string name)
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
            ViewBag.name = name;
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
                    return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
                }
            }
            TempData["msg"] = "User Role Updated";
            TempData["result"] = "success";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }

        [HttpGet]
        public async Task<IActionResult> EditUser (string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user== null)
            {
                ViewBag.ErrorMessage = $"User with id = {id} cannot be found";
                return View("NotFound");
            }

            var userClaims = await userManager.GetClaimsAsync(user);
            var roleClaims = await userManager.GetRolesAsync(user);

            var data = userClaims.ToString();
            var model = new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Departement = user.Department,
                Position = user.Position,
                Claims = userClaims.Select(a=>a.Value).ToList(),
                Roles = roleClaims,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel EditUserViewModel)
        {
            var user = await userManager.FindByIdAsync(EditUserViewModel.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {EditUserViewModel.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.Id = EditUserViewModel.Id;
                user.UserName = EditUserViewModel.UserName;
                user.Email = EditUserViewModel.Email;
                user.Department = EditUserViewModel.Departement;
                user.Position = EditUserViewModel.Position;

                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    TempData["msg"] = "User Updated";
                    TempData["result"] = "success";
                    return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            TempData["msg"] = "User Failed to Update";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string id)
        {
            ViewBag.id = id;
            var user = await userManager.FindByIdAsync(id);
            ViewBag.UserName = user.UserName;

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {id} cannot be found";
                return View("NotFound");
            }

            var model = new List<UserRoleViewModel>();

            foreach(var role in roleManager.Roles)
            {
                var UserRole = new UserRoleViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    UserRole.IsSelected = true;
                }
                else
                {
                    UserRole.IsSelected = false;
                }

                model.Add(UserRole);
            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<UserRoleViewModel> model,string id)
        {
            ViewBag.id = id;
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {id} cannot be found";
                return View("NotFound");
            }

            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot Remove user existing roles");
                return View(model);
            }

            IList<string> a = model.Where(x => x.IsSelected).Select(y => y.RoleName).ToList();

            for (int i = 0;i<  a.Count ; i++)
            {
                result = await userManager.AddToRoleAsync(user, a[i]);
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }

            TempData["msg"] = "User Role Updated";
            TempData["result"] = "success";
            return RedirectToAction("EditUser", new { id=id });
        }

        [HttpGet]
        public async Task<IActionResult> ManageRoleClaims(string id)
        {
            var roles = await roleManager.FindByIdAsync(id);
            ViewBag.RoleName = roles.Name;
            ViewBag.id = roles.Id;

            if (roles == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {id} cannot be found";
                return View("NotFound");
            }

            var existingRoleClaims = await roleManager.GetClaimsAsync(roles);

            var model = new ClaimViewModel
            {
                UserId = id
            };
            foreach (Claim claim in ClaimsStore.AllClaims)
            {
                ClaimList RoleClaim = new ClaimList
                {
                    ClaimType = claim.Type
                };

                if (existingRoleClaims.Any(c => c.Type == claim.Type))
                {
                    RoleClaim.IsSelected = true;
                }

                model.Claims.Add(RoleClaim);
            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> ManageRoleClaims(ClaimViewModel model, string id)
        {
            var roles = await roleManager.FindByIdAsync(id);
            ViewBag.RoleName = roles.Name;
            if (roles == null)
            {
                ViewBag.ErrorMessage = $"User with id = {id} cannot be found";
                return View("NotFound");
            }

            var claims = await roleManager.GetClaimsAsync(roles);

            for (int i = 0; i < claims.Count; i++)
            {
                var result = await roleManager.RemoveClaimAsync(roles, claims[i]);
            }

            List<string> add_new = model.Claims.Where(c => c.IsSelected).Select(c => c.ClaimType).ToList();

            for (int i = 0; i < add_new.Count; i++)
            {
                var result = await roleManager.AddClaimAsync(roles, new Claim(add_new[i], add_new[i]));
            }

            TempData["msg"] = "Role Claim Updated";
            TempData["result"] = "success";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }


        [HttpGet]
        public async Task<IActionResult> ManageUserClaims (string id)
        {
            var user = await userManager.FindByIdAsync(id);
            ViewBag.UserName = user.UserName;
            ViewBag.id = user.Id;
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {id} cannot be found";
                return View("NotFound");
            }

            var existingUserClaims = await userManager.GetClaimsAsync(user);

            var model = new ClaimViewModel
            {
                UserId = id
            };

            //if the user has the claim, set IsSelected property to true, so the checkbox
            //next to the claim is checked on the UI
            foreach (Claim claim in ClaimsStore.AllClaims)
            {
                ClaimList userClaim = new ClaimList
                {
                    ClaimType = claim.Type
                };

                if (existingUserClaims.Any(c=>c.Type == claim.Type))
                {
                    userClaim.IsSelected = true;
                }

                model.Claims.Add(userClaim);
            }
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> ManageUserClaims(ClaimViewModel model,string id)
        {
            var user = await userManager.FindByIdAsync(id);
            ViewBag.UserName = user.UserName;
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {id} cannot be found";
                return View("NotFound");
            }

            var claims = await userManager.GetClaimsAsync(user);

            for (int i = 0; i <claims.Count; i++)
            {
                var result = await userManager.RemoveClaimAsync(user,claims[i]);
            }

            List<string> add_new = model.Claims.Where(c => c.IsSelected).Select(c => c.ClaimType).ToList();

            for (int i = 0; i < add_new.Count; i++)
            {
                var result = await userManager.AddClaimAsync(user, new Claim (add_new[i],add_new[i]));
            }

            TempData["msg"] = "User Role Updated";
            TempData["result"] = "success";
            return RedirectToAction("EditUser", new { id = id });

        }




    }
}
