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
using System.Text;

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

        public async Task< IActionResult> DetailFG(string material)
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
                    total = k.Sum(k => k.qty * 2.204 / k.MasterBMIModel.lbs)
                    - _db.Shipment.Where(c => c.bmi_code == k.Key.bmi_code && c.batch == k.Key.po).Sum(a => a.qty)
                    - _db.AdjustmentFG.Where(c => c.bmi_code == k.Key.bmi_code && c.po == k.Key.po).Sum(a => a.qty)
                    - _db.Repack.Where(c => c.from_bmi_code == k.Key.bmi_code && c.from_po == k.Key.po).Sum(a => a.qty)
                    + _db.Repack.Where(c => c.to_bmi_code == k.Key.bmi_code && c.to_po == k.Key.po).Sum(a => a.qty * 2.204 / a.toMasterBMIModel.lbs)
                })
                .Where(k => k.total >= 0.9)
                .OrderByDescending(a => a.POModel.pt)
                .ToList();
            return await Task.Run(()=> View(fg));
        }

        public async Task<IActionResult> DetailRaw(string material)
        {
            var raw = _db.Rm_detail
                 .Where(k => k.RmModel.status == "Plant")
                 .AsEnumerable()
                 .GroupBy(k => k.raw_source)
                 .Select(k => new ProductionView
                 {
                     raw_source = k.Key,
                     Masterdatamodel = k.Max(m => m.Masterdatamodel),
                     total = Convert.ToDouble(k.Sum(k => k.qty_received) -
                     _db.Production_input.Include(a => a.Masterdatamodel).Where(c => c.raw_source == k.Key).Sum(a => a.qty) -
                     _db.AdjustmentRaw.Where(c => c.raw_source == k.Key).Sum(a => a.qty))
                 })
                 .Where(k => k.total > 0)
                 .ToList();
            return await Task.Run(()=> View(raw));
        }

        public async Task<IActionResult> EachDetailRaw (string raw_source)
        {
            var raw = _db.Rm_detail
                 .Include(k => k.Masterdatamodel)
                 .Include(k => k.RmModel)
                 .Where(k => k.RmModel.status == "Plant" && k.raw_source == raw_source)
                 .AsEnumerable()
                 .GroupBy(k => new {k.sap_code,k.landing_site_received })
                 .Select(k => new ProductionView
                 {
                     sap_code = k.Key.sap_code,
                     landing_site = k.Key.landing_site_received,
                     Masterdatamodel = k.Max(m => m.Masterdatamodel),
                     total = Convert.ToDouble(k.Sum(k => k.qty_received) -
                     _db.Production_input.Include(a => a.Masterdatamodel).Where(c => c.raw_source == raw_source && c.sap_code == k.Key.sap_code).Sum(a => a.qty) -
                     _db.AdjustmentRaw.Where(c => c.raw_source == raw_source && c.sap_code == k.Key.sap_code).Sum(a => a.qty))
                 })
                 .Where(k => k.total > 0)
                 .ToList();
            return await Task.Run(() => Json(raw));
        }

        public async Task<IActionResult> DownloadFG()
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
                    total = Convert.ToInt32 ( k.Sum(k => k.qty * 2.204 / k.MasterBMIModel.lbs)
                    - _db.Shipment.Where(c => c.bmi_code == k.Key.bmi_code && c.batch == k.Key.po).Sum(a => a.qty)
                    - _db.AdjustmentFG.Where(c => c.bmi_code == k.Key.bmi_code && c.po == k.Key.po).Sum(a => a.qty)
                    - _db.Repack.Where(c => c.from_bmi_code == k.Key.bmi_code && c.from_po == k.Key.po).Sum(a => a.qty)
                    + _db.Repack.Where(c => c.to_bmi_code == k.Key.bmi_code && c.to_po == k.Key.po).Sum(a => a.qty * 2.204 / a.toMasterBMIModel.lbs))
                })
                .Where(k => k.total >= 0.9)
                .OrderByDescending(a => a.POModel.pt)
                .ToList();

            var builder = new StringBuilder();
            builder.AppendLine("PT,SAP Code,Description,Batch,FG Qty");
            foreach (var data in fg)
            {
                builder.AppendLine($"{data.POModel.pt},{data.MasterBMIModel.sap_code},{data.MasterBMIModel.description},{data.POModel.batch},{data.total}");
            }
            return await Task.Run(() => File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "InventoryFG.csv"));
        }





    }
}
