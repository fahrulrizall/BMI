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

        public IActionResult DetailFG(string material)
        {
            
                var fg = _db.Production_output
                  .Include(k => k.MasterBMIModel)
                  .Include(k => k.PTModel)
                  .AsEnumerable()
                  .GroupBy(k => new { k.id_pt, k.bmi_code, k.PTModel.batch })
                  .Select(k => new ProductionView
                  {
                      bmi_code = k.Key.bmi_code,
                      MasterBMIModel = k.Max(m => m.MasterBMIModel),
                      PTModel = k.Max(m => m.PTModel),
                      total = k.Sum(k => k.qty * 2.204 / k.MasterBMIModel.lbs) - _db.Shipment_detail.Where(c => c.bmi_code == k.Key.bmi_code && c.batch == k.Key.batch).Sum(a => a.qty) - _db.DestroyFG.Where(c => c.bmi_code == k.Key.bmi_code && c.PTModel.batch == k.Key.batch).Sum(a => a.qty)
                  })
                  .Where(k => k.total >= 0.9)
                  .ToList();
                return View(fg);
        }

        public IActionResult DetailRaw(string material)
        {

            var raw = _db.Production_input
                 .Include(k => k.MasterBMIModel)
                 .Include(k => k.PTModel)
                 .AsEnumerable()
                 .GroupBy(k => new { k.raw_source, k.bmi_code, k.MasterBMIModel })
                 .Select(k => new ProductionView
                 {
                     raw_source = k.Key.raw_source,
                     MasterBMIModel = k.Max(m => m.MasterBMIModel),
                     total = Convert.ToDouble(k.Sum(k => k.qty) - _db.Rm.Where(c => c.raw_source == k.Key.raw_source && c.Masterdatamodel.sap_code == k.Key.MasterBMIModel.sap_code).Sum(a => a.qty_received))
                 })
                 .Where(k => k.total > 0)
                 .ToList();
            return View(raw);
        }



    }
}
