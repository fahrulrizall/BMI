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
using Microsoft.AspNetCore.Authorization;

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


        public async Task< IActionResult> Index(string plant, string status)
        {
            var list = _db.PO.Where(a=>a.plant == plant && a.po_status == status)
                .OrderByDescending(e => e.shipment_no)
                .ToList();
            ViewBag.status = status;
            return await Task.Run(()=> View(list)) ;
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize(Roles = "CC,Logistic,Admin")]
        public async Task< IActionResult> Update(POModel POModel)
        {
            if (ModelState.IsValid)
            {
                var po = _db.PO.Find(POModel.po);

                po.shipment_no = POModel.shipment_no;
                po.etd = POModel.etd;
                po.eta = POModel.eta;
                po.document_date = POModel.document_date;
                po.ocean_carrier = POModel.ocean_carrier;
                po.container = POModel.container;
                po.voyage_no = POModel.voyage_no;
                po.house_bol = POModel.house_bol;
                po.vessel_name = POModel.vessel_name;
                po.inv_no = POModel.inv_no;
                po.fda_no = POModel.fda_no;
                po.seal_no = POModel.seal_no;
                po.destination = POModel.destination;
                po.port_loading = "Surabaya";
                po.updated_at = DateTime.Now;
                po.po_status = POModel.po_status;
                _db.Entry(po).State = EntityState.Modified;
                _db.SaveChanges();

                TempData["msg"] = "Shipment Succesfully Updated";
                TempData["result"] = "success";
                return await Task.Run(()=> Redirect(Request.Headers["Referer"].ToString()));
            }
            TempData["msg"] = "Shipment Failed to Updated";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize(Roles = "CC,Logistic,Admin")]
        public async Task< IActionResult> Delete(string po, string code, string batch)
        {
            var obj = _db.Shipment
                .Where(a => a.po == po && a.bmi_code == code && a.batch == batch)
                .ToList();
            if (obj != null) {
                _db.Shipment.RemoveRange(obj);
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Deleted";
                TempData["result"] = "success";
                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
            }
            TempData["msg"] = "Item Failed to Delete";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));

        }

        public async Task<JsonResult> Getshipment(string po)
        {
            var obj = _db.PO.Find(po);
            return await Task.Run(()=> Json(obj));
        }

        public async Task< IActionResult> Detail(string po)
        {
            var obj = _db.Shipment
                .Where(a => a.po == po)
                .Include(a => a.MasterBMIModel)
                .Include(a => a.POModelBatch)
                .AsEnumerable()
                .GroupBy(a=> new { a.bmi_code,a.batch})
                .Select(a=> new ShipmentModel 
                {
                    bmi_code = a.Key.bmi_code,
                    batch = a.Key.batch,
                    qty = a.Sum(b=>b.qty),
                    MasterBMIModel = a.Max(b=>b.MasterBMIModel),
                    POModelBatch = a.Max(b=>b.POModelBatch)
                })
                .OrderByDescending(a=>a.bmi_code)
                .ToList();
            var total_cases = obj.Sum(a => a.qty);
            if (obj != null)
            {
                ViewBag.po = po;
                ViewBag.totalcase = total_cases;
                return await Task.Run(() => View(obj));
            }
            return await Task.Run(() => View("NotFound"));
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize(Roles = "CC,Logistic,Admin")]
        public async Task<IActionResult> Import(IFormFile postedFile, string po)
        {
            if (postedFile != null)
            {
                var allowedExtensions = new[] { ".xls", ".xlsx" };
                string fileName = Path.GetFileName(postedFile.FileName);
                var checkextension = Path.GetExtension(fileName).ToLower();

                if (allowedExtensions.Contains(checkextension))
                {
                    List<ShipmentModel> shipmentdata = new List<ShipmentModel>();
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
                            if (excelDataReader.FieldCount == 6)
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
                                    var po_batch = Convert.ToString(_db.PO.Where(a => a.batch == Convert.ToString(rowDataList[2])).Select(a => a.po).First());
                                    shipmentdata.Add(new ShipmentModel
                                    {
                                        po = po,
                                        pdc = Convert.ToDateTime(rowDataList[4]),
                                        bmi_code = Convert.ToString(rowDataList[0]),
                                        batch = po_batch,
                                        qty = Convert.ToInt32(rowDataList[3]),
                                        raw_source = Convert.ToString(rowDataList[5]),
                                        created_at = DateTime.Now,
                                        created_by = User.Identity.Name,

                                    });
                                }
                                _db.Shipment.AddRange(shipmentdata);
                                _db.SaveChanges();
                                stream.Close();
                                System.IO.File.Delete(filePath);
                                TempData["msg"] = "File Succesfully Uploaded";
                                TempData["result"] = "success";
                                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
                            }
                            //jika kolom lebih besar dari 4
                            stream.Close();
                            System.IO.File.Delete(filePath);
                            TempData["msg"] = "Field Column not Match";
                            TempData["result"] = "failed";
                            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
                        }
                    }
                }
                //jika tidak sesuai extension
                TempData["msg"] = "Field Extension must excel file format 'xlsx or xls'";
                TempData["result"] = "failed";
                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));

            }
            //jika file kosong
            TempData["msg"] = "File Empty";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }

        [Authorize(Roles = "CC,Logistic,Admin")]
        public async Task<IActionResult> Updateitem(ShipmentModel shipmentModel)
        {
            if (ModelState.IsValid)
            {
                var shipment = _db.Shipment.Find(shipmentModel.id_shipment);
                shipment.batch = shipment.batch;
                shipment.updated_at = DateTime.Now;
                _db.Entry(shipment).State = EntityState.Modified;
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Updated";
                TempData["result"] = "success";
                return await Task.Run (()=> Redirect(Request.Headers["Referer"].ToString()));
            }
            TempData["msg"] = "Item Failed to Update";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }

        public async Task<JsonResult> Getpdc(string batch, string po, string code )
        {
            var obj = _db.Shipment
                .Where(a => a.po == po && a.bmi_code == code && a.batch == batch)
                .Select(a => new { a.pdc,a.raw_source,a.qty}).OrderByDescending(a=>a.pdc)
                .ToList();
            return await Task.Run(() => Json(obj));
        }







    }
}
