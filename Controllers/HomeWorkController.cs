using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using slnAJAX.Models;
using System.Xml.Linq;

namespace slnAJAX.Controllers
{
    public class HomeWorkController : Controller
    {
        DemoContext _context = new();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Regist()
        {
            return View();
        }
        public IActionResult RegionList()
        {
            return View();
        }
        public IActionResult City()
        {
            var cities = from i in _context.Addresses select i.City;
            cities = cities.Distinct();
            return Json(cities);
        }
        public IActionResult Site(string city)
        {
            var sites = from i in _context.Addresses
                        where i.City == city
                        select i.SiteId;
            sites = sites.Distinct();
            return Json(sites);
        }
        public IActionResult Road(string siteid)
        {
            var roads = from i in _context.Addresses
                        where i.SiteId == siteid
                        select i.Road;
            roads = roads.Distinct();
            return Json(roads);
        }
        public IActionResult Check(string name)
        {
            DemoContext context = new();
            var Q = from u in context.Members
                    where u.Name == name
                    select u.Name;
            if (!Q.Any())
            {
                return Content("帳號可以使用", "text/plain", System.Text.Encoding.UTF8);
            }
            return Content("帳號已存在", "text/plain", System.Text.Encoding.UTF8);
        }
        public IActionResult First()
        {
            return View();
        }
    }
}
