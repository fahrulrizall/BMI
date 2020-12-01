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
                .Include(k => k.POModel)
                .ToList();
            ViewBag.status = status;
            TempData["status"] = status;
            return View(obj);
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(AdjustmentFGModel adjustmentFGModel)
        {
            var getPO = _db.PO.Where(a => a.pt == Convert.ToInt32(adjustmentFGModel.pt)).Select(a => a.po).ToList();
            var po = Convert.ToString(getPO[0]);
            if (adjustmentFGModel.id_adjustmentFG == 0)
            {
                var create = new AdjustmentFGModel
                {
                    bmi_code = adjustmentFGModel.bmi_code,
                    qty = adjustmentFGModel.qty,
                    po = po,
                    raw_source = adjustmentFGModel.raw_source,
                    reason = adjustmentFGModel.reason,
                    status = adjustmentFGModel.status
                };

                _db.AdjustmentFG.Add(create);
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Added";
                TempData["result"] = "success";
                return Redirect(Request.Headers["Referer"].ToString());
            }
            TempData["msg"] = "Failed Add Item";
            TempData["result"] = "failed";
            return Redirect(Request.Headers["Referer"].ToString());
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
            var obj = _db.AdjustmentFG.Include(a=>a.POModel).Where(a=>a.id_adjustmentFG == id).First();
            return Json(obj);
        }

        public IActionResult Getitemraw(int id)
        {
            var obj = _db.AdjustmentRaw.Find(id);
            return Json(obj);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult UpdateRaw(AdjustmentRawModel adjustmentRawModel)
        {
            if (ModelState.IsValid)
            {
                _db.AdjustmentRaw.Update(adjustmentRawModel);
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Updated";
                TempData["result"] = "success";
                return Redirect(Request.Headers["Referer"].ToString());
            }
            TempData["msg"] = "Item Failed to Update";
            TempData["result"] = "failed";
            return Redirect(Request.Headers["Referer"].ToString());

        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(AdjustmentFGModel destroyModel)
        {
            var getPO = _db.PO.Where(a => a.pt == Convert.ToInt32(destroyModel.pt)).Select(a => a.po).ToList();
            var po = Convert.ToString(getPO[0]);
            if (ModelState.IsValid)
            {
                var create = new AdjustmentFGModel
                {
                    id_adjustmentFG = destroyModel.id_adjustmentFG,
                    bmi_code = destroyModel.bmi_code,
                    qty = destroyModel.qty,
                    po = po,
                    raw_source = destroyModel.raw_source,
                    reason = destroyModel.reason,
                    status = destroyModel.status
                };
                _db.AdjustmentFG.Update(create);
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Updated";
                TempData["result"] = "success";
                return Redirect(Request.Headers["Referer"].ToString());
            }
            TempData["msg"] = "Item Failed Update";
            TempData["result"] = "failed";
            return Redirect(Request.Headers["Referer"].ToString());
        }


        public IActionResult AdjustmentRaw(string status)
        {
            var obj = _db.AdjustmentRaw.Include(a => a.Masterdatamodel).Where(a => a.status == status).ToList();
            ViewBag.status = status;
            return View(obj);
        }

        public IActionResult AddDestroyRaw(AdjustmentRawModel adjustmentRawModel)
        {
            if (adjustmentRawModel.id_adjustmentRaw == 0)
            {
                _db.AdjustmentRaw.Add(adjustmentRawModel);
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Added";
                TempData["result"] = "success";
                return Redirect(Request.Headers["Referer"].ToString());
            }
            TempData["msg"] = "Item Failed to Add";
            TempData["result"] = "failed";
            return Redirect(Request.Headers["Referer"].ToString());
        }


        [AutoValidateAntiforgeryToken]
        public IActionResult Deleteraw(int id)
        {
            if (id != 0)
            {
                var obj = _db.AdjustmentRaw.Find(id);
                _db.AdjustmentRaw.Remove(obj);
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Deleted";
                TempData["result"] = "success";
                return Redirect(Request.Headers["Referer"].ToString());
            }
            TempData["msg"] = "Item Failed to Delete";
            TempData["result"] = "failed";
            return Redirect(Request.Headers["Referer"].ToString());
        }

    }
}
