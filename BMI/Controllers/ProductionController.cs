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
            var obj = _db.Production_output
                .Include(k=>k.PTModel)
                .AsEnumerable()
                .GroupBy(x => x.id_pt, (key, g) => g.OrderByDescending(e => e.id_productionoutput).First())
                .OrderByDescending(e => e.id_productionoutput)
                .ToList();
            return View(obj);
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
                    List<PTModel> bmi_po = new List<PTModel>();
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
                            if (excelDataReader.FieldCount == 8)
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
                                    var id_pt = rowDataList[2] + "3700";
                                    var unique = _db.Pt.FirstOrDefault(m => m.id_pt == id_pt);
                                    if (unique == null)
                                    {
                                        var obj = new PTModel
                                        {
                                            id_pt = id_pt,
                                            pt = Convert.ToInt32(rowDataList[2]),
                                            plant="3770",
                                            batch = Convert.ToString(rowDataList[7])
                                        };
                                        _db.Pt.Add(obj);
                                        _db.SaveChanges();
                                    }
                                }

                                foreach (DataRow item in row)
                                {
                                    rowDataList = item.ItemArray.ToList();
                                    var id_pt = rowDataList[2] + "3700";
                                    prod_input.Add(new ProductionInputModel
                                    {
                                        po = Convert.ToString(rowDataList[0]),
                                        date = Convert.ToDateTime(rowDataList[1]),
                                        id_pt = id_pt,
                                        raw_source = Convert.ToString(rowDataList[3]),
                                        bmi_code = Convert.ToString(rowDataList[4]),
                                        qty = Convert.ToSingle(rowDataList[5]),
                                        landing_site = Convert.ToString(rowDataList[6])
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
                                    var id_pt = rowDataList[2] + "3700";
                                    var po = Convert.ToString(rowDataList[0]);
                                    var date = Convert.ToDateTime(rowDataList[1]);
                                    var source = _db.Production_input.Where(k => k.po == po && k.date == date).First();

                                    prod_output.Add(new ProductionOutputModel
                                    {
                                        po = Convert.ToString(rowDataList[0]),
                                        date = Convert.ToDateTime(rowDataList[1]),
                                        id_pt = id_pt,
                                        bmi_code = Convert.ToString(rowDataList[3]),
                                        qty = Convert.ToSingle(rowDataList[4]),
                                        raw_source = source.raw_source
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
            var no_pt = pt + "3700";

            var model = new ProductionView();

            model.ProductionOutputModel = _db.Production_output
                .Where(a => a.id_pt == no_pt)
                .Include(k => k.MasterBMIModel)
                .Include(k=>k.PTModel)
                .AsEnumerable()
                .GroupBy(k => new { k.bmi_code,k.PTModel,k.id_pt})
                .Select(a => new ProductionOutputModel
                {
                    bmi_code = a.Key.bmi_code,
                    MasterBMIModel = a.Max(m => m.MasterBMIModel),
                    qty_production = a.Sum(x => x.qty) - _db.DestroyFG.Where(c => c.status== "adjustment" &&  c.bmi_code == a.Key.bmi_code && c.PTModel.batch == a.Key.PTModel.batch).Sum(a => a.qty * a.MasterBMIModel.lbs / 2.204),
                    available = a.Sum(k => k.qty * 2.204 / k.MasterBMIModel.lbs)
                    - _db.Shipment_detail.Where(c => c.bmi_code == a.Key.bmi_code && c.batch == a.Key.PTModel.batch).Sum(a => a.qty) 
                    - _db.DestroyFG.Where(c => c.bmi_code == a.Key.bmi_code && c.PTModel.batch == a.Key.PTModel.batch).Sum(a => a.qty)

                })
                .ToList();
            ViewBag.totaloutput = model.ProductionOutputModel.Sum(a=>a.qty_production*2.204).ToString("0.00");

            model.CategoryView = _db.Production_output
                .Where(a => a.id_pt == no_pt)
                .Include(k => k.MasterBMIModel)
                .AsEnumerable()
                .GroupBy(k => k.MasterBMIModel.daily_category)
                .Select(a => new CategoryView
                 {
                     category = a.Key,
                     qty = a.Sum(x => x.qty*2.204),
                     yield = a.Sum(x => x.qty * 2.204) / _db.Production_output.Where(a => a.id_pt == no_pt).Sum(k=>k.qty*2.204)
                })
                .ToList();

            model.ProductionInputModel = _db.Production_input
                .Where(a => a.id_pt == no_pt)
                .Include(k => k.MasterBMIModel)
                .AsEnumerable()
                .GroupBy(k => new { k.raw_source, k.bmi_code })
                .Select(a => new ProductionInputModel
                {
                    raw_source = a.Key.raw_source,
                    bmi_code = a.Key.bmi_code,
                    MasterBMIModel = a.Max(m => m.MasterBMIModel),
                    qty = a.Sum(x => x.qty)
                })
                .ToList();
            ViewBag.totalinputkg = model.ProductionInputModel.Sum(k => k.qty).ToString("0.00");
            ViewBag.totalinputlbs = model.ProductionInputModel.Sum(k => k.qty*2.204).ToString("0.00");

            ViewBag.yield = ((model.ProductionOutputModel.Sum(k => k.qty_production * 2.204) / model.ProductionInputModel.Sum(k => k.qty * 2.204)) *100).ToString("0.00");

            ViewBag.pt = pt;
            return View(model);
        }

        public IActionResult Detailperitem(int pt,string code)
        {
            
            var obj = _db.Production_output
                .Where(a => a.PTModel.pt == pt && a.bmi_code==code)
                .Include(k => k.MasterBMIModel)
                .Include(k=>k.PTModel)
                .AsEnumerable()
                .GroupBy(k => k.date)
                .Select(a => new ProductionOutputModel
                {
                    date = a.Key,
                    MasterBMIModel = a.Max(m => m.MasterBMIModel),
                    PTModel = a.Max(m=>m.PTModel),
                    qty = a.Sum(x => x.qty)
                    
                })
                .OrderByDescending(a=>a.date)
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
                .Include(k=>k.PTModel)
                .AsEnumerable()
                .Where(m => m.date.Year == date.Year &&
                       m.date.Month == date.Month &&
                       m.date.Day == date.Day)
                .GroupBy(k => k.po)
                .Select(a => new ProductionInputModel
                {
                    po = a.Key,
                    raw_source = a.Max(k=>k.raw_source),
                    PTModel = a.Max(k=>k.PTModel),
                    landing_site = a.Max(k => k.landing_site),
                })
                .ToList();
            ViewBag.date = date.Date;
            return View(obj);
        }

        public IActionResult RawMaterial(string po,DateTime date)
        {
            var obj = _db.Production_input
                .Include(k => k.MasterBMIModel)
                .AsEnumerable()
                .Where(m => m.po == po && m.date.Year == date.Year &&
                       m.date.Month == date.Month &&
                       m.date.Day == date.Day)
                .GroupBy(k => new {k.raw_source, k.bmi_code, k.landing_site })
                .Select(a => new ProductionInputModel
                {
                    raw_source = a.Key.raw_source,
                    bmi_code = a.Key.bmi_code,
                    landing_site = a.Key.landing_site,
                    MasterBMIModel = a.Max(m => m.MasterBMIModel),
                    qty = a.Sum(k => k.qty)
                })
                .ToList();
            return Json(obj);
        }

        public IActionResult FinishedGood(string po,DateTime date)
        {
            var obj = _db.Production_output
                .Include(k => k.MasterBMIModel)
                .AsEnumerable()
                .Where(m => m.po == po && m.date.Year == date.Year &&
                       m.date.Month == date.Month &&
                       m.date.Day == date.Day)
                .GroupBy(k =>k.bmi_code)
                .Select(a => new ProductionOutputModel
                {
                    bmi_code = a.Key,
                    MasterBMIModel = a.Max(m => m.MasterBMIModel),
                    qty = a.Sum(k => k.qty)
                })
                .ToList();
            return Json(obj);
        }


        public IActionResult Getitemdata(string code,string pt)
        {
            pt = pt + "3700";
            var obj = _db.Production_output
                .Where(k => k.id_pt == pt && k.bmi_code == code)
                .GroupBy(x => x.bmi_code)
                .Select(a=>new ProductionOutputModel
                { 
                    bmi_code = a.Key,
                    id_pt = a.Max(k=>k.id_pt)
                })
                .ToList();
            return Json(obj);
        }

        public IActionResult Adjustment(DestroyFGModel destroyFGModel)
        {
            if (ModelState.IsValid)
            {
                var destroy = new DestroyFGModel
                {
                    bmi_code = destroyFGModel.bmi_code,
                    qty = destroyFGModel.qty,
                    id_pt = destroyFGModel.id_pt + "3700",
                    status = "adjustment"
                };
                _db.DestroyFG.Add(destroy);
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Added";
                TempData["result"] = "success";
                return RedirectToAction("Detail", new { pt = destroyFGModel.id_pt });
            }
            TempData["msg"] = "Item Failded to Added";
            TempData["result"] = "failed";
            return RedirectToAction("Detail", new { pt = destroyFGModel.id_pt });
        }

        public IActionResult Repack(ProductionView repackModel) 
        {
            if (ModelState.IsValid)
            {
                //var repack = new RepackModel
                //{
                //    bmi_code = destroyFGModel.bmi_code,
                //    qty = destroyFGModel.qty,
                //    id_pt = destroyFGModel.id_pt + "3700",
                //    status = "adjustment"
                //};
            }
            return View();
        }


    }
}
