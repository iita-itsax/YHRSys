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
using System.Web.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Infrastructure;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Text;
namespace YHRSys.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            dynamic model = new ExpandoObject();
            var reagents = (from rea in db.Reagents join st in db.Stocks on rea.reagentId equals st.reagentId into reagentInfo from reagentStock in reagentInfo.DefaultIfEmpty()
                            select new CustomReagentsViewModel
                            {
                                rName = rea.name,
                                createdDate = rea.inventories.Select(i => i.createdDate).FirstOrDefault(),
                                quantity = rea.inventories.Select(i => i.quantity).FirstOrDefault(),
                                qtySum = reagentStock.totalIn //.inventories.Sum(i => i.quantity)
                            }).Take(3);
            model.Reagents = reagents;
            ViewBag.reagentCounter = reagents.Count();

            var activities = (from a in db.Activities
                              orderby a.createdDate descending
                              select new CustomActivitiesViewModel
                              {
                                  lName = a.location.name,
                                  activityName = a.activityDefinition.name,
                                  description = a.description,
                                  activityDate = a.activityDate
                              }).Take(3);
            model.Activities = activities;
            ViewBag.activityCounter = activities.Count();

            var locations = (from l in db.Locations
                             orderby l.createdDate descending
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
        
        /*
        public ActionResult DrawChart()
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());//pn = r.partner.name, 
            var myChart = new Chart(width: 600, height: 400)
            .AddTitle("Resource Utilization in Projects in Week 1")
            .AddSeries(
                name: "Project1",
                chartType: "Column",
                xValue: new[] { "W1", "W2", "W3", "W4", "W5" },
                yValues: new[] { 80, 60, 40, 20, 10 }
            )
            .AddSeries(
                name: "Project2",
                chartType: "Column",
                xValue: new[] { "W1", "W2", "W3", "W4", "W5" },
                yValues: new[] { 10, 10, 0, 10, 10 }
            )
            .AddSeries(
                name: "Available",
                chartType: "Column",
                xValue: new[] { "W1", "W2", "W3", "W4", "W5" },
                yValues: new[] { "10", "30", "50", "70", "80" }
            )
            .AddLegend();

            myChart.Write();
            myChart.Save("~/Content/home_chart" + currentUser.Id, "jpeg");
            // Return the contents of the Stream to the client
            return base.File("~/Content/home_chart" + currentUser.Id, "jpeg");
        }
        */
        public ActionResult ReOrderLevelStatus()
        {
            var reagentReOrderStatus = (from rea in db.Reagents
                            join st in db.Stocks on rea.reagentId equals st.reagentId into reagentInfo
                            from reagentStock in reagentInfo.DefaultIfEmpty() where rea.reOrderLevel>=reagentStock.totalIn
                            select new
                            {
                                rName = rea.name,
                                createdDate = rea.inventories.Select(i => i.createdDate).FirstOrDefault(),
                                quantity = rea.inventories.Select(i => i.quantity).FirstOrDefault(),
                                qtySum = reagentStock.totalIn 
                            }).ToList();
            //model.Reagents = reagents;
            //ViewBag.reagentCounter = reagents.Count();
            StringBuilder sb = new StringBuilder("");
            
            foreach(var lev in reagentReOrderStatus){
                sb.Append("<li class='list-group-item'>").Append("<span class='badge'>").Append(lev.qtySum).Append("</span>").Append(lev.rName).Append("</span></li>");
            }
            if(sb.Length>0){
                sb.Insert(0, "<div class='row'><div class='col-lg-12'><h4>Re-Order Now!</h4></div></div><div class='row'><div class='col-lg-12'><div class='bs-component'><ul class='list-group'>");
                sb.Append("<li class='list-group-item alert alert-dismissible alert-danger'><strong>Hint:</strong> Above info signifies stock level of corresponding reagents. Place an order now!</li></ul></div></div></div>");
            }
            ViewBag.Status = sb.ToString();
            return PartialView("_ReagentReOrderStatus");
        }
    }
}