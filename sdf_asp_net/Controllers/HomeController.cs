
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sdf_asp_net.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult MyCalendar()
        {
            ViewBag.Message = "Calendar info";

            return View();
        }

        public JsonResult GetEvents()
        {
            using (MyCalendarEntities dc = new MyCalendarEntities())
            {
                var events = dc.Events.ToList();
                return new JsonResult {Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet};
            }
        }

        public void AddEvents(Event e)
        {
            using (MyCalendarEntities dc = new MyCalendarEntities())
            {
                e.EventID = dc.Events.Count() +1 ;
                dc.Events.Add(e);
                dc.SaveChanges();
            }
        }
    }
}