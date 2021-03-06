﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BMI.Models;
using BMI.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using ExcelDataReader;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System.Dynamic;
using BMI.UtilityModels;
using System.Text;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;

namespace BMI.Controllers
{
    public class ProductionController : Controller
    {
        private readonly ApplicationDbContext _db;
        private IWebHostEnvironment Environment;
        private IConfiguration Configuration;

        public ProductionController(ApplicationDbContext db, IWebHostEnvironment _environment, IConfiguration _configuration)
        {
            _db = db;
            Environment = _environment;
            Configuration = _configuration;
        }

        public async Task<IActionResult> Index(string plant, string status)
        {
            if (status == "Open" || status == "Process")
            {
                var result = _db.PO
                            .Where(a => a.plant == plant && a.pt != null && a.pt_status == status)
                            .Select(a => new POModel
                            {
                                pt = a.pt,
                                po = a.po,
                                batch = a.batch,
                                pt_status = a.pt_status,
                                yield = (_db.Production_output.Where(b => b.po == a.po).Sum(b => b.qty) / _db.Production_input.Where(b => b.po == a.po).Sum(b => b.qty)) * 100
                            })
                            .OrderByDescending(a => a.pt).ToList();
                ViewBag.status = status;
                return await Task.Run(() => View(result));
            }
            var obj = _db.PO
                .Where(a => a.plant == plant && a.pt != null && a.pt_status == status)
                .Select(a => new POModel
                {
                    pt = a.pt,
                    po = a.po,
                    batch = a.batch,
                    pt_status = a.pt_status,
                    yield = _db.Production_output.Where(a => a.po == a.po).Sum(a => a.qty) / _db.Production_input.Where(a => a.po == a.po).Sum(a => a.qty)
                })
                .OrderByDescending(a => a.pt).ToList();
            ViewBag.status = status;
            return await Task.Run(() => View(obj));

        }

        public async Task<JsonResult> Otw ()
        {
            var obj = _db.PO.Where(a => a.plant == "3700" && a.pt != null && a.pt_status == "Closed")
                .Select(a => new POModel
                {
                    pt = a.pt,
                    po = a.po,
                    batch = a.batch,
                    pt_status = a.pt_status
                })
                .OrderByDescending(a => a.pt).ToList();
            return await Task.Run(() => Json(obj));
        }


        public async Task<JsonResult> Getptdata(string po)
        {
            var obj = _db.PO.Find(po);
            return await Task.Run(()=> Json(obj));
        }

        [Authorize(Roles = "CC,Admin")]
        [Authorize(Policy = "Update")]
        public async Task<IActionResult> Updatept(POModel POModel)
        {
            var po = new POModel() { po = POModel.po, pt_status = POModel.pt_status };
            _db.PO.Attach(po);
            _db.Entry(po).Property(x => x.pt_status).IsModified = true; 
            _db.SaveChanges();
            TempData["msg"] = "File Succesfully Updated";
            TempData["result"] = "success";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize(Roles = "CC,Admin")]
        [Authorize(Policy = "Update")]
        public async Task<IActionResult> Import(IFormFile postedFile, DateTime date)
        {
            if (postedFile != null)
            {
                var allowedExtensions = new[] { ".xls", ".xlsx" };
                string fileName = Path.GetFileName(postedFile.FileName);
                var checkextension = Path.GetExtension(fileName).ToLower();

                if (allowedExtensions.Contains(checkextension))
                {
                    List<ProductionInputModel> prod_input = new List<ProductionInputModel>();
                    List<ProductionOutputModel> prod_output = new List<ProductionOutputModel>();
                    List<POModel> bmi_po = new List<POModel>();

                    
                    string path = Path.Combine(this.Environment.WebRootPath, "Uploads");

                    string filePath = Path.Combine(path, fileName);
                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                    }
                    // For .net core, the next line requires the NuGet package, 
                    //System.Text.Encoding.CodePages
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                    using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                    {
                        {
                            IExcelDataReader excelDataReader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream);
                            var conf = new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = a => new ExcelDataTableConfiguration
                                {
                                    UseHeaderRow = true
                                }
                            };

                            DataSet dataSet = excelDataReader.AsDataSet(conf);
                            List<object> rowDataList = null;

                            if (excelDataReader.FieldCount == 7)
                            {  
                                DataRowCollection row = dataSet.Tables["GI"].Rows;

                                foreach (DataRow item in row)
                                {
                                    rowDataList = item.ItemArray.ToList();
                                    var po = _db.PO.Where(a => a.pt == Convert.ToInt32(rowDataList[2])).Select(a => a.po).ToList();
                                    
                                    if (po.Count == 0)
                                    {
                                        TempData["msg"] = $"PO For PT {rowDataList[2]} Not Available";
                                        TempData["result"] = "warning";
                                        return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
                                    }

                                    prod_input.Add(new ProductionInputModel
                                    {
                                        po_bmi = Convert.ToString(rowDataList[0]),
                                        date = Convert.ToDateTime(rowDataList[1]),
                                        po = po.First(),
                                        raw_source = Convert.ToString(rowDataList[3]),
                                        sap_code = Convert.ToString(rowDataList[4]),
                                        qty = Convert.ToSingle(rowDataList[5]),
                                        landing_site = Convert.ToString(rowDataList[6]),
                                        created_at = DateTime.Now,
                                        created_by = User.Identity.Name,
                                        gi_date = date
                                    });
                                }
                                stream.Close();
                                System.IO.File.Delete(filePath);
                                _db.Production_input.AddRange(prod_input);
                                _db.SaveChanges();
                                TempData["msg"] = "File Succesfully Uploaded";
                                TempData["result"] = "success";
                                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
                            }
                            else if (excelDataReader.FieldCount == 6)
                            {
                                DataRowCollection row = dataSet.Tables["GR"].Rows;

                                foreach (DataRow item in row)
                                {
                                    rowDataList = item.ItemArray.ToList();
                                    var source_site = _db.Production_input.Where(k => k.po_bmi == Convert.ToString(rowDataList[0]) && k.date == Convert.ToDateTime(rowDataList[1])).First();

                                    if (source_site == null)
                                    {
                                        TempData["msg"] = $"Data GI for {source_site.raw_source} Not Available";
                                        TempData["result"] = "warning";
                                        return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
                                    }

                                    var ft_code = new string[] {
                                        "202028",
                                        "202026",
                                        "202020",
                                        "202024",
                                        "202049",
                                        "202050",
                                        "202048",
                                        "202045",
                                        "202047",
                                        "202044",
                                        "202046",
                                        "202041",
                                        "202043",
                                        "202040",
                                        "202042",
                                     };

                                    var fairtrade_status = "NFT";

                                    if (ft_code.Contains(source_site.sap_code))
                                    {
                                        fairtrade_status = "FT";
                                    }

                                    var po = _db.PO.Where(a => a.pt == Convert.ToInt32(rowDataList[2])).Select(a => a.po).ToList();

                                    if (po.Count == 0)
                                    {
                                        TempData["msg"] = $"PO For PT {rowDataList[2]} Not Available";
                                        TempData["result"] = "warning";
                                        return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
                                    }

                                    var bmi_code = _db.Master_BMI.Find(rowDataList[3]);

                                    if (bmi_code == null)
                                    {
                                        TempData["msg"] = $"BMI Code {rowDataList[3]} Not Available";
                                        TempData["result"] = "warning";
                                        return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
                                    }

                                    prod_output.Add(new ProductionOutputModel
                                    {
                                        po_bmi = Convert.ToString(rowDataList[0]),
                                        date = Convert.ToDateTime(rowDataList[1]),
                                        po = po.First(),
                                        bmi_code = Convert.ToString(rowDataList[3]),
                                        qty = Convert.ToSingle(rowDataList[4]),
                                        raw_source = source_site.raw_source,
                                        landing_site = source_site.landing_site,
                                        fairtrade_status = fairtrade_status,
                                        grade = Convert.ToString(rowDataList[5]),
                                        created_at = DateTime.Now,
                                        created_by = User.Identity.Name,
                                        gr_date = date
                                    });
                                }
                                stream.Close();
                                System.IO.File.Delete(filePath);
                                _db.Production_output.AddRange(prod_output);
                                _db.SaveChanges();

                                TempData["msg"] = "File Succesfully Uploaded";
                                TempData["result"] = "success";
                                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
                            }
                            stream.Close();
                            System.IO.File.Delete(filePath);
                            TempData["msg"] = "Field Column not Match";
                            TempData["result"] = "failed";
                            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
                        }
                    }
                }
                //jika tidak sesuai extension
                TempData["msg"] = "File Extension must excel file format 'xlsx or xls'";
                TempData["result"] = "failed";
                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
            }
            TempData["msg"] = "File Empty";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }

        public async Task<IActionResult> Detail(string pt,string po)
        {
            var model = new ProductionView();
            model.ProductionOutputModel = _db.Production_output
                .Where(a => a.po == po)
                .Include(k => k.MasterBMIModel)
                .AsEnumerable()
                .GroupBy(k => new { k.bmi_code, k.po })
                .Select(a => new ProductionOutputModel
                {
                    bmi_code = a.Key.bmi_code,
                    MasterBMIModel = a.Max(m => m.MasterBMIModel),
                    qty_production = a.Sum(x => x.qty)  
                    - _db.AdjustmentFG.Where(c => c.status == "Adjustment" && c.bmi_code == a.Key.bmi_code && c.po == a.Key.po).Sum(a => a.qty * a.MasterBMIModel.lbs / 2.204),
                    available = Convert.ToInt32( a.Sum(x => x.qty * 2.204 / x.MasterBMIModel.lbs) 
                    - _db.AdjustmentFG.Where(c => c.bmi_code == a.Key.bmi_code && c.po == a.Key.po).Sum(a => a.qty)
                    - _db.Shipment.Where(c=>c.batch == po && c.bmi_code== a.Key.bmi_code).Sum(c=>c.qty)
                    - _db.Repack.Where(c=>c.from_po == po && c.from_bmi_code == a.Key.bmi_code).Sum(c=>c.qty * 2.204 / c.fromMasterBMIModel.lbs ) 
                    + _db.Repack.Where(c => c.to_po == po && c.to_bmi_code == a.Key.bmi_code).Sum(c => c.qty * 2.204 / c.toMasterBMIModel.lbs )  ),
                })
                .OrderByDescending(a=>a.bmi_code)
                .ToList();
   
            model.CategoryView = _db.Production_output
                .Where(a => a.po == po)
                .Include(k => k.MasterBMIModel)
                .AsEnumerable()
                .GroupBy(k => k.MasterBMIModel.daily_category)
                .Select(a => new CategoryView
                {
                    category = a.Key,
                    qty = a.Sum(x => x.qty * 2.204),
                    yield = a.Sum(x => x.qty * 2.204) / _db.Production_output.Where(a => a.po == po).Sum(k => k.qty * 2.204)
                })
                .OrderByDescending(a=>a.category)
                .ToList();

            model.ProductionInputModel = _db.Production_input
                .Where(a => a.po == po)
                .Include(k => k.Masterdatamodel)
                .AsEnumerable()
                .GroupBy(k => new { k.raw_source, k.sap_code })
                .Select(a => new ProductionInputModel
                {
                    raw_source = a.Key.raw_source,
                    sap_code = a.Key.sap_code,
                    Masterdatamodel = a.Max(m => m.Masterdatamodel),
                    qty = a.Sum(x => x.qty)
                })
                .OrderByDescending(a=>a.raw_source)
                .ToList();

            ViewBag.totaloutput = model.ProductionOutputModel.Sum(a => a.qty_production * 2.204).ToString("0.00");
            ViewBag.totalinputkg = model.ProductionInputModel.Sum(k => k.qty).ToString("0.00");
            ViewBag.totalinputlbs = model.ProductionInputModel.Sum(k => k.qty * 2.204).ToString("0.00");
           
            ViewBag.yield = ((model.ProductionOutputModel.Sum(k => k.qty_production * 2.204) / model.ProductionInputModel.Sum(k => k.qty * 2.204)) * 100).ToString("0.00");

            ViewBag.pt = pt;
            ViewBag.po = po;
            return await Task.Run(()=> View(model));
        }

        public async Task<JsonResult> Detailperitem(string po, string bmicode)
        {
            var output = _db.Production_output
                .Where(a => a.po == po && a.bmi_code == bmicode)
                .Include(k => k.MasterBMIModel)
                .AsEnumerable()
                .GroupBy(k => new { k.date, k.raw_source })
                .Select(a => new ProductionOutputModel
                {
                    production_date = a.Key.date,
                    raw_source = a.Key.raw_source,
                    cases = Convert.ToInt32( a.Sum(x => x.qty *2.204) / a.Max(b=>b.MasterBMIModel.lbs)) ,
                    available = Convert.ToInt32(a.Sum(x => x.qty * 2.204) / a.Max(b => b.MasterBMIModel.lbs)
                    ///- _db.Shipment.Where(i=>i.batch == po &&  i.pdc == a.Key.date &&  i.bmi_code == bmicode && i.raw_source == a.Key.raw_source ).Sum(i=>i.qty) 
                    - _db.AdjustmentFG.Where( i=>i.status == "destroy" && i.production_date == a.Key.date && i.bmi_code == bmicode && i.raw_source == a.Key.raw_source).Sum(i=>i.qty)
                    - _db.Repack.Where(i =>i.from_po == po && i.production_date == a.Key.date && i.from_bmi_code == bmicode && i.raw_source == a.Key.raw_source).Sum(i => i.qty * 2.204 / i.fromMasterBMIModel.lbs))
                })
                .ToList();


            // tambahan repack
            var repack = _db.Repack
                .Where(a => a.to_po == po && a.to_bmi_code == bmicode)
                .Include(a => a.toMasterBMIModel)
                .AsEnumerable()
                .GroupBy(a => new { a.production_date, a.raw_source, a.to_bmi_code })
                .Select(a => new ProductionOutputModel
                {
                    production_date = a.Key.production_date,
                    raw_source = a.Key.raw_source,
                    cases = a.Sum(a => a.qty * 2.204 /a.toMasterBMIModel.lbs),
                    available = a.Sum(a=>a.qty *2.204 / a.toMasterBMIModel.lbs) 
                    //- _db.Shipment.Where(b=>b.batch == po && b.pdc == a.Key.production_date && b.bmi_code == a.Key.to_bmi_code && b.raw_source == a.Key.raw_source).Sum(b=>b.qty)
                })
                .ToList();

            var union = new List<ProductionOutputModel> (output);
            union.AddRange(repack);

            var result = union.GroupBy(a => new { a.production_date, a.raw_source })
                .Select(a => new
                {
                    date = a.Key.production_date,
                    raw_source = a.Key.raw_source,
                    cases = a.Sum(b => b.cases),
                    available = a.Sum(b => b.available) 
                    - _db.Shipment.Where(k=>k.batch == po && k.bmi_code == bmicode && k.pdc == a.Key.production_date && k.raw_source == a.Key.raw_source).Sum(k=>k.qty)
                })
                .OrderBy(a => a.date)
                .ToList();
            return await Task.Run(()=> Json(result));
        }

        public async Task<JsonResult> Detaillandingsite(string po, string bmicode, DateTime pdc, string raw_source)
        {
            var output = _db.Production_output
                .Include(a => a.MasterBMIModel)
                .Where(a => a.po == po && a.bmi_code == bmicode && a.raw_source == raw_source && a.date == pdc)
                .AsEnumerable()
                .GroupBy(a => a.landing_site)
                .Select(a => new RepackModel
                {
                    landing_site = a.Key + " " +a.Max(b=>b.fairtrade_status),
                    cases = Convert.ToInt32(a.Sum(x => x.qty * 2.204) / a.Max(b => b.MasterBMIModel.lbs))
                }).ToList();

            var repack = _db.Repack
                .Include(a=> a.toMasterBMIModel)
                .Where(a => a.to_po == po && a.to_bmi_code == bmicode && a.raw_source == raw_source && a.production_date == pdc) 
                .AsEnumerable()
                .GroupBy(a => a.landing_site)
                .Select(a => new RepackModel
                {
                    landing_site = a.Key +" "+ a.Max(b=>b.fairtrade_status),
                    cases = Convert.ToInt32(a.Sum(b => b.qty * 2.204 / a.Max(b=>b.toMasterBMIModel.lbs)))
                }).ToList();


            var union = new List<RepackModel> (output);
            union.AddRange(repack);

            var result = union
                .GroupBy(a => a.landing_site)
                .Select(a=> new 
                { 
                    landing_site = a.Key,
                    cases = a.Sum(b=>b.cases)
                }).ToList();

            return await Task.Run(() => Json(result));

        }

        public async Task<JsonResult> DateExist(DateTime date)
        {
            var unique = _db.Production_input.FirstOrDefault(m => m.date == date);
            if (unique != null)
            {
                return await Task.Run(()=> Json(true));
            }
            return await Task.Run(() => Json(false));
        }

        public async Task<IActionResult> Daily(DateTime date)
        {
            var raw = _db.Production_input
                .Include(k => k.POModel)
                .AsEnumerable()
                .Where(m => m.date == date)
                .GroupBy(k => k.po_bmi)
                .Select(a => new 
                {
                    po_bmi = a.Key,
                    raw_source = a.Max(k => k.raw_source),
                    POModel = a.Max(k => k.POModel),
                    landing_site = a.Max(k => k.landing_site),
                    qty = Math.Round( a.Sum(k=>k.qty),2)
                })
                .ToList();

            var fg = _db.Production_output
               .Include(k => k.POModel)
               .AsEnumerable()
               .Where(m => m.date == date)
               .GroupBy(k => k.po_bmi)
               .Select(a => new 
               {
                   po_bmi = a.Key,
                   raw_source = a.Max(k => k.raw_source),
                   POModel = a.Max(k => k.POModel),
                   landing_site = a.Max(k => k.landing_site),
                   qty = Math.Round(a.Sum(k=>k.qty),2)
               })
               .ToList();

            var join = from i in raw
                       join f in fg on i.po_bmi equals f.po_bmi
                       select new ProductionInputModel
                       {
                           po_bmi = i.po_bmi,
                           raw_source = i.raw_source,
                           POModel = i.POModel,
                           landing_site = i.landing_site,
                           qty_raw = i.qty,
                           qty_fg = f.qty
                       };

            ViewBag.date = date.Date;
            ViewBag.total_raw = Math.Round(raw.Sum(a => a.qty),2);
            ViewBag.total_fg = Math.Round(fg.Sum(a => a.qty),2);
            return await Task.Run(() => View(join));
        }


        public async Task<JsonResult> AllMaterial(string po, DateTime date)
        {
            var model = new ProductionView();
            model.ProductionInputModel = _db.Production_input
                .Include(k => k.Masterdatamodel)
                .AsEnumerable()
                .Where(m => m.po_bmi == po && m.date == date)
                .GroupBy(k => new { k.raw_source, k.sap_code, k.landing_site })
                .Select(a => new ProductionInputModel
                {
                    raw_source = a.Key.raw_source,
                    sap_code = a.Key.sap_code,
                    landing_site = a.Key.landing_site,
                    Masterdatamodel = a.Max(m => m.Masterdatamodel),
                    qty = a.Sum(k => k.qty)
                })
                .ToList();

            model.ProductionOutputModel = _db.Production_output
                .Include(k => k.MasterBMIModel)
                .AsEnumerable()
                .Where(m => m.po_bmi == po && m.date == date)
                .GroupBy(k => k.bmi_code)
                .Select(a => new ProductionOutputModel
                {
                    bmi_code = a.Key,
                    MasterBMIModel = a.Max(m => m.MasterBMIModel),
                    qty = a.Sum(k => k.qty)
                })
                .ToList();
            return await Task.Run(() => Json(model));
        }

        public async Task<JsonResult> Getitemdata(string code, string po)
        {
            var obj = _db.Production_output
                .Where(k => k.po == po && k.bmi_code == code)
                .GroupBy(x => new { x.date, x.raw_source })
                .Select(a => new ProductionOutputModel
                {
                    date = a.Key.date,
                    bmi_code = code,
                    raw_source = a.Key.raw_source,
                    landing_site = a.Max(b=>b.landing_site),
                    //fairtrade_status = a.Max(b=>b.fairtrade_status),
                    po = a.Max(k => k.po)
                })
                .OrderBy(m=>m.date)
                .ToList();
            return await Task.Run(()=> Json(obj));
        }

        [Authorize(Roles = "CC,Admin")]
        [Authorize(Policy = "Update")]
        public async Task<IActionResult> Adjustment(AdjustmentFGModel destroyFGModel)
        {
            if (ModelState.IsValid)
            {
                var destroy = new AdjustmentFGModel
                {
                    bmi_code = destroyFGModel.bmi_code,
                    qty = destroyFGModel.qty,
                    po = destroyFGModel.po,
                    status = "Adjustment",
                    created_at = DateTime.Now,
                    created_by = User.Identity.Name
                };
                _db.AdjustmentFG.Add(destroy);
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Destroyed";
                TempData["result"] = "success";
                return await Task.Run(()=> Redirect(Request.Headers["Referer"].ToString()));
            }
            TempData["msg"] = "Item Failded to Destroyed";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }

        [Authorize(Roles = "CC,Admin")]
        [Authorize(Policy = "Update")]
        public async Task<IActionResult> Repack(ProductionView productionView)
        {
            var to_po = Convert.ToString(_db.PO.Where(a => a.pt == Convert.ToInt32(productionView.destination_pt)).Select(a => a.po).First());

            var repack = new RepackModel
            {
                po = productionView.po_bmi,
                date = productionView.date,
                raw_source = productionView.raw_source,
                qty = productionView.qty,
                production_date = productionView.production_date,
                from_po = productionView.po,
                from_bmi_code = productionView.bmi_code,
                to_po = to_po,
                to_bmi_code = productionView.to_bmi_code,
                landing_site = productionView.landing_site,
                fairtrade_status = productionView.fairtrade_status,
                created_by = User.Identity.Name,
                created_at = DateTime.Now
            };
            if (repack != null)
            {
                _db.Repack.Add(repack);
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Repacked";
                TempData["result"] = "success";
                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
            }
            TempData["msg"] = "Item Failed to Repack";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }

        public async Task<IActionResult> History()
        {
            var model = new ProductionView();
            model.ProductionInputModel = _db.Production_input
                .Where(a=>a.gi_date != null)
                .AsEnumerable()
                .GroupBy(a =>a.gi_date)
                .Select(a=> new ProductionInputModel
                { 
                   gi_date = a.Key,
                   created_by = a.Max(b=>b.created_by),
                   created_at = a.Max(b=>b.created_at)
                })
                .OrderByDescending(a=>a.gi_date).Take(10)
                .ToList();

            model.ProductionOutputModel = _db.Production_output
                .Where(a => a.gr_date != null)
               .AsEnumerable()
               .GroupBy(a => a.gr_date)
               .Select(a => new ProductionOutputModel
               {
                   gr_date = a.Key,
                   created_by = a.Max(b => b.created_by),
                   created_at = a.Max(b => b.created_at),
               })
               .OrderByDescending(a => a.gr_date).Take(10)
               .ToList();

            return await Task.Run(() => View(model));
        }

        [Authorize(Roles = "CC,Admin")]
        [Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteGI(DateTime date)
        {
            var datetime = _db.Production_input.Where(a => a.gi_date == date).ToList();
            if (datetime.Count>0)
            {
                _db.Production_input.RemoveRange(datetime);
                _db.SaveChanges();
                TempData["msg"] = "Data Successfuly Deleted";
                TempData["result"] = "success";
                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
            }
            TempData["msg"] = "Data Failed to Delete";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }

        [Authorize(Roles = "CC,Admin")]
        [Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteGR(DateTime date)
        {
            var datetime = _db.Production_output.Where(a => a.gr_date == date).ToList();
            if(datetime.Count > 0)
            {
                _db.Production_output.RemoveRange(datetime);
                _db.SaveChanges();
                TempData["msg"] = "Data Successfuly Deleted";
                TempData["result"] = "success";
                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
            }
            TempData["msg"] = "Data Failed to Delete";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }

        [Authorize(Roles = "CC,Admin")]
        [Authorize(Policy = "Delete")]
        public async Task<IActionResult> Destroy(ProductionView productionView)
        {
           
            var obj = new AdjustmentFGModel
            {
                bmi_code = productionView.bmi_code,
                raw_source = productionView.raw_source,
                production_date = productionView.production_date,
                landing_site = productionView.landing_site,
                reason = productionView.reason,
                status = "destroy",
                qty = productionView.qty,
                po = productionView.po,
                created_at = DateTime.Now,
                created_by = User.Identity.Name
            };

            if(obj != null)
            {
                _db.AdjustmentFG.Add(obj);
                _db.SaveChanges();
                TempData["msg"] = "Data Successfuly Added";
                TempData["result"] = "success";
                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
            }
            TempData["msg"] = "Data Failed to Add";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }


        public IActionResult DownloadFG(string po,string pt)
        {
            var ProductionOutput = _db.Production_output
             .Where(a => a.po == po)
             .Include(k => k.MasterBMIModel)
             .Include(k => k.POModel)
             .AsEnumerable()
             .GroupBy(k => new { k.date, k.po_bmi, k.bmi_code })
             .Select(a => new ProductionView
             {
                 po_bmi = a.Key.po_bmi,
                 date = a.Key.date,
                 raw_source = a.Max(m => m.raw_source),
                 landing_site = a.Max(m => m.landing_site) + " " + a.Max(m=>m.fairtrade_status),
                 bmi_code = a.Key.bmi_code,
                 qty = a.Sum(x => x.qty),
                 lbs = Math.Round( (a.Sum(x=>x.qty * 2.204)),2),
                 cases = Convert.ToInt32(a.Sum(x => x.qty * 2.204 / a.Max(m => m.MasterBMIModel.lbs))),
                 MasterBMIModel = a.Max(m => m.MasterBMIModel),
                 POModel = a.Max(m => m.POModel)
             })
             .OrderBy(a=>a.date).ThenBy(a=>a.po_bmi)
             .ToList();

            var repack = _db.Repack
                .Where(a=>a.to_po == po)
                .Include(k => k.toMasterBMIModel)
                .Include(k => k.toPOModel)
                .AsEnumerable()
                .GroupBy(k => new {k.production_date,k.po,k.to_bmi_code })
                .Select(a=> new ProductionView
                {
                    po_bmi = a.Key.po,
                    date = a.Key.production_date,
                    raw_source = a.Max(m => m.raw_source),
                    landing_site = a.Max(m => m.landing_site) +" "+ a.Max(m=>m.fairtrade_status),
                    bmi_code = a.Key.to_bmi_code,
                    qty = a.Sum(x => x.qty),
                    lbs = Math.Round(( a.Sum(x => x.qty * 2.204)),2),
                    cases = Convert.ToInt32(a.Sum(x => x.qty * 2.204 / a.Max(m => m.toMasterBMIModel.lbs))),
                    MasterBMIModel = a.Max(m => m.toMasterBMIModel),
                    POModel = a.Max(m => m.toPOModel)
                })
                .OrderBy(a => a.date).ThenBy(a => a.po_bmi)
                .ToList();

            var union = new List<ProductionView>(ProductionOutput);
            union.AddRange(repack);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("PT "+pt);
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "PO";
                worksheet.Cell(currentRow, 2).Value = "Date";
                worksheet.Cell(currentRow, 3).Value = "PT";
                worksheet.Cell(currentRow, 4).Value = "Raw Source";
                worksheet.Cell(currentRow, 5).Value = "Landing Site";
                worksheet.Cell(currentRow, 6).Value = "FG Code";
                worksheet.Cell(currentRow, 7).Value = "FG Description";
                worksheet.Cell(currentRow, 8).Value = "FG Qty in Kg";
                worksheet.Cell(currentRow, 9).Value = "FG Qty in Lbs";
                worksheet.Cell(currentRow, 10).Value = "FG Qty in CS";
                worksheet.Cell(currentRow, 11).Value = "FG Batch";

                foreach (var data in union)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = data.po_bmi;
                    worksheet.Cell(currentRow, 2).Value = data.date;
                    worksheet.Cell(currentRow, 3).Value = data.POModel.pt;
                    worksheet.Cell(currentRow, 4).Value = data.raw_source;
                    worksheet.Cell(currentRow, 5).Value = data.landing_site;
                    worksheet.Cell(currentRow, 6).Value = data.MasterBMIModel.sap_code;
                    worksheet.Cell(currentRow, 7).Value = data.MasterBMIModel.description;
                    worksheet.Cell(currentRow, 8).Value = data.qty;
                    worksheet.Cell(currentRow, 9).Value = data.lbs;
                    worksheet.Cell(currentRow, 10).Value = data.cases;
                    worksheet.Cell(currentRow, 11).Value = data.POModel.batch;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "FG-PT "+pt+".xlsx");
                }
            }

        }


        public IActionResult DownloadRaw(string po, string pt)
        {
            var ProductionOutput = _db.Production_input
             .Where(a => a.po == po)
             .Include(k => k.Masterdatamodel)
             .Include(k => k.POModel)
             .AsEnumerable()
             .GroupBy(k => new { k.date, k.po_bmi, k.sap_code })
             .Select(a => new ProductionInputModel
             {
                 po_bmi = a.Key.po_bmi,
                 date = a.Key.date,
                 raw_source = a.Max(m => m.raw_source),
                 landing_site = a.Max(m => m.landing_site),
                 sap_code = a.Key.sap_code,
                 qty = a.Sum(x => x.qty),
                 Masterdatamodel = a.Max(m => m.Masterdatamodel),
                 POModel = a.Max(m => m.POModel)
             })
             .OrderBy(a => a.date).ThenBy(a => a.po_bmi)
             .ToList();


            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("PT " + pt);
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "PO";
                worksheet.Cell(currentRow, 2).Value = "Date";
                worksheet.Cell(currentRow, 3).Value = "PT";
                worksheet.Cell(currentRow, 4).Value = "Raw Source";
                worksheet.Cell(currentRow, 5).Value = "Landing Site";
                worksheet.Cell(currentRow, 6).Value = "Raw Code";
                worksheet.Cell(currentRow, 7).Value = "Raw Description";
                worksheet.Cell(currentRow, 8).Value = "Raw Qty";

                foreach (var data in ProductionOutput)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = data.po_bmi;
                    worksheet.Cell(currentRow, 2).Value = data.date;
                    worksheet.Cell(currentRow, 3).Value = data.POModel.pt;
                    worksheet.Cell(currentRow, 4).Value = data.raw_source;
                    worksheet.Cell(currentRow, 5).Value = data.landing_site;
                    worksheet.Cell(currentRow, 6).Value = data.Masterdatamodel.sap_code;
                    worksheet.Cell(currentRow, 7).Value = data.Masterdatamodel.description;
                    worksheet.Cell(currentRow, 8).Value = data.qty;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "RAW-PT " + pt + ".xlsx");
                }
            }

        }


    }
}
