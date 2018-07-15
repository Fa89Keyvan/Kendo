using Kendo.Models.Kendo;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kendo.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Persons()
        {
            var gridRequest = new GridRequest(Request.QueryString);
            var sort = gridRequest.Sorts.Any() ? gridRequest.Sorts[0] : null;
            var list = Person.GetList();
            
            var model = list.OrderBy(sort).Skip(gridRequest.skip).Take(gridRequest.take);

            return Json(new { HasError = false,Data = new { List = model }, total = list.Count() },JsonRequestBehavior.AllowGet);
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static List<Person> GetList()
        {
            var lst = new List<Person>();

            for (int i = 0; i < 100; i++)
            {
                lst.Add(new Person { Id = i, Name = $"Name {i}" });
            }
            return lst;
        }
    }
}