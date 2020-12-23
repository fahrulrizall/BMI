using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BMI.Data;
using BMI.Models;
using Microsoft.EntityFrameworkCore;

namespace BMI.Controllers
{
    public class FTController : Controller
    {
        private readonly ApplicationDbContext _db;

        public FTController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var obj = _db.Rm.AsEnumerable().OrderByDescending(a=>a.created_at).ToList();
            return await Task.Run(()=> View(obj));
        }

        public async Task<IActionResult> Detail(DateTime start_date, DateTime finish_date)
        {
            var ft_code = new string[] {
                "202028",
                "202026",
                "202020",
                "202024",
                "202049",
                "202050",
                "202048",
                "202045",
                "202047",
                "202044",
                "202046",
                "202041",
                "202043",
                "202040",
                "202042",
                };

            var input = _db.Production_input
                .Where(a => a.date >= start_date && a.date <= finish_date && ft_code.Contains(a.sap_code))
                .AsEnumerable()
                .GroupBy(a=>a.raw_source)
                .Select(a=> new  
                {
                    raw_source = a.Key,
                    qty = a.Sum(a=>a.qty)
                })
                .ToList();

           var output = _db.Production_output
                .Include(a=>a.MasterBMIModel)
                .Where(a => a.date >= start_date && a.date <= finish_date && a.landing_site.Contains("FT") )
                .AsEnumerable()
                .GroupBy(a=>a.raw_source)
                .Select(a=> new  
                {
                    raw_source = a.Key,
                    a5384909483 = a.Where(c=>c.MasterBMIModel.sap_code == "5384909483").Sum(c=>c.qty),
                    a5384909495 = a.Where(c=>c.MasterBMIModel.sap_code == "5384909495").Sum(c=>c.qty),
                    a5384909502 = a.Where(c=>c.MasterBMIModel.sap_code == "5384909502").Sum(c=>c.qty),
                    a5384909498 = a.Where(c=>c.MasterBMIModel.sap_code == "5384909498").Sum(c=>c.qty),
                    a5384909499 = a.Where(c=>c.MasterBMIModel.sap_code == "5384909499").Sum(c=>c.qty),
                    a5384909491 = a.Where(c=>c.MasterBMIModel.sap_code == "5384909491").Sum(c=>c.qty),
                    a5384909501 = a.Where(c=>c.MasterBMIModel.sap_code == "5384909501").Sum(c=>c.qty),
                    a5384909554 = a.Where(c=>c.MasterBMIModel.sap_code == "5384909554").Sum(c=>c.qty),
                    a5384909512 = a.Where(c=>c.MasterBMIModel.sap_code == "5384909512").Sum(c=>c.qty),
                    qty = a.Sum(c=>c.qty)
                })
                .ToList();

            var result = from i in input
                      join o in output on i.raw_source equals o.raw_source
                      select new FTView
                      {
                          raw_source = i.raw_source,
                          qty = i.qty,
                          a5384909483 = o.a5384909483,
                          a5384909495 = o.a5384909495,
                          a5384909502 = o.a5384909502,
                          a5384909498 = o.a5384909498,
                          a5384909499 = o.a5384909499,
                          a5384909491 = o.a5384909491,
                          a5384909501 = o.a5384909501,
                          a5384909554 = o.a5384909554,
                          a5384909512 = o.a5384909512,
                          yield = ((o.qty / i.qty)*100).ToString("0.00")
                      };


            return await Task.Run(()=> View(result));
        }


    }
}
