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
    public class ActivityAchievementController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        string[] groups = new string[] { "SuperAdmins" };

        // GET: /ActivityAchievement/
        public ActionResult Index(string sortOrder, string currentFilter, string currentStartDateFilter, string currentEndDateFilter, string searchString, DateTime? searchStartActivityDate, DateTime? searchEndActivityDate, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.ActivityLogSortParm = String.IsNullOrEmpty(sortOrder) ? "activityLog_desc" : "";
            ViewBag.AchievementDateSortParm = sortOrder == "AchievementDate" ? "achievementdate_desc" : "AchievementDate";
            ViewBag.DescriptionSortParm = sortOrder == "Description" ? "description_desc" : "Description";

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

            var activityachievements = db.ActivityAchievements.Include(a => a.WeeklyActivityLog);

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            bool groupExists = CheckGroupAffiliate.checkGroupExist(currentUser, groups);

            if (currentUser != null)
            {
                if (!groupExists)
                    activityachievements = activityachievements.Where(rg => (rg.createdBy.Contains(currentUser.UserName)));
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                if (searchStartActivityDate != null && searchEndActivityDate != null)
                {
                    activityachievements = activityachievements.Where(rg => (rg.WeeklyActivityLog.description.Contains(searchString) || rg.description.Contains(searchString)) && (rg.achievementDate >= (DateTime)searchStartActivityDate && rg.achievementDate <= (DateTime)searchEndActivityDate));
                }
                else if (searchStartActivityDate != null)
                {
                    activityachievements = activityachievements.Where(rg => (rg.WeeklyActivityLog.description.Contains(searchString) || rg.description.Contains(searchString)) && (rg.achievementDate == (DateTime)searchStartActivityDate));
                }
                else if (searchEndActivityDate != null)
                {
                    activityachievements = activityachievements.Where(rg => (rg.WeeklyActivityLog.description.Contains(searchString) || rg.description.Contains(searchString)) && (rg.achievementDate == (DateTime)searchEndActivityDate));
                }
                else
                {
                    activityachievements = activityachievements.Where(rg => (rg.WeeklyActivityLog.description.Contains(searchString) || rg.description.Contains(searchString)));
                }
            }
            else
            {
                if (searchStartActivityDate != null && searchEndActivityDate != null)
                {
                    activityachievements = activityachievements.Where(rg => (rg.achievementDate >= (DateTime)searchStartActivityDate && rg.achievementDate <= (DateTime)searchEndActivityDate));
                }
                else if (searchStartActivityDate != null)
                {
                    activityachievements = activityachievements.Where(rg => rg.achievementDate == (DateTime)searchStartActivityDate);
                }
                else if (searchEndActivityDate != null)
                {
                    activityachievements = activityachievements.Where(rg => rg.achievementDate <= (DateTime)searchEndActivityDate);
                }
            }

            switch (sortOrder)
            {
                case "activityLog_desc":
                    activityachievements = activityachievements.OrderByDescending(rg => rg.WeeklyActivityLog.description);
                    break;
                case "AchievementDate":
                    activityachievements = activityachievements.OrderBy(rg => rg.achievementDate);
                    break;
                case "achievementdate_desc":
                    activityachievements = activityachievements.OrderByDescending(rg => rg.achievementDate);
                    break;
                case "Description":
                    activityachievements = activityachievements.OrderBy(rg => rg.description);
                    break;
                case "description_desc":
                    activityachievements = activityachievements.OrderByDescending(rg => rg.description);
                    break;
                default:  // Name ascending 
                    activityachievements = activityachievements.OrderBy(rg => rg.WeeklyActivityLog.description);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(activityachievements.ToPagedList(pageNumber, pageSize));
            //return View(activityachievements.ToList());
        }

        // GET: /ActivityAchievement/Details/5
        [Authorize(Roles = "Admin, CanViewActivityAchievement, ActivityAchievement")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ActivityAchievement activityachievement = db.ActivityAchievements.Find(id);

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            bool groupExists = CheckGroupAffiliate.checkGroupExist(currentUser, groups);

            if (currentUser != null)
            {
                if (!groupExists)
                    activityachievement = (ActivityAchievement)db.ActivityAchievements.Where(rg => (rg.createdBy.Contains(currentUser.UserName) && rg.activityLogId == id)).FirstOrDefault();
            }

            if (activityachievement == null)
            {
                return HttpNotFound();
            }
            return View(activityachievement);
        }

        // GET: /ActivityAchievement/Create
        [Authorize(Roles = "Admin, CanAddActivityAchievement, ActivityAchievement")]
        public ActionResult Create()
        {
            
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            bool groupExists = CheckGroupAffiliate.checkGroupExist(currentUser, groups);

            if (groupExists)
            {
                ViewBag.activityLogId = new SelectList(db.WeeklyActivityLogs, "activityLogId", "FullDescription");
            }
            else
            {
                ViewBag.activityLogId = new SelectList(db.WeeklyActivityLogs.Where(r => r.staff.UserName == currentUser.UserName), "activityLogId", "FullDescription");
            }

            return View();
        }

        // POST: /ActivityAchievement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanAddActivityAchievement, ActivityAchievement")]
        public ActionResult Create([Bind(Include="activityLogId,achievementDate,description")] ActivityAchievement activityachievement)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            bool groupExists = CheckGroupAffiliate.checkGroupExist(currentUser, groups);

            try
            {

                if (ModelState.IsValid)
                {
                    var wal = db.ActivityAchievements.FirstOrDefault(p => p.description == activityachievement.description && p.achievementDate == activityachievement.achievementDate);
                    if (wal == null)
                    {
                        if (currentUser != null)
                            activityachievement.createdBy = currentUser.UserName;
                        else
                            activityachievement.createdBy = User.Identity.Name;

                        activityachievement.createdDate = DateTime.Now;

                        db.ActivityAchievements.Add(activityachievement);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Weekly Activity Log already entered: " + activityachievement.description);

                    if (groupExists)
                    {
                        ViewBag.activityLogId = new SelectList(db.WeeklyActivityLogs, "activityLogId", "FullDescription", activityachievement.activityLogId);
                    }
                    else
                    {
                        ViewBag.activityLogId = new SelectList(db.WeeklyActivityLogs.Where(r => r.staff.UserName == currentUser.UserName), "activityLogId", "description", activityachievement.activityLogId);
                    }

                    return View(activityachievement);
                }
                //ViewBag.userId = new SelectList(db.Users, "Id", "FullName", weeklyactivitylog.userId);
                if (groupExists)
                {
                    ViewBag.activityLogId = new SelectList(db.WeeklyActivityLogs, "activityLogId", "FullDescription", activityachievement.activityLogId);
                }
                else
                {
                    ViewBag.activityLogId = new SelectList(db.WeeklyActivityLogs.Where(r => r.staff.UserName == currentUser.UserName), "activityLogId", "description", activityachievement.activityLogId);
                }
                return View(activityachievement);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error occurred while saving record: " + ex.Message);
                //Check if user belongs to specified group determining owned activity achievement entries
                if (groupExists)
                {
                    ViewBag.activityLogId = new SelectList(db.WeeklyActivityLogs, "activityLogId", "FullDescription", activityachievement.activityLogId);
                }
                else
                {
                    ViewBag.activityLogId = new SelectList(db.WeeklyActivityLogs.Where(r => r.staff.UserName == currentUser.UserName), "activityLogId", "FullDescription", activityachievement.activityLogId);
                }
                return View(activityachievement);
            }
        }

        // GET: /ActivityAchievement/Edit/5
        [Authorize(Roles = "Admin, CanEditActivityAchievement, ActivityAchievement")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityAchievement activityachievement = db.ActivityAchievements.Find(id);
            if (activityachievement == null)
            {
                return HttpNotFound();
            }

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            bool groupExists = CheckGroupAffiliate.checkGroupExist(currentUser, groups);

            if (groupExists)
            {
                ViewBag.activityLogId = new SelectList(db.WeeklyActivityLogs, "activityLogId", "FullDescription", activityachievement.activityLogId);
            }
            else
            {
                activityachievement = (ActivityAchievement)db.ActivityAchievements.Where(rg => (rg.createdBy == currentUser.UserName && rg.achievementId == id)).FirstOrDefault();
                ViewBag.activityLogId = new SelectList(db.WeeklyActivityLogs.Where(r => r.staff.UserName == currentUser.UserName), "activityLogId", "FullDescription", activityachievement.activityLogId);
            }

            //ViewBag.activityLogId = new SelectList(db.WeeklyActivityLogs, "activityLogId", "userId", activityachievement.activityLogId);
            return View(activityachievement);
        }

        // POST: /ActivityAchievement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanEditActivityAchievement, ActivityAchievement")]
        public ActionResult Edit([Bind(Include="achievementId,activityLogId,achievementDate,description")] ActivityAchievement activityachievement)
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
                        var act = db.ActivityAchievements.FirstOrDefault();
                        if (groupExists)
                            act = (ActivityAchievement)db.WeeklyActivityLogs.Where(c => c.activityLogId == activityachievement.activityLogId);
                        else
                            act = (ActivityAchievement)db.WeeklyActivityLogs.Where(c => c.activityLogId == activityachievement.activityLogId && c.staff.UserName == currentUser.UserName);

                        act.description = activityachievement.description;
                        act.activityLogId = activityachievement.activityLogId;
                        act.achievementDate = activityachievement.achievementDate;
                        act.description = activityachievement.description;

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
                        var databaseValues = (ActivityAchievement)entry.GetDatabaseValues().ToObject();
                        var clientValues = (ActivityAchievement)entry.Entity;
                        if (databaseValues.WeeklyActivityLog.description != clientValues.WeeklyActivityLog.description)
                            ModelState.AddModelError("Activity Log", "Current value: " + databaseValues.WeeklyActivityLog.description);
                        if (databaseValues.description != clientValues.description)
                            ModelState.AddModelError("Description", "Current value: " + databaseValues.description);
                        if (databaseValues.achievementDate != clientValues.achievementDate)
                            ModelState.AddModelError("Achievement Date", "Current value: " + databaseValues.achievementDate);

                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                          + "was modified by another user after you got the original value. The "
                          + "edit operation was canceled and the current values in the database "
                          + "have been displayed. If you still want to edit this record, click "
                          + "the Save button again. Otherwise click the Back to List hyperlink.");

                        activityachievement.Timestamp = databaseValues.Timestamp;
 
                        if (groupExists)
                        {
                            ViewBag.activityLogId = new SelectList(db.WeeklyActivityLogs, "activityLogId", "FullDescription", activityachievement.activityLogId);
                        }
                        else
                        {
                            ViewBag.activityLogId = new SelectList(db.WeeklyActivityLogs.Where(r => r.staff.UserName == currentUser.UserName), "activityLogId", "FullDescription", activityachievement.activityLogId);
                        }
                        return View();
                    }
                    return RedirectToAction("Index");
                }

                if (groupExists)
                {
                    ViewBag.activityLogId = new SelectList(db.WeeklyActivityLogs, "activityLogId", "FullDescription", activityachievement.activityLogId);
                }
                else
                {
                    ViewBag.activityLogId = new SelectList(db.WeeklyActivityLogs.Where(r => r.staff.UserName == currentUser.UserName), "activityLogId", "FullDescription", activityachievement.activityLogId);
                }
                return View(activityachievement);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error occurred while saving record: " + ex.Message);
                //ViewBag.assignedToId = new SelectList(db.Users, "userId", "FullName", weeklyactivitylog.userId);
                if (groupExists)
                {
                    ViewBag.activityLogId = new SelectList(db.WeeklyActivityLogs, "activityLogId", "FullDescription", activityachievement.activityLogId);
                }
                else
                {
                    ViewBag.activityLogId = new SelectList(db.WeeklyActivityLogs.Where(r => r.staff.UserName == currentUser.UserName), "activityLogId", "FullDescription", activityachievement.activityLogId);
                }
                return View(activityachievement);
            }
        }

        // GET: /ActivityAchievement/Delete/5
        [Authorize(Roles = "Admin, CanDeleteActivityAchievement, ActivityAchievement")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityAchievement activityachievement = db.ActivityAchievements.Find(id);

            //Check if the user belongs to AdminGroup to be able to show all data in the list else show only user's own records
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());

            bool groupExists = CheckGroupAffiliate.checkGroupExist(currentUser, groups);

            if (currentUser != null)
            {
                if (!groupExists)
                    activityachievement = (ActivityAchievement)db.ActivityAchievements.Where(rg => (rg.createdBy == currentUser.UserName && rg.activityLogId == id)).FirstOrDefault();
                else
                    activityachievement = (ActivityAchievement)db.ActivityAchievements.Where(rg => (rg.activityLogId == id)).FirstOrDefault();
            }

            if (activityachievement == null)
            {
                return HttpNotFound();
            }
            return View(activityachievement);
        }

        // POST: /ActivityAchievement/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin, CanDeleteActivityAchievement, ActivityAchievement")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ActivityAchievement activityachievement = db.ActivityAchievements.Find(id);
            db.ActivityAchievements.Remove(activityachievement);
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
