using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Threading.Tasks;
using BMI.Data;
using BMI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using ExcelDataReader;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BMI.Controllers
{
    public class RmController : Controller
    {
        private readonly ApplicationDbContext _db;

        private IWebHostEnvironment Environment;
        private IConfiguration Configuration;

        public RmController(ApplicationDbContext db,IWebHostEnvironment _environment, IConfiguration _configuration)
        {
            _db = db;
            Environment = _environment;
            Configuration = _configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List(string status)
        {
            if (status=="in_plant" || status=="otw")
            {
                var list = _db.Rm
                    .Where(x => x.status == status)
                    .AsEnumerable()
                    .GroupBy(x => x.reff, (key, g) => g.OrderByDescending(e => e.id_raw).First())
                    .OrderByDescending(e => e.id_raw)
                    .ToList();

                if (status == "in_plant")
                {
                    ViewBag.status = "In Plant";
                }
                else
                {
                    ViewBag.status = "On The Water";
                }
                return View(list);
            }
            return NotFound();
        }

        public IActionResult Detail (string reff)
        {
            var list = _db.Rm
                .Where(x => x.reff == reff)
                .Include(x=>x.Masterdatamodel)
                .OrderByDescending(e=>e.id_raw)
                .ToList();
            ViewBag.reff = reff;
            return View(list);
        }

        public IActionResult Getdata(int id)
        {
            var obj = _db.Rm.Find(id);
            return Json(obj);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(Rmmodel rmmodel)
        {
            if (ModelState.IsValid)
            {
                var list = _db.Rm.Where(x => x.reff == rmmodel.reff).ToList();
                if (rmmodel.status == "In Plant")
                {
                    var status = "in_plant";
                    list.ForEach(a =>
                    {
                        a.eta = rmmodel.eta;
                        a.etd = rmmodel.etd;
                        a.container = rmmodel.container;
                        a.status = status;
                        a.qty_received = a.qty_pl;
                    });
                    _db.SaveChanges();
                    TempData["msg"] = "Item Succesfully Updated";
                    TempData["result"] = "success";
                    return RedirectToAction("List", new { status = status });
                }
                else
                {
                    var status = "otw";
                    list.ForEach(a =>
                    {
                        a.eta = rmmodel.eta;
                        a.etd = rmmodel.etd;
                        a.container = rmmodel.container;
                        a.status = status;
                    });
                    _db.SaveChanges();
                    TempData["msg"] = "Item Succesfully Updated";
                    TempData["result"] = "success";
                    return RedirectToAction("List", new { status = status });
                }
            }
            TempData["msg"] = "Item Failed Updated";
            TempData["result"] = "failed";
            return RedirectToAction("Index");
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Import(IFormFile postedFile)
        {
            if (postedFile != null)
            {
                var allowedExtensions = new[] { ".xls", ".xlsx" };
                string fileName = Path.GetFileName(postedFile.FileName);
                var checkextension = Path.GetExtension(fileName).ToLower();

                if (allowedExtensions.Contains(checkextension))
                {
                    List<Rmmodel> rm = new List<Rmmodel>();
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
                            if (excelDataReader.FieldCount == 10)
                            {

                                var conf = new ExcelDataSetConfiguration()
                                {
                                    ConfigureDataTable = a => new ExcelDataTableConfiguration
                                    {
                                        UseHeaderRow = true
                                    }
                                };

                                DataSet dataSet = excelDataReader.AsDataSet(conf);
                                DataRowCollection row = dataSet.Tables["Rm"].Rows;
                                List<object> rowDataList = null;

                                foreach (DataRow item in row)
                                {
                                    rowDataList = item.ItemArray.ToList();
                                    rm.Add(new Rmmodel
                                    {
                                        etd = Convert.ToDateTime(rowDataList[0]),
                                        eta = Convert.ToDateTime(rowDataList[1]),
                                        container = Convert.ToString(rowDataList[2]),
                                        reff = Convert.ToString(rowDataList[3]),
                                        landing_site = Convert.ToString(rowDataList[4]),
                                        sap_code = Convert.ToString(rowDataList[5]),
                                        cases = Convert.ToInt32(rowDataList[6]),
                                        qty_pl = Convert.ToSingle(rowDataList[7]),
                                        usd_price = Convert.ToSingle(rowDataList[8]),
                                        ex_rate = Convert.ToSingle(rowDataList[9]),
                                        status = "otw"
                                    });
                                }
                                _db.Rm.AddRange(rm);
                                _db.SaveChanges();
                                TempData["msg"] = "File Succesfully Uploaded";
                                TempData["result"] = "success";
                                return RedirectToAction("List", new { status = "otw" });
                            }
                            TempData["msg"] = "Field Column not Match";
                            TempData["result"] = "failed";
                            return RedirectToAction("List", new { status = "otw" });
                        }
                    }
                }
                //jika tidak sesuai extension
                TempData["msg"] = "File Extension must excel file format 'xlsx or xls'";
                TempData["result"] = "failed";
                return RedirectToAction("List", new { status = "otw" });
            }
            TempData["msg"] = "File Empty";
            TempData["result"] = "failed";
            return RedirectToAction("List", new { status = "otw" });
        }


        //[HttpPost]
        //public IActionResult Import(IFormFile postedFile)
        //{
        //    if (postedFile != null)
        //    {
        //        List<Rmmodel> rm = new List<Rmmodel>();
        //        string path = Path.Combine(this.Environment.WebRootPath, "Uploads");
        //        string fileName = Path.GetFileName(postedFile.FileName);
        //        string filePath = Path.Combine(path, fileName);
        //        using (FileStream stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            postedFile.CopyTo(stream);
        //        }
        //        // For .net core, the next line requires the NuGet package, 
        //        //System.Text.Encoding.CodePages
        //        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        //        using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
        //        {
        //            using (var reader = ExcelReaderFactory.CreateReader(stream))
        //            {
        //                if (reader.FieldCount == 10)
        //                {
        //                    while (reader.Read()) //Each row of the file
        //                    {
        //                        rm.Add(new Rmmodel
        //                        {
        //                            etd = reader.GetDateTime(0),
        //                            eta = reader.GetDateTime(1),
        //                            container = reader.GetValue(2).ToString(),
        //                            reff = reader.GetValue(3).ToString(),
        //                            landing_site = reader.GetValue(4).ToString(),
        //                            sap_code = reader.GetValue(5).ToString(),
        //                            cases = Convert.ToInt32(reader.GetValue(6)),
        //                            qty_pl = Convert.ToSingle(reader.GetValue(7)),
        //                            usd_price = Convert.ToSingle(reader.GetValue(8)),
        //                            ex_rate = Convert.ToSingle(reader.GetValue(9)),
        //                            status = "otw"
        //                        });

        //                    }
        //                    var obj = rm;
        //                    _db.Rm.AddRange(rm);
        //                    _db.SaveChanges();
        //                    return RedirectToAction("List", new { status = "otw" });
        //                }
        //                return RedirectToAction("List", new { status = "otw" });
        //            }
        //        }
        //    }
        //    return RedirectToAction("List", new { status = "otw" });
        //}

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Delete(string reff,string status)
        {
            var datareff = _db.Rm.Where(x => x.reff == reff).ToList();
            _db.Rm.RemoveRange(datareff);
            _db.SaveChanges();
            TempData["msg"] = "Item Succesfully Deleted";
            TempData["result"] = "success";
            return RedirectToAction("List", new { status = status });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Updatedetail(Rmmodel obj)
        {
            _db.Rm.Update(obj);
            _db.SaveChanges();
            TempData["msg"] = "Item Succesfully Updated";
            TempData["result"] = "success";
            return RedirectToAction("Detail",new { reff = obj.reff });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Duplicateitem(Rmmodel obj)
        {
            if (obj.id_raw == 0)
            {
                _db.Rm.Add(obj);
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Duplicated";
                TempData["result"] = "success";
                return RedirectToAction("Detail", new { reff = obj.reff });
            }
            else
            {
                TempData["msg"] = "Item Failed Duplicated";
                TempData["result"] = "failed";
                return RedirectToAction("Detail", new { reff = obj.reff });
            }
        }

        public IActionResult Getdetailitem(int id)
        {
            var obj = _db.Rm.Find(id);
            return Json(obj);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Deleteitem(int id,string reff)
        {
            var obj = _db.Rm.Find(id);
            _db.Rm.Remove(obj);
            _db.SaveChanges();
            TempData["msg"] = "Item Succesfully Deleted";
            TempData["result"] = "success";
            return RedirectToAction("Detail", new { reff = reff });
        }
    }
}
