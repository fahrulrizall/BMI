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

        public async Task<IActionResult> Index(string plant)
        {
            var obj = _db.PO
                .Where(a => a.plant == plant)
                .Select(a => new POModel
                {
                    pt = a.pt,
                    po = a.po,
                    batch = a.batch,
                    pt_status = a.pt_status
                })
                .OrderByDescending(a => a.pt).ToList();
            return await Task.Run(()=>View(obj));
        }


        public async Task<JsonResult> Getptdata(string po)
        {
            var obj = _db.PO.Find(po);
            return await Task.Run(()=> Json(obj));
        }

        public async Task<IActionResult> Updatept(POModel POModel)
        {
            var po = new POModel() { po = POModel.po, pt_status = POModel.pt_status };
            _db.PO.Attach(po);
            _db.Entry(po).Property(x => x.pt_status).IsModified = true; 
            _db.SaveChanges();
            TempData["msg"] = "File Succesfully Updated";
            TempData["result"] = "success";
            return await Task.Run(()=> RedirectToAction("Index"));
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
                    List<ProductionInputModel> prod_input = new List<ProductionInputModel>();
                    List<ProductionOutputModel> prod_output = new List<ProductionOutputModel>();
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
                            var conf = new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = a => new ExcelDataTableConfiguration
                                {
                                    UseHeaderRow = true
                                }
                            };

                            DataSet dataSet = excelDataReader.AsDataSet(conf);
                            List<object> rowDataList = null;

                            if (excelDataReader.FieldCount == 7)
                            {  
                                DataRowCollection row = dataSet.Tables["GI"].Rows;

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
                                stream.Close();
                                System.IO.File.Delete(filePath);
                                _db.Production_input.AddRange(prod_input);
                                _db.SaveChanges();
                                TempData["msg"] = "File Succesfully Uploaded";
                                TempData["result"] = "success";
                                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
                            }
                            else if (excelDataReader.FieldCount == 5)
                            {
                                DataRowCollection row = dataSet.Tables["GR"].Rows;

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
                                stream.Close();
                                System.IO.File.Delete(filePath);
                                _db.Production_output.AddRange(prod_output);
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

        public async Task<IActionResult> Detail(string pt,string po)
        {
            var model = new ProductionView();
            model.ProductionOutputModel = _db.Production_output
                .Where(a => a.po == po)
                .Include(k => k.MasterBMIModel)
                .AsEnumerable()
                .GroupBy(k => new { k.bmi_code, k.po })
                .Select(a => new ProductionOutputModel
                {
                    bmi_code = a.Key.bmi_code,
                    MasterBMIModel = a.Max(m => m.MasterBMIModel),
                    qty_production = a.Sum(x => x.qty) 
                    - _db.AdjustmentFG.Where(c => c.status == "rounded" && c.bmi_code == a.Key.bmi_code && c.po == a.Key.po).Sum(a => a.qty * a.MasterBMIModel.lbs / 2.204)
                })
                .OrderByDescending(a=>a.bmi_code)
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
                .OrderByDescending(a=>a.category)
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
                .OrderByDescending(a=>a.raw_source)
                .ToList();
            ViewBag.totalinputkg = model.ProductionInputModel.Sum(k => k.qty).ToString("0.00");
            ViewBag.totalinputlbs = model.ProductionInputModel.Sum(k => k.qty * 2.204).ToString("0.00");

            ViewBag.yield = ((model.ProductionOutputModel.Sum(k => k.qty_production * 2.204) / model.ProductionInputModel.Sum(k => k.qty * 2.204)) * 100).ToString("0.00");

            ViewBag.pt = pt;
            ViewBag.po = po;
            return await Task.Run(()=> View(model));
        }

        public async Task<JsonResult> Detailperitem(string po, string bmicode)
        {
            var output = _db.Production_output
                .Where(a => a.po == po && a.bmi_code == bmicode)
                .Include(k => k.MasterBMIModel)
                .AsEnumerable()
                .GroupBy(k => new { k.date, k.raw_source })
                .Select(a => new ProductionOutputModel
                {
                    production_date = a.Key.date,
                    raw_source = a.Key.raw_source,
                    cases = Convert.ToInt32( a.Sum(x => x.qty *2.204) / a.Max(b=>b.MasterBMIModel.lbs)) ,
                    available = Convert.ToInt32(a.Sum(x => x.qty * 2.204) / a.Max(b => b.MasterBMIModel.lbs))
                    - _db.Shipment.Where(i=>i.batch == po &&  i.pdc == a.Key.date &&  i.bmi_code == bmicode && i.raw_source == a.Key.raw_source ).Sum(i=>i.qty) 
                    - _db.AdjustmentFG.Where(i=>i.bmi_code == bmicode && i.raw_source == a.Key.raw_source).Sum(i=>i.qty)
                    - _db.Repack.Where(i =>i.from_po == po && i.production_date == a.Key.date && i.from_bmi_code == bmicode && i.raw_source == a.Key.raw_source).Sum(i => i.qty)
                })
                .OrderByDescending(a => a.production_date)
                .ToList();

            var repack = _db.Repack
                .Where(a => a.to_po == po && a.to_bmi_code == bmicode)
                .Include(a=>a.toMasterBMIModel)
                .AsEnumerable()
                .GroupBy(a => new { a.production_date, a.raw_source })
                .Select(a => new ProductionOutputModel
                {
                    production_date = a.Key.production_date,
                    raw_source = a.Key.raw_source,
                    cases = a.Sum(a => a.qty * a.fromMasterBMIModel.lbs /a.toMasterBMIModel.lbs),
                    available = a.Sum(a=>a.qty * a.fromMasterBMIModel.lbs / a.toMasterBMIModel.lbs)
                })
                .ToList();

            var union = new List<ProductionOutputModel> (output);
            union.AddRange(repack);

            var result = union.GroupBy(a => new { a.production_date, a.raw_source })
                .Select(a => new
                {
                    date = a.Key.production_date,
                    raw_source = a.Key.raw_source,
                    cases = a.Sum(b => b.cases),
                    available = a.Sum(b => b.available)
                }).ToList();
            return await Task.Run(()=> Json(result));
        }

        public async Task<JsonResult> Detaillandingsite(string po, string bmicode, DateTime pdc, string raw_source)
        {
            var output = _db.Production_output
                .Include(a => a.MasterBMIModel)
                .Where(a => a.po == po && a.bmi_code == bmicode && a.raw_source == raw_source && a.date == pdc)
                .AsEnumerable()
                .GroupBy(a => a.landing_site)
                .Select(a => new RepackModel
                {
                    landing_site = a.Key,
                    cases = Convert.ToInt32(a.Sum(x => x.qty * 2.204) / a.Max(b => b.MasterBMIModel.lbs))
                }).ToList();

            var repack = _db.Repack
                .Where(a => a.to_po == po && a.to_bmi_code == bmicode && a.raw_source == raw_source && a.production_date == pdc) 
                .AsEnumerable()
                .GroupBy(a => a.landing_site)
                .Select(a => new RepackModel
                {
                    landing_site = a.Key,
                    cases = a.Sum(b => b.qty)
                }).ToList();


            var union = new List<RepackModel> (output);
            union.AddRange(repack);

            var result = union
                .GroupBy(a => a.landing_site)
                .Select(a=> new 
                { 
                    landing_site = a.Key,
                    cases = a.Sum(b=>b.cases)
                }).ToList();

            return await Task.Run(() => Json(result));

        }

        public async Task<JsonResult> DateExist(DateTime date)
        {
            var unique = _db.Production_input.FirstOrDefault(m => m.date == date);
            if (unique != null)
            {
                return await Task.Run(()=> Json(true));
            }
            return await Task.Run(() => Json(false));
        }

        public async Task<IActionResult> Daily(DateTime date)
        {
            var obj = _db.Production_input
                .Include(k => k.POModel)
                .AsEnumerable()
                .Where(m => m.date == date)
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
            return await Task.Run(() => View(obj));
        }


        public async Task<JsonResult> AllMaterial(string po, DateTime date)
        {
            var model = new ProductionView();
            model.ProductionInputModel = _db.Production_input
                .Include(k => k.Masterdatamodel)
                .AsEnumerable()
                .Where(m => m.po_bmi == po && m.date == date)
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

            model.ProductionOutputModel = _db.Production_output
                .Include(k => k.MasterBMIModel)
                .AsEnumerable()
                .Where(m => m.po_bmi == po && m.date == date)
                .GroupBy(k => k.bmi_code)
                .Select(a => new ProductionOutputModel
                {
                    bmi_code = a.Key,
                    MasterBMIModel = a.Max(m => m.MasterBMIModel),
                    qty = a.Sum(k => k.qty)
                })
                .ToList();
            return await Task.Run(() => Json(model));
        }

        public async Task<JsonResult> Getitemdata(string code, string pt)
        {
            var po = Convert.ToString(_db.PO.Where(a => a.pt == Convert.ToInt32(pt)).Select(a => a.po).First());
            var obj = _db.Production_output
                .Where(k => k.po == po && k.bmi_code == code)
                .GroupBy(x => x.date )
                .Select(a => new ProductionOutputModel
                {
                    date = a.Key,
                    bmi_code = a.Max(b=>b.bmi_code),
                    raw_source = a.Max(b=>b.raw_source),
                    landing_site = a.Max(b=>b.landing_site),
                    po = a.Max(k => k.po)
                })
                .ToList();
            return await Task.Run(()=> Json(obj));
        }

        public async Task<IActionResult> Adjustment(AdjustmentFGModel destroyFGModel)
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
                return await Task.Run(()=> Redirect(Request.Headers["Referer"].ToString()));
            }
            TempData["msg"] = "Item Failded to Destroyed";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }

        public async Task<IActionResult> Repack(ProductionView productionView)
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
                    to_bmi_code = productionView.to_bmi_code,
                    landing_site = productionView.landing_site
                };
                _db.Repack.Add(repack);
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Repacked";
                TempData["result"] = "success";
                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
            }
            TempData["msg"] = "Item Failded to Repack";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }

        public async Task<IActionResult> History()
        {
            var model = new ProductionView();
            model.ProductionInputModel = _db.Production_input
                .Where(a=>a.created_at != null)
                .AsEnumerable()
                .GroupBy(a => new { a.created_at.Value.Date , a.created_at.Value.Hour, a.created_at.Value.Minute})
                .Select(a=> new ProductionInputModel
                { 
                   created_at = a.Key.Date,
                   hour = a.Key.Hour,
                   minute = a.Key.Minute,
                   saved = a.Max(b=>b.saved)
                })
                .OrderByDescending(a=>a.created_at)
                .ToList();

            model.ProductionOutputModel = _db.Production_output
                .Where(a => a.created_at != null)
               .AsEnumerable()
               .GroupBy(a => new { a.created_at.Value.Date, a.created_at.Value.Hour, a.created_at.Value.Minute })
               .Select(a => new ProductionOutputModel
               {
                   created_at = a.Key.Date,
                   hour = a.Key.Hour,
                   minute = a.Key.Minute,
                   saved = a.Max(b => b.saved)
               })
               .OrderByDescending(a => a.created_at)
               .ToList();

            return await Task.Run(() => View(model));
        }


        public async Task<IActionResult> DeleteGI(DateTime date, int hour,int minute)
        {
            var datetime = _db.Production_input.Where(a => a.created_at.Value.Date == date && a.created_at.Value.Hour == hour && a.created_at.Value.Minute == minute).ToList();
            if (datetime.Count>0)
            {
                _db.Production_input.RemoveRange(datetime);
                _db.SaveChanges();
                TempData["msg"] = "Data Successfuly Deleted";
                TempData["result"] = "success";
                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
            }
            TempData["msg"] = "Data Failed to Delete";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }


        public async Task<IActionResult> DeleteGR(DateTime date, int hour, int minute)
        {
            var datetime = _db.Production_output.Where(a => a.created_at.Value.Date == date && a.created_at.Value.Hour == hour && a.created_at.Value.Minute == minute).ToList();
            if(datetime.Count > 0)
            {
                _db.Production_output.RemoveRange(datetime);
                _db.SaveChanges();
                TempData["msg"] = "Data Successfuly Deleted";
                TempData["result"] = "success";
                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
            }
            TempData["msg"] = "Data Failed to Delete";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }


    }
}
