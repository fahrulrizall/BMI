using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BMI.Data;
using BMI.Models;
using Microsoft.EntityFrameworkCore;

namespace BMI.Controllers
{
    public class MasterBMIController : Controller
    {
        private readonly ApplicationDbContext _db;

        public MasterBMIController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult>Index()
        {
            var obj = _db.Master_BMI.OrderBy(a=>a.created_at).ToList();
            return await Task.Run(()=> View(obj));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MasterBMIModel masterBMIModel)
        {
            if (ModelState.IsValid)
            {
                var obj = new MasterBMIModel
                {
                    bmi_code = masterBMIModel.bmi_code,
                    sap_code = masterBMIModel.sap_code,
                    description = masterBMIModel.description,
                    lbs = masterBMIModel.lbs,
                    daily_category = masterBMIModel.daily_category,
                    created_at = DateTime.Now
                };
                _db.Master_BMI.Add(obj);
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Added";
                TempData["result"] = "success";
                return await Task.Run(() => RedirectToAction("Index"));
            }
            TempData["msg"] = "Item Failed to Add";
            TempData["result"] = "failed";
            return await Task.Run(() => RedirectToAction("Index"));

        }

        public JsonResult Getmasterbmi (string id)
        {
            var obj = _db.Master_BMI.Find(id);
            return Json(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(MasterBMIModel masterBMIModel)
        {
            if (ModelState.IsValid)
            {
                var obj = _db.Master_BMI.Find(masterBMIModel.bmi_code);

                //obj.bmi_code = masterBMIModel.bmi_code;
                obj.sap_code = masterBMIModel.sap_code;
                obj.description = masterBMIModel.description;
                obj.lbs = masterBMIModel.lbs;
                obj.daily_category = masterBMIModel.daily_category;
                obj.updated_at = DateTime.Now;

                _db.Entry(obj).State = EntityState.Modified;
                _db.SaveChanges();

                TempData["msg"] = "Item Succesfully Updated";
                TempData["result"] = "success";
                return await Task.Run(() => RedirectToAction("Index"));
            }
            TempData["msg"] = "Item Failed Added";
            TempData["result"] = "failed";
            return await Task.Run(() => RedirectToAction("Index"));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var obj = _db.Master_BMI.Find(id);
            _db.Master_BMI.Remove(obj);
            _db.SaveChanges();
            TempData["msg"] = "Item Succesfully Deleted";
            TempData["result"] = "success";
            return await Task.Run(() => RedirectToAction("Index"));
        }

    }
}
