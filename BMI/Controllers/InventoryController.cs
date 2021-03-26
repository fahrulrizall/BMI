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

        public async Task< IActionResult> DetailFG()
        {
            var fg = _db.Production_output
                .Where(k => k.POModel.pt_status == "Open" || k.POModel.pt_status == "Process")
                .Include(k => k.MasterBMIModel)
                .AsEnumerable()
                .GroupBy(k => new { k.po, k.bmi_code })
                .Select(k => new ProductionView
                {
                    po = k.Key.po,
                    bmi_code = k.Key.bmi_code,
                    MasterBMIModel = k.Max(a => a.MasterBMIModel),
                    total = Convert.ToInt32(k.Sum(k => k.qty * 2.204 / k.MasterBMIModel.lbs))
                    - _db.AdjustmentFG.Where(c => c.bmi_code == k.Key.bmi_code && c.po == k.Key.po && c.POModel.pt_status == "Open" || c.POModel.pt_status == "Process").Sum(a => a.qty)
                })
                .ToList();

            var result = fg
                .GroupBy(a => a.bmi_code)
                .Select(a => new ProductionView
                {
                    bmi_code = a.Key,
                    MasterBMIModel = a.Max(b => b.MasterBMIModel),
                    total = a.Sum(b => b.total)
                    - _db.Shipment.Where(c => c.POModelBatch.pt_status == "Open" || c.POModelBatch.pt_status == "Process").Where(c=>c.bmi_code == a.Key).Sum(a => a.qty)
                    - _db.Repack.Where(c => c.fromPOModel.pt_status == "Open" || c.fromPOModel.pt_status == "Process").Where(c => c.from_bmi_code == a.Key).Sum(a => a.qty * 2.204 / a.toMasterBMIModel.lbs)
                    + _db.Repack.Where(c => c.toPOModel.pt_status == "Open" || c.toPOModel.pt_status == "Process").Where(c => c.to_bmi_code == a.Key).Sum(a => a.qty * 2.204 / a.toMasterBMIModel.lbs)
                })
                .Where(a => a.total >= 1)
                .OrderBy(a => a.MasterBMIModel.sap_code)
                .ToList();

            return await Task.Run(()=> View(result));
        }



        public async Task<IActionResult> EachDetailFG(string bmi_code)
        {
            var fg = _db.Production_output
                .Include(a => a.MasterBMIModel)
                .Include(a => a.POModel)
                .Where(a => a.POModel.pt_status == "Open" || a.POModel.pt_status == "Process")
                .Where(a => a.bmi_code == bmi_code)
                .AsEnumerable()
                .GroupBy(a => a.po)
                .Select(a => new
                {
                    batch = a.Key,
                    POModel = a.Max(b => b.POModel),
                    total = Convert.ToInt32(a.Sum(b => b.qty * 2.204 / b.MasterBMIModel.lbs)
                    - _db.Shipment.Where(c => c.bmi_code == bmi_code && c.batch == a.Key).Sum(a => a.qty)
                    - _db.AdjustmentFG.Where(c => c.bmi_code == bmi_code && c.po == a.Key).Sum(a => a.qty)
                    - _db.Repack.Where(c => c.from_bmi_code == bmi_code && c.from_po == a.Key).Sum(a => a.qty * 2.204 / a.fromMasterBMIModel.lbs)
                    + _db.Repack.Where(c => c.to_bmi_code == bmi_code && c.to_po == a.Key).Sum(a => a.qty * 2.204 / a.toMasterBMIModel.lbs)),
                })
                .Where(a => a.total >= 1)
                .OrderByDescending(a => a.POModel.batch)
                .ToList();
            return await Task.Run(() => Json(fg));

        }


        public async Task<IActionResult> DetailRaw()
        {
            var rm = _db.Rm_detail
                 .Where(k => k.RmModel.status == "Plant")
                 .AsEnumerable()
                 .GroupBy(k => k.raw_source)
                 .Select(k => new ProductionView
                 {
                     raw_source = k.Key,
                     Masterdatamodel = k.Max(m => m.Masterdatamodel),
                     total = Convert.ToDouble(k.Sum(a=>a.qty_received) - ( _db.Production_input.Where(a=>a.raw_source== k.Key).Sum(a=>a.qty) + _db.AdjustmentRaw.Where(a => a.raw_source == k.Key).Sum(a => a.qty)))
                 })
                 .ToList();
            return await Task.Run(()=> View(rm));
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
                     Masterdatamodel = k.Max(a=>a.Masterdatamodel),
                     total = Convert.ToDouble(k.Sum(k => k.qty_received) 
                     - _db.Production_input.Where(c => c.raw_source == raw_source && c.sap_code == k.Key.sap_code).Sum(a => a.qty) 
                     - _db.AdjustmentRaw.Where(c => c.raw_source == raw_source && c.sap_code == k.Key.sap_code).Sum(a => a.qty))
                 })
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
                    cases = Convert.ToInt32(k.Sum(k => k.qty * 2.204 / k.MasterBMIModel.lbs)
                    -  _db.Shipment.Where(c => c.pdc==k.Key.date && c.bmi_code == k.Key.bmi_code && c.batch == k.Key.po).Sum(a => a.qty)
                    -  _db.Repack.Where(c => c.production_date== k.Key.date && c.from_bmi_code == k.Key.bmi_code && c.from_po == k.Key.po).Sum(a => a.qty * 2.204 / a.fromMasterBMIModel.lbs))
                })
                .Where(k => k.cases >= 1)
                .OrderByDescending(a => a.POModel.pt)
                .ToList();

                var repack_with_date = _db.Repack
                .Where(a => a.toPOModel.pt_status == "Open" || a.toPOModel.pt_status == "Process")
                .Include(a => a.toMasterBMIModel)
                .AsEnumerable()
                .GroupBy(a => new { a.production_date, a.raw_source, a.to_bmi_code, a.toPOModel.batch, a.to_po })
                .Select(a => new ProductionView
                {
                    date = a.Key.production_date,
                    raw_source = a.Key.raw_source,
                    bmi_code = a.Key.to_bmi_code,
                    batch = a.Key.batch,
                    MasterBMIModel = a.Max(k => k.toMasterBMIModel),
                    POModel = a.Max(k => k.toPOModel),
                    cases = Convert.ToInt32(a.Sum(a => a.qty * 2.204 / a.toMasterBMIModel.lbs) - _db.Shipment.Where(b => b.pdc == a.Key.production_date && b.bmi_code == a.Key.to_bmi_code && b.raw_source == a.Key.raw_source && b.batch == a.Key.to_po).Sum(b => b.qty) )
                })
                .Where(k => k.cases >= 1)
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
                        worksheet.Cell(currentRow, 5).Value = data.cases;
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
                    cases = Convert.ToInt32 ( k.Sum(k => k.qty * 2.204 / k.MasterBMIModel.lbs)
                    - _db.Shipment.Where(c => c.bmi_code == k.Key.bmi_code && c.batch == k.Key.po).Sum(a => a.qty)
                    - _db.AdjustmentFG.Where(c => c.bmi_code == k.Key.bmi_code && c.po == k.Key.po).Sum(a => a.qty)
                    - _db.Repack.Where(c => c.from_bmi_code == k.Key.bmi_code && c.from_po == k.Key.po).Sum(a => a.qty * 2.204 / a.fromMasterBMIModel.lbs)
                    + _db.Repack.Where(c => c.to_bmi_code == k.Key.bmi_code && c.to_po == k.Key.po).Sum(a => a.qty * 2.204 / a.toMasterBMIModel.lbs)),
                    lbs = Math.Round(k.Sum(k => k.qty * 2.204)
                    - _db.Shipment.Where(c => c.bmi_code == k.Key.bmi_code && c.batch == k.Key.po).Sum(a => a.qty * a.MasterBMIModel.lbs)
                    - _db.AdjustmentFG.Where(c => c.bmi_code == k.Key.bmi_code && c.po == k.Key.po).Sum(a => a.qty * a.MasterBMIModel.lbs)
                    - _db.Repack.Where(c => c.from_bmi_code == k.Key.bmi_code && c.from_po == k.Key.po).Sum(a => a.qty * 2.204)
                    + _db.Repack.Where(c => c.to_bmi_code == k.Key.bmi_code && c.to_po == k.Key.po).Sum(a => a.qty * 2.204),2)
                })
                .Where(k => k.cases >= 1)
                .OrderByDescending(a => a.POModel.pt)
                .ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("FG Inventory");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "PT";
                worksheet.Cell(currentRow, 2).Value = "SAP Code";
                worksheet.Cell(currentRow, 3).Value = "Description";
                worksheet.Cell(currentRow, 4).Value = "Batch";
                worksheet.Cell(currentRow, 5).Value = "FG Cases";
                worksheet.Cell(currentRow, 6).Value = "FG Lbs";

                foreach (var data in fg)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = data.POModel.pt;
                    worksheet.Cell(currentRow, 2).Value = data.MasterBMIModel.sap_code;
                    worksheet.Cell(currentRow, 3).Value = data.MasterBMIModel.description;
                    worksheet.Cell(currentRow, 4).Value = data.POModel.batch;
                    worksheet.Cell(currentRow, 5).Value = data.cases;
                    worksheet.Cell(currentRow, 6).Value = data.lbs;
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

        public async Task<IActionResult> Adjustment(string raw)
        {
            var model = new RmChecking();
            model.RmDetailModel = _db.Rm_detail
                .Include(a => a.Masterdatamodel)
                .Where(a => a.raw_source == raw)
                .AsEnumerable()
                .GroupBy(a => new { a.landing_site_received, a.sap_code })
                .Select(a => new RmDetailModel
                {
                    landing_site = a.Key.landing_site_received,
                    Masterdatamodel = a.Max(b => b.Masterdatamodel),
                    sap_code = a.Key.sap_code,
                    qty = a.Sum(b => b.qty_received)
                })
                .OrderBy(a => a.landing_site).ThenBy(a => a.sap_code)
                .ToList();

            model.ProductionInputModel = _db.Production_input
                .Include(a => a.Masterdatamodel)
                .Where(a => a.raw_source == raw)
                .AsEnumerable()
                .GroupBy(a => new { a.landing_site, a.sap_code })
                .Select(a => new ProductionInputModel
                {
                    landing_site = a.Key.landing_site,
                    Masterdatamodel = a.Max(b => b.Masterdatamodel),
                    sap_code = a.Key.sap_code,
                    qty_raw = Math.Round(( a.Sum(a => a.qty) + (float?) _db.AdjustmentRaw.Where(c => c.raw_source==raw && c.sap_code == a.Key.sap_code && c.landing_site == a.Key.landing_site).Sum(c => c.qty) ??0),2)
                })
                .OrderBy(a => a.landing_site).ThenBy(a => a.sap_code)
                .ToList();

            if (model.RmDetailModel.Count == model.ProductionInputModel.Count)
            {
                var checking = from i in model.ProductionInputModel
                             join r in model.RmDetailModel on new { i.landing_site, i.sap_code } equals new { r.landing_site, r.sap_code }
                             select new RmChecking
                             {
                                 landing_site_rm = r.landing_site,
                                 sap_code_rm = r.sap_code,
                                 masterdatamodel_rm = r.Masterdatamodel,
                                 qty_rm = r.qty,
                                 landing_site_prod = i.landing_site,
                                 sap_code_prod = i.sap_code,
                                 masterdatamodel_prod = i.Masterdatamodel,
                                 qty_prod = i.qty_raw,
                                 diffrence = Math.Round(Convert.ToDecimal(r.qty - i.qty_raw), 2)
                             };
                List<RmChecking> result = checking.ToList();
                ViewBag.raw_source = raw;
                return await Task.Run(() => View(result));
            }
            ViewBag.raw_source = raw;
            return await Task.Run(() => View("Checking",model));
        }

        [HttpPost]
        public async Task<IActionResult> FixAdjustment(List<RmChecking> rmCheckings)
        {
            List<AdjustmentRawModel> model = new List<AdjustmentRawModel>();
            for (int i = 0; i < rmCheckings.Count; i++){
                if (rmCheckings[i].diffrence != 0 )
                {
                    model.Add(new AdjustmentRawModel
                    {
                        raw_source = rmCheckings[i].raw_source,
                        landing_site = rmCheckings[i].landing_site_prod,
                        sap_code = rmCheckings[i].sap_code_prod,
                        qty = Convert.ToDouble(rmCheckings[i].diffrence),
                        status = "Adjustment",
                        created_at = DateTime.Now,
                        created_by = User.Identity.Name
                    });
                }
               
            }
            _db.AdjustmentRaw.AddRange(model);
            _db.SaveChanges();
            TempData["msg"] = "Adjustment Succesfully";
            TempData["result"] = "success";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }


    }
}
