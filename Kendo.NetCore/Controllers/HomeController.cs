using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Kendo.NetCore.Models;

namespace Kendo.NetCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Persons()
        {
            var model = Models.Person.GetList();
            return Json(new { Data = model, Total = model.Count() });
        }
    }
}
