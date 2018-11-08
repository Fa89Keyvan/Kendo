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

        [HttpPost]
        public ActionResult Persons(DataSourceRequest dataSourceRequest,string name)
        {
            var token = Request.Headers["token"];
            var list = Person.GetList();

            var query = list.AsQueryable();
            if (dataSourceRequest.sort != null && dataSourceRequest.sort.Any())
            {
                var fieldName = dataSourceRequest.sort[0].field;
                var dir = dataSourceRequest.sort[0].dir;

                query = ((dir == "asc") ? query.OrderBy(fieldName) : query.OrderByByDescending(fieldName)).AsQueryable();
            }

            var model = query.Skip(dataSourceRequest.skip ?? 0).Take(dataSourceRequest.take ?? int.MaxValue);
            return Json(new { HasError = false,Data = new { List = model }, total = list.Count() },JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPersons()
        {
            return Json(Person.GetList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult TsTest()
        {
            return View();
        }
    }
    public class KendoSort
    {
        public string field { get; set; }
        public string dir { get; set; }
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