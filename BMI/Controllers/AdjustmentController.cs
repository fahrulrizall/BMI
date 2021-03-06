﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BMI.Data;
using BMI.Models;
using System.Data;
using Microsoft.AspNetCore.Authorization;

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

        [Authorize(Roles = "CC,Admin")]
        public async Task< IActionResult> AdjustmentFG(string status)
        {
            var obj = _db.AdjustmentFG
                .Where(k => k.status == status)
                .Include(k => k.MasterBMIModel)
                .Include(k => k.POModel)
                .AsEnumerable()
                .GroupBy(a=>a.POModel)
                .Select(a=> new POModel
                {
                    pt = a.Key.pt,
                    po = a.Key.po,
                    batch = a.Key.batch
                })
                .ToList();

            if (obj != null) {
                ViewBag.status = status;
                TempData["status"] = status;
                return await Task.Run(()=> View(obj));
            }
            return await Task.Run(() => View("NotFound"));
        }
        
        public async Task<IActionResult> DetailFG(string po)
        {
            var result = _db.AdjustmentFG
                .Include(a => a.POModel)
                .Include(a => a.MasterBMIModel)
                .Where(a => a.po == po)
                .ToList();
            ViewBag.po = po;

            return await Task.Run(() => View(result));
        }

        public async Task<JsonResult> Getdataitem(int id)
        {
            var obj = _db.AdjustmentFG.Include(a => a.POModel).Where(a => a.id_adjustmentFG == id).First();
            return await Task.Run(() => Json(obj));
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize(Roles = "CC,Admin")]
        [Authorize(Policy = "Create")]
        public async Task<IActionResult> Create(AdjustmentFGModel adjustmentFGModel)
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
                    status = adjustmentFGModel.status,
                    created_at = DateTime.Now,
                    created_by = User.Identity.Name
                };

                _db.AdjustmentFG.Add(create);
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Added";
                TempData["result"] = "success";
                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
            }
            TempData["msg"] = "Failed Add Item";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize(Roles = "CC,Admin")]
        [Authorize(Policy = "Update")]
        public IActionResult Update(AdjustmentFGModel destroyModel)
        {
            var getPO = _db.PO.Where(a => a.pt == Convert.ToInt32(destroyModel.pt)).Select(a => a.po).ToList();
            var po = Convert.ToString(getPO[0]);
            if (ModelState.IsValid)
            {
                var update = _db.AdjustmentFG.Find(destroyModel.id_adjustmentFG);

                update.id_adjustmentFG = destroyModel.id_adjustmentFG;
                update.bmi_code = destroyModel.bmi_code;
                update.qty = destroyModel.qty;
                update.po = po;
                update.raw_source = destroyModel.raw_source;
                update.reason = destroyModel.reason;
                update.updated_by = User.Identity.Name;
                update.updated_at = DateTime.Now;

                _db.Entry(update).State = EntityState.Modified;
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Updated";
                TempData["result"] = "success";
                return Redirect(Request.Headers["Referer"].ToString());
            }
            TempData["msg"] = "Item Failed Update";
            TempData["result"] = "failed";
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize(Roles = "CC,Admin")]
        [Authorize(Policy = "Delete")]
        public async Task<IActionResult> Delete(int id, string status)
        {
            var obj = _db.AdjustmentFG.Find(id);
            if (obj != null)
            {
                _db.AdjustmentFG.Remove(obj);
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Deleted";
                TempData["result"] = "success";
                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
            }
            TempData["msg"] = "Item Failed to Delete";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));

        }


        // adjusmnet raw line
        [Authorize(Roles = "CC,Admin")]
        [Authorize(Policy = "Create")]
        public async Task<IActionResult> AdjustmentRaw(string status)
        {
            var obj = _db.AdjustmentRaw
                .Where(k => k.status == status)
                .AsEnumerable()
                .GroupBy(k=>k.raw_source)
                .Select(a => new AdjustmentRawModel
                {
                    raw_source = a.Key
                })
                .ToList();

            if (obj != null)
            {
                ViewBag.status = status;
                TempData["status"] = status;
                return await Task.Run(() => View(obj));
            }
            return await Task.Run(() => View("NotFound"));
        }


        public async Task<IActionResult> DetailRaw(string raw)
        {
            var result = _db.AdjustmentRaw
                .Include(a => a.Masterdatamodel)
                .Where(a => a.raw_source == raw)
                .ToList();
            ViewBag.raw = raw;
            return await Task.Run(() => View(result));
        }


        public async Task<JsonResult> Getitemraw(int id)
        {
            var obj = _db.AdjustmentRaw.Find(id);
            return await Task.Run(() => Json(obj));
        }


        [Authorize(Roles = "CC,Admin")]
        [Authorize(Policy = "Delete")]
        public IActionResult AddDestroyRaw(AdjustmentRawModel adjustmentRawModel)
        {
            if (adjustmentRawModel.id_adjustmentRaw == 0)
            {
                var obj = new AdjustmentRawModel()
                {
                    raw_source = adjustmentRawModel.raw_source,
                    sap_code = adjustmentRawModel.sap_code,
                    qty = adjustmentRawModel.qty,
                    reason = adjustmentRawModel.reason,
                    status = adjustmentRawModel.status,
                    created_at = DateTime.Now,
                    created_by = User.Identity.Name
                };
                _db.AdjustmentRaw.Add(obj);
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Added";
                TempData["result"] = "success";
                return Redirect(Request.Headers["Referer"].ToString());
            }
            TempData["msg"] = "Item Failed to Add";
            TempData["result"] = "failed";
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize(Roles = "CC,Admin")]
        [Authorize(Policy = "Update")]
        public async Task<IActionResult> UpdateRaw(AdjustmentRawModel adjustmentRawModel)
        {
            if (ModelState.IsValid)
            {
                var update = _db.AdjustmentRaw.Find(adjustmentRawModel.id_adjustmentRaw);

                update.id_adjustmentRaw = adjustmentRawModel.id_adjustmentRaw;
                update.raw_source = adjustmentRawModel.raw_source;
                update.sap_code = adjustmentRawModel.sap_code;
                update.qty = adjustmentRawModel.qty;
                update.reason = adjustmentRawModel.reason;
                update.updated_at = DateTime.Now;
                update.updated_by = User.Identity.Name;

                _db.Entry(update).State = EntityState.Modified;
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Updated";
                TempData["result"] = "success";
                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
            }
            TempData["msg"] = "Item Failed to Update";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));

        }

        [AutoValidateAntiforgeryToken]
        [Authorize(Roles = "CC,Admin")]
        [Authorize(Policy = "Delete")]
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
