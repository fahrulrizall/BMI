using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BMI.Models;
using BMI.Data;
using BMI.UtilityModels;
using Microsoft.EntityFrameworkCore;

namespace BMI.Controllers
{
    public class DepositController : Controller
    {
        private readonly ApplicationDbContext _db;

        public DepositController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
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
                    total_amount = Convert.ToDouble(_db.Rm_detail.Where(a => a.raw_source == k.Key).Sum(a => a.qty_pl * a.usd_price))
                })
                .ToList();

            var in_plant = _db.Rm_detail
                .Include(k => k.RmModel)
                .Where(k => k.RmModel.status == "in_plant")
                .GroupBy(k => new { k.raw_source, k.sap_code })
                .Select(a => new RmDetailModel
                {
                    raw_source = a.Key.raw_source,
                    sap_code = a.Key.sap_code,
                    total_qty = Convert.ToDouble(a.Sum(k => k.qty_received) - 
                        ((int?) _db.Production_input.Where(k => k.raw_source == a.Key.raw_source && k.sap_code == a.Key.sap_code).Sum(k => k.qty) ??0 ) ),
                    total_amount = Convert.ToDouble((a.Sum(k => k.qty_received * k.usd_price))
                        - (((int?) _db.Production_input.Where(k => k.raw_source == a.Key.raw_source && k.sap_code == a.Key.sap_code).Sum(k => k.qty) ??0)
                        * _db.Rm_detail.Where(k => k.raw_source == a.Key.raw_source && k.sap_code == a.Key.sap_code).Average(k => k.usd_price)))
                })
                .ToList();

            model.in_plant = in_plant
                .GroupBy(a => a.raw_source)
                .Select(a=> new RmDetailModel 
                { 
                    raw_source = a.Key,
                    total_qty = a.Sum(k=>k.total_qty),
                    total_amount = a.Sum(k=>k.total_amount)
                }) 
                .ToList();

            var fg = _db.Production_output
                .Include(k => k.POModel)
                .Include(k=>k.MasterBMIModel)
                .Where(k => k.POModel.status == "Open")
                .AsEnumerable()
                .GroupBy(k => new { k.POModel ,k.raw_source,k.bmi_code })
                .Select(k => new ProductionOutputModel
                {
                    po = Convert.ToString(k.Key.POModel.pt),
                    raw_source = k.Key.raw_source,
                    bmi_code = k.Key.bmi_code,
                    MasterBMIModel = k.Max(a=>a.MasterBMIModel),
                    lbs = k.Sum(k => k.qty * 2.20462),
                    //cases = k.Sum(k => k.qty * 2.20462 / k.MasterBMIModel.lbs) -
                    //    _db.Shipment_detail.Where(c => c.bmi_code == k.Key.bmi_code && c.batch == k.Key.POModel.batch).Sum(a => a.qty) -
                    //    _db.AdjustmentFG.Where(c => c.bmi_code == k.Key.bmi_code && c.po == k.Key.POModel.po).Sum(a => a.qty),
                    //amount = Convert.ToDouble( ((k.Sum(k => k.qty * 2.204 / k.MasterBMIModel.lbs) -
                    //    _db.Shipment_detail.Where(c => c.bmi_code == k.Key.bmi_code && c.batch == k.Key.POModel.batch).Sum(a => a.qty) -
                    //    _db.AdjustmentFG.Where(c => c.bmi_code == k.Key.bmi_code && c.po == k.Key.POModel.po).Sum(a => a.qty)) * k.Max(a=>a.MasterBMIModel.lbs)) *
                    //    (_db.Rm_detail.Where(a => a.raw_source == k.Key.raw_source).Sum(a => a.qty_received * a.usd_price) /  _db.Rm_detail.Where(a=>a.raw_source == k.Key.raw_source).Sum(a=>a.qty_received))
                    //    *0.45359237 /  
                    //    ( _db.Production_output.Where(a=>a.po == k.Key.POModel.po).Sum(a=>a.qty) / (_db.Production_input.Where(a => a.po == k.Key.POModel.po).Sum(a => a.qty)) ) 
                    //    ),
                    //amount = Convert.ToDouble(((k.Sum(k => k.qty * 2.20462) -
                    //    _db.Shipment_detail.Where(c => c.bmi_code == k.Key.bmi_code && c.batch == k.Key.POModel.batch).Sum(a => a.qty) -
                    //    _db.AdjustmentFG.Where(c => c.bmi_code == k.Key.bmi_code && c.po == k.Key.POModel.po).Sum(a => a.qty)) * k.Max(a => a.MasterBMIModel.lbs)) *
                    //    ( _db.Rm_detail.Where(a => a.raw_source == k.Key.raw_source).Sum(a => a.qty_received * a.usd_price) / _db.Rm_detail.Where(a => a.raw_source == k.Key.raw_source).Sum(a => a.qty_received))
                    //    * 0.45359237 /
                    //    (_db.Production_output.Where(a => a.po == k.Key.POModel.po).Sum(a => a.qty) / (_db.Production_input.Where(a => a.po == k.Key.POModel.po).Sum(a => a.qty))) 
                    //    ),
                    rm_cost = Convert.ToDouble(
                        (_db.Rm_detail.Where(a => a.raw_source == k.Key.raw_source).Sum(a => a.qty_received * a.usd_price) / _db.Rm_detail.Where(a => a.raw_source == k.Key.raw_source).Sum(a => a.qty_received))
                        * 0.45359237 /
                        (_db.Production_input.Where(a => a.raw_source == k.Key.raw_source).Sum(a => a.qty) / (_db.Rm_detail.Where(a => a.raw_source == k.Key.raw_source).Sum(a => a.qty_received)))
                        )
                })
                .ToList();

            model.fg = fg.GroupBy(a => a.raw_source)
                .Select(a => new ProductionOutputModel
                {
                    raw_source = a.Key,
                    lbs = a.Sum(k=>k.lbs),
                    rm_cost = a.Average(k=>k.rm_cost),
                    amount = a.Sum(k=>k.lbs * k.rm_cost)
                })
                .ToList();

            ViewBag.amount = (model.otw.Sum(a => a.total_amount) + model.in_plant.Sum(a => a.total_amount) + model.fg.Sum(a => a.amount)).ToString("0.00");

            return View(model);
        }
    }
}
