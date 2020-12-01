using System;
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

        public IActionResult Index()
        {
            var obj = _db.PO
                .Where(a => a.plant == "3700" && a.pt != null)
                .Select(a=> new POModel { 
                    pt = a.pt,
                    po = a.po,
                    batch = a.batch,
                    status = a.status
                })
                .OrderByDescending(a => a.pt).ToList();
            return View(obj);
        }


        public IActionResult Getptdata(string po)
        {
            var obj = _db.PO.Find(po);
            return Json(obj);
        }

        public IActionResult Updatept(POModel POModel)
        {
            var po = new POModel() { po = POModel.po, status = POModel.status };
            _db.PO.Attach(po);
            _db.Entry(po).Property(x => x.status).IsModified = true; 
            _db.SaveChanges();
            TempData["msg"] = "File Succesfully Updated";
            TempData["result"] = "success";
            return RedirectToAction("Index");
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult ImportGI(IFormFile postedFile)
        {
            if (postedFile != null)
            {
                var allowedExtensions = new[] { ".xls", ".xlsx" };
                string fileName = Path.GetFileName(postedFile.FileName);
                var checkextension = Path.GetExtension(fileName).ToLower();

                if (allowedExtensions.Contains(checkextension))
                {
                    List<ProductionInputModel> prod_input = new List<ProductionInputModel>();
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
                            if (excelDataReader.FieldCount == 7)
                            {

                                var conf = new ExcelDataSetConfiguration()
                                {
                                    ConfigureDataTable = a => new ExcelDataTableConfiguration
                                    {
                                        UseHeaderRow = true
                                    }
                                };

                                DataSet dataSet = excelDataReader.AsDataSet(conf);
                                DataRowCollection row = dataSet.Tables["GI"].Rows;
                                List<object> rowDataList = null;

                                foreach (DataRow item in row)
                                {
                                    rowDataList = item.ItemArray.ToList();
                                    var po = Convert.ToString(_db.PO.Where(a => a.pt == Convert.ToInt32(rowDataList[2])).Select(a => a.po).First());
                                    prod_input.Add(new ProductionInputModel
                                    {
                                        po_bmi = Convert.ToString(rowDataList[0]),
                                        date = Convert.ToDateTime(rowDataList[1]),
                                        po = po,
                                        raw_source = Convert.ToString(rowDataList[3]),
                                        sap_code = Convert.ToString(rowDataList[4]),
                                        qty = Convert.ToSingle(rowDataList[5]),
                                        landing_site = Convert.ToString(rowDataList[6]),
                                        created_at = DateTime.Now
                                    });
                                }
                                _db.Production_input.AddRange(prod_input);
                                _db.SaveChanges();
                                TempData["msg"] = "File Succesfully Uploaded";
                                TempData["result"] = "success";
                                return RedirectToAction("Index");
                            }
                            TempData["msg"] = "Field Column not Match";
                            TempData["result"] = "failed";
                            return RedirectToAction("Index");
                        }
                    }
                }
                //jika tidak sesuai extension
                TempData["msg"] = "File Extension must excel file format 'xlsx or xls'";
                TempData["result"] = "failed";
                return RedirectToAction("Index");
            }
            TempData["msg"] = "File Empty";
            TempData["result"] = "failed";
            return RedirectToAction("Index");
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult ImportGR(IFormFile postedFile)
        {
            if (postedFile != null)
            {
                var allowedExtensions = new[] { ".xls", ".xlsx" };
                string fileName = Path.GetFileName(postedFile.FileName);
                var checkextension = Path.GetExtension(fileName).ToLower();

                if (allowedExtensions.Contains(checkextension))
                {
                    List<ProductionOutputModel> prod_output = new List<ProductionOutputModel>();
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
                            if (excelDataReader.FieldCount == 5)
                            {

                                var conf = new ExcelDataSetConfiguration()
                                {
                                    ConfigureDataTable = a => new ExcelDataTableConfiguration
                                    {
                                        UseHeaderRow = true
                                    }
                                };

                                DataSet dataSet = excelDataReader.AsDataSet(conf);
                                DataRowCollection row = dataSet.Tables["GR"].Rows;
                                List<object> rowDataList = null;

                                foreach (DataRow item in row)
                                {
                                    rowDataList = item.ItemArray.ToList();
                                    var source_site = _db.Production_input.Where(k => k.po_bmi == Convert.ToString(rowDataList[0]) && k.date == Convert.ToDateTime(rowDataList[1])).First();
                                    var po = Convert.ToString(_db.PO.Where(a => a.pt == Convert.ToInt32(rowDataList[2])).Select(a => a.po).First());
                                    prod_output.Add(new ProductionOutputModel
                                    {
                                        po_bmi = Convert.ToString(rowDataList[0]),
                                        date = Convert.ToDateTime(rowDataList[1]),
                                        po = po,
                                        bmi_code = Convert.ToString(rowDataList[3]),
                                        qty = Convert.ToSingle(rowDataList[4]),
                                        raw_source = source_site.raw_source,
                                        landing_site = source_site.landing_site,
                                        created_at = DateTime.Now
                                    });
                                }
                                _db.Production_output.AddRange(prod_output);
                                _db.SaveChanges();
                                TempData["msg"] = "File Succesfully Uploaded";
                                TempData["result"] = "success";
                                return RedirectToAction("Index");
                            }
                            TempData["msg"] = "Field Column not Match";
                            TempData["result"] = "failed";
                            return RedirectToAction("Index");
                        }
                    }
                }
                //jika tidak sesuai extension
                TempData["msg"] = "File Extension must excel file format 'xlsx or xls'";
                TempData["result"] = "failed";
                return RedirectToAction("Index");
            }
            TempData["msg"] = "File Empty";
            TempData["result"] = "failed";
            return RedirectToAction("Index");
        }

        public IActionResult Detail(string pt)
        {
            var po = Convert.ToString(_db.PO.Where(a => a.pt == Convert.ToInt32(pt)).Select(a => a.po).First());
            var model = new ProductionView();
            model.ProductionOutputModel = _db.Production_output
                .Where(a => a.po == po)
                .Include(k => k.MasterBMIModel)
                .Include(k => k.POModel)
                .AsEnumerable()
                .GroupBy(k => new { k.bmi_code, k.POModel, k.po })
                .Select(a => new ProductionOutputModel
                {
                    bmi_code = a.Key.bmi_code,
                    MasterBMIModel = a.Max(m => m.MasterBMIModel),
                    qty_production = a.Sum(x => x.qty) - _db.AdjustmentFG.Where(c => c.status == "rounded" && c.bmi_code == a.Key.bmi_code && c.po == a.Key.po).Sum(a => a.qty * a.MasterBMIModel.lbs / 2.204),
                    available = a.Sum(k => k.qty * 2.204 / k.MasterBMIModel.lbs)
                    - _db.Shipment_detail.Where(c => c.bmi_code == a.Key.bmi_code && c.po == a.Key.po).Sum(a => a.qty)
                    - _db.AdjustmentFG.Where(c => c.bmi_code == a.Key.bmi_code && c.po == a.Key.po).Sum(a => a.qty)
                    - _db.Repack.Where(c => c.from_bmi_code == a.Key.bmi_code && c.from_po == a.Key.po).Sum(a => a.qty)
                    + _db.Repack.Where(c => c.to_bmi_code == a.Key.bmi_code && c.to_po == a.Key.po).Sum(a => a.qty)
                })
                .ToList();
            ViewBag.totaloutput = model.ProductionOutputModel.Sum(a => a.qty_production * 2.204).ToString("0.00");

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
                .ToList();
            ViewBag.totalinputkg = model.ProductionInputModel.Sum(k => k.qty).ToString("0.00");
            ViewBag.totalinputlbs = model.ProductionInputModel.Sum(k => k.qty * 2.204).ToString("0.00");

            ViewBag.yield = ((model.ProductionOutputModel.Sum(k => k.qty_production * 2.204) / model.ProductionInputModel.Sum(k => k.qty * 2.204)) * 100).ToString("0.00");

            ViewBag.pt = pt;
            ViewBag.po = po;
            return View(model);
        }

        public IActionResult Detailperitem(int pt, string code)
        {
            var po = Convert.ToString(_db.PO.Where(a => a.pt == Convert.ToInt32(pt)).Select(a => a.po).First());

            var obj = _db.Production_output
                .Where(a => a.po == po && a.bmi_code == code)
                .Include(k => k.MasterBMIModel)
                .AsEnumerable()
                .GroupBy(k => new { k.date, k.raw_source, k.landing_site })
                .Select(a => new ProductionOutputModel
                {
                    date = a.Key.date,
                    raw_source = a.Key.raw_source,
                    landing_site = a.Key.landing_site,
                    MasterBMIModel = a.Max(m => m.MasterBMIModel),
                    qty = a.Sum(x => x.qty)
                })
                .OrderByDescending(a => a.date)
                .ToList();
            return Json(obj);
        }

        public IActionResult DateExist(DateTime date)
        {
            var unique = _db.Production_input.FirstOrDefault(m => m.date.Year == date.Year &&
                                                                    m.date.Month == date.Month &&
                                                                    m.date.Day == date.Day);
            if (unique != null)
            {
                return Json(true);
            }
            return Json(false);
        }

        public IActionResult Daily(DateTime date)
        {
            var obj = _db.Production_input
                .Include(k => k.POModel)
                .AsEnumerable()
                .Where(m => m.date.Year == date.Year &&
                       m.date.Month == date.Month &&
                       m.date.Day == date.Day)
                .GroupBy(k => k.po_bmi)
                .Select(a => new ProductionInputModel
                {
                    po_bmi = a.Key,
                    raw_source = a.Max(k => k.raw_source),
                    POModel = a.Max(k => k.POModel),
                    landing_site = a.Max(k => k.landing_site),
                })
                .ToList();
            ViewBag.date = date.Date;
            return View(obj);
        }

        public IActionResult RawMaterial(string po, DateTime date)
        {
            var obj = _db.Production_input
                .Include(k => k.Masterdatamodel)
                .AsEnumerable()
                .Where(m => m.po_bmi == po && m.date.Year == date.Year &&
                       m.date.Month == date.Month &&
                       m.date.Day == date.Day)
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
            return Json(obj);
        }

        public IActionResult FinishedGood(string po, DateTime date)
        {
            var obj = _db.Production_output
                .Include(k => k.MasterBMIModel)
                .AsEnumerable()
                .Where(m => m.po_bmi == po && m.date.Year == date.Year &&
                       m.date.Month == date.Month &&
                       m.date.Day == date.Day)
                .GroupBy(k => k.bmi_code)
                .Select(a => new ProductionOutputModel
                {
                    bmi_code = a.Key,
                    MasterBMIModel = a.Max(m => m.MasterBMIModel),
                    qty = a.Sum(k => k.qty)
                })
                .ToList();
            return Json(obj);
        }


        public IActionResult Getitemdata(string code, string pt)
        {
            var po = Convert.ToString(_db.PO.Where(a => a.pt == Convert.ToInt32(pt)).Select(a => a.po).First());
            var obj = _db.Production_output
                .Where(k => k.po == po && k.bmi_code == code)
                .GroupBy(x => new { x.date, x.bmi_code, x.raw_source })
                .Select(a => new ProductionOutputModel
                {
                    date = a.Key.date,
                    bmi_code = a.Key.bmi_code,
                    raw_source = a.Key.raw_source,
                    po = a.Max(k => k.po)
                })
                .ToList();
            return Json(obj);
        }

        public IActionResult Adjustment(AdjustmentFGModel destroyFGModel)
        {
            if (ModelState.IsValid)
            {
                var destroy = new AdjustmentFGModel
                {
                    bmi_code = destroyFGModel.bmi_code,
                    qty = destroyFGModel.qty,
                    po = destroyFGModel.po,
                    status = "rounded"
                };
                _db.AdjustmentFG.Add(destroy);
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Destroyed";
                TempData["result"] = "success";
                return Redirect(Request.Headers["Referer"].ToString());
            }
            TempData["msg"] = "Item Failded to Destroyed";
            TempData["result"] = "failed";
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult Repack(ProductionView productionView)
        {
            var to_po = Convert.ToString(_db.PO.Where(a => a.pt == Convert.ToInt32(productionView.destination_pt)).Select(a => a.po).First());

            if (ModelState.IsValid)
            {
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
                    to_bmi_code = productionView.to_bmi_code
                };
                _db.Repack.Add(repack);
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Repacked";
                TempData["result"] = "success";
                return Redirect(Request.Headers["Referer"].ToString());
            }
            TempData["msg"] = "Item Failded to Repack";
            TempData["result"] = "failed";
            return Redirect(Request.Headers["Referer"].ToString());
        }


    }
}
