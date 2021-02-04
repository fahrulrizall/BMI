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
using ClosedXML.Excel;
using System.IO;

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

        public IActionResult DownloadFG(bool Selected)
        {
            if (Selected == true)
            {
                var fg_with_date = _db.Production_output
                .Include(k => k.MasterBMIModel)
                .Include(k => k.POModel)
                .Where(k => k.POModel.pt_status == "Open" || k.POModel.pt_status == "Process")
                .AsEnumerable()
                .GroupBy(k => new { k.date,k.raw_source,k.po, k.bmi_code, k.POModel.batch })
                .Select(k => new ProductionView
                {
                    date = k.Key.date,
                    raw_source = k.Key.raw_source,
                    bmi_code= k.Key.bmi_code,
                    batch = k.Key.batch,
                    MasterBMIModel = k.Max(m => m.MasterBMIModel),
                    POModel = k.Max(m => m.POModel),
                    total = Convert.ToInt32(k.Sum(k => k.qty * 2.204 / k.MasterBMIModel.lbs)
                    - _db.Shipment.Where(c => c.pdc==k.Key.date && c.bmi_code == k.Key.bmi_code && c.batch == k.Key.po).Sum(a => a.qty)
                    - _db.Repack.Where(c => c.production_date== k.Key.date && c.from_bmi_code == k.Key.bmi_code && c.from_po == k.Key.po).Sum(a => a.qty)
                    + _db.Repack.Where(c => c.production_date== k.Key.date && c.to_bmi_code == k.Key.bmi_code && c.to_po == k.Key.po).Sum(a => a.qty * 2.204 / a.toMasterBMIModel.lbs))
                })
                .Where(k => k.total >= 0.9)
                .OrderByDescending(a => a.POModel.pt)
                .ToList();

                var repack_with_date = _db.Repack
                .Where(a => a.toPOModel.pt_status == "Open" || a.toPOModel.pt_status == "Process")
                .Include(a => a.toMasterBMIModel)
                .AsEnumerable()
                .GroupBy(a => new { a.production_date, a.raw_source, a.to_bmi_code,a.toPOModel.batch })
                .Select(a => new ProductionView
                {
                    date = a.Key.production_date,
                    raw_source = a.Key.raw_source,
                    bmi_code = a.Key.to_bmi_code,
                    batch = a.Key.batch,
                    MasterBMIModel = a.Max(k=>k.toMasterBMIModel),
                    POModel = a.Max(k=>k.toPOModel),
                    total = Convert.ToInt32(a.Sum(a => a.qty * 2.204 / a.toMasterBMIModel.lbs))
                })
                .ToList();

                var union_with_date = new List<ProductionView>(fg_with_date);
                union_with_date.AddRange(repack_with_date);

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("FG Inventory");
                    var currentRow = 1;
                    worksheet.Cell(currentRow, 1).Value = "PT";
                    worksheet.Cell(currentRow, 2).Value = "SAP Code";
                    worksheet.Cell(currentRow, 3).Value = "Description";
                    worksheet.Cell(currentRow, 4).Value = "Batch";
                    worksheet.Cell(currentRow, 5).Value = "FG Qty";
                    worksheet.Cell(currentRow, 6).Value = "Date";
                    worksheet.Cell(currentRow, 7).Value = "Raw Source";

                    foreach (var data in union_with_date)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = data.POModel.pt;
                        worksheet.Cell(currentRow, 2).Value = data.MasterBMIModel.sap_code;
                        worksheet.Cell(currentRow, 3).Value = data.MasterBMIModel.description;
                        worksheet.Cell(currentRow, 4).Value = data.POModel.batch;
                        worksheet.Cell(currentRow, 5).Value = data.total;
                        worksheet.Cell(currentRow, 6).Value = data.date;
                        worksheet.Cell(currentRow, 7).Value = data.raw_source;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "FGInventory.xlsx");
                    }
                }
            }
            var fg = _db.Production_output
                .Include(k => k.MasterBMIModel)
                .Include(k => k.POModel)
                .Where(k => k.POModel.pt_status == "Open" || k.POModel.pt_status=="Process" )
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

            var repack = _db.Repack
                .Where(a => a.toPOModel.pt_status == "Open" || a.toPOModel.pt_status == "Process")
                .Include(a => a.toMasterBMIModel)
                .AsEnumerable()
                .GroupBy(a => new { a.to_bmi_code, a.toPOModel.batch })
                .Select(a => new ProductionView
                {
                    bmi_code = a.Key.to_bmi_code,
                    MasterBMIModel = a.Max(k => k.toMasterBMIModel),
                    POModel = a.Max(k => k.toPOModel),
                    total = Convert.ToInt32(a.Sum(a => a.qty * 2.204 / a.toMasterBMIModel.lbs))
                })
                .ToList();

            var union = new List<ProductionView>(fg);
            union.AddRange(repack);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("FG Invemtory");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "PT";
                worksheet.Cell(currentRow, 2).Value = "SAP Code";
                worksheet.Cell(currentRow, 3).Value = "Description";
                worksheet.Cell(currentRow, 4).Value = "Batch";
                worksheet.Cell(currentRow, 5).Value = "FG Qty";

                foreach (var data in union)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = data.POModel.pt;
                    worksheet.Cell(currentRow, 2).Value = data.MasterBMIModel.sap_code;
                    worksheet.Cell(currentRow, 3).Value = data.MasterBMIModel.description;
                    worksheet.Cell(currentRow, 4).Value = data.POModel.batch;
                    worksheet.Cell(currentRow, 5).Value = data.total;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "FGInventory.xlsx");
                }
            }
        }





    }
}
