using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BMI.Models;
using BMI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;


namespace BMI.Controllers
{
    public class InventoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public InventoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(string material)
        {
            if (material == "fg")
            {
                var obj = _db.Production_output
                  .Include(k => k.MasterBMIModel)
                  .Include(k => k.PTModel)
                  .AsEnumerable()
                  .GroupBy(k => new { k.id_pt, k.bmi_code, k.PTModel.batch })
                  .Select(k => new ProductionView
                  {
                      code = k.Key.bmi_code,
                      MasterBMIModel = k.Max(m => m.MasterBMIModel),
                      PTModel = k.Max(m => m.PTModel),
                      total = k.Sum(k => k.qty * 2.204 / k.MasterBMIModel.lbs) - _db.Shipment_detail.Where(c => c.bmi_code == k.Key.bmi_code && c.batch == k.Key.batch).Sum(a => a.qty)
                  })
                  .Where(k => k.total >= 0.9)
                  .ToList();
                return View(obj);
            }
            
            return View();
        }

        public IActionResult GetInventoryFG()
        {
            var obj = _db.Production_output
                  .Include(k => k.MasterBMIModel)
                  .Include(k => k.PTModel)
                  .AsEnumerable()
                  .GroupBy(k => new { k.id_pt, k.bmi_code, k.PTModel.batch })
                  .Select(k => new ProductionView
                  {
                      code = k.Key.bmi_code,
                      MasterBMIModel = k.Max(m => m.MasterBMIModel),
                      PTModel = k.Max(m => m.PTModel),
                      total = k.Sum(k => k.qty * 2.204 / k.MasterBMIModel.lbs) - _db.Shipment_detail.Where(c => c.bmi_code == k.Key.bmi_code && c.batch == k.Key.batch).Sum(a => a.qty)
                  })
                  .Where(k => k.total >= 0.9)
                  .ToList();
            return Json(obj);
        }

    }
}
