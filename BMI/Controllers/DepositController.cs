using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BMI.Models;
using BMI.Data;
using BMI.UtilityModels;

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
            var otw = _db.Rm
                .Where(k => k.status == "otw")
                .GroupBy(k => k.status)
                .Select(a => new Rmmodel
                {
                    status = a.Key,
                    total_qty = Convert.ToDouble(a.Sum(k => k.qty_pl)),
                    total_amount = Convert.ToDouble(a.Sum(k=>k.qty_pl) * a.Sum(k=>k.usd_price))
                })
                .ToList();

            var inplant = _db.Rm
                .Where(k => k.status == "in_plant")
                .GroupBy(k => k.status)
                .Select(a => new Rmmodel
                {
                    status = a.Key,
                    total_qty = Convert.ToDouble( a.Sum(k => k.qty_pl) - _db.Production_input.Sum(k => k.qty) - _db.DestroyRaw.Sum(k => k.qty)),
                    //total_amount = Convert.ToDouble(a.Sum(k => k.qty_pl) - _db.Production_input.Sum(k => k.qty) - _db.DestroyRaw.Sum(k => k.qty) * _db.Rm.Where(k=>k.qty_received >0) )
                });





            //var inplant = _db.Rm.Where(k => k.status == "in_plant").Sum(k => k.qty_pl) - _db.Production_input.Sum(k => k.qty) - _db.DestroyRaw.Sum(k => k.qty);
            var fg = _db.Production_output.Sum(k => k.qty) - _db.Shipment_detail.Sum(k => k.qty * k.MasterBMIModel.lbs *2.204) - _db.DestroyFG.Sum(k => k.qty * k.MasterBMIModel.lbs/2.204);
            return View();
        }
    }
}
