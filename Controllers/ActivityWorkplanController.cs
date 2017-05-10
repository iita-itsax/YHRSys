using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YHRSys.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Infrastructure;
using Microsoft.Owin.Security;
using System.Security.Claims;
using DropdownSelect.Models;
using PagedList;

namespace YHRSys.Controllers
{
    [Authorize]
    public class ActivityWorkplanController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /ActivityWorkplan/
        public ActionResult Index(string sortOrder, string currentFilter, string currentStartDateFilter, string currentEndDateFilter, string searchString, DateTime? searchStartActivityDate, DateTime? searchEndActivityDate, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.ObjectiveSortParm = String.IsNullOrEmpty(sortOrder) ? "objective_desc" : "";
            ViewBag.StartDateSortParm = sortOrder == "StartDate" ? "startdate_desc" : "StartDate";
            ViewBag.EndDateSortParm = sortOrder == "EndDate" ? "enddate_desc" : "EndDate";
            ViewBag.PISortParm = sortOrder == "PI" ? "pi_desc" : "PI";

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

            var workplans = from r in db.ActivityWorkplans select r;

            if (!String.IsNullOrEmpty(searchString))
            {
                if (searchStartActivityDate != null && searchEndActivityDate != null)
                {
                    workplans = workplans.Where(rg => (rg.Objective.Contains(searchString) || rg.PerformanceIndicator.Contains(searchString)) && (rg.StartPeriod >= (DateTime)searchStartActivityDate && rg.EndPeriod <= (DateTime)searchEndActivityDate));
                }
                else if (searchStartActivityDate != null)
                {
                    workplans = workplans.Where(rg => (rg.Objective.Contains(searchString) || rg.PerformanceIndicator.Contains(searchString)) && (rg.StartPeriod == (DateTime)searchStartActivityDate));
                }
                else if (searchEndActivityDate != null)
                {
                    workplans = workplans.Where(rg => (rg.Objective.Contains(searchString) || rg.PerformanceIndicator.Contains(searchString)) && (rg.EndPeriod == (DateTime)searchEndActivityDate));
                }
                else
                {
                    workplans = workplans.Where(rg => rg.Objective.Contains(searchString) || rg.PerformanceIndicator.Contains(searchString));
                }
            }
            else
            {
                if (searchStartActivityDate != null && searchEndActivityDate != null)
                {
                    workplans = workplans.Where(rg => (rg.StartPeriod >= (DateTime)searchStartActivityDate && rg.EndPeriod <= (DateTime)searchEndActivityDate));
                }
                else if (searchStartActivityDate != null)
                {
                    workplans = workplans.Where(rg => rg.StartPeriod == (DateTime)searchStartActivityDate);
                }
                else if (searchEndActivityDate != null)
                {
                    workplans = workplans.Where(rg => rg.EndPeriod <= (DateTime)searchEndActivityDate);
                }
            }

            switch (sortOrder)
            {
                case "objective_desc":
                    workplans = workplans.OrderByDescending(rg => rg.Objective);
                    break;
                case "StartDate":
                    workplans = workplans.OrderBy(rg => rg.StartPeriod);
                    break;
                case "startdate_desc":
                    workplans = workplans.OrderByDescending(rg => rg.StartPeriod);
                    break;
                case "EndDate":
                    workplans = workplans.OrderBy(rg => rg.EndPeriod);
                    break;
                case "enddate_desc":
                    workplans = workplans.OrderByDescending(rg => rg.EndPeriod);
                    break;
                case "PI":
                    workplans = workplans.OrderBy(rg => rg.PerformanceIndicator);
                    break;
                case "pi_desc":
                    workplans = workplans.OrderByDescending(rg => rg.PerformanceIndicator);
                    break;
                default:  // Name ascending 
                    workplans = workplans.OrderBy(rg => rg.Objective);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(workplans.ToPagedList(pageNumber, pageSize));
            //return View(db.ActivityWorkplans.ToList());
        }

        // GET: /ActivityWorkplan/Details/5
        [Authorize(Roles = "Admin, CanViewActivityWorkplan, ActivityWorkplan")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityWorkplan activityworkplan = db.ActivityWorkplans.Find(id);
            if (activityworkplan == null)
            {
                return HttpNotFound();
            }
            return View(activityworkplan);
        }

        // GET: /ActivityWorkplan/Create
        [Authorize(Roles = "Admin, CanAddActivityWorkplan, ActivityWorkplan")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /ActivityWorkplan/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanAddActivityWorkplan, ActivityWorkplan")]
        public ActionResult Create([Bind(Include="StartPeriod,EndPeriod,Objective,PerformanceIndicator")] ActivityWorkplan activityworkplan)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());

            try
            {

                if (ModelState.IsValid)
                {
                    var wal = db.ActivityWorkplans.FirstOrDefault(p => p.StartPeriod == activityworkplan.StartPeriod && p.EndPeriod == activityworkplan.EndPeriod && p.Objective == activityworkplan.Objective);
                    if (wal == null)
                    {
                        if (currentUser != null)
                            activityworkplan.createdBy = currentUser.UserName;
                        else
                            activityworkplan.createdBy = User.Identity.Name;

                        activityworkplan.createdDate = DateTime.Now;

                        db.ActivityWorkplans.Add(activityworkplan);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Activity Workplan already entered: " + activityworkplan.Objective
                        + " Start Period: " + activityworkplan.StartPeriod
                        + " End Period: " + activityworkplan.EndPeriod);

                    return View(activityworkplan);
                }

                return View(activityworkplan);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error occurred while saving record: " + ex.Message);
                return View();
            }
        }

        // GET: /ActivityWorkplan/Edit/5
        [Authorize(Roles = "Admin, CanEditActivityWorkplan, ActivityWorkplan")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityWorkplan activityworkplan = db.ActivityWorkplans.Find(id);
            if (activityworkplan == null)
            {
                return HttpNotFound();
            }
            return View(activityworkplan);
        }

        // POST: /ActivityWorkplan/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanEditActivityWorkplan, ActivityWorkplan")]
        public ActionResult Edit([Bind(Include="workplanId,StartPeriod,EndPeriod,Objective,PerformanceIndicator")] ActivityWorkplan activityworkplan)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());

            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var act = db.ActivityWorkplans.FirstOrDefault();
                        act = (ActivityWorkplan)db.WeeklyActivityLogs.Where(c => c.workplanId == activityworkplan.workplanId);

                        act.Objective = activityworkplan.Objective;
                        act.PerformanceIndicator = activityworkplan.PerformanceIndicator;
                        act.StartPeriod = activityworkplan.StartPeriod;
                        act.EndPeriod = activityworkplan.EndPeriod;

                        if (currentUser != null)
                            act.updatedBy = currentUser.UserName;
                        else
                            act.updatedBy = User.Identity.Name;

                        act.updatedDate = DateTime.Now;

                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        var entry = ex.Entries.Single();
                        var databaseValues = (ActivityWorkplan)entry.GetDatabaseValues().ToObject();
                        var clientValues = (ActivityWorkplan)entry.Entity;
                        if (databaseValues.Objective != clientValues.Objective)
                            ModelState.AddModelError("Objective", "Current value: " + databaseValues.Objective);
                        if (databaseValues.StartPeriod != clientValues.StartPeriod)
                            ModelState.AddModelError("Start Period", "Current value: " + databaseValues.StartPeriod);
                        if (databaseValues.EndPeriod != clientValues.EndPeriod)
                            ModelState.AddModelError("End Period", "Current value: " + databaseValues.EndPeriod);

                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                          + "was modified by another user after you got the original value. The "
                          + "edit operation was canceled and the current values in the database "
                          + "have been displayed. If you still want to edit this record, click "
                          + "the Save button again. Otherwise click the Back to List hyperlink.");

                        activityworkplan.Timestamp = databaseValues.Timestamp;

                        return View();
                    }
                    return RedirectToAction("Index");
                }

                return View(activityworkplan);
            }
            catch
            {
                return View(activityworkplan);
            }
        }

        // GET: /ActivityWorkplan/Delete/5
        [Authorize(Roles = "Admin, CanDeleteActivityWorkplan, ActivityWorkplan")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityWorkplan activityworkplan = db.ActivityWorkplans.Find(id);
            if (activityworkplan == null)
            {
                return HttpNotFound();
            }
            return View(activityworkplan);
        }

        // POST: /ActivityWorkplan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanDeleteActivityWorkplan, ActivityWorkplan")]
        public ActionResult DeleteConfirmed(int id)
        {
            ActivityWorkplan activityworkplan = db.ActivityWorkplans.Find(id);
            db.ActivityWorkplans.Remove(activityworkplan);
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
    }
}
