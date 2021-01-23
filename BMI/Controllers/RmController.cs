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

        public async Task<IActionResult> Index(string status)
        {
            if (status == "plant" || status == "otw" || status == "closed")
            {
                var list = _db.Rm
                    .Where(x => x.status == status)
                    .OrderByDescending(e => e.created_at)
                    .ToList();

                if (status == "plant")
                {
                    ViewBag.status = "Plant";
                }
                else if (status == "otw")
                {
                    ViewBag.status = "On The Water";
                }
                else
                {
                    ViewBag.status = "Closed";
                }
               
                return await Task.Run(()=> View(list));
            }
            return NotFound();
        }

        public async Task<IActionResult> Detail (string raw_source, string status)
        {
            var list = _db.Rm_detail
                .Where(x => x.raw_source == raw_source)
                .Include(x => x.Masterdatamodel)
                .OrderByDescending(e => e.id_raw)
                .ToList();
            ViewBag.raw_source = raw_source;
            ViewBag.status = status;
            ViewBag.qty_pl = list.Sum(a => a.qty_pl);
            ViewBag.qty_received = list.Sum(a => a.qty_received);
            return await Task.Run(()=> View(list));
        }

        public async Task<JsonResult> Getdata(string raw_source)
        {
            var obj = _db.Rm.Find(raw_source);
            return await Task.Run(()=> Json(obj));
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(RmModel rmModel)
        {
            if (ModelState.IsValid)
            {
                var obj = _db.Rm.Find(rmModel.raw_source);
                var list_rm_detail = _db.Rm_detail.Where(x => x.raw_source == rmModel.raw_source).ToList();
                if (rmModel.status == "Plant")
                {
                    obj.eta = rmModel.eta;
                    obj.etd = rmModel.etd;
                    obj.container = rmModel.container;
                    obj.updated_at = DateTime.Now;
                    obj.updated_by = User.Identity.Name;
                    obj.status = "Plant";

                    list_rm_detail.ForEach(a =>
                    {
                        a.qty_received = a.qty_pl;
                        a.landing_site_received = a.landing_site;
                        a.updated_at = DateTime.Now;
                        a.updated_by = User.Identity.Name;
                    });
                    _db.SaveChanges();

                }
                else if (rmModel.status == "On The Water")
                {
                    obj.eta = rmModel.eta;
                    obj.etd = rmModel.etd;
                    obj.container = rmModel.container;
                    obj.updated_at = DateTime.Now;
                    obj.created_by = User.Identity.Name;
                    obj.status = "Otw";
                }
                else
                {
                    obj.eta = rmModel.eta;
                    obj.etd = rmModel.etd;
                    obj.container = rmModel.container;
                    obj.updated_at = DateTime.Now;
                    obj.created_by = User.Identity.Name;
                    obj.status = "Closed";
                }
         
                _db.Entry(obj).State = EntityState.Modified;
                _db.SaveChanges();

                TempData["msg"] = "Item Successfully Updated";
                TempData["result"] = "success";
                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
            }
            TempData["msg"] = "Item Failed Updated";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Import(IFormFile postedFile)
        {
            if (postedFile != null)
            {
                var allowedExtensions = new[] { ".xls", ".xlsx" };
                string fileName = Path.GetFileName(postedFile.FileName);
                var checkextension = Path.GetExtension(fileName).ToLower();

                if (allowedExtensions.Contains(checkextension))
                {
                    List<RmDetailModel> rm_detail = new List<RmDetailModel>();
                    List<RmModel> rm = new List<RmModel>();
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
                                    var raw_source = Convert.ToString(rowDataList[3]);
                                    var unique = _db.Rm.FirstOrDefault(m => m.raw_source == raw_source);
                                    if (unique == null)
                                    {
                                        var obj = new RmModel
                                        {
                                            raw_source = Convert.ToString(rowDataList[3]),
                                            etd = Convert.ToDateTime(rowDataList[0]),
                                            eta = Convert.ToDateTime(rowDataList[1]),
                                            container = Convert.ToString(rowDataList[2]),
                                            created_at = DateTime.Now,
                                            created_by = User.Identity.Name,
                                            status = "otw"
                                        };
                                        _db.Rm.Add(obj);
                                        _db.SaveChanges();
                                    }
                                }


                                foreach (DataRow item in row)
                                {
                                    rowDataList = item.ItemArray.ToList();
                                    rm_detail.Add(new RmDetailModel
                                    {
                                        raw_source = Convert.ToString(rowDataList[3]),
                                        landing_site = Convert.ToString(rowDataList[4]),
                                        sap_code = Convert.ToString(rowDataList[5]),
                                        cases = Convert.ToInt32(rowDataList[6]),
                                        qty_pl = Convert.ToSingle(rowDataList[7]),
                                        usd_price = Convert.ToSingle(rowDataList[8]),
                                        ex_rate = Convert.ToSingle(rowDataList[9]),
                                        created_at = DateTime.Now,
                                        created_by = User.Identity.Name
                                    });
                                }
                                stream.Close();
                                System.IO.File.Delete(filePath);
                                _db.Rm_detail.AddRange(rm_detail);
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

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(string raw_source,string status)
        {
            var rm_detail = _db.Rm_detail.Where(x => x.raw_source == raw_source).ToList();
            _db.Rm_detail.RemoveRange(rm_detail);
            var rm = _db.Rm.Find(raw_source);
            _db.Rm.Remove(rm);
            _db.SaveChanges();
            TempData["msg"] = "Item Succesfully Deleted";
            TempData["result"] = "success";
            return await Task.Run(()=> RedirectToAction("List", new { status = status }));
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Updatedetail(RmDetailModel rmDetailModel)
        {
            if (ModelState.IsValid)
            {
                var obj = _db.Rm_detail.Find(rmDetailModel.id_raw);
                obj.sap_code = rmDetailModel.sap_code;
                obj.usd_price = rmDetailModel.usd_price;
                obj.ex_rate = rmDetailModel.ex_rate;
                obj.landing_site = rmDetailModel.landing_site;
                obj.qty_pl = rmDetailModel.qty_pl;
                obj.landing_site_received = rmDetailModel.landing_site_received;
                obj.qty_received = rmDetailModel.qty_received;

                _db.Entry(obj).State = EntityState.Modified;
                _db.SaveChanges();
                TempData["msg"] = "Item Successfully Updated";
                TempData["result"] = "success";
                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
            }
            TempData["msg"] = "Item Failed to Update";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }


        public async Task<JsonResult> Getdetailitem(int id)
        {
            var obj = _db.Rm_detail.Find(id);
            return await Task.Run(()=> Json(obj));
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Deleteitem(int id,string raw_source)
        {
            var obj = _db.Rm_detail.Find(id);
            _db.Rm_detail.Remove(obj);
            _db.SaveChanges();
            TempData["msg"] = "Item Succesfully Deleted";
            TempData["result"] = "success";
            return await Task.Run(() => RedirectToAction("Detail", new { raw_source = obj.raw_source }));
        }


        public async Task< IActionResult> Adddestroy(AdjustmentRawModel adjustmentRawModel)
        {
            if (ModelState.IsValid)
            {
                _db.AdjustmentRaw.Add(adjustmentRawModel);
                _db.SaveChanges();
                TempData["msg"] = "Raw Material Succesfully Destroy";
                TempData["result"] = "success";
                return await Task.Run(()=> Redirect(Request.Headers["Referer"].ToString())) ;
            }
            TempData["msg"] = "Raw Material Failed to Destroy";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }



    }
}
