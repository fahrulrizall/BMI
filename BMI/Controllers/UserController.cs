using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMI.Data;
using BMI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BMI.Controllers
{
    public class UserController : Controller
    {

        private readonly ApplicationDbContext _db;

        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }

        //public IActionResult List()
        //{
        //    IEnumerable<Usermodel> list = _db.User;
        //    return View(list);
        //}


        //public IActionResult SignUp()
        //{
            
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult SignUp(Usermodel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        return RedirectToAction("SignIn");    
        //    }
        //    return View();
        //}

        public IActionResult SignIn()
        {
            return View();
        }
    }
}
