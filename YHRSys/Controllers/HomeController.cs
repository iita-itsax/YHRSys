using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YHRSys.Models;
using System.Dynamic;
using System.Web.Security;

namespace YHRSys.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            dynamic model = new ExpandoObject();

            var reagents = (from r in db.Reagents
                            select new CustomReagentsViewModel
                            {
                                rName = r.name,
                                createdDate = r.inventories.Select(i => i.baseDateEntity.createdDate).FirstOrDefault(),
                                quantity = r.inventories.Select(i => i.quantity).FirstOrDefault(),
                                qtySum = r.inventories.Sum(i => i.quantity)
                            }).Take(3);
            model.Reagents = reagents;
            ViewBag.reagentCounter = reagents.Count();

            var activities = (from a in db.Activities
                              orderby a.baseDateEntity.createdDate descending
                              select new CustomActivitiesViewModel
                              {
                                  lName = a.location.name,
                                  activityName = a.name,
                                  description = a.description
                              }).Take(3);
            model.Activities = activities;
            ViewBag.activityCounter = activities.Count();

            var locations = (from l in db.Locations
                             orderby l.baseDateEntity.createdDate descending
                             select new CustomLocationViewModel
                             {
                                 lName = l.name,
                                 locSource = l.source,
                                 locNumber = l.locNumber
                             }).Take(3);
            model.Locations = locations;
            ViewBag.locationCounter = locations.Count();

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "About Us";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Our Contacts";

            return View();
        }
    }
}