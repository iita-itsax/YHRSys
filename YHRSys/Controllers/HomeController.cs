﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Net;
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
using System.Diagnostics;
namespace YHRSys.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            dynamic model = new ExpandoObject();
            var reagents = (from rea in db.Reagents join st in db.Stocks on rea.reagentId equals st.reagentId into reagentInfo 
                            from reagentStock in reagentInfo.DefaultIfEmpty() orderby rea.inventories.Select(j=>j.createdDate).FirstOrDefault() descending
                            select new CustomReagentsViewModel
                            {
                                rName = rea.name,
                                createdDate =  rea.inventories.Select(i => i.createdDate).FirstOrDefault(),
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
                              }).OrderByDescending(j=>j.activityDate).Take(3);
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
            //Debug.WriteLine("TESTING CONSOLE WRITEPAD");
            return View(model);
        }

        public ActionResult ReOrderLevelStatus()
        {
            var reagentReOrderStatus = (from rea in db.Reagents
                            join st in db.Stocks on rea.reagentId equals st.reagentId into reagentInfo
                            from stocks in reagentInfo.AsEnumerable()
                            .DefaultIfEmpty() where rea.reOrderLevel >= stocks.totalIn || stocks.totalIn == null || stocks.totalIn == 0 orderby rea.name ascending
                            select new
                            {
                                rName = rea.name,
                                qtySum = (int?) stocks.totalIn ?? 0
                            }).ToList();

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

        public ActionResult SiteMenuList()
        {
            var menus = (from rea in db.SiteContents
                                        where rea.status == PUBLISHSTATUS.Published
                                        orderby rea.caption ascending
                                        select new
                                        {
                                            rName = rea.caption,
                                            id = (int?)rea.id ?? 0
                                        }).ToList();

            StringBuilder sb = new StringBuilder("");
            sb.Append("<li><a href=\"/Home/Index\">Home</a></li>");
            sb.Append("<li class=\"divider\"></li>");
            sb.Append("<li><a href=\"/AvailableVariety/Index\">Order Varieties</a></li>");
            foreach (var menu in menus)
            {
                if (sb.Length > 0) { 
                    sb.Append("<li class=\"divider\"></li>");
                    sb.Append("<li><a href=\"/Home/Content/").Append(menu.id).Append("\">").Append(menu.rName).Append("</a></li>");
                }
                else
                {
                    sb.Append("<li><a href=\"/Home/Content/").Append(menu.id).Append("\">").Append(menu.rName).Append("</a></li>");
                }
            }
            ViewBag.Menus = sb.ToString();
            return PartialView("_SiteMenuList");
        }

        public ActionResult Content(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteContent sitecontent = db.SiteContents.Find(id);
            if (sitecontent == null)
            {
                return HttpNotFound();
            }
            return View(sitecontent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}