using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BMI.Data;
using BMI.Models;
using System.Data;

namespace BMI.Controllers
{
    public class DestroyController : Controller
    {
        private readonly ApplicationDbContext _db;

        public DestroyController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(string status)
        {
            var obj = _db.DestroyFG
                .Where(k=>k.status == status)
                .Include(k=>k.MasterBMIModel)
                .Include(k=>k.PTModel)
                .ToList();
            ViewBag.status = status;
            TempData["status"] = status;
            return View(obj);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(DestroyFGModel destroyModel)
        {
            if (destroyModel.id_destroyFG ==0)
            {
                var create = new DestroyFGModel
                {
                    bmi_code = destroyModel.bmi_code,
                    qty = destroyModel.qty,
                    id_pt = destroyModel.id_pt + "3700",
                    raw_source = destroyModel.raw_source,
                    reason = destroyModel.reason,
                    status = destroyModel.status
                };

                _db.DestroyFG.Add(create);
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Added";
                TempData["result"] = "success";
                return RedirectToAction("Index", new { status = create.status });
            }
            TempData["msg"] = "Failed Add Item Destroy";
            TempData["result"] = "failed";
            return RedirectToAction("Index", new { status = destroyModel.status });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Delete(int id,string status)
        {
            var obj = _db.DestroyFG.Find(id);
            _db.DestroyFG.Remove(obj);
            _db.SaveChanges();
            TempData["msg"] = "Item Succesfully Deleted";
            TempData["result"] = "success";
            return RedirectToAction("Index", new { status = status });
        }


        public IActionResult Getdataitem(int id)
        {
            var obj = _db.DestroyFG.Find(id);
            return Json(obj);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(DestroyFGModel destroyModel)
        {
            if (ModelState.IsValid)
            {
                var create = new DestroyFGModel
                {
                    id_destroyFG = destroyModel.id_destroyFG,
                    bmi_code = destroyModel.bmi_code,
                    qty = destroyModel.qty,
                    id_pt = destroyModel.id_pt + "3700",
                    raw_source = destroyModel.raw_source,
                    reason = destroyModel.reason,
                    status = destroyModel.status
                };
                _db.DestroyFG.Update(create);
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Updated";
                TempData["result"] = "success";
                return RedirectToAction("Index", new { status = destroyModel.status });
            }
            TempData["msg"] = "Item Failed Update";
            TempData["result"] = "failed";
            return RedirectToAction("Index", new { status = destroyModel.status });
        }
    }
}
