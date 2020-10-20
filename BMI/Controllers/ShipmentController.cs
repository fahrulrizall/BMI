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

namespace BMI.Controllers
{
    public class ShipmentController : Controller
    {
        private readonly ApplicationDbContext _db;
        private IWebHostEnvironment Environment;
        private IConfiguration Configuration;

        public ShipmentController(ApplicationDbContext db, IWebHostEnvironment _environment, IConfiguration _configuration)
        {
            _db = db;
            Environment = _environment;
            Configuration = _configuration;
        }


        public IActionResult Index()
        {
            var list = _db.Shipment.OrderByDescending(e=>e.id_ship).ToList();
            return View(list);
        }

        [HttpPost]
        public IActionResult IdExist(Shipmentmodel shipmentmodel)
        {
            var unique = _db.Shipment.SingleOrDefault(m => m.id_ship == shipmentmodel.id_ship);
            if (unique != null)
            {
                return Json(false);
            }
            return Json(true);
        }

        [HttpPost]
        public IActionResult POExist(Shipmentmodel shipmentmodel)
        {
            var unique = _db.Shipment.SingleOrDefault(m => m.po == shipmentmodel.po);
            if (unique != null)
            {
                return Json(false);
            }
            return Json(true);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Shipmentmodel shipmentmodel)
        {
            if (ModelState.IsValid)
            {
                _db.Shipment.Add(shipmentmodel);
                _db.SaveChanges();
                TempData["msg"] = "New Shipment Succesfully Added";
                TempData["result"] = "success";
                return RedirectToAction("Index");
            }
            TempData["msg"] = "New Shipment Failed to Added";
            TempData["result"] = "failed";
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(Shipmentmodel shipmentmodel)
        {
            if (ModelState.IsValid)
            {
                _db.Shipment.Update(shipmentmodel);
                _db.SaveChanges();
                TempData["msg"] = "Shipment Succesfully Updated";
                TempData["result"] = "success";
                return RedirectToAction("Index");
            }
            TempData["msg"] = "Shipment Failed to Updated";
            TempData["result"] = "failed";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Delete(int id)
        {
            var obj = _db.Shipment.Find(id);
            _db.Shipment.Remove(obj);
            _db.SaveChanges();
            TempData["msg"] = "Shipment Succesfully Deleted";
            TempData["result"] = "success";
            return RedirectToAction("Index");
        }

        public IActionResult Getshipment(int id)
        {
            var obj = _db.Shipment.Find(id);
            return Json(obj);
        }

        public IActionResult Detail(int id)
        {
            ViewBag.no = id;
            var obj = _db.Shipment_detail
                .Where(a => a.id_ship == id)
                .Include(c => c.MasterBMIModel)
                .ToList();
            return View(obj);
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Import(IFormFile postedFile,int id)
        {
            if (postedFile != null)
            {
                var allowedExtensions = new[] { ".xls", ".xlsx"};
                string fileName = Path.GetFileName(postedFile.FileName);
                var checkextension = Path.GetExtension(fileName).ToLower();

                if (allowedExtensions.Contains(checkextension))
                {
                    List<ShipmentDetailModel> shipmentdata = new List<ShipmentDetailModel>();
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
                            if (excelDataReader.FieldCount == 4)
                            {

                                var conf = new ExcelDataSetConfiguration()
                                {
                                    ConfigureDataTable = a => new ExcelDataTableConfiguration
                                    {
                                        UseHeaderRow = true
                                    }
                                };

                                DataSet dataSet = excelDataReader.AsDataSet(conf);
                                DataRowCollection row = dataSet.Tables["Shipment"].Rows;
                                List<object> rowDataList = null;

                                foreach (DataRow item in row)
                                {
                                    rowDataList = item.ItemArray.ToList();
                                    shipmentdata.Add(new ShipmentDetailModel
                                    {
                                        id_ship = id,
                                        bmi_code = Convert.ToString(rowDataList[0]),
                                        batch = Convert.ToString(rowDataList[1]),
                                        qty = Convert.ToInt32(rowDataList[2]),
                                        index = Convert.ToSingle(rowDataList[3])
                                    });
                                }
                                _db.Shipment_detail.AddRange(shipmentdata);
                                _db.SaveChanges();
                                TempData["msg"] = "File Succesfully Uploaded";
                                TempData["result"] = "success";
                                return RedirectToAction("Detail", "Shipment", new { id = id });
                            }
                            //jika kolom lebih besar dari 4
                            TempData["msg"] = "Field Column not Match";
                            TempData["result"] = "failed";
                            return RedirectToAction("Detail", "Shipment", new { id = id });
                        }
                    }
                }
                //jika tidak sesuai extension
                TempData["msg"] = "Field Extension must excel file format 'xlsx or xls'";
                TempData["result"] = "failed";
                return RedirectToAction("Detail", "Shipment", new { id = id });

            }
            //jika file kosong
            TempData["msg"] = "File Empty";
            TempData["result"] = "failed";
            return RedirectToAction("Detail", "Shipment", new { id = id });
        }















    }
}
