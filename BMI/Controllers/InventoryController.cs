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
                .Include(k => k.POModel)
                .Where(k => k.POModel.pt_status == "Open")
                .AsEnumerable()
                .GroupBy(k => new { k.po, k.bmi_code, k.POModel.batch })
                .Select(k => new ProductionView
                {
                    bmi_code = k.Key.bmi_code,
                    MasterBMIModel = k.Max(m => m.MasterBMIModel),
                    POModel = k.Max(m => m.POModel),
                    total = k.Sum(k => k.qty * 2.204 / k.MasterBMIModel.lbs) -
                   // _db.Shipment_detail.Include(a=>a.ShipmentModel).Where(c => c.ShipmentModel.bmi_code == k.Key.bmi_code && c.ShipmentModel.batch == k.Key.batch).Sum(a => a.qty) -
                    _db.AdjustmentFG.Where(c => c.bmi_code == k.Key.bmi_code && c.po == k.Key.po).Sum(a => a.qty)
                })
                .Where(k => k.total >= 0.9)
                .ToList();
            return View(fg);
        }

        public IActionResult DetailRaw(string material)
        {
            var raw = _db.Rm_detail
                 .Include(k => k.Masterdatamodel)
                 .Include(k => k.RmModel)
                 .Where(k => k.RmModel.status == "in_plant")
                 .AsEnumerable()
                 .GroupBy(k => new { k.raw_source, k.sap_code })
                 .Select(k => new ProductionView
                 {
                     raw_source = k.Key.raw_source,
                     sap_code = k.Key.sap_code,
                     Masterdatamodel = k.Max(m => m.Masterdatamodel),
                     total = Convert.ToDouble(k.Sum(k => k.qty_received) -
                     _db.Production_input.Include(a => a.Masterdatamodel).Where(c => c.raw_source == k.Key.raw_source && c.sap_code == k.Key.sap_code).Sum(a => a.qty) -
                     _db.AdjustmentRaw.Where(c => c.raw_source == k.Key.raw_source && c.sap_code == k.Key.sap_code).Sum(a => a.qty))
                 })
                 .Where(k => k.total > 0)
                 .ToList();
            return View(raw);
        }


    }
}
