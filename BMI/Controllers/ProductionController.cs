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

namespace BMI.Controllers
{
    public class ProductionController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductionController (ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var obj = _db.Production_output
                .AsEnumerable()
                .GroupBy(x => x.pt, (key, g) => g.OrderByDescending(e => e.id_productionoutput).First())
                .OrderByDescending(e => e.id_productionoutput)
                .ToList();
            return View(obj);
        }

        public IActionResult Detail(int pt)
        {
            //var obj = _db.Production_output
            //    .Where(a => a.pt == pt)
            //    .AsEnumerable()
            //    .GroupBy(x => x.bmi_code, (key, g) => g.OrderByDescending(e => e.id_productionoutput).First())
            //    .Select(a=> new { a.bmi_code,a.qty})
            //    .ToList();

            var abc = _db.Production_output
                .Where(a => a.pt == pt)
                .AsEnumerable()
                .GroupBy(x => x.date)
                .ToList();

            var day = abc[0];
            

            var obj = _db.Production_output
                .Where(a => a.pt == pt)
                .AsEnumerable()
                .GroupBy(x => x.bmi_code)
                .Select(a => new { 
                    code = a.Key,
                    pertama = a.Where(c => c.date.Day == 12).Sum(c => c.qty),
                    kedua = a.Where(c => c.date.Day == 13).Sum(c => c.qty),
                    ketiga = a.Where(c => c.date.Day == 14).Sum(c => c.qty),
                    keempat = a.Where(c => c.date.Day == 15).Sum(c => c.qty),
                    kelima = a.Where(c => c.date.Day == 16).Sum(c => c.qty)
                })
                .ToList();
            return View(abc);
        }


    }
}
