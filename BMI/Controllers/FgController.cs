using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMI.Data;
using BMI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace BMI.Controllers
{
 
    public class FgController : Controller
    {
        private readonly ApplicationDbContext _db;

        public FgController(ApplicationDbContext db)
        {
            _db = db;
        }

  
        public IActionResult Index(int? plant)
        {
            if (plant== 3770 || plant == 3710)
            {
                ViewBag.plant = plant;
                var list = _db.Fg
                    .Where(d => d.plant == plant)
                    .Include(c=>c.Masterdatamodel)
                    .OrderByDescending(a=>a.id_fg)
                    .ToList();
                return View(list);
            }
            return NotFound();
        }

       
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize(Roles = "CC,Admin")]
        public IActionResult Create(Fgmodel obj)
        {
            if (obj.id_fg == 0)
            {
                _db.Fg.Add(obj);
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Added";
                TempData["result"] = "success";
                return RedirectToAction("Index", new { plant = obj.plant });
            }
            else
            {
                TempData["msg"] = "Item Failed Added";
                TempData["result"] = "failed";
                return RedirectToAction("Index", new { plant = obj.plant });
            }
            
        }

        public JsonResult Getdata(int id)
        {
            var obj = _db.Fg.Find(id);
            return Json(obj);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize(Roles = "CC,Admin")]
        public IActionResult Delete(int id,int plant)
        {
            var obj = _db.Fg.Find(id);
            _db.Fg.Remove(obj);
            _db.SaveChanges();
            TempData["msg"] = "Item Succesfully Deleted";
            TempData["result"] = "success";
            return RedirectToAction("Index",new { plant =plant });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize(Roles = "CC,Admin")]
        public IActionResult Update(Fgmodel obj)
        {
            if (ModelState.IsValid)
            {
                _db.Fg.Update(obj);
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Updated";
                TempData["result"] = "success";
                return RedirectToAction("Index", new { plant = obj.plant });
            }
            TempData["msg"] = "Item Failed Update";
            TempData["result"] = "failed";
            return RedirectToAction("Index", new { plant = obj.plant });
        }
    }
}
