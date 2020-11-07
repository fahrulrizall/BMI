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
                                    var unique = _db.pt.FirstOrDefault(m => m.id_pt == id_pt);
                                    if (unique == null)
                                    {
                                        var obj = new PTModel
                                        {
                                            id_pt = id_pt,
                                            pt = Convert.ToInt32(rowDataList[2]),
                                            plant="3770",
                                            batch = Convert.ToString(rowDataList[7])
                                        };
                                        _db.pt.Add(obj);
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
                                    prod_output.Add(new ProductionOutputModel
                                    {
                                        po = Convert.ToString(rowDataList[0]),
                                        date = Convert.ToDateTime(rowDataList[1]),
                                        id_pt = id_pt,
                                        bmi_code = Convert.ToString(rowDataList[3]),
                                        qty = Convert.ToSingle(rowDataList[4]),
                                        //batch = Convert.ToString(rowDataList[5]),
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
            var obj = _db.Production_output
                .Where(a => a.id_pt == no_pt)
                .Include(k=>k.MasterBMIModel)
                .AsEnumerable()
                .GroupBy(k=>k.bmi_code)
                .Select(a=>new ProductionView { 
                    code = a.Key,
                    MasterBMIModel = a.Max(m=>m.MasterBMIModel),
                    total = a.Sum(x=>x.qty)
                })
                .ToList();
            ViewBag.pt = pt;
            return View(obj);
        }

        public IActionResult Detailperitem(int pt,string code)
        {
            
            var obj = _db.Production_output
                .Where(a => a.PTModel.pt == pt && a.bmi_code==code)
                .Include(k => k.MasterBMIModel)
                .AsEnumerable()
                .GroupBy(k => k.date)
                .Select(a => new ProductionView
                {
                    date = a.Key,
                    MasterBMIModel = a.Max(m => m.MasterBMIModel),
                    total = a.Sum(x => x.qty)
                    
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

    }
}
