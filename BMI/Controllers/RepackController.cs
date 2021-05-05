using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using BMI.Data;
using BMI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace BMI.Controllers
{

    public class RepackController : Controller
    {
        private readonly ApplicationDbContext _db;

        public RepackController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var obj = _db.Repack
                .GroupBy(a => new { a.date, a.toPOModel.pt })
                .Select(a => new RepackModel
                {
                    date = a.Key.date,
                    pt = a.Key.pt
                }).Take(10).ToList();
            return await Task.Run(()=> View(obj)) ;
        }

        public JsonResult DateExist(DateTime date)
        {
            var unique = _db.Repack.FirstOrDefault(m => m.date.Year == date.Year &&
                                                                    m.date.Month == date.Month &&
                                                                    m.date.Day == date.Day);
            if (unique != null)
            {
                return Json(true);
            }
            return Json(false);
        }


        public async Task<IActionResult> Detail (DateTime date)
        {
            var obj = _db.Repack
                .Include(k => k.fromMasterBMIModel)
                .Include(k => k.toMasterBMIModel)
                .Include(k => k.fromPOModel)
                .Include(k => k.toPOModel)
                .AsEnumerable()
                .Where(m => m.date.Year == date.Year &&
                       m.date.Month == date.Month &&
                       m.date.Day == date.Day)
                .ToList();
            ViewBag.date = date.Day+"-"+date.Month+"-"+date.Year ;
            return await Task.Run(()=> View(obj));
        }

        //[HttpPost]
        //[AutoValidateAntiforgeryToken]
        [Authorize(Roles = "CC,Admin")]
        [Authorize(Policy  = "Delete")]
        public IActionResult Delete(int id)
        {
            var remove = _db.Repack.Find(id);
            _db.Remove(remove);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


        public JsonResult Getitemrepack(int id)
        {
            var obj = _db.Repack.Where(a=>a.id_repack==id)
                .Include(a => a.fromPOModel)
                .Include(a => a.toPOModel)
                .First();
            return Json(obj);
        }

        [Authorize(Roles = "CC,Admin")]
        [Authorize(Policy = "Update")]
        public IActionResult Update(RepackModel repackModel)
        {
            if (ModelState.IsValid)
            {
                var from_getPO = _db.PO.Where(a => a.pt == Convert.ToInt32(repackModel.from_po)).Select(a => a.po).ToList();
                var from_po = Convert.ToString(from_getPO[0]);
                var to_getPO = _db.PO.Where(a => a.pt == Convert.ToInt32(repackModel.to_po)).Select(a => a.po).ToList();
                var to_po = Convert.ToString(to_getPO[0]);
                var repack = new RepackModel
                {
                    id_repack = repackModel.id_repack,
                    po = repackModel.po,
                    date = repackModel.date,
                    qty = repackModel.qty,
                    from_po = from_po,
                    from_bmi_code = repackModel.from_bmi_code,
                    to_po = to_po,
                    to_bmi_code = repackModel.to_bmi_code
                };
                _db.Repack.Update(repack);
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Updated";
                TempData["result"] = "success";
                return Redirect(Request.Headers["Referer"].ToString());
            }
            TempData["msg"] = "Item Failed to Update";
            TempData["result"] = "failed";
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
