using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BMI.Data;
using BMI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BMI.Controllers
{
    [Authorize(Roles = "Planning,Admin")] 
    public class POController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ApplicationDbContext _db;

        public POController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext db)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _db = db;
        }

        public async Task<IActionResult> Index(string plant, string status)
        {
            var obj = _db.PO.Where(a => a.plant == plant).Where(a=>a.plant == plant && a.po_status == status).OrderByDescending(a=>a.po).ToList();
            ViewBag.plant = plant;
            if (plant == "3700")
            {
                ViewBag.factory = "BMI";
            }else 
            if (plant == "3710" ) 
            {
                ViewBag.factory = "MOWI";
            }else
            {
                ViewBag.factory = "GFF";
            }
            ViewBag.status = status;
            return await Task.Run(()=> View(obj));
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize(Policy ="Create")]
        public async Task<IActionResult> Create(POModel POModel, string plant)
        {
            if (ModelState.IsValid)
            {
                var po = new POModel()
                {
                    po = POModel.po,
                    pt = POModel.pt,
                    plant = plant,
                    pt_status = "Process",
                    po_status = "Open",
                    batch =  (POModel.po).Substring((POModel.po).Length-5) + "SIDID",
                    created_by = User.Identity.Name,
                    created_at = DateTime.Now
                };
                _db.PO.Add(po);
                _db.SaveChanges();
                TempData["msg"] = "Data Successfuly Added";
                TempData["result"] = "success";
                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
            }
            TempData["msg"] = "Data Failed to Create";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }

        public async Task<JsonResult> GetPO(string po)
        {
            var obj = _db.PO.Find(po);
            return await Task.Run(() => Json(obj));
        }

        [Authorize(Policy = "Update")]
        public async Task<IActionResult> Update(POModel POModel)
        {
            if (ModelState.IsValid)
            {
                var po = _db.PO.Find(POModel.po);

                po.pt = POModel.pt;
                _db.Entry(po).State = EntityState.Modified;
                _db.SaveChanges();
                TempData["msg"] = "PO Successfuly Updated";
                TempData["result"] = "success";
                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
            }
            TempData["msg"] = "PO Failed Update";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }

        [Authorize(Policy = "Delete")]
        public async Task<IActionResult> Delete (string po)
        {
            var obj = _db.PO.Find(po);
            if (obj != null)
            {
                _db.PO.Remove(obj);
                _db.SaveChanges();
                TempData["msg"] = "PO Successfuly Deleted";
                TempData["result"] = "success";
                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
            }
            TempData["msg"] = "PO Failed to Delete";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }

    }
}
