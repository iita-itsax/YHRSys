﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YHRSys.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Infrastructure;
using Microsoft.Owin.Security;
using System.Security.Claims;
using PagedList;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.Helpers;
using System.Collections;

namespace YHRSys.Controllers
{
    [Authorize]
    public class PartnerActivityController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private int diffInQty = 0;
        AccessRoleCheck accessRoleCheck = new AccessRoleCheck();
        string[] groups = new string[] { "Partner" };


        // GET: /PartnerActivity/
        [Authorize(Roles = "Admin, CanViewPartnerActivity, CanViewOwnPartnerActivity, PartnerActivity")]
        public ActionResult Index(string sortOrder, string currentFilter, string currentStartDateFilter, string currentEndDateFilter, string searchString, DateTime? searchStartActivityDate, DateTime? searchEndActivityDate, int? page, string btnSubmit)
        {
            string perms = "CanViewOwnPartnerActivity";
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            ApplicationUser accessUser = manager.FindById(User.Identity.GetUserId());
            Boolean OwnAccess = accessRoleCheck.checkOwnPerm(groups, accessUser, perms);

            switch (btnSubmit)
            {
                case "Print Report!":
                    return ExportReport(searchString, searchStartActivityDate, searchEndActivityDate);
                default:
                    ViewBag.CurrentSort = sortOrder;
                    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                    ViewBag.ReagentSortParm = sortOrder == "Reagent" ? "reagent_desc" : "Reagent";
                    ViewBag.VarietySortParm = sortOrder == "Variety" ? "variety_desc" : "Variety";
                    ViewBag.GiverSortParm = sortOrder == "Giver" ? "giver_desc" : "Giver";
                    ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                    ViewBag.OiCNameSortParm = sortOrder == "OiC" ? "oicnaame_desc" : "OiC";
                    ViewBag.QuantitySortParm = sortOrder == "Quantity" ? "quantity_desc" : "Quantity";
                    //ViewBag.VarQuantitySortParm = sortOrder == "VarQuantity" ? "varquantity_desc" : "VarQuantity";

                    if (searchString != null)
                    {
                        page = 1;
                    }
                    else
                    {
                        searchString = currentFilter;
                    }

                    ViewBag.CurrentFilter = searchString;

                    if (searchStartActivityDate != null)
                    {
                        page = 1;
                    }
                    else
                    {
                        if (currentStartDateFilter != null)
                            searchStartActivityDate = DateTime.Parse(currentStartDateFilter.ToString());
                    }
                    ViewBag.CurrentStartDateFilter = searchStartActivityDate;

                    if (searchEndActivityDate != null)
                    {
                        page = 1;
                    }
                    else
                    {
                        if (currentEndDateFilter != null)
                            searchEndActivityDate = DateTime.Parse(currentEndDateFilter.ToString());
                    }
                    ViewBag.CurrentEndDateFilter = searchEndActivityDate;

                    if (searchStartActivityDate != null)
                        searchStartActivityDate = DateTime.Parse(searchStartActivityDate.ToString());
                    if (searchEndActivityDate != null)
                        searchEndActivityDate = DateTime.Parse(searchEndActivityDate.ToString());

                    var activities = from r in db.PartnerActivities select r;

                    if (!String.IsNullOrEmpty(searchString))
                    {
                        if (searchStartActivityDate != null && searchEndActivityDate != null)
                        {
                            activities = activities.Where(rg => (rg.giver.name.Contains(searchString)
                                               || rg.partner.name.Contains(searchString)
                                               || rg.variety.varietyDefinition.name.Contains(searchString)
                                               || rg.reagent.name.Contains(searchString) || rg.oic.LastName.Contains(searchString)
                                                  || rg.oic.FirstName.Contains(searchString) || rg.backStopping.Contains(searchString)) && (rg.activityDate >= (DateTime)searchStartActivityDate && rg.activityDate <= (DateTime)searchEndActivityDate));
                        }
                        else if (searchStartActivityDate != null)
                        {
                            activities = activities.Where(rg => (rg.giver.name.Contains(searchString)
                                               || rg.partner.name.Contains(searchString) 
                                               || rg.variety.varietyDefinition.name.Contains(searchString)
                                               || rg.reagent.name.Contains(searchString) || rg.oic.LastName.Contains(searchString)
                                                  || rg.oic.FirstName.Contains(searchString) || rg.backStopping.Contains(searchString)) && (rg.activityDate == (DateTime)searchStartActivityDate));
                        }
                        else if (searchEndActivityDate != null)
                        {
                            activities = activities.Where(rg => (rg.giver.name.Contains(searchString)
                                               || rg.partner.name.Contains(searchString)
                                               || rg.variety.varietyDefinition.name.Contains(searchString)
                                               || rg.reagent.name.Contains(searchString) || rg.oic.LastName.Contains(searchString)
                                                  || rg.oic.FirstName.Contains(searchString) || rg.backStopping.Contains(searchString)) && (rg.activityDate == (DateTime)searchEndActivityDate));
                        }
                        else
                        {
                            activities = activities.Where(rg => rg.giver.name.Contains(searchString)
                                               || rg.partner.name.Contains(searchString)
                                               || rg.variety.varietyDefinition.name.Contains(searchString)
                                               || rg.reagent.name.Contains(searchString) || rg.oic.LastName.Contains(searchString)
                                                  || rg.oic.FirstName.Contains(searchString) || rg.backStopping.Contains(searchString));
                        }
                    }
                    else
                    {
                        if (searchStartActivityDate != null && searchEndActivityDate != null)
                        {
                            activities = activities.Where(rg => (rg.activityDate >= (DateTime)searchStartActivityDate && rg.activityDate <= (DateTime)searchEndActivityDate));
                        }
                        else if (searchStartActivityDate != null)
                        {
                            activities = activities.Where(rg => rg.activityDate == (DateTime)searchStartActivityDate);
                        }
                        else if (searchEndActivityDate != null)
                        {
                            activities = activities.Where(rg => rg.activityDate <= (DateTime)searchEndActivityDate);
                        }
                    }

                    if (OwnAccess)
                    {
                        activities = activities.Where(rg => rg.createdBy == accessUser.UserName || rg.partnerId == accessUser.partnerId);
                    }

                    switch (sortOrder)
                    {
                        case "name_desc":
                            activities = activities.OrderByDescending(rg => rg.partner.name);
                            break;
                        case "giver_desc":
                            activities = activities.OrderByDescending(rg => rg.giver.name);
                            break;
                        case "Giver":
                            activities = activities.OrderBy(rg => rg.giver.name);
                            break;
                        case "reagent_desc":
                            activities = activities.OrderByDescending(rg => rg.reagent.name);
                            break;
                        case "Reagent":
                            activities = activities.OrderBy(rg => rg.reagent.name);
                            break;
                        case "variety_desc":
                            activities = activities.OrderByDescending(rg => rg.variety.varietyDefinition.name);
                            break;
                        case "Variety":
                            activities = activities.OrderBy(rg => rg.variety.varietyDefinition.name);
                            break;
                        case "oicname_desc":
                            activities = activities.OrderByDescending(rg => rg.oic.LastName).ThenBy(rg => rg.oic.FirstName);
                            break;
                        case "OiC":
                            activities = activities.OrderBy(rg => rg.oic.LastName).ThenBy(rg => rg.oic.FirstName);
                            break;
                        case "quantity_desc":
                            activities = activities.OrderByDescending(rg => rg.reagentQty);
                            break;
                        case "Quantity":
                            activities = activities.OrderBy(rg => rg.reagentQty);
                            break;
                        case "Date":
                            activities = activities.OrderBy(rg => rg.activityDate);
                            break;
                        case "date_desc":
                            activities = activities.OrderByDescending(rg => rg.activityDate);
                            break;
                        default:  // Name ascending 
                            activities = activities.OrderBy(rg => rg.partner.name);
                            break;
                    }

                    int pageSize = 10;
                    int pageNumber = (page ?? 1);
                    return View(activities.ToPagedList(pageNumber, pageSize));
            }
            
            //var tblpartneractivities = db.PartnerActivities;
            //return View(tblpartneractivities.ToList());
        }

        // GET: /PartnerActivity/Details/5
        [Authorize(Roles = "Admin, CanViewPartnerActivity, CanViewOwnPartnerActivity, PartnerActivity")]
        public ActionResult Details(long? id)
        {
            string perms = "CanViewOwnPartnerActivity";
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            ApplicationUser accessUser = manager.FindById(User.Identity.GetUserId());
            Boolean OwnAccess = accessRoleCheck.checkOwnPerm(groups, accessUser, perms);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tblpartneractivity = db.PartnerActivities.Find(id);

            if (OwnAccess)
            {
                tblpartneractivity = db.PartnerActivities.Where(rg => rg.partnerActivityId==id && rg.createdBy == accessUser.UserName).FirstOrDefault();
            }

            if (tblpartneractivity == null)
            {
                return HttpNotFound();
            }
            var varQtyGiven = tblpartneractivity.tcPlantletsGiven + tblpartneractivity.bioreactorplantsGiven + tblpartneractivity.tubersGiven + tblpartneractivity.seedsGiven;
            var varQtyAvailable = tblpartneractivity.tcPlantletsAvailable + tblpartneractivity.tibPlantletsAvailable + tblpartneractivity.tubersAvailable + tblpartneractivity.seedsAvailable;
            
            ViewBag.VarietyQtyGiven = varQtyGiven;
            ViewBag.VarietyQtyAvailable = varQtyAvailable;

            ViewBag.varietyId = new SelectList(db.Varieties, "varietyId", "FullDescription");
            ViewBag.reagentId = new SelectList(db.Reagents, "reagentId", "name");
            return View(tblpartneractivity);
        }

        // GET: /PartnerActivity/Create
        [Authorize(Roles = "Admin, CanAddPartnerActivity, CanAddOwnPartnerActivity, PartnerActivity")]
        public ActionResult Create()
        {
            string perms = "CanAddOwnPartnerActivity";
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            ApplicationUser accessUser = manager.FindById(User.Identity.GetUserId());
            Boolean OwnAccess = accessRoleCheck.checkOwnPerm(groups, accessUser, perms);

            //try {
                ViewBag.reagentId = new SelectList(db.Reagents.Join(db.Stocks, r => r.reagentId, s => s.reagentId, (r, s) => new { Reagent = r, Stock = s }).Where(t => t.Stock.reagentId == t.Reagent.reagentId && (t.Stock.totalIn - t.Stock.totalOut) > 0).Select(m => new { m.Reagent.reagentId, m.Reagent.name }), "reagentId", "name");
                if (OwnAccess)
                {
                    ViewBag.partnerId = new SelectList(db.Partners.Where(p => p.partnerId == accessUser.partnerId), "partnerId", "name");
                    ViewBag.giverId = new SelectList(db.Partners.Where(p => p.partnerId != accessUser.partnerId), "partnerId", "name");
                    ViewBag.userId = new SelectList(db.Users.Where(u => u.partnerId == accessUser.partnerId), "Id", "FullName");
                }
                else
                {
                    ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name");
                    ViewBag.giverId = new SelectList(db.Partners, "partnerId", "name");
                    ViewBag.userId = new SelectList(db.Users, "Id", "FullName");
                }
                var varieties = db.Varieties.ToList()
                .Select(v => new
                {
                    varietyId = v.varietyId,
                    description = string.Format("{0}--{1}", v.varietyDefinition.name, v.sampleNumber)
                });
                ViewBag.varietyId = new SelectList(varieties, "varietyId", "description");

                //ViewBag.seedlingId = new SelectList(db.Seedlings, "seedlingId", "name");

            //}catch(Exception ex){
            //    throw ex;
            //}
            return View();
        }

        // POST: /PartnerActivity/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanAddPartnerActivity, CanAddOwnPartnerActivity, PartnerActivity")]
        public ActionResult Create([Bind(Include = "partnerId,giverId,reagentId,reagentQty,varietyId,backStopping,tcPlantletsGiven,bioreactorplantsGiven,tubersGiven,tcPlantletsAvailable,tibPlantletsAvailable,tubersAvailable,activityDate,userId,seedsGiven,seedsAvailable")] PartnerActivity tblpartneractivity)
        {
            string perms = "CanAddOwnPartnerActivity";
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            ApplicationUser currentUser = manager.FindById(User.Identity.GetUserId());
            Boolean OwnAccess = accessRoleCheck.checkOwnPerm(groups, currentUser, perms);

            var varieties = db.Varieties.ToList()
                .Select(v => new
                {
                  varietyId = v.varietyId,
                  description = string.Format("{0}--{1}", v.varietyDefinition.name, v.sampleNumber)
                });

            if (ModelState.IsValid)
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var loc = db.PartnerActivities.FirstOrDefault(p => p.partnerId == tblpartneractivity.partnerId && p.giverId == tblpartneractivity.giverId && p.reagentId == tblpartneractivity.reagentId && p.userId == tblpartneractivity.userId && p.activityDate == tblpartneractivity.activityDate);
                        if (loc == null)
                        {
                            //Call stock tracker table for STOCK-IN TRANSACTION
                            var stocklevel = db.Stocks.Where(c => c.reagentId == tblpartneractivity.reagentId).FirstOrDefault();
                            if (stocklevel != null)
                            {
                                if ((stocklevel.totalIn - tblpartneractivity.reagentQty) >= 0)
                                {
                                    stocklevel.reagentId = Convert.ToInt32(tblpartneractivity.reagentId);
                                    stocklevel.totalIn = stocklevel.totalIn - Convert.ToInt32(tblpartneractivity.reagentQty);
                                    db.SaveChanges();

                                    //var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                                    //var currentUser = manager.FindById(User.Identity.GetUserId());

                                    if (currentUser != null)
                                        tblpartneractivity.createdBy = currentUser.UserName;
                                    else
                                        tblpartneractivity.createdBy = User.Identity.Name;

                                    tblpartneractivity.createdDate = DateTime.Now;

                                    db.PartnerActivities.Add(tblpartneractivity);
                                    db.SaveChanges();

                                    dbContextTransaction.Commit();
                                }
                                else
                                {
                                    ModelState.AddModelError(string.Empty, "Reagent quantity entered: {" + tblpartneractivity.reagentQty + "} is more than Stock level: {" + stocklevel.totalIn + "}!. "
                                    + "Reduce quantity entered and try again.");
                                    ViewBag.reagentId = new SelectList(db.Reagents, "reagentId", "name", tblpartneractivity.reagentId);
                                    //ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.partner.partnerId);
                                    //ViewBag.giverId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.giver.partnerId);
                                    ViewBag.varietyId = new SelectList(varieties, "varietyId", "description");
                                    //ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblpartneractivity.userId);

                                    if (OwnAccess)
                                    {
                                        ViewBag.partnerId = new SelectList(db.Partners.Where(p => p.partnerId == currentUser.partnerId), "partnerId", "name", tblpartneractivity.partnerId);
                                        ViewBag.giverId = new SelectList(db.Partners.Where(p => p.partnerId != currentUser.partnerId), "partnerId", "name", tblpartneractivity.giverId);
                                        ViewBag.userId = new SelectList(db.Users.Where(u => u.partnerId == currentUser.partnerId), "Id", "FullName", tblpartneractivity.userId);
                                    }
                                    else
                                    {
                                        ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.partnerId);
                                        ViewBag.giverId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.giverId);
                                        ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblpartneractivity.userId);
                                    }

                                    return View(tblpartneractivity);
                                }
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Reagent selected could not be found in the system!. "
                                    + "Please try again or contact the System Administrator.");
                                ViewBag.reagentId = new SelectList(db.Reagents, "reagentId", "name", tblpartneractivity.reagentId);
                                //ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.partner.partnerId);
                                //ViewBag.giverId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.giver.partnerId);
                                //ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblpartneractivity.userId);
                                ViewBag.varietyId = new SelectList(varieties, "varietyId", "description");
                                //ViewBag.seedlingId = new SelectList(db.Seedlings, "seedlingId", "name", tblpartneractivity.varietyId);
                                if (OwnAccess)
                                {
                                    ViewBag.partnerId = new SelectList(db.Partners.Where(p => p.partnerId == currentUser.partnerId), "partnerId", "name", tblpartneractivity.partnerId);
                                    ViewBag.giverId = new SelectList(db.Partners.Where(p => p.partnerId != currentUser.partnerId), "partnerId", "name", tblpartneractivity.giverId);
                                    ViewBag.userId = new SelectList(db.Users.Where(u => u.partnerId == currentUser.partnerId), "Id", "FullName", tblpartneractivity.userId);
                                }
                                else
                                {
                                    ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.partnerId);
                                    ViewBag.giverId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.giverId);
                                    ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblpartneractivity.userId);
                                }
                                return View(tblpartneractivity);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Partner activity and/or reagent usage already entered & quantity also deducted from the stock: " + loc.reagent.name);
                            ViewBag.reagentId = new SelectList(db.Reagents, "reagentId", "name", tblpartneractivity.reagentId);
                            //ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.partner.partnerId);
                            //ViewBag.giverId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.giver.partnerId);
                            //ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblpartneractivity.userId);
                            ViewBag.varietyId = new SelectList(varieties, "varietyId", "description");
                            //ViewBag.seedlingId = new SelectList(db.Seedlings, "seedlingId", "name", tblpartneractivity.varietyId);

                            if (OwnAccess)
                            {
                                ViewBag.partnerId = new SelectList(db.Partners.Where(p => p.partnerId == currentUser.partnerId), "partnerId", "name", tblpartneractivity.partnerId);
                                ViewBag.giverId = new SelectList(db.Partners.Where(p => p.partnerId != currentUser.partnerId), "partnerId", "name", tblpartneractivity.giverId);
                                ViewBag.userId = new SelectList(db.Users.Where(u => u.partnerId == currentUser.partnerId), "Id", "FullName", tblpartneractivity.userId);
                            }
                            else
                            {
                                ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.partnerId);
                                ViewBag.giverId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.giverId);
                                ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblpartneractivity.userId);
                            }

                            return View(tblpartneractivity);
                        }
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        if (dbContextTransaction != null) dbContextTransaction.Rollback();
                        ModelState.AddModelError(string.Empty, "Error occurred saving partner activity. " + "\n\nError message: " + ex.Message);
                        ViewBag.reagentId = new SelectList(db.Reagents, "reagentId", "name", tblpartneractivity.reagentId);
                        //ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.partner.partnerId);
                        //ViewBag.giverId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.giver.partnerId);
                        //ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblpartneractivity.userId);
                        ViewBag.varietyId = new SelectList(varieties, "varietyId", "description");
                        //ViewBag.seedlingId = new SelectList(db.Seedlings, "seedlingId", "name", tblpartneractivity.varietyId);
                        if (OwnAccess)
                        {
                            ViewBag.partnerId = new SelectList(db.Partners.Where(p => p.partnerId == currentUser.partnerId), "partnerId", "name", tblpartneractivity.partnerId);
                            ViewBag.giverId = new SelectList(db.Partners.Where(p => p.partnerId != currentUser.partnerId), "partnerId", "name", tblpartneractivity.giverId);
                            ViewBag.userId = new SelectList(db.Users.Where(u => u.partnerId == currentUser.partnerId), "Id", "FullName", tblpartneractivity.userId);
                        }
                        else
                        {
                            ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.partnerId);
                            ViewBag.giverId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.giverId);
                            ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblpartneractivity.userId);
                        }

                        return View(tblpartneractivity);
                    }
                }
            }
            ViewBag.reagentId = new SelectList(db.Reagents, "reagentId", "name", tblpartneractivity.reagentId);
            //ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.partner.partnerId);
            //ViewBag.giverId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.giver.partnerId);
            //ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblpartneractivity.userId);

            if (OwnAccess)
            {
                ViewBag.partnerId = new SelectList(db.Partners.Where(p => p.partnerId == currentUser.partnerId), "partnerId", "name", tblpartneractivity.partnerId);
                ViewBag.giverId = new SelectList(db.Partners.Where(p => p.partnerId != currentUser.partnerId), "partnerId", "name", tblpartneractivity.giverId);
                ViewBag.userId = new SelectList(db.Users.Where(u => u.partnerId == currentUser.partnerId), "Id", "FullName", tblpartneractivity.userId);
            }
            else
            {
                ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.partnerId);
                ViewBag.giverId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.giverId);
                ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblpartneractivity.userId);
            }

            ViewBag.varietyId = new SelectList(varieties, "varietyId", "description");
            //ViewBag.seedlingId = new SelectList(db.Seedlings, "seedlingId", "name", tblpartneractivity.varietyId);

            return View(tblpartneractivity);
        }

        // GET: /PartnerActivity/Edit/5
        [Authorize(Roles = "Admin, CanEditPartnerActivity, CanEditOwnPartnerActivity, PartnerActivity")]
        public ActionResult Edit(long? id)
        {
            string perms = "CanEditOwnPartnerActivity";
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            ApplicationUser accessUser = manager.FindById(User.Identity.GetUserId());
            Boolean OwnAccess = accessRoleCheck.checkOwnPerm(groups, accessUser, perms);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tblpartneractivity = db.PartnerActivities.Find(id);

            if (OwnAccess)
            {
                tblpartneractivity = db.PartnerActivities.Where(p => p.partnerActivityId == id && p.createdBy == accessUser.UserName).FirstOrDefault();
            }

            if (tblpartneractivity == null)
            {
                return HttpNotFound();
            }
            ViewBag.reagentId = new SelectList(db.Reagents, "reagentId", "name", tblpartneractivity.reagentId);
            //ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.partner.partnerId);
            //ViewBag.giverId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.giver.partnerId);
            //ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblpartneractivity.userId);
            if (OwnAccess)
            {
                ViewBag.partnerId = new SelectList(db.Partners.Where(p => p.partnerId == accessUser.partnerId), "partnerId", "name", tblpartneractivity.partnerId);
                ViewBag.giverId = new SelectList(db.Partners.Where(p => p.partnerId != accessUser.partnerId), "partnerId", "name", tblpartneractivity.giverId);
                ViewBag.userId = new SelectList(db.Users.Where(u => u.partnerId == accessUser.partnerId), "Id", "FullName", tblpartneractivity.userId);
            }
            else
            {
                ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.partnerId);
                ViewBag.giverId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.giverId);
                ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblpartneractivity.userId);
            }

            var varieties = db.Varieties.ToList()
                .Select(v => new
                {
                    varietyId = v.varietyId,
                    description = string.Format("{0}--{1}", v.varietyDefinition.name, v.sampleNumber)
                });

            ViewBag.varietyId = new SelectList(varieties, "varietyId", "description");
            //ViewBag.seedlingId = new SelectList(db.Seedlings, "seedlingId", "name", tblpartneractivity.varietyId);
            return View(tblpartneractivity);
        }

        // POST: /PartnerActivity/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanEditPartnerActivity, CanEditOwnPartnerActivity, PartnerActivity")]
        public ActionResult Edit([Bind(Include = "partnerActivityId,partnerId,giverId,reagentId,reagentQty,varietyId,backStopping,tcPlantletsGiven,bioreactorplantsGiven,tubersGiven,tcPlantletsAvailable,tibPlantletsAvailable,tubersAvailable,userId,activityDate,seedsGiven,seedsAvailable")] PartnerActivity tblpartneractivity)
        {
            string perms = "CanEditOwnPartnerActivity";
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            ApplicationUser currentUser = manager.FindById(User.Identity.GetUserId());
            Boolean OwnAccess = accessRoleCheck.checkOwnPerm(groups, currentUser, perms);

            var varieties = db.Varieties.ToList()
                .Select(v => new
                {
                    varietyId = v.varietyId,
                    description = string.Format("{0}--{1}", v.varietyDefinition.name, v.sampleNumber)
                });

            if (ModelState.IsValid)
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //db.Entry(tbllocation).State = EntityState.Modified;
                        //var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                        //var currentUser = manager.FindById(User.Identity.GetUserId());

                        var act = db.PartnerActivities.Where(c => c.partnerActivityId == tblpartneractivity.partnerActivityId).FirstOrDefault();

                        if (OwnAccess)
                        {
                            act = db.PartnerActivities.Where(p => p.partnerActivityId == tblpartneractivity.partnerActivityId && p.createdBy == currentUser.UserName).FirstOrDefault();
                        }

                        if (act != null)
                            diffInQty = Convert.ToInt32(tblpartneractivity.reagentQty) - Convert.ToInt32(act.reagentQty);

                        if (diffInQty > 0)//Update the reagent usage and stock tables
                        {
                            //Call stock tracker table for STOCK-IN TRANSACTION
                            var stocklevel = db.Stocks.Where(c => c.reagentId == tblpartneractivity.reagentId).FirstOrDefault();
                            if (stocklevel != null)
                            {
                                if (stocklevel.totalIn > diffInQty)
                                {
                                    act.partner.partnerId = tblpartneractivity.partnerId;//tblpartneractivity.partner.partnerId
                                    act.giver.partnerId = tblpartneractivity.giverId;//tblpartneractivity.giver.partnerId
                                    act.reagentId = tblpartneractivity.reagentId;
                                    act.varietyId = tblpartneractivity.varietyId;
                                    act.userId = tblpartneractivity.userId;
                                    act.activityDate = tblpartneractivity.activityDate;
                                    act.backStopping = tblpartneractivity.backStopping;
                                    act.tcPlantletsGiven = tblpartneractivity.tcPlantletsGiven;
                                    act.bioreactorplantsGiven = tblpartneractivity.bioreactorplantsGiven;
                                    act.tubersGiven = tblpartneractivity.tubersGiven;
                                    act.tcPlantletsAvailable = tblpartneractivity.tcPlantletsAvailable;
                                    act.tibPlantletsAvailable = tblpartneractivity.tibPlantletsAvailable;
                                    act.tubersAvailable = tblpartneractivity.tubersAvailable;

                                    act.seedsAvailable = tblpartneractivity.seedsAvailable;
                                    act.seedsGiven = tblpartneractivity.seedsGiven;

                                    act.reagentQty = tblpartneractivity.reagentQty;
                                    //act.varietyQty = tblpartneractivity.varietyQty;

                                    if (currentUser != null)
                                        act.updatedBy = currentUser.UserName;
                                    else
                                        act.updatedBy = User.Identity.Name;

                                    act.updatedDate = DateTime.Now;

                                    db.SaveChanges();

                                    stocklevel.reagentId = Convert.ToInt32(tblpartneractivity.reagentId);
                                    stocklevel.totalIn = stocklevel.totalIn - diffInQty;
                                    db.SaveChanges();

                                    dbContextTransaction.Commit();
                                }
                                else
                                {
                                    ModelState.AddModelError(string.Empty, "Balance of Reagent quantity entered to be deducted from the stock level: {" + diffInQty + "} is more than Stock level: {" + stocklevel.totalIn + "}!. "
                                    + "Reduce quantity entered and try again.");
                                    ViewBag.reagentId = new SelectList(db.Reagents, "reagentId", "name", tblpartneractivity.reagentId);
                                    //ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.partnerId);//tblpartneractivity.partner.partnerId
                                    //ViewBag.giverId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.giverId);//tblpartneractivity.giver.partnerId
                                    //ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblpartneractivity.userId);
                                    ViewBag.varietyId = new SelectList(varieties, "varietyId", "description");
                                    //ViewBag.seedlingId = new SelectList(db.Seedlings, "seedlingId", "name", tblpartneractivity.varietyId);

                                    if (OwnAccess)
                                    {
                                        ViewBag.partnerId = new SelectList(db.Partners.Where(p => p.partnerId == currentUser.partnerId), "partnerId", "name", tblpartneractivity.partnerId);
                                        ViewBag.giverId = new SelectList(db.Partners.Where(p => p.partnerId != currentUser.partnerId), "partnerId", "name", tblpartneractivity.giverId);
                                        ViewBag.userId = new SelectList(db.Users.Where(u => u.partnerId == currentUser.partnerId), "Id", "FullName", tblpartneractivity.userId);
                                    }
                                    else
                                    {
                                        ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.partnerId);
                                        ViewBag.giverId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.giverId);
                                        ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblpartneractivity.userId);
                                    }

                                    return View(tblpartneractivity);
                                }
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Reagent selected to be updated could not be found in the system!. "
                                    + "Please try again or contact the System Administrator.");
                                ViewBag.reagentId = new SelectList(db.Reagents, "reagentId", "name", tblpartneractivity.reagentId);
                                //ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.partnerId);//tblpartneractivity.partner.partnerId
                                //ViewBag.giverId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.giverId);//tblpartneractivity.giver.partnerId
                                //ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblpartneractivity.userId);
                                ViewBag.varietyId = new SelectList(varieties, "varietyId", "description");
                                //ViewBag.seedlingId = new SelectList(db.Seedlings, "seedlingId", "name", tblpartneractivity.varietyId);

                                if (OwnAccess)
                                {
                                    ViewBag.partnerId = new SelectList(db.Partners.Where(p => p.partnerId == currentUser.partnerId), "partnerId", "name", tblpartneractivity.partnerId);
                                    ViewBag.giverId = new SelectList(db.Partners.Where(p => p.partnerId != currentUser.partnerId), "partnerId", "name", tblpartneractivity.giverId);
                                    ViewBag.userId = new SelectList(db.Users.Where(u => u.partnerId == currentUser.partnerId), "Id", "FullName", tblpartneractivity.userId);
                                }
                                else
                                {
                                    ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.partnerId);
                                    ViewBag.giverId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.giverId);
                                    ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblpartneractivity.userId);
                                }

                                return View(tblpartneractivity);
                            }
                        }
                        else if (diffInQty == 0)//Update the inventory table but no update on the stock table
                        {
                            act.partner.partnerId = tblpartneractivity.partnerId;//tblpartneractivity.partner.partnerId
                            act.giver.partnerId = tblpartneractivity.giverId;//tblpartneractivity.giver.partnerId
                            act.reagentId = tblpartneractivity.reagentId;
                            act.varietyId = tblpartneractivity.varietyId;
                            act.userId = tblpartneractivity.userId;
                            act.activityDate = tblpartneractivity.activityDate;
                            act.backStopping = tblpartneractivity.backStopping;
                            act.tcPlantletsGiven = tblpartneractivity.tcPlantletsGiven;
                            act.bioreactorplantsGiven = tblpartneractivity.bioreactorplantsGiven;
                            act.tubersGiven = tblpartneractivity.tubersGiven;
                            act.tcPlantletsAvailable = tblpartneractivity.tcPlantletsAvailable;
                            act.tibPlantletsAvailable = tblpartneractivity.tibPlantletsAvailable;
                            act.tubersAvailable = tblpartneractivity.tubersAvailable;

                            act.seedsAvailable = tblpartneractivity.seedsAvailable;
                            act.seedsGiven = tblpartneractivity.seedsGiven;

                            act.reagentQty = tblpartneractivity.reagentQty;
                            //act.varietyQty = tblpartneractivity.varietyQty;
                            

                            if (currentUser != null)
                                act.updatedBy = currentUser.UserName;
                            else
                                act.updatedBy = User.Identity.Name;

                            act.updatedDate = DateTime.Now;

                            db.SaveChanges();

                            dbContextTransaction.Commit();
                        }
                        else if (diffInQty < 0)//check indices to make sure the stock table will not end up with minus value after this update
                        {
                            //Get stock level
                            var stocklevel = db.Stocks.Where(c => c.reagentId == tblpartneractivity.reagentId).FirstOrDefault();
                            if (stocklevel != null)
                            {
                                act.partner.partnerId = tblpartneractivity.partnerId;//tblpartneractivity.partner.partnerId
                                act.giver.partnerId = tblpartneractivity.giverId;//tblpartneractivity.giver.partnerId
                                act.reagentId = tblpartneractivity.reagentId;
                                act.varietyId = tblpartneractivity.varietyId;
                                act.userId = tblpartneractivity.userId;
                                act.activityDate = tblpartneractivity.activityDate;
                                act.backStopping = tblpartneractivity.backStopping;
                                act.tcPlantletsGiven = tblpartneractivity.tcPlantletsGiven;
                                act.bioreactorplantsGiven = tblpartneractivity.bioreactorplantsGiven;
                                act.tubersGiven = tblpartneractivity.tubersGiven;
                                act.tcPlantletsAvailable = tblpartneractivity.tcPlantletsAvailable;
                                act.tibPlantletsAvailable = tblpartneractivity.tibPlantletsAvailable;
                                act.tubersAvailable = tblpartneractivity.tubersAvailable;

                                act.seedsAvailable = tblpartneractivity.seedsAvailable;
                                act.seedsGiven = tblpartneractivity.seedsGiven;

                                act.reagentQty = tblpartneractivity.reagentQty;
                                //act.varietyQty = tblpartneractivity.varietyQty;

                                if (currentUser != null)
                                    act.updatedBy = currentUser.UserName;
                                else
                                    act.updatedBy = User.Identity.Name;

                                act.updatedDate = DateTime.Now;

                                db.SaveChanges();

                                //Call stock tracker table for STOCK-IN TRANSACTION
                                stocklevel.totalIn = stocklevel.totalIn - (diffInQty);
                                db.SaveChanges();

                                dbContextTransaction.Commit();
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Reagent selected to be updated could not be found in the system!. "
                                    + "Please try again or contact the System Administrator.");
                                ViewBag.reagentId = new SelectList(db.Reagents, "reagentId", "name", tblpartneractivity.reagentId);
                                //ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.partnerId);//tblpartneractivity.partner.partnerId
                                //ViewBag.giverId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.giverId);//tblpartneractivity.giver.partnerId
                                //ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblpartneractivity.userId);
                                if (OwnAccess)
                                {
                                    ViewBag.partnerId = new SelectList(db.Partners.Where(p => p.partnerId == currentUser.partnerId), "partnerId", "name", tblpartneractivity.partnerId);
                                    ViewBag.giverId = new SelectList(db.Partners.Where(p => p.partnerId != currentUser.partnerId), "partnerId", "name", tblpartneractivity.giverId);
                                    ViewBag.userId = new SelectList(db.Users.Where(u => u.partnerId == currentUser.partnerId), "Id", "FullName", tblpartneractivity.userId);
                                }
                                else
                                {
                                    ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.partnerId);
                                    ViewBag.giverId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.giverId);
                                    ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblpartneractivity.userId);
                                }

                                ViewBag.varietyId = new SelectList(varieties, "varietyId", "description");
                                //ViewBag.seedlingId = new SelectList(db.Seedlings, "seedlingId", "name", tblpartneractivity.varietyId);
                                return View(tblpartneractivity);
                            }
                        }
                        return RedirectToAction("Index");
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        var entry = ex.Entries.Single();
                        var databaseValues = (InternalReagentUsage)entry.GetDatabaseValues().ToObject();
                        var clientValues = (InternalReagentUsage)entry.Entity;
                        if (databaseValues.reagent.name != clientValues.reagent.name)
                            ModelState.AddModelError("Reagent", "Current value: " + databaseValues.reagent.name);
                        if (databaseValues.quantity != clientValues.quantity)
                            ModelState.AddModelError("Dispatch Qty", "Current value: " + databaseValues.quantity);

                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                          + "was modified by another user after you got the original value. The "
                          + "edit operation was canceled and the current values in the database "
                          + "have been displayed. If you still want to edit this record, click "
                          + "the Save button again. Otherwise click the Back to List hyperlink.");

                        if (dbContextTransaction != null) dbContextTransaction.Rollback();

                        tblpartneractivity.Timestamp = databaseValues.Timestamp;
                        ViewBag.reagentId = new SelectList(db.Reagents, "reagentId", "name", tblpartneractivity.reagentId);
                        //ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.partnerId);//tblpartneractivity.partner.partnerId
                        //ViewBag.giverId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.giverId);//tblpartneractivity.giver.partnerId
                        //ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblpartneractivity.userId);
                        if (OwnAccess)
                        {
                            ViewBag.partnerId = new SelectList(db.Partners.Where(p => p.partnerId == currentUser.partnerId), "partnerId", "name", tblpartneractivity.partnerId);
                            ViewBag.giverId = new SelectList(db.Partners.Where(p => p.partnerId != currentUser.partnerId), "partnerId", "name", tblpartneractivity.giverId);
                            ViewBag.userId = new SelectList(db.Users.Where(u => u.partnerId == currentUser.partnerId), "Id", "FullName", tblpartneractivity.userId);
                        }
                        else
                        {
                            ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.partnerId);
                            ViewBag.giverId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.giverId);
                            ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblpartneractivity.userId);
                        }

                        ViewBag.varietyId = new SelectList(varieties, "varietyId", "description");
                        //ViewBag.seedlingId = new SelectList(db.Seedlings, "seedlingId", "name", tblpartneractivity.varietyId);
                        return View(tblpartneractivity);
                    }
                    catch (Exception ex)
                    {
                        if (dbContextTransaction != null) dbContextTransaction.Rollback();
                        ModelState.AddModelError(string.Empty, "Error occurred saving inventory. " + "\n\nError message: " + ex.Message);
                        ViewBag.reagentId = new SelectList(db.Reagents, "reagentId", "name", tblpartneractivity.reagentId);
                        //ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.partnerId);//tblpartneractivity.giver.partnerId
                        //ViewBag.giverId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.giverId);//tblpartneractivity.giver.partnerId
                        //ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblpartneractivity.userId);
                        if (OwnAccess)
                        {
                            ViewBag.partnerId = new SelectList(db.Partners.Where(p => p.partnerId == currentUser.partnerId), "partnerId", "name", tblpartneractivity.partnerId);
                            ViewBag.giverId = new SelectList(db.Partners.Where(p => p.partnerId != currentUser.partnerId), "partnerId", "name", tblpartneractivity.giverId);
                            ViewBag.userId = new SelectList(db.Users.Where(u => u.partnerId == currentUser.partnerId), "Id", "FullName", tblpartneractivity.userId);
                        }
                        else
                        {
                            ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.partnerId);
                            ViewBag.giverId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.giverId);
                            ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblpartneractivity.userId);
                        }

                        ViewBag.varietyId = new SelectList(varieties, "varietyId", "description");
                        //ViewBag.seedlingId = new SelectList(db.Seedlings, "seedlingId", "name", tblpartneractivity.varietyId);
                        return View(tblpartneractivity);
                    }
                }
            }

            ViewBag.reagentId = new SelectList(db.Reagents, "reagentId", "name", tblpartneractivity.reagentId);
            //ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.partnerId);//tblpartneractivity.partner.partnerId
            //ViewBag.giverId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.giverId);//tblpartneractivity.giver.partnerId
            //ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblpartneractivity.userId);
            if (OwnAccess)
            {
                ViewBag.partnerId = new SelectList(db.Partners.Where(p => p.partnerId == currentUser.partnerId), "partnerId", "name", tblpartneractivity.partnerId);
                ViewBag.giverId = new SelectList(db.Partners.Where(p => p.partnerId != currentUser.partnerId), "partnerId", "name", tblpartneractivity.giverId);
                ViewBag.userId = new SelectList(db.Users.Where(u => u.partnerId == currentUser.partnerId), "Id", "FullName", tblpartneractivity.userId);
            }
            else
            {
                ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.partnerId);
                ViewBag.giverId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.giverId);
                ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblpartneractivity.userId);
            }

            ViewBag.varietyId = new SelectList(varieties, "varietyId", "description");
            //ViewBag.seedlingId = new SelectList(db.Seedlings, "seedlingId", "name", tblpartneractivity.varietyId);
            return View(tblpartneractivity);
        }

        // GET: /PartnerActivity/Delete/5
        [Authorize(Roles = "Admin, CanDeletePartnerActivity, CanDeleteOwnPartnerActivity, PartnerActivity")]
        public ActionResult Delete(long? id)
        {
            string perms = "CanDeleteOwnPartnerActivity";
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            ApplicationUser currentUser = manager.FindById(User.Identity.GetUserId());
            Boolean OwnAccess = accessRoleCheck.checkOwnPerm(groups, currentUser, perms);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tblpartneractivity = db.PartnerActivities.Find(id);

            if (OwnAccess)
            {
                tblpartneractivity = db.PartnerActivities.Where(p => p.partnerActivityId == id && p.createdBy == currentUser.UserName).FirstOrDefault();
            }

            if (tblpartneractivity == null)
            {
                return HttpNotFound();
            }
            return View(tblpartneractivity);
        }

        // POST: /PartnerActivity/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanDeletePartnerActivity, CanDeleteOwnPartnerActivity, PartnerActivity")]
        public ActionResult DeleteConfirmed(long id)
        {
            string perms = "CanDeleteOwnPartnerActivity";
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            ApplicationUser currentUser = manager.FindById(User.Identity.GetUserId());
            Boolean OwnAccess = accessRoleCheck.checkOwnPerm(groups, currentUser, perms);

            var tblpartneractivity = db.PartnerActivities.Find(id);

            if (OwnAccess)
            {
                tblpartneractivity = db.PartnerActivities.Where(p => p.partnerActivityId == id && p.createdBy == currentUser.UserName).FirstOrDefault();
            }

            db.PartnerActivities.Remove(tblpartneractivity);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult ExportReport(string searchString, DateTime? searchStartActivityDate, DateTime? searchEndActivityDate)
        {
            string perms = "CanViewOwnPartnerActivity";
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            ApplicationUser currentUser = manager.FindById(User.Identity.GetUserId());
            Boolean OwnAccess = accessRoleCheck.checkOwnPerm(groups, currentUser, perms);

            DateTime StartDate;// = Convert.ToDateTime(dateFrom);
            DateTime EndDate;// = Convert.ToDateTime(dateTo);

            var dailyReport = (from pa in db.PartnerActivities.AsEnumerable() 
                               select new CustomPartnerActivities {
                                   ActivityDate = pa.activityDate.HasValue ? pa.activityDate.Value.ToString("dd/MM/yyyy") : String.Empty,
                                   BackStopping = pa.backStopping,
                                   BioRPG = Convert.ToDecimal(pa.bioreactorplantsGiven),
                                   OiCFullName = pa.oic.FirstName + " " + pa.oic.LastName,
                                   PartnerName = pa.partner.name,
                                   GiverName = pa.giver.name,
                                   ReagentName = pa.reagent.name,
                                   ReagentQty = Convert.ToInt32(pa.reagentQty),
                                   TA = Convert.ToDecimal(pa.tubersAvailable),
                                   TCPA = Convert.ToDecimal(pa.tcPlantletsAvailable),
                                   TCPG = Convert.ToDecimal(pa.tcPlantletsGiven),
                                   TG = Convert.ToDecimal(pa.tubersGiven),
                                   TIPA = Convert.ToDecimal(pa.tibPlantletsAvailable),
                                   SA = Convert.ToDecimal(pa.seedsAvailable),
                                   SG = Convert.ToDecimal(pa.seedsGiven),
                                   VarietyQtyA = Convert.ToDecimal(pa.seedsAvailable) + Convert.ToDecimal(pa.tubersAvailable) + Convert.ToDecimal(pa.tcPlantletsAvailable) + Convert.ToDecimal(pa.tibPlantletsAvailable),
                                   VarietyQtyG = Convert.ToDecimal(pa.tcPlantletsGiven) + Convert.ToDecimal(pa.tubersGiven) + Convert.ToDecimal(pa.seedsGiven) + Convert.ToDecimal(pa.bioreactorplantsGiven),
                                   userName = pa.createdBy
                               }).ToArray();


            ReportDocument read = new ReportDocument();

            if (searchString != null)
            {
                if (searchStartActivityDate != null && searchEndActivityDate != null)
                {
                    StartDate = Convert.ToDateTime(searchStartActivityDate);
                    EndDate = Convert.ToDateTime(searchEndActivityDate);
                    dailyReport = (from pa in db.PartnerActivities.AsEnumerable().Where(pa => pa.activityDate >= StartDate && pa.activityDate <= EndDate && pa.partner.name.Contains(searchString))
                                   select new CustomPartnerActivities
                                   {
                                       ActivityDate = pa.activityDate.HasValue ? pa.activityDate.Value.ToString("dd/MM/yyyy") : String.Empty,
                                       BackStopping = pa.backStopping,
                                       BioRPG = Convert.ToDecimal(pa.bioreactorplantsGiven),
                                       OiCFullName = pa.oic.FirstName + " " + pa.oic.LastName,
                                       PartnerName = pa.partner.name,
                                       GiverName = pa.giver.name,
                                       ReagentName = pa.reagent.name,
                                       ReagentQty = Convert.ToInt32(pa.reagentQty),
                                       TA = Convert.ToDecimal(pa.tubersAvailable),
                                       TCPA = Convert.ToDecimal(pa.tcPlantletsAvailable),
                                       TCPG = Convert.ToDecimal(pa.tcPlantletsGiven),
                                       TG = Convert.ToDecimal(pa.tubersGiven),
                                       TIPA = Convert.ToDecimal(pa.tibPlantletsAvailable),
                                       SA = Convert.ToDecimal(pa.seedsAvailable),
                                       SG = Convert.ToDecimal(pa.seedsGiven),
                                       VarietyQtyA = Convert.ToDecimal(pa.seedsAvailable) + Convert.ToDecimal(pa.tubersAvailable) + Convert.ToDecimal(pa.tcPlantletsAvailable) + Convert.ToDecimal(pa.tibPlantletsAvailable),
                                       VarietyQtyG = Convert.ToDecimal(pa.tcPlantletsGiven) + Convert.ToDecimal(pa.tubersGiven) + Convert.ToDecimal(pa.seedsGiven) + Convert.ToDecimal(pa.bioreactorplantsGiven),
                                       userName = pa.createdBy
                                   }).ToArray();
                }
                else if (searchStartActivityDate != null && searchEndActivityDate == null)
                {
                    StartDate = Convert.ToDateTime(searchStartActivityDate);
                    dailyReport = (from pa in db.PartnerActivities.AsEnumerable().Where(x => x.activityDate == StartDate && x.partner.name.Contains(searchString))
                                   select new CustomPartnerActivities
                                   {
                                       ActivityDate = pa.activityDate.HasValue ? pa.activityDate.Value.ToString("dd/MM/yyyy") : String.Empty,
                                       BackStopping = pa.backStopping,
                                       BioRPG = Convert.ToDecimal(pa.bioreactorplantsGiven),
                                       OiCFullName = pa.oic.FirstName + " " + pa.oic.LastName,
                                       PartnerName = pa.partner.name,
                                       GiverName = pa.giver.name,
                                       ReagentName = pa.reagent.name,
                                       ReagentQty = Convert.ToInt32(pa.reagentQty),
                                       TA = Convert.ToDecimal(pa.tubersAvailable),
                                       TCPA = Convert.ToDecimal(pa.tcPlantletsAvailable),
                                       TCPG = Convert.ToDecimal(pa.tcPlantletsGiven),
                                       TG = Convert.ToDecimal(pa.tubersGiven),
                                       TIPA = Convert.ToDecimal(pa.tibPlantletsAvailable),
                                       SA = Convert.ToDecimal(pa.seedsAvailable),
                                       SG = Convert.ToDecimal(pa.seedsGiven),
                                       VarietyQtyA = Convert.ToDecimal(pa.seedsAvailable) + Convert.ToDecimal(pa.tubersAvailable) + Convert.ToDecimal(pa.tcPlantletsAvailable) + Convert.ToDecimal(pa.tibPlantletsAvailable),
                                       VarietyQtyG = Convert.ToDecimal(pa.tcPlantletsGiven) + Convert.ToDecimal(pa.tubersGiven) + Convert.ToDecimal(pa.seedsGiven) + Convert.ToDecimal(pa.bioreactorplantsGiven),
                                       userName = pa.createdBy
                                   }).ToArray();
                }
                else if (searchStartActivityDate == null && searchEndActivityDate != null)
                {
                    EndDate = Convert.ToDateTime(searchEndActivityDate);
                    dailyReport = (from pa in db.PartnerActivities.AsEnumerable().Where(x => x.activityDate <= EndDate && x.partner.name.Contains(searchString))
                                   select new CustomPartnerActivities
                                   {
                                       ActivityDate = pa.activityDate.HasValue ? pa.activityDate.Value.ToString("dd/MM/yyyy") : String.Empty,
                                       BackStopping = pa.backStopping,
                                       BioRPG = Convert.ToDecimal(pa.bioreactorplantsGiven),
                                       OiCFullName = pa.oic.FirstName + " " + pa.oic.LastName,
                                       PartnerName = pa.partner.name,
                                       GiverName = pa.giver.name,
                                       ReagentName = pa.reagent.name,
                                       ReagentQty = Convert.ToInt32(pa.reagentQty),
                                       TA = Convert.ToDecimal(pa.tubersAvailable),
                                       TCPA = Convert.ToDecimal(pa.tcPlantletsAvailable),
                                       TCPG = Convert.ToDecimal(pa.tcPlantletsGiven),
                                       TG = Convert.ToDecimal(pa.tubersGiven),
                                       TIPA = Convert.ToDecimal(pa.tibPlantletsAvailable),
                                       SA = Convert.ToDecimal(pa.seedsAvailable),
                                       SG = Convert.ToDecimal(pa.seedsGiven),
                                       VarietyQtyA = Convert.ToDecimal(pa.seedsAvailable) + Convert.ToDecimal(pa.tubersAvailable) + Convert.ToDecimal(pa.tcPlantletsAvailable) + Convert.ToDecimal(pa.tibPlantletsAvailable),
                                       VarietyQtyG = Convert.ToDecimal(pa.tcPlantletsGiven) + Convert.ToDecimal(pa.tubersGiven) + Convert.ToDecimal(pa.seedsGiven) + Convert.ToDecimal(pa.bioreactorplantsGiven),
                                       userName = pa.createdBy
                                   }).ToArray();
                }
                else
                {
                    dailyReport = (from pa in db.PartnerActivities.AsEnumerable().Where(x => x.partner.name.Contains(searchString))
                                   select new CustomPartnerActivities
                                   {
                                       ActivityDate = pa.activityDate.HasValue ? pa.activityDate.Value.ToString("dd/MM/yyyy") : String.Empty,
                                       BackStopping = pa.backStopping,
                                       BioRPG = Convert.ToDecimal(pa.bioreactorplantsGiven),
                                       OiCFullName = pa.oic.FirstName + " " + pa.oic.LastName,
                                       PartnerName = pa.partner.name,
                                       GiverName = pa.giver.name,
                                       ReagentName = pa.reagent.name,
                                       ReagentQty = Convert.ToInt32(pa.reagentQty),
                                       TA = Convert.ToDecimal(pa.tubersAvailable),
                                       TCPA = Convert.ToDecimal(pa.tcPlantletsAvailable),
                                       TCPG = Convert.ToDecimal(pa.tcPlantletsGiven),
                                       TG = Convert.ToDecimal(pa.tubersGiven),
                                       TIPA = Convert.ToDecimal(pa.tibPlantletsAvailable),
                                       SA = Convert.ToDecimal(pa.seedsAvailable),
                                       SG = Convert.ToDecimal(pa.seedsGiven),
                                       VarietyQtyA = Convert.ToDecimal(pa.seedsAvailable) + Convert.ToDecimal(pa.tubersAvailable) + Convert.ToDecimal(pa.tcPlantletsAvailable) + Convert.ToDecimal(pa.tibPlantletsAvailable),
                                       VarietyQtyG = Convert.ToDecimal(pa.tcPlantletsGiven) + Convert.ToDecimal(pa.tubersGiven) + Convert.ToDecimal(pa.seedsGiven) + Convert.ToDecimal(pa.bioreactorplantsGiven),
                                       userName = pa.createdBy
                                   }).ToArray();
                }
            }
            else {
                if (searchStartActivityDate != null && searchEndActivityDate != null)
                {
                    StartDate = Convert.ToDateTime(searchStartActivityDate);
                    EndDate = Convert.ToDateTime(searchEndActivityDate);
                    dailyReport = (from pa in db.PartnerActivities.AsEnumerable().Where(pa => pa.activityDate >= StartDate && pa.activityDate <= EndDate)
                                   select new CustomPartnerActivities
                                   {
                                       ActivityDate = pa.activityDate.HasValue ? pa.activityDate.Value.ToString("dd/MM/yyyy") : String.Empty,
                                       BackStopping = pa.backStopping,
                                       BioRPG = Convert.ToDecimal(pa.bioreactorplantsGiven),
                                       OiCFullName = pa.oic.FirstName + " " + pa.oic.LastName,
                                       PartnerName = pa.partner.name,
                                       GiverName = pa.giver.name,
                                       ReagentName = pa.reagent.name,
                                       ReagentQty = Convert.ToInt32(pa.reagentQty),
                                       TA = Convert.ToDecimal(pa.tubersAvailable),
                                       TCPA = Convert.ToDecimal(pa.tcPlantletsAvailable),
                                       TCPG = Convert.ToDecimal(pa.tcPlantletsGiven),
                                       TG = Convert.ToDecimal(pa.tubersGiven),
                                       TIPA = Convert.ToDecimal(pa.tibPlantletsAvailable),
                                       SA = Convert.ToDecimal(pa.seedsAvailable),
                                       SG = Convert.ToDecimal(pa.seedsGiven),
                                       VarietyQtyA = Convert.ToDecimal(pa.seedsAvailable) + Convert.ToDecimal(pa.tubersAvailable) + Convert.ToDecimal(pa.tcPlantletsAvailable) + Convert.ToDecimal(pa.tibPlantletsAvailable),
                                       VarietyQtyG = Convert.ToDecimal(pa.tcPlantletsGiven) + Convert.ToDecimal(pa.tubersGiven) + Convert.ToDecimal(pa.seedsGiven) + Convert.ToDecimal(pa.bioreactorplantsGiven),
                                       userName = pa.createdBy
                                   }).ToArray();
                }
                else if (searchStartActivityDate != null && searchEndActivityDate == null)
                {
                    StartDate = Convert.ToDateTime(searchStartActivityDate);
                    dailyReport = (from pa in db.PartnerActivities.AsEnumerable().Where(x => x.activityDate == StartDate)
                                   select new CustomPartnerActivities
                                   {
                                       ActivityDate = pa.activityDate.HasValue ? pa.activityDate.Value.ToString("dd/MM/yyyy") : String.Empty,
                                       BackStopping = pa.backStopping,
                                       BioRPG = Convert.ToDecimal(pa.bioreactorplantsGiven),
                                       OiCFullName = pa.oic.FirstName + " " + pa.oic.LastName,
                                       PartnerName = pa.partner.name,
                                       GiverName = pa.giver.name,
                                       ReagentName = pa.reagent.name,
                                       ReagentQty = Convert.ToInt32(pa.reagentQty),
                                       TA = Convert.ToDecimal(pa.tubersAvailable),
                                       TCPA = Convert.ToDecimal(pa.tcPlantletsAvailable),
                                       TCPG = Convert.ToDecimal(pa.tcPlantletsGiven),
                                       TG = Convert.ToDecimal(pa.tubersGiven),
                                       TIPA = Convert.ToDecimal(pa.tibPlantletsAvailable),
                                       SA = Convert.ToDecimal(pa.seedsAvailable),
                                       SG = Convert.ToDecimal(pa.seedsGiven),
                                       VarietyQtyA = Convert.ToDecimal(pa.seedsAvailable) + Convert.ToDecimal(pa.tubersAvailable) + Convert.ToDecimal(pa.tcPlantletsAvailable) + Convert.ToDecimal(pa.tibPlantletsAvailable),
                                       VarietyQtyG = Convert.ToDecimal(pa.tcPlantletsGiven) + Convert.ToDecimal(pa.tubersGiven) + Convert.ToDecimal(pa.seedsGiven) + Convert.ToDecimal(pa.bioreactorplantsGiven),
                                       userName = pa.createdBy
                                   }).ToArray();
                }
                else if (searchEndActivityDate != null && searchStartActivityDate == null)
                {
                    EndDate = Convert.ToDateTime(searchEndActivityDate);
                    dailyReport = (from pa in db.PartnerActivities.AsEnumerable().Where(x => x.activityDate <= EndDate)
                                   select new CustomPartnerActivities
                                   {
                                       ActivityDate = pa.activityDate.HasValue ? pa.activityDate.Value.ToString("dd/MM/yyyy") : String.Empty,
                                       BackStopping = pa.backStopping,
                                       BioRPG = Convert.ToDecimal(pa.bioreactorplantsGiven),
                                       OiCFullName = pa.oic.FirstName + " " + pa.oic.LastName,
                                       PartnerName = pa.partner.name,
                                       GiverName = pa.giver.name,
                                       ReagentName = pa.reagent.name,
                                       ReagentQty = Convert.ToInt32(pa.reagentQty),
                                       TA = Convert.ToDecimal(pa.tubersAvailable),
                                       TCPA = Convert.ToDecimal(pa.tcPlantletsAvailable),
                                       TCPG = Convert.ToDecimal(pa.tcPlantletsGiven),
                                       TG = Convert.ToDecimal(pa.tubersGiven),
                                       TIPA = Convert.ToDecimal(pa.tibPlantletsAvailable),
                                       SA = Convert.ToDecimal(pa.seedsAvailable),
                                       SG = Convert.ToDecimal(pa.seedsGiven),
                                       VarietyQtyA = Convert.ToDecimal(pa.seedsAvailable) + Convert.ToDecimal(pa.tubersAvailable) + Convert.ToDecimal(pa.tcPlantletsAvailable) + Convert.ToDecimal(pa.tibPlantletsAvailable),
                                       VarietyQtyG = Convert.ToDecimal(pa.tcPlantletsGiven) + Convert.ToDecimal(pa.tubersGiven) + Convert.ToDecimal(pa.seedsGiven) + Convert.ToDecimal(pa.bioreactorplantsGiven),
                                       userName = pa.createdBy
                                   }).ToArray();
                }
            }

            if (OwnAccess)
            {
                dailyReport = dailyReport.Where(p => p.userName == currentUser.UserName).ToArray();
            }

            read.Load(Path.Combine(Server.MapPath("~/Content/Reports"), "PartnerActivitiesReport.rpt"));
            read.SetDataSource(dailyReport);
            if (!string.IsNullOrEmpty(searchString))
                read.SetParameterValue("partner", searchString);
            if (searchStartActivityDate!=null)
                read.SetParameterValue("dateFrom", searchStartActivityDate);
            if (searchEndActivityDate!=null)
                read.SetParameterValue("dateTo", searchEndActivityDate);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            try
            {
                if (dailyReport.Length > 0)
                {
                    Stream stream = read.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "PartnerActivitiesReport.pdf");
                }
                else {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //return RedirectToAction("Index");

        }

        public ActionResult PartnerActivitiesChart(string pid)
        {
            /*var dataPartner = db.PartnerActivities.GroupBy(r => new { r.reagent.name, partnerName = r.partner.name }).AsEnumerable()
                .Select(a => new { Qty = a.Sum(b => b.reagentQty), ReagName = a.Key.name, partnerName = a.Key.partnerName })
                .ToList();*/
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());//pn = r.partner.name, 
            var dataPartners = db.PartnerActivities.GroupBy(r => new { r.partner.name }).AsEnumerable()
                .Select(a => new { partnerName = a.Key.name })
                .ToList();
            var myChart = new Chart(width: 800, height: 200, theme: ChartTheme.Blue);
                foreach(var partner in dataPartners){//, partnerName = r.partner.name
                    var dataReagents = db.PartnerActivities.GroupBy(r => new { r.reagent.name }).AsEnumerable()
                    .Select(a => new { reagentName = a.Key.name })
                    .ToList();
                    ArrayList qtyArray = new ArrayList();
                    ArrayList reagentArray = new ArrayList();
                    foreach (var reagent in dataReagents) {
                        reagentArray.Add(reagent.reagentName);
                        //var dataQty = db.PartnerActivities.Where(a => a.partner.name.Contains(partner.partnerName) && a.giver.name.Contains(partner.partnerName) && a.reagent.name.Contains(reagent.reagentName))
                        var dataQty = db.PartnerActivities.Where(a => a.partner.name.Contains(partner.partnerName) && a.reagent.name.Contains(reagent.reagentName))
                            .GroupBy(r => new { r.reagent.name, partnerName = r.partner.name }).AsEnumerable()
                            .Select(a => new { Qty = a.Sum(b => b.reagentQty) })
                            .FirstOrDefault();
                        if (dataQty != null)
                            qtyArray.Add(dataQty.Qty);
                        else
                            qtyArray.Add(0);
                    }
                    //myChart.AddSeries(name: partner.partnerName, chartType: "Column", xValue: dataReagents, xField: "ReagName", yValues: dataReagents, yFields: "Qty");
                    myChart.AddSeries(name: partner.partnerName, chartType: "Column", xValue: reagentArray, yValues: qtyArray);
                }
                myChart.AddLegend(title: "Partners");
                myChart.AddTitle("Reagents Usage");
                myChart.Write();
            myChart.Save("~/Content/uploads/chart" + currentUser.Id, "jpeg");
            // Return the contents of the Stream to the client
            return base.File("~/Content/uploads/chart" + currentUser.Id, "jpeg");
        }

        // GET: /Report/Delete/5
        [Authorize(Roles = "Admin, CanDeleteReport, CanDeleteOwnReport, PartnerActivity")]
        public ActionResult DeleteReport(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartnerReporting reporting = db.PartnerReportings.Find(id);
            if (reporting == null)
            {
                return HttpNotFound();
            }

            var title = "Partner Activity dated: " + String.Format("{0:d}", reporting.activity.activityDate) + ", Recieved from partner agency: " + reporting.activity.partner.name;

            if (reporting.activity.reagentId != null) title = title + " with Reagent: " + reporting.activity.reagent.name + " (#" + reporting.activity.reagentQty + ")";

            if (reporting.activity.varietyId != null) title = title + " & Variety: " + reporting.activity.variety.FullDescription + " (#" + (reporting.activity.bioreactorplantsGiven + reporting.activity.tcPlantletsGiven + reporting.activity.tubersGiven + reporting.activity.seedsGiven + ")");

            ViewBag.ActivityTitle = title;

            return View(reporting);
        }

        // POST: /LocationUser/Delete/5
        [HttpPost, ActionName("DeleteReport")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanDeleteReport, CanDeleteOwnReport, PartnerActivity")]
        public ActionResult DeleteReportConfirmed(long id, long activityId)
        {
            PartnerReporting reporting = db.PartnerReportings.Find(id);
            db.PartnerReportings.Remove(reporting);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = activityId });
        }

        // POST: /LocationUser/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanAddReport, CanAddOwnReport, PartnerActivity")]
        public ActionResult AddReport([Bind(Include = "activityId,varietyQty,reagentQty,comment,reportDate,spQty,tcpQty,tpQty,bioRPQty")] PartnerReporting frmReporting)
        {
            if (ModelState.IsValid)
            {
                var dbReporting = db.PartnerReportings.FirstOrDefault(p => p.spQty == frmReporting.spQty
                    && p.tcpQty == frmReporting.tcpQty
                    && p.tpQty == frmReporting.tpQty
                    && p.bioRPQty == frmReporting.bioRPQty
                    && p.reagentQty == frmReporting.reagentQty
                    && p.activityId == frmReporting.activityId
                    && p.reportDate == frmReporting.reportDate
                    && p.comment == frmReporting.comment);
                if (dbReporting == null)
                {
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var currentUser = manager.FindById(User.Identity.GetUserId());

                    if (currentUser != null)
                        frmReporting.createdBy = currentUser.UserName;
                    else
                        frmReporting.createdBy = User.Identity.Name;

                    frmReporting.createdDate = DateTime.Now;

                    db.PartnerReportings.Add(frmReporting);
                    db.SaveChanges();
                }
                else
                {
                    var report = db.PartnerReportings.SingleOrDefault(p => p.reportDate == frmReporting.reportDate);
                    ModelState.AddModelError(string.Empty, "Report already entered for partner activity with date " + frmReporting.reportDate);
                    //return View(tbllocationsubordinate);
                }

                return RedirectToAction("Details", new { id = frmReporting.activityId });
            }

            return RedirectToAction("Details", new { id = frmReporting.activityId });
        }

        // GET: /Activity/EditAssignment/5
        [Authorize(Roles = "Admin, CanEditReport, CanEditOwnReport, PartnerActivity")]
        public ActionResult EditReport(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartnerReporting dbReporting = db.PartnerReportings.Find(id);
            if (dbReporting == null)
            {
                return HttpNotFound();
            }

            var title = "Partner Activity dated: " + String.Format("{0:d}", dbReporting.activity.activityDate) + ", Recieved from partner agency: " + dbReporting.activity.partner.name;

            if (dbReporting.activity.reagentId != null) title = title + " with Reagent: " + dbReporting.activity.reagent.name + " (#" + dbReporting.activity.reagentQty + ")";

            if (dbReporting.activity.varietyId != null) title = title + " & Variety: " + dbReporting.activity.variety.FullDescription + " (#" + (dbReporting.activity.bioreactorplantsGiven + dbReporting.activity.tcPlantletsGiven + dbReporting.activity.tubersGiven + dbReporting.activity.seedsGiven + ")");

            ViewBag.ActivityTitle = title;
            return View(dbReporting);
        }

        // POST: /Activity/EditAssignment/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanEditReport, CanEditOwnReport, PartnerActivity")]
        public ActionResult EditReport([Bind(Include = "reportId,activityId,reagentId,varietyId,varietyQty,reagentQty,comment,reportDate,spQty,tcpQty,tpQty,bioRPQty")] PartnerReporting frmReporting)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());

            if (ModelState.IsValid)
            {
                try
                {
                    //db.Entry(tbllocationuser).State = EntityState.Modified;
                    var dbReporting = db.PartnerReportings.Where(c => c.reportId == frmReporting.reportId).FirstOrDefault();
                    dbReporting.reportDate = frmReporting.reportDate;
                    dbReporting.spQty = frmReporting.spQty;
                    dbReporting.tcpQty = frmReporting.tcpQty;
                    dbReporting.tpQty = frmReporting.tpQty;
                    dbReporting.bioRPQty = frmReporting.bioRPQty;
                    dbReporting.reagentQty = frmReporting.reagentQty;
                    dbReporting.comment = frmReporting.comment;
                    if (currentUser != null)
                        dbReporting.updatedBy = currentUser.UserName;
                    else
                        dbReporting.updatedBy = User.Identity.Name;

                    dbReporting.updatedDate = DateTime.Now;
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var databaseValues = (PartnerReporting)entry.GetDatabaseValues().ToObject();
                    var clientValues = (PartnerReporting)entry.Entity;
                    if (databaseValues.activity.variety.varietyDefinition.name != clientValues.activity.variety.varietyDefinition.name)
                        ModelState.AddModelError("Activity date", "Current value: " + databaseValues.reportDate);

                    ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                      + "was modified by another user after you got the original value. The "
                      + "edit operation was canceled and the current values in the database "
                      + "have been displayed. If you still want to edit this record, click "
                      + "the Save button again. Otherwise click the Back to List hyperlink.");

                    frmReporting.Timestamp = databaseValues.Timestamp;
                    
                    return View();
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                    
                    return View();
                }
                return RedirectToAction("Details", new { id = frmReporting.activityId });
            }
           
            return View(frmReporting);
        }

    }
}
