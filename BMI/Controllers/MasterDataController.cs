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
    public class MasterDataController : Controller
    {
        private readonly ApplicationDbContext _db;

        public MasterDataController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var obj = _db.Master_data.OrderBy(a => a.created_at).ToList();
            return await Task.Run(() => View(obj));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Masterdatamodel masterdatamodel)
        {
            if (ModelState.IsValid)
            {
                var obj = new Masterdatamodel
                {
                    sap_code = masterdatamodel.sap_code,
                    description = masterdatamodel.description,
                    lbs = masterdatamodel.lbs,
                    category = masterdatamodel.category,
                    created_at = DateTime.Now
                };
                _db.Master_data.Add(obj);
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Added";
                TempData["result"] = "success";
                return await Task.Run(() => RedirectToAction("Index"));
            }
            TempData["msg"] = "Item Failed to Add";
            TempData["result"] = "failed";
            return await Task.Run(() => RedirectToAction("Index"));

        }

        public JsonResult Getmasterbmi(string id)
        {
            var obj = _db.Master_data.Find(id);
            return Json(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Masterdatamodel masterdatamodel)
        {
            if (ModelState.IsValid)
            {
                var obj = _db.Master_data.Find(masterdatamodel.sap_code);

                obj.description = masterdatamodel.description;
                obj.lbs = masterdatamodel.lbs;
                obj.category = masterdatamodel.category;
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
            var obj = _db.Master_data.Find(id);
            _db.Master_data.Remove(obj);
            _db.SaveChanges();
            TempData["msg"] = "Item Succesfully Deleted";
            TempData["result"] = "success";
            return await Task.Run(() => RedirectToAction("Index"));
        }

    }
}
