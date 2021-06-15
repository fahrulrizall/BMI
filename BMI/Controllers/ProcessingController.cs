using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Data;
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
using System.Text;
using Microsoft.AspNetCore.Authorization;
using BMI.UtilityModels;

namespace BMI.Controllers
{
    public class ProcessingController : Controller
    {
        private readonly ApplicationDbContext _db;

        private IWebHostEnvironment Environment;
        private IConfiguration Configuration;

        public ProcessingController(ApplicationDbContext db, IWebHostEnvironment _environment, IConfiguration _configuration)
        {
            _db = db;
            Environment = _environment;
            Configuration = _configuration;
        }

        public IActionResult Index(int line)
        {
            var result = _db.SAP_PO.AsEnumerable().GroupBy(a => a.refference)
                .Select(a => new RefferenceView
                {
                    Refference = a.Key,
                    Input = Math.Round(_db.QtyLine1Input.Where(b => b.refference == a.Key).Sum(b => b.qty), 2),
                    Output = Math.Round(_db.QtyLine1Output.Where(b => b.refference == a.Key).Sum(b => b.qty), 2),
                    Yield = Math.Round(_db.QtyLine1Output.Where(b => b.refference == a.Key).Sum(b => b.qty) / _db.QtyLine1Input.Where(b => b.refference == a.Key).Sum(b => b.qty), 2)
                })
                .ToList();
            ViewBag.line = line;
            return View("Line1/Index", result);
        }

        public async Task<IActionResult> Refference(string reff)
        {
            var result = _db.Date_vessel.Where(a => a.refference == reff)
                .GroupBy(a => a.id)
                .Select(a => new RefferenceView
                {
                    Id = a.Key,
                    Refference = a.Max(b => b.refference),
                    Date = a.Max(b => b.date),
                    Vessel = a.Max(b => b.vessel),
                    Input = Math.Round(_db.QtyLine1Input.Where(b => b.id_dateVessel == a.Key).Sum(b => b.qty), 2),
                    Output = Math.Round(_db.QtyLine1Output.Where(b => b.id_dateVessel == a.Key).Sum(b => b.qty), 2),
                    Yield = Math.Round((_db.QtyLine1Output.Where(b => b.id_dateVessel == a.Key).Sum(b => b.qty) / _db.QtyLine1Input.Where(b => b.id_dateVessel == a.Key).Sum(b => b.qty)) * 100, 2)
                })
                .ToList();
            ViewBag.Refference = reff;
            return await Task.Run(() => View("Line1/Refference", result));
        }

        public async Task<IActionResult> GetVessel(string reff)
        {
            var result = _db.SAP_PO_Detail
                .Where(a => a.refference == reff)
                .GroupBy(a => a.vessel)
                .Select(a => a.Key).ToList();
            return await Task.Run(() => Json(result));
        }

        public async Task<IActionResult> CreateDateVessel(DateVesselModel dateVessel)
        {
            _db.Date_vessel.Add(dateVessel);
            _db.SaveChanges();
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }

        public async Task<IActionResult> UpdateDateVessel(DateVesselModel dateVesselModel)
        {
            if (dateVesselModel == null)
            {
                _db.Date_vessel.Update(dateVesselModel);
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Updated";
                TempData["result"] = "success";
                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
            }
            TempData["msg"] = "Item Not Caontain Id";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }

        public async Task<IActionResult> DeleteDateVessel(string id)
        {
            if (id != null)
            {
                var result = _db.Date_vessel.Find(id);
                _db.Date_vessel.Remove(result);
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Deleted";
                TempData["result"] = "success";
                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
            }
            TempData["msg"] = $"Cant Remove Data with id {id}";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }

        public async Task<IActionResult> Import(IFormFile postedFile, int id, string reff)
        {
            if (postedFile != null)
            {
                var allowedExtensions = new[] { ".xls", ".xlsx" };
                string fileName = Path.GetFileName(postedFile.FileName);
                var checkextension = Path.GetExtension(fileName).ToLower();

                if (allowedExtensions.Contains(checkextension))
                {
                    List<QtyLine1Input> Qty = new List<QtyLine1Input>();
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
                            if (excelDataReader.FieldCount == 1)
                            {

                                var conf = new ExcelDataSetConfiguration()
                                {
                                    ConfigureDataTable = a => new ExcelDataTableConfiguration
                                    {
                                        UseHeaderRow = true
                                    }
                                };

                                DataSet dataSet = excelDataReader.AsDataSet(conf);
                                DataRowCollection row = dataSet.Tables["Qty"].Rows;
                                List<object> rowDataList = null;

                                foreach (DataRow item in row)
                                {
                                    rowDataList = item.ItemArray.ToList();

                                    var obj = new QtyLine1Input
                                    {
                                        id_dateVessel = id,
                                        qty = Convert.ToSingle(rowDataList[0]),
                                        refference = reff

                                    };
                                    _db.QtyLine1Input.Add(obj);
                                    _db.SaveChanges();

                                }

                                stream.Close();
                                System.IO.File.Delete(filePath);
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




        public async Task<IActionResult> Detail(int id, string type)
        {

            var newid = _db.Date_vessel.Find(id);
            ViewBag.Date = newid.date.Year + "-" + newid.date.Month + "-" + newid.date.Day;
            ViewBag.Vessel = newid.vessel;
            ViewBag.Id = id;
            ViewBag.reff = newid.refference;
            if (type == "input")
            {
                var input = new RangeView()
                {
                    //A = new ValueRange() { range = "A", sum = _db.QtyLine1Input.Where(a => a.Id_DateVessel == id && a.Qty <= 10).Sum(a => a.Qty), count = _db.QtyLine1Input.Where(a => a.Id_DateVessel == id && a.Qty <= 10).Count() },
                    //B = new ValueRange() { range = "B", sum = _db.QtyLine1Input.Where(a => a.Id_DateVessel == id && a.Qty > 10 && a.Qty <= 15).Sum(a => a.Qty), count = _db.QtyLine1Input.Where(a => a.Id_DateVessel == id && a.Qty > 10 && a.Qty <= 15).Count() },
                    //C = new ValueRange() { range = "C", sum = _db.QtyLine1Input.Where(a => a.Id_DateVessel == id && a.Qty > 15 && a.Qty <= 20).Sum(a => a.Qty), count = _db.QtyLine1Input.Where(a => a.Id_DateVessel == id && a.Qty > 15 && a.Qty <= 20).Count() },
                    //D = new ValueRange() { range = "D", sum = _db.QtyLine1Input.Where(a => a.Id_DateVessel == id && a.Qty > 20 && a.Qty <= 25).Sum(a => a.Qty), count = _db.QtyLine1Input.Where(a => a.Id_DateVessel == id && a.Qty > 20 && a.Qty <= 25).Count() },
                    //E = new ValueRange() { range = "E", sum = _db.QtyLine1Input.Where(a => a.Id_DateVessel == id && a.Qty > 25 && a.Qty <= 35).Sum(a => a.Qty), count = _db.QtyLine1Input.Where(a => a.Id_DateVessel == id && a.Qty > 25 && a.Qty <= 35).Count() },
                    //F = new ValueRange() { range = "F", sum = _db.QtyLine1Input.Where(a => a.Id_DateVessel == id && a.Qty > 35 && a.Qty <= 40).Sum(a => a.Qty), count = _db.QtyLine1Input.Where(a => a.Id_DateVessel == id && a.Qty > 35 && a.Qty <= 40).Count() },
                    //G = new ValueRange() { range = "G", sum = _db.QtyLine1Input.Where(a => a.Id_DateVessel == id && a.Qty > 40 && a.Qty <= 45).Sum(a => a.Qty), count = _db.QtyLine1Input.Where(a => a.Id_DateVessel == id && a.Qty > 40 && a.Qty <= 45).Count() },
                    //H = new ValueRange() { range = "H", sum = _db.QtyLine1Input.Where(a => a.Id_DateVessel == id && a.Qty > 45 && a.Qty <= 50).Sum(a => a.Qty), count = _db.QtyLine1Input.Where(a => a.Id_DateVessel == id && a.Qty > 45 && a.Qty <= 50).Count() },
                    //I = new ValueRange() { range = "I", sum = _db.QtyLine1Input.Where(a => a.Id_DateVessel == id && a.Qty > 50 && a.Qty <= 55).Sum(a => a.Qty), count = _db.QtyLine1Input.Where(a => a.Id_DateVessel == id && a.Qty > 50 && a.Qty <= 55).Count() },
                    //J = new ValueRange() { range = "J", sum = _db.QtyLine1Input.Where(a => a.Id_DateVessel == id && a.Qty > 55 && a.Qty <= 60).Sum(a => a.Qty), count = _db.QtyLine1Input.Where(a => a.Id_DateVessel == id && a.Qty > 55 && a.Qty <= 60).Count() },
                    //K = new ValueRange() { range = "K", sum = _db.QtyLine1Input.Where(a => a.Id_DateVessel == id && a.Qty > 60).Sum(a => a.Qty), count = _db.QtyLine1Input.Where(a => a.Id_DateVessel == id && a.Qty > 60).Count() }
                    A = new ValueRange() { range = "A", sum = _db.QtyLine1Input.Where(a => a.id_dateVessel == id && a.qty <= 10).Sum(a => a.qty), count = _db.QtyLine1Input.Where(a => a.id_dateVessel == id && a.qty <= 10).Count() },
                    B = new ValueRange() { range = "B", sum = _db.QtyLine1Input.Where(a => a.id_dateVessel == id && a.qty > 10 && a.qty <= 15).Sum(a => a.qty), count = _db.QtyLine1Input.Where(a => a.id_dateVessel == id && a.qty > 10 && a.qty <= 15).Count() },
                    C = new ValueRange() { range = "C", sum = _db.QtyLine1Input.Where(a => a.id_dateVessel == id && a.qty > 15).Sum(a => a.qty), count = _db.QtyLine1Input.Where(a => a.id_dateVessel == id && a.qty > 15).Count() },
                };

                return await Task.Run(() => View("Line1/InputDetail", input));
            }

            var output = _db.QtyLine1Output
                .Include(a => a.Masterdatamodel)
                .Where(a => a.id_dateVessel == id)
                .ToList();
            return await Task.Run(() => View("Line1/OutputDetail", output));

        }

        public async Task<IActionResult> AddItem(QtyLine1Output qtyLine1Output)
        {
            if (qtyLine1Output != null)
            {
                _db.QtyLine1Output.Add(qtyLine1Output);
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Added";
                TempData["result"] = "success";
                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
            }
            TempData["msg"] = "Item Failed to Add";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }

        public async Task<IActionResult> UpdateItem(QtyLine1Output qtyLine1Output)
        {
            if (qtyLine1Output == null)
            {
                _db.QtyLine1Output.Update(qtyLine1Output);
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Updated";
                TempData["result"] = "success";
                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
            }
            TempData["msg"] = "Item Failed to Update";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }

        public async Task<JsonResult> GetDetailOutput(string id)
        {
            var result = _db.QtyLine1Output.Find(id);
            return await Task.Run(() => Json(result));
        }

        public async Task<IActionResult> DeleteItem(string id)
        {
            if (id != null)
            {
                var result = _db.QtyLine1Output.Find(id);
                _db.QtyLine1Output.Remove(result);
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Added";
                TempData["result"] = "success";
                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
            }
            TempData["msg"] = "Item Failed to Delete";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }



    }
}