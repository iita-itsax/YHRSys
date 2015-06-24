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
    public class WeeklyActivityLogController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        SelectedValue selVal = new SelectedValue();
        string[] groups = new string[] { "SuperAdmins" };

        private List<SelectListItem> listStatus(string status)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "--Select--", Value = "" });
            items.Add(new SelectListItem { Text = "OPEN", Value = "OPEN", Selected = selVal.checkForSelectedValue("1", status) });
            items.Add(new SelectListItem { Text = "CLOSED", Value = "CLOSED", Selected = selVal.checkForSelectedValue("2", status) });
            return items;
        }

        // GET: /WeeklyActivityLog/
        [Authorize(Roles = "Admin, CanViewWeeklyActivityLog, WeeklyActivityLog")]
        public ActionResult Index(string sortOrder, string currentFilter, string currentStartDateFilter, string currentEndDateFilter, string searchString, DateTime? searchStartActivityDate, DateTime? searchEndActivityDate, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.StartDateSortParm = sortOrder == "StartDate" ? "startdate_desc" : "StartDate";
            ViewBag.EndDateSortParm = sortOrder == "EndDate" ? "enddate_desc" : "EndDate";
            ViewBag.StatusSortParm = sortOrder == "Status" ? "status_desc" : "Status";
            ViewBag.WorkplanSortParm = sortOrder == "Workplan" ? "workplan_desc" : "Workplan";

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

            var activities = from r in db.WeeklyActivityLogs select r;

            //Check if the user belongs to AdminGroup to be able to show all data in the list else show only user's own records
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            bool groupExists = CheckGroupAffiliate.checkGroupExist(currentUser, groups);

            if (currentUser != null) {
                if (!groupExists)
                    activities = activities.Where(rg => (rg.createdBy.Contains(currentUser.UserName)));
            }
            //Check stops here

            if (!String.IsNullOrEmpty(searchString))
            {
                if (searchStartActivityDate != null && searchEndActivityDate != null)
                {
                    activities = activities.Where(rg => (rg.staff.LastName.Contains(searchString) || rg.staff.FirstName.Contains(searchString) || rg.staff.UserName.Contains(searchString) || rg.description.Contains(searchString) || rg.status.Contains(searchString) || rg.activityWorkplan.Objective.Contains(searchString) || rg.activityWorkplan.PerformanceIndicator.Contains(searchString)) && (rg.startDate >= (DateTime)searchStartActivityDate && rg.endDate <= (DateTime)searchEndActivityDate));
                }
                else if (searchStartActivityDate != null)
                {
                    activities = activities.Where(rg => (rg.staff.LastName.Contains(searchString) || rg.staff.FirstName.Contains(searchString) || rg.staff.UserName.Contains(searchString) || rg.description.Contains(searchString) || rg.status.Contains(searchString) || rg.activityWorkplan.Objective.Contains(searchString) || rg.activityWorkplan.PerformanceIndicator.Contains(searchString)) && (rg.startDate == (DateTime)searchStartActivityDate));
                }
                else if (searchEndActivityDate != null)
                {
                    activities = activities.Where(rg => (rg.staff.LastName.Contains(searchString) || rg.staff.FirstName.Contains(searchString) || rg.staff.UserName.Contains(searchString) || rg.description.Contains(searchString) || rg.status.Contains(searchString) || rg.activityWorkplan.Objective.Contains(searchString) || rg.activityWorkplan.PerformanceIndicator.Contains(searchString)) && (rg.endDate == (DateTime)searchEndActivityDate));
                }
                else
                {
                    activities = activities.Where(rg => rg.staff.LastName.Contains(searchString) || rg.staff.FirstName.Contains(searchString) || rg.staff.UserName.Contains(searchString) || rg.description.Contains(searchString) || rg.status.Contains(searchString) || rg.activityWorkplan.Objective.Contains(searchString) || rg.activityWorkplan.PerformanceIndicator.Contains(searchString));
                }
            }
            else
            {
                if (searchStartActivityDate != null && searchEndActivityDate != null)
                {
                    activities = activities.Where(rg => (rg.startDate >= (DateTime)searchStartActivityDate && rg.endDate <= (DateTime)searchEndActivityDate));
                }
                else if (searchStartActivityDate != null)
                {
                    activities = activities.Where(rg => rg.startDate == (DateTime)searchStartActivityDate);
                }
                else if (searchEndActivityDate != null)
                {
                    activities = activities.Where(rg => rg.endDate <= (DateTime)searchEndActivityDate);
                }
            }

            switch (sortOrder)
            {
                case "name_desc":
                    activities = activities.OrderByDescending(rg => rg.staff.LastName);
                    break;
                case "StartDate":
                    activities = activities.OrderBy(rg => rg.startDate);
                    break;
                case "startdate_desc":
                    activities = activities.OrderByDescending(rg => rg.startDate);
                    break;
                case "EndDate":
                    activities = activities.OrderBy(rg => rg.endDate);
                    break;
                case "enddate_desc":
                    activities = activities.OrderByDescending(rg => rg.endDate);
                    break;
                case "Status":
                    activities = activities.OrderBy(rg => rg.status);
                    break;
                case "status_desc":
                    activities = activities.OrderByDescending(rg => rg.status);
                    break;
                case "Workplan":
                    activities = activities.OrderBy(rg => rg.activityWorkplan.Objective);
                    break;
                case "workplan_desc":
                    activities = activities.OrderByDescending(rg => rg.activityWorkplan.Objective);
                    break;
                default:  // Name ascending 
                    activities = activities.OrderBy(rg => rg.staff.LastName);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(activities.ToPagedList(pageNumber, pageSize));
        }

        // GET: /WeeklyActivityLog/Details/5
        [Authorize(Roles = "Admin, CanViewWeeklyActivityLog, WeeklyActivityLog")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WeeklyActivityLog weeklyactivitylog = db.WeeklyActivityLogs.Find(id);
            

            //Check if the user belongs to AdminGroup to be able to show all data in the list else show only user's own records
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());

            if (currentUser != null)
            {
                if (!CheckGroupAffiliate.checkGroupExist(currentUser, groups))
                    weeklyactivitylog = (WeeklyActivityLog)db.WeeklyActivityLogs.Where(rg => (rg.createdBy.Contains(currentUser.UserName) && rg.activityLogId == id)).FirstOrDefault();
            }
            //Check stops here

            if (weeklyactivitylog == null)
            {
                return HttpNotFound();
            }

            return View(weeklyactivitylog);
        }

        // GET: /WeeklyActivityLog/Create
        [Authorize(Roles = "Admin, CanAddWeeklyActivityLog, WeeklyActivityLog")]
        public ActionResult Create()
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            bool groupExists = CheckGroupAffiliate.checkGroupExist(currentUser, groups);

            if (groupExists)
            {
                ViewBag.userId = new SelectList(db.Users, "Id", "FullName");
            }
            else {
                ViewBag.userId = new SelectList(db.Users.Where(r => r.UserName == currentUser.UserName), "Id", "FullName");
            }
            ViewBag.workplanId = new SelectList(db.ActivityWorkplans, "workplanId", "FullObjective");
            //ViewBag.status = listStatus(null);
            return View();
        }

        // POST: /WeeklyActivityLog/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanAddWeeklyActivityLog, WeeklyActivityLog")]
        public ActionResult Create([Bind(Include="workplanId,userId,startDate,endDate,description")] WeeklyActivityLog weeklyactivitylog)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            bool groupExists = CheckGroupAffiliate.checkGroupExist(currentUser, groups);

            try
            {
                
                if (ModelState.IsValid)
                {
                    var wal = db.WeeklyActivityLogs.FirstOrDefault(p => p.userId == weeklyactivitylog.userId && p.startDate == weeklyactivitylog.startDate && p.endDate == weeklyactivitylog.endDate);
                    if (wal == null)
                    {
                        if (currentUser != null)
                            weeklyactivitylog.createdBy = currentUser.UserName;
                        else
                            weeklyactivitylog.createdBy = User.Identity.Name;

                        weeklyactivitylog.createdDate = DateTime.Now;
                        weeklyactivitylog.status = "OPEN";
                        db.WeeklyActivityLogs.Add(weeklyactivitylog);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Weekly Activity Log already entered: " + weeklyactivitylog.description);

                    if (groupExists)
                    {
                        ViewBag.userId = new SelectList(db.Users, "Id", "FullName", weeklyactivitylog.userId);
                    }
                    else
                    {
                        ViewBag.userId = new SelectList(db.Users.Where(r => r.UserName == currentUser.UserName), "Id", "FullName", weeklyactivitylog.userId);
                    }
                    //ViewBag.status = listStatus(weeklyactivitylog.status);
                    ViewBag.workplanId = new SelectList(db.ActivityWorkplans, "workplanId", "FullObjective", weeklyactivitylog.workplanId);
                    return View(weeklyactivitylog);
                }
                //ViewBag.userId = new SelectList(db.Users, "Id", "FullName", weeklyactivitylog.userId);
                if (groupExists)
                {
                    ViewBag.userId = new SelectList(db.Users, "Id", "FullName", weeklyactivitylog.userId);
                }
                else
                {
                    ViewBag.userId = new SelectList(db.Users.Where(r => r.UserName == currentUser.UserName), "Id", "FullName", weeklyactivitylog.userId);
                }

                //ViewBag.status = listStatus(weeklyactivitylog.status);
                ViewBag.workplanId = new SelectList(db.ActivityWorkplans, "workplanId", "FullObjective", weeklyactivitylog.workplanId);
                return View(weeklyactivitylog);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error occurred while saving record: " + ex.Message);
                if (groupExists)
                {
                    ViewBag.userId = new SelectList(db.Users, "Id", "FullName", weeklyactivitylog.userId);
                }
                else
                {
                    ViewBag.userId = new SelectList(db.Users.Where(r => r.UserName == currentUser.UserName), "Id", "FullName", weeklyactivitylog.userId);
                }
                //ViewBag.status = listStatus(weeklyactivitylog.status);
                ViewBag.workplanId = new SelectList(db.ActivityWorkplans, "workplanId", "FullObjective", weeklyactivitylog.workplanId);
                return View();
            }
        }

        // GET: /WeeklyActivityLog/Edit/5
        [Authorize(Roles = "Admin, CanEditWeeklyActivityLog, WeeklyActivityLog")]
        public ActionResult Edit(int? id)
        {
            //Check if the user belongs to AdminGroup to be able to show all data in the list else show only user's own records
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            bool groupExists = CheckGroupAffiliate.checkGroupExist(currentUser, groups);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string status = "OPEN";
            WeeklyActivityLog weeklyactivitylog = (WeeklyActivityLog)db.WeeklyActivityLogs.Where(r => r.activityLogId == id && r.status == status).FirstOrDefault();

            if (currentUser != null)
            {
                if (!groupExists)
                    weeklyactivitylog = db.WeeklyActivityLogs.Where(rg => (rg.createdBy == currentUser.UserName && rg.activityLogId == id)).FirstOrDefault();
            }
            //Check stops here

            if (weeklyactivitylog == null)
            {
                return HttpNotFound();
            }

            if (groupExists)
            {
                ViewBag.userId = new SelectList(db.Users, "Id", "FullName", weeklyactivitylog.userId);
            }
            else
            {
                ViewBag.userId = new SelectList(db.Users.Where(r => r.UserName == currentUser.UserName), "Id", "FullName", weeklyactivitylog.userId);
            }
            //ViewBag.status = listStatus(weeklyactivitylog.status);
            ViewBag.workplanId = new SelectList(db.ActivityWorkplans, "workplanId", "FullObjective", weeklyactivitylog.workplanId);
            return View(weeklyactivitylog);
        }

        // POST: /WeeklyActivityLog/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanEditWeeklyActivityLog, WeeklyActivityLog")]
        public ActionResult Edit([Bind(Include = "activityLogId,workplanId,userId,startDate,endDate,description")] WeeklyActivityLog weeklyactivitylog)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            bool groupExists = CheckGroupAffiliate.checkGroupExist(currentUser, groups);

            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        WeeklyActivityLog act = (WeeklyActivityLog)db.WeeklyActivityLogs.Where(c => c.activityLogId == weeklyactivitylog.activityLogId).FirstOrDefault();
                        if (groupExists)
                            act = (WeeklyActivityLog)db.WeeklyActivityLogs.Where(c => c.activityLogId == weeklyactivitylog.activityLogId).FirstOrDefault();
                        else
                            act = (WeeklyActivityLog)db.WeeklyActivityLogs.Where(c => c.activityLogId == weeklyactivitylog.activityLogId && c.staff.UserName == currentUser.UserName).FirstOrDefault();

                        act.description = weeklyactivitylog.description;
                        act.workplanId = weeklyactivitylog.workplanId;
                        act.userId = weeklyactivitylog.userId;
                        act.startDate = weeklyactivitylog.startDate;
                        act.endDate = weeklyactivitylog.endDate;

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
                        var databaseValues = (WeeklyActivityLog)entry.GetDatabaseValues().ToObject();
                        var clientValues = (WeeklyActivityLog)entry.Entity;
                        if (databaseValues.staff.FullName != clientValues.staff.LastName)
                            ModelState.AddModelError("Staff", "Current value: " + databaseValues.staff.LastName);
                        if (databaseValues.startDate != clientValues.startDate)
                            ModelState.AddModelError("Start Date", "Current value: " + databaseValues.startDate);
                        if (databaseValues.endDate != clientValues.endDate)
                            ModelState.AddModelError("End Date", "Current value: " + databaseValues.endDate);
                        if (databaseValues.status != clientValues.status)
                            ModelState.AddModelError("Status", "Current value: " + databaseValues.status);

                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                          + "was modified by another user after you got the original value. The "
                          + "edit operation was canceled and the current values in the database "
                          + "have been displayed. If you still want to edit this record, click "
                          + "the Save button again. Otherwise click the Back to List hyperlink.");

                        weeklyactivitylog.Timestamp = databaseValues.Timestamp;
                        //ViewBag.userId = new SelectList(db.Users, "Id", "FullName", weeklyactivitylog.userId);
                        if (groupExists)
                        {
                            ViewBag.userId = new SelectList(db.Users, "Id", "FullName", weeklyactivitylog.userId);
                        }
                        else
                        {
                            ViewBag.userId = new SelectList(db.Users.Where(r => r.UserName == currentUser.UserName), "Id", "FullName", weeklyactivitylog.userId);
                        }
                        //ViewBag.status = listStatus(weeklyactivitylog.status);
                        ViewBag.workplanId = new SelectList(db.ActivityWorkplans, "workplanId", "FullObjective", weeklyactivitylog.workplanId);
                        return View();
                    }
                    return RedirectToAction("Index");
                }
                //ViewBag.userId = new SelectList(db.Users, "Id", "FullName", weeklyactivitylog.userId);

                if (groupExists)
                {
                    ViewBag.userId = new SelectList(db.Users, "Id", "FullName", weeklyactivitylog.userId);
                }
                else
                {
                    ViewBag.userId = new SelectList(db.Users.Where(r => r.UserName == currentUser.UserName), "Id", "FullName", weeklyactivitylog.userId);
                }
                //ViewBag.status = listStatus(weeklyactivitylog.status);
                ViewBag.workplanId = new SelectList(db.ActivityWorkplans, "workplanId", "FullObjective", weeklyactivitylog.workplanId);
                return View(weeklyactivitylog);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error occurred while saving record: " + ex.Message);
                //ViewBag.assignedToId = new SelectList(db.Users, "userId", "FullName", weeklyactivitylog.userId);
                if (groupExists)
                {
                    ViewBag.userId = new SelectList(db.Users, "Id", "FullName", weeklyactivitylog.userId);
                }
                else
                {
                    ViewBag.userId = new SelectList(db.Users.Where(r => r.UserName == currentUser.UserName), "Id", "FullName", weeklyactivitylog.userId);
                }
                //ViewBag.status = listStatus(weeklyactivitylog.status);
                ViewBag.workplanId = new SelectList(db.ActivityWorkplans, "workplanId", "FullObjective", weeklyactivitylog.workplanId);
                return View(weeklyactivitylog);
            }
        }

        // GET: /WeeklyActivityLog/Delete/5
        [Authorize(Roles = "Admin, CanDeleteWeeklyActivityLog, WeeklyActivityLog")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WeeklyActivityLog weeklyactivitylog = db.WeeklyActivityLogs.Find(id);

            //Check if the user belongs to AdminGroup to be able to show all data in the list else show only user's own records
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());

            bool groupExists = CheckGroupAffiliate.checkGroupExist(currentUser, groups);

            if (currentUser != null)
            {
                if (!groupExists)
                    weeklyactivitylog = (WeeklyActivityLog)db.WeeklyActivityLogs.Where(rg => (rg.createdBy.Contains(currentUser.UserName) && rg.activityLogId == id)).FirstOrDefault();
                else
                    weeklyactivitylog = (WeeklyActivityLog)db.WeeklyActivityLogs.Where(rg => (rg.activityLogId == id)).FirstOrDefault();
            }
            //Check stops here

            if (weeklyactivitylog == null)
            {
                return HttpNotFound();
            }
            return View(weeklyactivitylog);
        }

        // POST: /WeeklyActivityLog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanDeleteWeeklyActivityLog, WeeklyActivityLog")]
        public ActionResult DeleteConfirmed(int id)
        {
            WeeklyActivityLog weeklyactivitylog = db.WeeklyActivityLogs.Find(id);
            db.WeeklyActivityLogs.Remove(weeklyactivitylog);
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
