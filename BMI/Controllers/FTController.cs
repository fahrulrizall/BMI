using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BMI.Data;
using BMI.Models;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using System.IO;

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
            //var ft_code = new List<string> {
            //    "202028",
            //    "202026",
            //    "202020",
            //    "202024",
            //    "202049",
            //    "202050",
            //    "202048",
            //    "202045",
            //    "202047",
            //    "202044",
            //    "202046",
            //    "202041",
            //    "202043",
            //    "202040",
            //    "202042",
            //    };

            var obj = _db.Rm
                //.Where(a => ft_code.Contains(a.sap_code))
                .Where(a => a.status == "Plant" || a.status == "Closed")
                .OrderByDescending(a => a.created_at)
                .AsEnumerable()
                .GroupBy(a => a.raw_source)
                .Select(a => new RmDetailModel { 
                    raw_source = a.Key
                })
                .ToList();
            return await Task.Run(()=> View(obj));
        }


        public async Task<IActionResult> Raw(string raw)
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
                .Where(a => a.raw_source == raw)
                .Where(a=> ft_code.Contains(a.sap_code))
                .AsEnumerable()
                .GroupBy(a => a.raw_source)
                .Select(a => new
                {
                    raw_source = a.Key,
                    qty = a.Sum(a => a.qty)
                })
                .ToList();

            var output = _db.Production_output
                .Include(a => a.MasterBMIModel)
                .Where(a => a.raw_source== raw && a.fairtrade_status=="FT")
                .AsEnumerable()
                .GroupBy(a => a.raw_source)
                .Select(a => new
                {
                    raw_source = a.Key,
                    a5384909483 = a.Where(c => c.MasterBMIModel.sap_code == "5384909483").Sum(c => c.qty),
                    a5384909495 = a.Where(c => c.MasterBMIModel.sap_code == "5384909495").Sum(c => c.qty),
                    a5384909502 = a.Where(c => c.MasterBMIModel.sap_code == "5384909502").Sum(c => c.qty),
                    a5384909498 = a.Where(c => c.MasterBMIModel.sap_code == "5384909498").Sum(c => c.qty),
                    a5384909499 = a.Where(c => c.MasterBMIModel.sap_code == "5384909499").Sum(c => c.qty),
                    a5384909491 = a.Where(c => c.MasterBMIModel.sap_code == "5384909491").Sum(c => c.qty),
                    a5384909501 = a.Where(c => c.MasterBMIModel.sap_code == "5384909501").Sum(c => c.qty),
                    a5384909554 = a.Where(c => c.MasterBMIModel.sap_code == "5384909554").Sum(c => c.qty),
                    a5384909512 = a.Where(c => c.MasterBMIModel.sap_code == "5384909512").Sum(c => c.qty),
                    qty = a.Where(c => 
                    c.MasterBMIModel.sap_code == "5384909483" ||
                    c.MasterBMIModel.sap_code == "5384909495" ||
                    c.MasterBMIModel.sap_code == "5384909502" ||
                    c.MasterBMIModel.sap_code == "5384909498" ||
                    c.MasterBMIModel.sap_code == "5384909499" ||
                    c.MasterBMIModel.sap_code == "5384909491" ||
                    c.MasterBMIModel.sap_code == "5384909501" ||
                    c.MasterBMIModel.sap_code == "5384909554" ||
                    c.MasterBMIModel.sap_code == "5384909512"
                    ).Sum(c => c.qty)
                })
                .ToList();

            var result = from i in input
                         join o in output on i.raw_source equals o.raw_source
                         select new FTView
                         {
                             raw_source = i.raw_source,
                             input_qty = i.qty,
                             a5384909483 = o.a5384909483,
                             a5384909495 = o.a5384909495,
                             a5384909502 = o.a5384909502,
                             a5384909498 = o.a5384909498,
                             a5384909499 = o.a5384909499,
                             a5384909491 = o.a5384909491,
                             a5384909501 = o.a5384909501,
                             a5384909554 = o.a5384909554,
                             a5384909512 = o.a5384909512,
                             output_qty = output.Sum(a=>a.qty),
                             yield = ((o.qty / i.qty) * 100).ToString("0.00")
                         };
            ViewBag.raw = raw;
            return await Task.Run(() => View(result));
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
                .Include(a => a.MasterBMIModel)
                .Where(a => a.date >= start_date && a.date <= finish_date && a.fairtrade_status=="FT" )
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
                    qty = a.Where(c =>
                       c.MasterBMIModel.sap_code == "5384909483" ||
                       c.MasterBMIModel.sap_code == "5384909495" ||
                       c.MasterBMIModel.sap_code == "5384909502" ||
                       c.MasterBMIModel.sap_code == "5384909498" ||
                       c.MasterBMIModel.sap_code == "5384909499" ||
                       c.MasterBMIModel.sap_code == "5384909491" ||
                       c.MasterBMIModel.sap_code == "5384909501" ||
                       c.MasterBMIModel.sap_code == "5384909554" ||
                       c.MasterBMIModel.sap_code == "5384909512"
                        ).Sum(c => c.qty)
                })
                .ToList();

            var result = from i in input
                      join o in output on i.raw_source equals o.raw_source
                      select new FTView
                      {
                          raw_source = i.raw_source,
                          input_qty = i.qty,
                          a5384909483 = o.a5384909483,
                          a5384909495 = o.a5384909495,
                          a5384909502 = o.a5384909502,
                          a5384909498 = o.a5384909498,
                          a5384909499 = o.a5384909499,
                          a5384909491 = o.a5384909491,
                          a5384909501 = o.a5384909501,
                          a5384909554 = o.a5384909554,
                          a5384909512 = o.a5384909512,
                          output_qty = output.Sum(a => a.qty),
                          yield = ((o.qty / i.qty)*100).ToString("0.00")
                      };
            ViewBag.start_date = start_date;
            ViewBag.finish_date = finish_date;
            return await Task.Run(()=> View(result));
        }



        public IActionResult Download(DateTime start_date, DateTime finish_date)
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
                .GroupBy(a => a.raw_source)
                .Select(a => new
                {
                    raw_source = a.Key,
                    qty = a.Sum(a => a.qty)
                })
                .ToList();

            var output = _db.Production_output
                 .Include(a => a.MasterBMIModel)
                 .Where(a => a.date >= start_date && a.date <= finish_date && a.fairtrade_status=="FT")
                 .AsEnumerable()
                 .GroupBy(a => a.raw_source)
                 .Select(a => new
                 {
                     raw_source = a.Key,
                     a5384909483 = a.Where(c => c.MasterBMIModel.sap_code == "5384909483").Sum(c => c.qty),
                     a5384909495 = a.Where(c => c.MasterBMIModel.sap_code == "5384909495").Sum(c => c.qty),
                     a5384909502 = a.Where(c => c.MasterBMIModel.sap_code == "5384909502").Sum(c => c.qty),
                     a5384909498 = a.Where(c => c.MasterBMIModel.sap_code == "5384909498").Sum(c => c.qty),
                     a5384909499 = a.Where(c => c.MasterBMIModel.sap_code == "5384909499").Sum(c => c.qty),
                     a5384909491 = a.Where(c => c.MasterBMIModel.sap_code == "5384909491").Sum(c => c.qty),
                     a5384909501 = a.Where(c => c.MasterBMIModel.sap_code == "5384909501").Sum(c => c.qty),
                     a5384909554 = a.Where(c => c.MasterBMIModel.sap_code == "5384909554").Sum(c => c.qty),
                     a5384909512 = a.Where(c => c.MasterBMIModel.sap_code == "5384909512").Sum(c => c.qty),
                     qty = a.Where(c =>
                       c.MasterBMIModel.sap_code == "5384909483" ||
                       c.MasterBMIModel.sap_code == "5384909495" ||
                       c.MasterBMIModel.sap_code == "5384909502" ||
                       c.MasterBMIModel.sap_code == "5384909498" ||
                       c.MasterBMIModel.sap_code == "5384909499" ||
                       c.MasterBMIModel.sap_code == "5384909491" ||
                       c.MasterBMIModel.sap_code == "5384909501" ||
                       c.MasterBMIModel.sap_code == "5384909554" ||
                       c.MasterBMIModel.sap_code == "5384909512"
                        ).Sum(c => c.qty)
                 })
                 .ToList();

            var result = from i in input
                         join o in output on i.raw_source equals o.raw_source
                         select new FTView
                         {
                             raw_source = i.raw_source,
                             input_qty = i.qty,
                             a5384909483 = o.a5384909483,
                             a5384909495 = o.a5384909495,
                             a5384909502 = o.a5384909502,
                             a5384909498 = o.a5384909498,
                             a5384909499 = o.a5384909499,
                             a5384909491 = o.a5384909491,
                             a5384909501 = o.a5384909501,
                             a5384909554 = o.a5384909554,
                             a5384909512 = o.a5384909512,
                             output_qty = output.Sum(a => a.qty),
                             yield = ((o.qty / i.qty) * 100).ToString("0.00")
                         };
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("FT Report");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Raw Material";
                worksheet.Cell(currentRow, 2).Value = "Input Qty";
                worksheet.Cell(currentRow, 3).Value = "5384909483";
                worksheet.Cell(currentRow, 4).Value = "5384909495";
                worksheet.Cell(currentRow, 5).Value = "5384909502";
                worksheet.Cell(currentRow, 6).Value = "5384909498";
                worksheet.Cell(currentRow, 7).Value = "5384909499";
                worksheet.Cell(currentRow, 8).Value = "5384909491";
                worksheet.Cell(currentRow, 9).Value = "5384909501";
                worksheet.Cell(currentRow, 10).Value = "5384909554";
                worksheet.Cell(currentRow, 11).Value = "5384909512";
                worksheet.Cell(currentRow, 12).Value = "Input Qty";
                worksheet.Cell(currentRow, 13).Value = "Yield";

                foreach (var data in result)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = data.raw_source;
                    worksheet.Cell(currentRow, 2).Value = data.input_qty;
                    worksheet.Cell(currentRow, 3).Value = data.a5384909483;
                    worksheet.Cell(currentRow, 4).Value = data.a5384909495;
                    worksheet.Cell(currentRow, 5).Value = data.a5384909502;
                    worksheet.Cell(currentRow, 6).Value = data.a5384909498;
                    worksheet.Cell(currentRow, 7).Value = data.a5384909499;
                    worksheet.Cell(currentRow, 8).Value = data.a5384909491;
                    worksheet.Cell(currentRow, 9).Value = data.a5384909501;
                    worksheet.Cell(currentRow, 10).Value = data.a5384909554;
                    worksheet.Cell(currentRow, 11).Value = data.a5384909512;
                    worksheet.Cell(currentRow, 12).Value = data.output_qty;
                    worksheet.Cell(currentRow, 13).Value = data.yield;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "FT Report.xlsx");
                }
            }

        }





    }
}
