using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BMI.Data;
using Microsoft.EntityFrameworkCore;
using BMI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using ExcelDataReader;
using System.Data;

namespace BMI.Controllers
{
    public class SAP_POController : Controller
    {
        private readonly ApplicationDbContext _db;
        private IWebHostEnvironment Environment;
        private IConfiguration Configuration;

        public SAP_POController(ApplicationDbContext db, IWebHostEnvironment _environment, IConfiguration _configuration)
        {
            _db = db;
            Environment = _environment;
            Configuration = _configuration;
        }

        public async Task<IActionResult> Index(string status, string plant)
        {
            var obj = _db.SAP_PO.Include(a => a.VendorModel).Where(a => a.status == status && a.plant == plant).ToList();
            if (status == "Plant")
            {
                ViewBag.status = "Plant";
            }
            else if (status == "Otw")
            {
                ViewBag.status = "On The Water";
            }
            else
            {
                ViewBag.status = "Closed";
            }
            ViewBag.plant = plant;
            
            return await Task.Run(() => View(obj));
        }

        public async Task<IActionResult> Detail(string refference, string status)
        {
            var obj = _db.SAP_PO_Detail
                .Where(a => a.refference == refference)
                .Select(a => new SAP_PODetailModel
                {
                    id = a.id,
                    sap_code = a.sap_code,
                    Masterdatamodel = a.Masterdatamodel,
                    style = a.style,
                    vessel = a.vessel,
                    unit_price = a.unit_price,
                    qty_pl = a.qty_pl,
                    amount_pl = Math.Round(Convert.ToDouble(a.unit_price * a.qty_pl), 2),
                    qty_received = a.qty_received,
                    amount_received = Math.Round(Convert.ToDouble(a.unit_price * a.qty_received), 2),
                }).ToList();
            ViewBag.qty_pl = obj.Sum(a => a.qty_pl);
            ViewBag.amount_pl = obj.Sum(a => a.amount_pl);
            ViewBag.qty_received = obj.Sum(a => a.qty_received);
            ViewBag.amount_received = obj.Sum(a => a.amount_received);
            ViewBag.refference = refference;
            ViewBag.status = status;
            return await Task.Run(() => View(obj));
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize(Roles = "PF,Admin")]
        public async Task<IActionResult> Import(IFormFile postedFile, string plant)
        {
            if (postedFile != null)
            {
                var allowedExtensions = new[] { ".xls", ".xlsx" };
                string fileName = Path.GetFileName(postedFile.FileName);
                var checkextension = Path.GetExtension(fileName).ToLower();

                if (allowedExtensions.Contains(checkextension))
                {
                    List<SAP_PODetailModel> rm_detail = new List<SAP_PODetailModel>();
                    List<SAP_POModel> rm = new List<SAP_POModel>();
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
                            if (excelDataReader.FieldCount == 21)
                            {

                                var conf = new ExcelDataSetConfiguration()
                                {
                                    ConfigureDataTable = a => new ExcelDataTableConfiguration
                                    {
                                        UseHeaderRow = true
                                    }
                                };

                                DataSet dataSet = excelDataReader.AsDataSet(conf);
                                DataRowCollection row = dataSet.Tables["RM"].Rows;
                                List<object> rowDataList = null;

                                foreach (DataRow item in row)
                                {
                                    rowDataList = item.ItemArray.ToList();
                                    var refference = Convert.ToString(rowDataList[0]);
                                    var unique = _db.SAP_PO.FirstOrDefault(m => m.refference == refference);
                                    if (unique == null)
                                    {
                                        var obj = new SAP_POModel
                                        {
                                            plant = plant,
                                            refference = Convert.ToString(rowDataList[0]),
                                            vendor = Convert.ToString(rowDataList[1]),
                                            sap_po = Convert.ToString(rowDataList[3]),
                                            //pgi = Convert.ToString(rowDataList[4]),
                                            //pgr = Convert.ToString(rowDataList[5]),
                                            //return_no = Convert.ToString(rowDataList[6]),
                                            //delivery_date = Convert.ToDateTime(rowDataList[11]),
                                            etd = Convert.ToDateTime(rowDataList[12]),
                                            eta = Convert.ToDateTime(rowDataList[13]),
                                            invoice = Convert.ToString(rowDataList[14]),
                                            container = Convert.ToString(rowDataList[15]),
                                            bl_no = Convert.ToString(rowDataList[16]),
                                            shipping_line = Convert.ToString(rowDataList[17]),
                                            loading_port = Convert.ToString(rowDataList[18]),
                                            destination = Convert.ToString(rowDataList[19]),
                                            //pgr_date = Convert.ToDateTime(rowDataList[20]),
                                            status = "Otw"
                                        };
                                        _db.SAP_PO.Add(obj);
                                        _db.SaveChanges();
                                    }
                                }


                                foreach (DataRow item in row)
                                {
                                    rowDataList = item.ItemArray.ToList();
                                    rm_detail.Add(new SAP_PODetailModel
                                    {
                                        refference = Convert.ToString(rowDataList[0]),
                                        style = Convert.ToString(rowDataList[2]),
                                        sap_code = Convert.ToString(rowDataList[7]),
                                        unit_price = Convert.ToSingle(rowDataList[8]),
                                        qty_pl = Convert.ToSingle(rowDataList[9]),
                                        vessel = Convert.ToString(rowDataList[10]),
                                    }); ;
                                }
                                _db.SAP_PO_Detail.AddRange(rm_detail);
                                _db.SaveChanges();
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

        public async Task<IActionResult> GetData(string refference)
        {
            var obj = _db.SAP_PO.Find(refference);
            return await Task.Run(() => Json(obj));
        }

        public async Task<IActionResult> Delete(string refference)
        {
            var obj = _db.SAP_PO.Find(refference);
            if (obj == null)
            {
                TempData["msg"] = $"Refference {refference} not available";
                TempData["result"] = "failed";
                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
            }

            var mowi_detail = _db.SAP_PO_Detail.Where(a => a.refference == refference).ToList();
            _db.SAP_PO_Detail.RemoveRange(mowi_detail);
            _db.SAP_PO.Remove(obj);
            _db.SaveChanges();
            TempData["msg"] = $"Reffrerence {refference} Succesfully Deleted";
            TempData["result"] = "success";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }

        public async Task<IActionResult> Update(SAP_POModel RMMOWIModel)
        {
            if (ModelState.IsValid)
            {
                var obj = _db.SAP_PO.Find(RMMOWIModel.refference);
                var list_rm_detail = _db.SAP_PO_Detail.Where(x => x.refference == RMMOWIModel.refference).ToList();

                if (RMMOWIModel.status == "Plant")
                {
                    // edit bagian RM
                    obj.vendor = RMMOWIModel.vendor;
                    obj.sap_po = RMMOWIModel.sap_po;
                    obj.pgi = RMMOWIModel.pgi;
                    obj.pgr = RMMOWIModel.pgr;
                    obj.return_no = RMMOWIModel.return_no;
                    obj.plant = RMMOWIModel.plant;
                    obj.delivery_date = RMMOWIModel.delivery_date;
                    obj.etd = RMMOWIModel.etd;
                    obj.eta = RMMOWIModel.eta;
                    obj.invoice = RMMOWIModel.invoice;
                    obj.container = RMMOWIModel.container;
                    obj.bl_no = RMMOWIModel.bl_no;
                    obj.shipping_line = RMMOWIModel.shipping_line;
                    obj.loading_port = RMMOWIModel.loading_port;
                    obj.destination = RMMOWIModel.destination;
                    obj.pgr_date = RMMOWIModel.pgr_date;
                    obj.status = "Plant";

                    // edit RM_DETAIL
                    list_rm_detail.ForEach(a =>
                    {
                        a.qty_received = a.qty_pl;
                    });
                    _db.SaveChanges();

                }
                else if (RMMOWIModel.status == "On The Water")
                {
                    obj.vendor = RMMOWIModel.vendor;
                    obj.sap_po = RMMOWIModel.sap_po;
                    obj.pgi = RMMOWIModel.pgi;
                    obj.pgr = RMMOWIModel.pgr;
                    obj.return_no = RMMOWIModel.return_no;
                    obj.plant = RMMOWIModel.plant;
                    obj.delivery_date = RMMOWIModel.delivery_date;
                    obj.etd = RMMOWIModel.etd;
                    obj.eta = RMMOWIModel.eta;
                    obj.invoice = RMMOWIModel.invoice;
                    obj.container = RMMOWIModel.container;
                    obj.bl_no = RMMOWIModel.bl_no;
                    obj.shipping_line = RMMOWIModel.shipping_line;
                    obj.loading_port = RMMOWIModel.loading_port;
                    obj.destination = RMMOWIModel.destination;
                    obj.pgr_date = RMMOWIModel.pgr_date;
                    obj.status = "Otw";
                }
                else
                {
                    obj.vendor = RMMOWIModel.vendor;
                    obj.sap_po = RMMOWIModel.sap_po;
                    obj.pgi = RMMOWIModel.pgi;
                    obj.pgr = RMMOWIModel.pgr;
                    obj.return_no = RMMOWIModel.return_no;
                    obj.plant = RMMOWIModel.plant;
                    obj.delivery_date = RMMOWIModel.delivery_date;
                    obj.etd = RMMOWIModel.etd;
                    obj.eta = RMMOWIModel.eta;
                    obj.invoice = RMMOWIModel.invoice;
                    obj.container = RMMOWIModel.container;
                    obj.bl_no = RMMOWIModel.bl_no;
                    obj.shipping_line = RMMOWIModel.shipping_line;
                    obj.loading_port = RMMOWIModel.loading_port;
                    obj.destination = RMMOWIModel.destination;
                    obj.pgr_date = RMMOWIModel.pgr_date;
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

        public async Task<IActionResult> GetDetail(string id)
        {
            var result = _db.SAP_PO_Detail.Find(id);
            return await Task.Run(() => Json(result));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDetail(SAP_PODetailModel RMMOWIDetailModel)
        {
            var result = _db.SAP_PO_Detail.Find(RMMOWIDetailModel.id);
            if (result == null)
            {
                TempData["msg"] = "Id Not Found";
                TempData["result"] = "warning";
                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
            }

            result.sap_code = RMMOWIDetailModel.sap_code;
            result.style = RMMOWIDetailModel.style;
            result.vessel = RMMOWIDetailModel.vessel;
            result.unit_price = RMMOWIDetailModel.unit_price;
            result.qty_pl = RMMOWIDetailModel.qty_pl;
            result.qty_received = RMMOWIDetailModel.qty_received;
            _db.SaveChanges();

            TempData["msg"] = "Item Successfully Updated";
            TempData["result"] = "success";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }

        public async Task<IActionResult> DeleteItemDetail(string id)
        {
            var result = _db.SAP_PO_Detail.Find(id);
            if (result == null)
            {
                TempData["msg"] = "Id Not Found";
                TempData["result"] = "warning";
                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
            }
            _db.SAP_PO_Detail.Remove(result);
            _db.SaveChanges();
            TempData["msg"] = "Item Successfully Updated";
            TempData["result"] = "success";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }


    }
}
