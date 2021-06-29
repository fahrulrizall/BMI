using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BMI.Data;
using BMI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using BMI.UtilityModels;
using Microsoft.AspNetCore.Http;
using System.IO;
using ExcelDataReader;
using System.Data;

namespace BMI.Controllers
{
    [Authorize(Roles = "Planning,Admin")] 
    public class POController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private IWebHostEnvironment Environment;
        private IConfiguration Configuration;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ApplicationDbContext _db;

        public POController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext db, IWebHostEnvironment _environment, IConfiguration _configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _db = db;
            Environment = _environment;
            Configuration = _configuration;
        }

        public async Task<IActionResult> Index(string plant, string status)
        {
            var obj = _db.PO.Where(a => a.plant == plant).Where(a=>a.plant == plant && a.po_status == status).OrderByDescending(a=>a.po).ToList();
            ViewBag.plant = plant;
            if (plant == "3700")
            {
                ViewBag.factory = "BMI";
            }else 
            if (plant == "3710" ) 
            {
                ViewBag.factory = "MOWI";
            }else
            {
                ViewBag.factory = "GFF";
            }
            ViewBag.status = status;
            return await Task.Run(()=> View(obj));
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize(Policy ="Create")]
        public async Task<IActionResult> Create(POModel POModel, string plant)
        {
            if (ModelState.IsValid)
            {
                var po = new POModel()
                {
                    po = POModel.po,
                    pt = POModel.pt,
                    plant = plant,
                    pt_status = "Process",
                    po_status = "Open",
                    batch =  (POModel.po).Substring((POModel.po).Length-5) + "SIDID",
                    created_by = User.Identity.Name,
                    created_at = DateTime.Now
                };
                _db.PO.Add(po);
                _db.SaveChanges();
                TempData["msg"] = "Data Successfuly Added";
                TempData["result"] = "success";
                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
            }
            TempData["msg"] = "Data Failed to Create";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }

        public async Task<JsonResult> GetPO(string po)
        {
            var obj = _db.PO.Find(po);
            return await Task.Run(() => Json(obj));
        }

        [Authorize(Policy = "Update")]
        public async Task<IActionResult> Update(POModel POModel)
        {
            if (ModelState.IsValid)
            {
                var po = _db.PO.Find(POModel.po);

                po.pt = POModel.pt;
                po.po_status = POModel.po_status;
                _db.Entry(po).State = EntityState.Modified;
                _db.SaveChanges();
                TempData["msg"] = "PO Successfuly Updated";
                TempData["result"] = "success";
                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
            }
            TempData["msg"] = "PO Failed Update";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }

        [Authorize(Policy = "Delete")]
        public async Task<IActionResult> Delete (string po)
        {
            var obj = _db.PO.Find(po);
            if (obj != null)
            {
                _db.PO.Remove(obj);
                _db.SaveChanges();
                TempData["msg"] = "PO Successfuly Deleted";
                TempData["result"] = "success";
                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
            }
            TempData["msg"] = "PO Failed to Delete";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }


        public IActionResult Detail(string po, string pt, string plant)
        {
            ViewBag.po = po;
            ViewBag.pt = pt;
            ViewBag.plant = plant;

            var material = _db.Rm_Cost.Where(a => a.PO == po).Select(a => a.Material).ToList();

            var result = new List<CostAnalystModel>();

            if (material.Count == 0)
            {
                result = _db.CostAnalyst.Include(a => a.masterdatamodel).Where(a => a.PO == po)
                    .Select(a => new CostAnalystModel
                    {
                        Id = a.Id,
                        SAP_Code = a.SAP_Code,
                        masterdatamodel = a.masterdatamodel,
                        Price = 0,
                        Target_Lbs = a.Target_Lbs,
                        Yield = Math.Round((a.Target_Lbs / (_db.CostAnalyst.Where(a => a.PO == po).Sum(a => a.Target_Lbs)) * 100), 2),
                        Result = 0
                    }).ToList();
            }
            else
            {
             
                
            var price = (_db.SAP_PO_Detail.Where(a => material.Contains(a.refference)).Sum(a => a.qty_pl * a.unit_price)/
                    _db.SAP_PO_Detail.Where(a => material.Contains(a.refference)).Sum(a => a.qty_pl* 2.204 * (a.style=="WR" ? 0.45: a.style=="DL" ?0.67: a.style == "HGT" ? 0.61 : a.style == "GG" ? 0.55:1 ))   );

            if (plant == "3770")
                {
                            result = _db.CostAnalyst
                                .Where(a => a.PO == po)
                                .Include(a => a.masterdatamodel)
                                .Select(a => new CostAnalystModel
                                {
                                    Id = a.Id,
                                    SAP_Code = a.SAP_Code,
                                    masterdatamodel = a.masterdatamodel,
                                    Price = Math.Round(price, 2),
                                    Target_Lbs = a.Target_Lbs,
                                    Yield = Math.Round((a.Target_Lbs / (_db.CostAnalyst.Where(a => a.PO == po).Sum(a => a.Target_Lbs)) * 100), 2),
                                    Result = Math.Round(Convert.ToDouble(((a.masterdatamodel.standard_price) * a.Target_Lbs) - (a.Target_Lbs * ((price / 1 / 0.95) + a.masterdatamodel.PF3770 + 0.44))), 2)
                                }).ToList();
                            ViewBag.total_result = Math.Round(Convert.ToDouble(result.Sum(a => a.Result)), 2);
                }
                else
                {
                    result = _db.CostAnalyst
                                .Where(a => a.PO == po)
                                .Include(a => a.masterdatamodel)
                                .Select(a => new CostAnalystModel
                                {
                                    Id = a.Id,
                                    SAP_Code = a.SAP_Code,
                                    masterdatamodel = a.masterdatamodel,
                                    Price = Math.Round(price+0.29, 2),
                                    Target_Lbs = a.Target_Lbs,
                                    Yield = Math.Round((a.Target_Lbs / (_db.CostAnalyst.Where(a => a.PO == po).Sum(a => a.Target_Lbs)) * 100), 2),
                                    //Result = Math.Round( ((price+0.29) / 1 / 0.95),2)
                                    //Result = Math.Round(Convert.ToDouble (((a.masterdatamodel.standard_price) * a.Target_Lbs) - (a.Target_Lbs * (((price+0.29) / 1 / 0.95) + a.masterdatamodel.PF3710 + 0.22))), 2)
                                    //Result = Math.Round(Convert.ToDouble (((a.masterdatamodel.standard_price) * a.Target_Lbs) - (a.Target_Lbs * (    Math.Round(    ((price+0.29) / 1 / 0.95),2) + a.masterdatamodel.PF3710 + 0.22))), 2)
                                    Result = Math.Round(Convert.ToDouble (((a.masterdatamodel.standard_price) * a.Target_Lbs) - (a.Target_Lbs * ( ((price+0.29) / 1 / 0.95) + a.masterdatamodel.PF3710 + 0.22))),2)
                                }).ToList();
                        ViewBag.total_result = Math.Round(Convert.ToDouble(result.Sum(a => a.Result)), 2);
                }
           }
            
            ViewBag.total_lbs = result.Sum(a => a.Target_Lbs);
            return View(result);
        }


        public async Task<IActionResult> Import(IFormFile postedFile, string po)
        {
            if (postedFile != null)
            {
                var allowedExtensions = new[] { ".xls", ".xlsx" };
                string fileName = Path.GetFileName(postedFile.FileName);
                var checkextension = Path.GetExtension(fileName).ToLower();

                if (allowedExtensions.Contains(checkextension))
                {
                    List<CostAnalystModel> Analyst = new List<CostAnalystModel>();

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

                            if (excelDataReader.FieldCount == 2)
                            {
                                DataRowCollection row = dataSet.Tables["CA"].Rows;

                                foreach (DataRow item in row)
                                {
                                    rowDataList = item.ItemArray.ToList();

                                    Analyst.Add(new CostAnalystModel
                                    {
                                        PO = po,
                                        SAP_Code = Convert.ToString(rowDataList[0]),
                                        Target_Lbs = Convert.ToSingle(rowDataList[1]),
                                        //Material = Convert.ToString(rowDataList[2]),
                                        //Version = version
                                    });
                                }
                                stream.Close();
                                System.IO.File.Delete(filePath);
                                _db.CostAnalyst.AddRange(Analyst);
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


        // when keyup
        public async Task<JsonResult> GetMaterial(string material,string plant)
        {
            var result = _db.SAP_PO.Where(a => a.plant == plant && a.refference.Contains(material)).Select(a => a.refference);
            return await Task.Run(() => Json(result));
        }

        public async Task<IActionResult> UpdateMaterial(RmCostView rmCostView)
        {
            List<RmCostModel> add_new = new List<RmCostModel>();

            var result = _db.Rm_Cost.Where(a => a.PO == rmCostView.po).ToList();
            _db.Rm_Cost.RemoveRange(result);
            _db.SaveChanges();
            for (int i = 0; i < rmCostView.Material.Count(); i++)
            {
                add_new.Add(new RmCostModel()
                {
                    Material = rmCostView.Material[i],
                    PO = rmCostView.po
                });
            }

            _db.Rm_Cost.AddRange(add_new);
            _db.SaveChanges();
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }


        public async Task<JsonResult> SelectedMaterial(string po)
        {
            var result = _db.Rm_Cost.Where(a => a.PO == po).Select(a => a.Material).ToList();
            return await Task.Run(() => Json(result));
        }

        public async Task<IActionResult> DeleteMaterial (int id)
        {
            if (id != 0)
            {
                var result = _db.CostAnalyst.Find(id);
                _db.CostAnalyst.Remove(result);
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Deleted";
                TempData["result"] = "success";
                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
            }
            TempData["msg"] = "Item Failed to Delete";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }


        public async Task<JsonResult> GetTargetLBS (int id)
        {
            var result = _db.CostAnalyst.Find(id);
            return await Task.Run(() => Json(result.Target_Lbs));
        }

        public async Task<IActionResult> UpdateTargetLBS(int Id,float Target_Lbs)
        {
            var id = _db.CostAnalyst.Find(Id);
            if (id.Id != 0)
            {
                id.Target_Lbs = Target_Lbs;
                _db.Entry(id).State = EntityState.Modified;
                _db.SaveChanges();
                TempData["msg"] = "Item Succesfully Updated";
                TempData["result"] = "success";
                return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
            }
            TempData["msg"] = "Item Failed to Update";
            TempData["result"] = "failed";
            return await Task.Run(() => Redirect(Request.Headers["Referer"].ToString()));
        }

    }
}
