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
using Microsoft.AspNetCore.Authorization;


namespace BMI.Controllers
{
    [Authorize(Roles = "Accounting,Admin")]
    public class DepositController : Controller
    {
        private readonly ApplicationDbContext _db;
        private IWebHostEnvironment Environment;
        private IConfiguration Configuration;

        public DepositController(ApplicationDbContext db, IWebHostEnvironment _environment, IConfiguration _configuration)
        {
            _db = db;
            Environment = _environment;
            Configuration = _configuration;
        }

        public async Task<IActionResult> Index()
        {
            var model = new DepositView();
            model.otw = _db.Rm_detail
                .Include(k => k.RmModel)
                .Where(k => k.RmModel.status == "otw")
                .GroupBy(k => k.raw_source)
                .Select(k => new RmDetailModel
                {
                    raw_source = k.Key,
                    total_qty = Convert.ToDouble(k.Sum(a => a.qty_pl)),
                    amount_pl = Math.Round(Convert.ToDouble(_db.Rm_detail.Where(a => a.raw_source == k.Key).Sum(a => a.qty_pl * a.usd_price)), 2)
                })
                .ToList();

            var in_plant = _db.Rm_detail
                .Include(k => k.RmModel)
                .Where(k => k.RmModel.status == "plant")
                .GroupBy(k => new { k.raw_source, k.sap_code })
                .Select(a => new RmDetailModel
                {
                    raw_source = a.Key.raw_source,
                    sap_code = a.Key.sap_code,
                    total_qty = Convert.ToDouble(a.Sum(k => k.qty_received) -
                        ((int?)_db.Production_input.Where(k => k.raw_source == a.Key.raw_source && k.sap_code == a.Key.sap_code).Sum(k => k.qty) ?? 0)),
                    amount_received = Math.Round(Convert.ToDouble((a.Sum(k => k.qty_received * k.usd_price))
                        - (((int?)_db.Production_input.Where(k => k.raw_source == a.Key.raw_source && k.sap_code == a.Key.sap_code).Sum(k => k.qty) ?? 0)
                        * _db.Rm_detail.Where(k => k.raw_source == a.Key.raw_source && k.sap_code == a.Key.sap_code).Average(k => k.usd_price))), 2)
                })
                .ToList();

            model.in_plant = in_plant
                .GroupBy(a => a.raw_source)
                .Select(a => new RmDetailModel
                {
                    raw_source = a.Key,
                    total_qty = a.Sum(k => k.total_qty),
                    amount_received = a.Sum(k => k.amount_received)
                })
                .ToList();

            var fg = _db.Production_output
                .Include(k => k.POModel)
                .Include(k => k.MasterBMIModel)
                .Where(k => k.POModel.pt_status == "Open" || k.POModel.pt_status == "Process")
                .AsEnumerable()
                .GroupBy(k => k.raw_source)
                .Select(k => new ProductionOutputModel
                {
                    raw_source = k.Key,
                    lbs = k.Sum(k => k.qty * 2.20462) ,
                    rm_cost = Convert.ToDouble(
                        (_db.Rm_detail.Where(a => a.raw_source == k.Key).Sum(a => a.qty_received * a.usd_price) / _db.Rm_detail.Where(a => a.raw_source == k.Key).Sum(a => a.qty_received))
                        * 0.45359237 /
                        ((_db.Production_output.Where(a => a.raw_source == k.Key).Sum(a => a.qty) + (float?)_db.Pending.Where(a => a.raw_source == k.Key).Sum(a => a.qty) ?? 0) / (_db.Production_input.Where(a => a.raw_source == k.Key).Sum(a => a.qty)))
                        )
                })
                .ToList();

            model.fg = fg.GroupBy(a => a.raw_source)
                .Select(a => new ProductionOutputModel
                {
                    raw_source = a.Key,
                    lbs = a.Sum(k => k.lbs) + _db.Pending.Where(k => k.raw_source == a.Key).Sum(k => k.qty * 2.20462) -  _db.Shipment.Where(k => k.raw_source == a.Key).Sum(k => k.qty * k.MasterBMIModel.lbs),
                    amount = (a.Sum(k => k.lbs) + _db.Pending.Where(k => k.raw_source == a.Key).Sum(k => k.qty * 2.20462) - _db.Shipment.Where(k => k.raw_source == a.Key).Sum(k => k.qty * k.MasterBMIModel.lbs)) * a.Average(k => k.rm_cost),
                })
                .Where(k=>k.lbs > 10)
                .ToList();

            ViewBag.amount = Math.Round( Convert.ToDouble(model.otw.Sum(a => a.amount_received) + model.in_plant.Sum(a => a.amount_received) + model.fg.Sum(a => a.amount)),2);

            return await Task.Run(() => View(model));
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
                    List<PendingModel> Pending = new List<PendingModel>();
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
                            if (excelDataReader.FieldCount == 2)
                            {

                                var conf = new ExcelDataSetConfiguration()
                                {
                                    ConfigureDataTable = a => new ExcelDataTableConfiguration
                                    {
                                        UseHeaderRow = true
                                    }
                                };

                                DataSet dataSet = excelDataReader.AsDataSet(conf);
                                DataRowCollection row = dataSet.Tables["Pending"].Rows;
                                List<object> rowDataList = null;

                                _db.Pending.RemoveRange();
                                _db.SaveChanges();

                                foreach (DataRow item in row)
                                {
                                    rowDataList = item.ItemArray.ToList();
                                    Pending.Add(new PendingModel
                                    {
                                        raw_source = Convert.ToString(rowDataList[0]),
                                        qty = Convert.ToSingle(rowDataList[1]),
                                        created_at = DateTime.Now,
                                        created_by = User.Identity.Name
                                    });
                                }
                                _db.Pending.AddRange(Pending);
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






    }
}
