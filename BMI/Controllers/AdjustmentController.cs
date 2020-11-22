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
    public class AdjustmentController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AdjustmentController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Adjustment(string status)
        {
            var obj = _db.AdjustmentFG
                .Where(k => k.status == status)
                .Include(k => k.MasterBMIModel)
                .Include(k => k.PTModel)
                .ToList();
            ViewBag.status = status;
            TempData["status"] = status;
            return View(obj);
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(AdjustmentFGModel adjustmentFGModel)
        {
            if (adjustmentFGModel.id_adjustmentFG == 0)
            {
                var create = new AdjustmentFGModel
                {
                    bmi_code = adjustmentFGModel.bmi_code,
                    qty = adjustmentFGModel.qty,
                    id_pt = adjustmentFGModel.id_pt + "3700",
                    raw_source = adjustmentFGModel.raw_source,
                    reason = adjustmentFGModel.reason,
                    status = adjustmentFGModel.status
                };

                _db.AdjustmentFG.Add(create);
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Added";
                TempData["result"] = "success";
                return RedirectToAction("Adjustment", new { status = create.status });
            }
            TempData["msg"] = "Failed Add Item";
            TempData["result"] = "failed";
            return RedirectToAction("Adjustment", new { status = adjustmentFGModel.status });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Delete(int id, string status)
        {
            var obj = _db.AdjustmentFG.Find(id);
            _db.AdjustmentFG.Remove(obj);
            _db.SaveChanges();
            TempData["msg"] = "Item Succesfully Deleted";
            TempData["result"] = "success";
            return RedirectToAction("Adjustment", new { status = status });
        }


        public IActionResult Getdataitem(int id)
        {
            var obj = _db.AdjustmentFG.Find(id);
            return Json(obj);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(AdjustmentFGModel destroyModel)
        {
            if (ModelState.IsValid)
            {
                var create = new AdjustmentFGModel
                {
                    id_adjustmentFG = destroyModel.id_adjustmentFG,
                    bmi_code = destroyModel.bmi_code,
                    qty = destroyModel.qty,
                    id_pt = destroyModel.id_pt + "3700",
                    raw_source = destroyModel.raw_source,
                    reason = destroyModel.reason,
                    status = destroyModel.status
                };
                _db.AdjustmentFG.Update(create);
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Updated";
                TempData["result"] = "success";
                return RedirectToAction("Adjustment", new { status = destroyModel.status });
            }
            TempData["msg"] = "Item Failed Update";
            TempData["result"] = "failed";
            return RedirectToAction("Adjustment", new { status = destroyModel.status });
        }


        public IActionResult AdjustmentRaw()
        {
            return View();
        }

    }
}
