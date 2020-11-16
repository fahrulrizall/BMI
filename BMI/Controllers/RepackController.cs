using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using BMI.Data;
using BMI.Models;
using Microsoft.EntityFrameworkCore;

namespace BMI.Controllers
{
    public class RepackController : Controller
    {
        private readonly ApplicationDbContext _db;

        public RepackController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var obj = _db.Repack
                .Include(k=>k.fromMasterBMIModel)
                .Include(k=>k.toMasterBMIModel)
                .Include(k=>k.fromPTModel)
                .Include(k=>k.toPTModel)
                //.AsEnumerable()
                //.GroupBy(k => k.date).First()
                .ToList();
            return View(obj);
        }
    }
}
