using System;
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
using DropdownSelect.Models;
using PagedList;

namespace YHRSys.Controllers
{
    [Authorize]
    public class ActivityController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        SelectedValue selVal = new SelectedValue();

        private List<SelectListItem> listQuality(int quality)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "--Select--", Value = "", Selected = selVal.checkForSelectedValue(0, quality) });
            items.Add(new SelectListItem { Text = "100%", Value = "100", Selected = selVal.checkForSelectedValue(100, quality) });
            items.Add(new SelectListItem { Text = "80%", Value = "80", Selected = selVal.checkForSelectedValue(80, quality) });
            items.Add(new SelectListItem { Text = "60%", Value = "60", Selected = selVal.checkForSelectedValue(60, quality) });
            items.Add(new SelectListItem { Text = "40%", Value = "40", Selected = selVal.checkForSelectedValue(40, quality) });
            items.Add(new SelectListItem { Text = "20%", Value = "20", Selected = selVal.checkForSelectedValue(20, quality) });
            return items;
        }

        // GET: /Activity/
        public ActionResult Index(string sortOrder, string currentFilter, string currentStartDateFilter, string currentEndDateFilter, string searchString, DateTime? searchStartActivityDate, DateTime? searchEndActivityDate, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.LocationSortParm = sortOrder == "Location" ? "location_desc" : "Location";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.TypeSortParm = sortOrder == "Type" ? "type_desc" : "Type";
            ViewBag.VarietySortParm = sortOrder == "Variety" ? "variety_desc" : "Variety";
            ViewBag.QualitySortParm = sortOrder == "Quality" ? "quality_desc" : "Quality";
            ViewBag.QuantitySortParm = sortOrder == "Quantity" ? "quantity_desc" : "Quantity";
            ViewBag.StatusSortParm = sortOrder == "Status" ? "status_desc" : "Status";

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

            var activities = from r in db.Activities select r;

            if (!String.IsNullOrEmpty(searchString))
            {
                if (searchStartActivityDate != null && searchEndActivityDate != null)
                {
                    activities = activities.Where(rg => (rg.variety.varietyDefinition.name.Contains(searchString)
                                       || rg.activityDefinition.name.Contains(searchString) || rg.mediumPrepType.typename.Contains(searchString) || rg.status.Equals(searchString)
                                          || rg.location.name.Contains(searchString) || rg.description.Contains(searchString)) && (rg.activityDate >= (DateTime)searchStartActivityDate && rg.activityDate <= (DateTime)searchEndActivityDate));
                }
                else if (searchStartActivityDate != null)
                {
                    activities = activities.Where(rg => (rg.variety.varietyDefinition.name.Contains(searchString)
                                          || rg.activityDefinition.name.Contains(searchString) || rg.mediumPrepType.typename.Contains(searchString) || rg.status.Equals(searchString)
                                          || rg.location.name.Contains(searchString) || rg.description.Contains(searchString)) && (rg.activityDate == (DateTime)searchStartActivityDate));
                }
                else if (searchEndActivityDate != null)
                {
                    activities = activities.Where(rg => (rg.variety.varietyDefinition.name.Contains(searchString)
                                          || rg.activityDefinition.name.Contains(searchString) || rg.mediumPrepType.typename.Contains(searchString) || rg.status.Equals(searchString)
                                          || rg.location.name.Contains(searchString) || rg.description.Contains(searchString)) && (rg.activityDate == (DateTime)searchEndActivityDate));
                }
                else
                {
                    activities = activities.Where(rg => rg.variety.varietyDefinition.name.Contains(searchString)
                                          || rg.activityDefinition.name.Contains(searchString) || rg.mediumPrepType.typename.Contains(searchString) || rg.status.Equals(searchString)
                                          || rg.location.name.Contains(searchString) || rg.description.Contains(searchString));
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

            switch (sortOrder)
            {
                case "name_desc":
                    activities = activities.OrderByDescending(rg => rg.activityDefinition.name);
                    break;
                case "location_desc":
                    activities = activities.OrderByDescending(rg => rg.location.name);
                    break;
                case "Location":
                    activities = activities.OrderBy(rg => rg.location.name);
                    break;
                case "type_desc":
                    activities = activities.OrderByDescending(rg => rg.mediumPrepType.typename);
                    break;
                case "Type":
                    activities = activities.OrderBy(rg => rg.mediumPrepType.typename);
                    break;
                case "Variety":
                    activities = activities.OrderBy(rg => rg.variety.sampleNumber).ThenBy(rg => rg.variety.varietyDefinition.name);
                    break;
                case "variety_desc":
                    activities = activities.OrderByDescending(rg => rg.variety.sampleNumber).ThenBy(rg => rg.variety.varietyDefinition.name);
                    break;
                case "quality_desc":
                    activities = activities.OrderByDescending(rg => rg.quality);
                    break;
                case "Quality":
                    activities = activities.OrderBy(rg => rg.quality);
                    break;
                case "quantity_desc":
                    activities = activities.OrderByDescending(rg => rg.quantity);
                    break;
                case "Quantity":
                    activities = activities.OrderBy(rg => rg.quantity);
                    break;
                case "Date":
                    activities = activities.OrderBy(rg => rg.activityDate);
                    break;
                case "date_desc":
                    activities = activities.OrderByDescending(rg => rg.activityDate);
                    break;
                case "Status":
                    activities = activities.OrderBy(rg => rg.status);
                    break;
                case "status_desc":
                    activities = activities.OrderByDescending(rg => rg.status);
                    break;
                default:  // Name ascending 
                    activities = activities.OrderBy(rg => rg.activityDefinition.name);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(activities.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /Activity/Details/5
        [Authorize(Roles = "Admin, CanViewActivity, Activity")]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            ViewBag.userId = new SelectList(db.Users, "Id", "FullName");
            return View(activity);
        }

        //
        // GET: /Activity/Create
        [Authorize(Roles = "Admin, CanAddActivity, Activity")]
        public ActionResult Create()
        {
            ViewBag.locationId = new SelectList(db.Locations, "locationId", "name");
            ViewBag.typeId = new SelectList(db.MediumPrepTypes, "typeId", "typename");
            ViewBag.activityDefinitionId = new SelectList(db.ActivityDefinitions, "activityDefinitionId", "name");
            ViewBag.varietyId = new SelectList(db.Varieties, "varietyId", "FullDescription");
            //ViewBag.assignedToId = new SelectList(db.Users, "userId", "FullName");
            ViewBag.quality = listQuality(0);
            return View();
        }

        //
        // POST: /Activity/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanAddActivity, Activity")]
        public ActionResult Create([Bind(Include = "activityDefinitionId,locationId,typeId,varietyId,quantity,quality,description,activityDate,status")] Activity tblactivity)
        {
            //Activity activity = new Activity();
            try
            {
                if (ModelState.IsValid)
                {
                    var med = db.Activities.FirstOrDefault(p => p.varietyId == tblactivity.varietyId && p.locationId == tblactivity.locationId && p.typeId == tblactivity.typeId && p.activityDefinitionId == tblactivity.activityDefinitionId);
                    if (med == null)
                    {
                        var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                        var currentUser = manager.FindById(User.Identity.GetUserId());

                        if (currentUser != null)
                            tblactivity.createdBy = currentUser.UserName;
                        else
                            tblactivity.createdBy = User.Identity.Name;

                        tblactivity.createdDate = DateTime.Now;

                        db.Activities.Add(tblactivity);
                        db.SaveChanges();
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Activity already registered: " + tblactivity.activityDefinition.name);
                        ViewBag.locationId = new SelectList(db.Locations, "locationId", "name", tblactivity.locationId);
                        ViewBag.typeId = new SelectList(db.MediumPrepTypes, "typeId", "typename", tblactivity.typeId);
                        ViewBag.activityDefinitionId = new SelectList(db.ActivityDefinitions, "activityDefinitionId", "name", tblactivity.activityDefinitionId);
                        ViewBag.varietyId = new SelectList(db.Varieties, "varietyId", "FullDescription");
                        //ViewBag.assignedToId = new SelectList(db.Users, "userId", "FullName", tblactivity.userId);
                        ViewBag.quality = listQuality(tblactivity.quality);

                        return View(tblactivity);
                    }
                    return RedirectToAction("Index");
                }

                ViewBag.locationId = new SelectList(db.Locations, "locationId", "name", tblactivity.locationId);
                ViewBag.typeId = new SelectList(db.MediumPrepTypes, "typeId", "typename", tblactivity.typeId);
                ViewBag.activityDefinitionId = new SelectList(db.ActivityDefinitions, "activityDefinitionId", "name", tblactivity.activityDefinitionId);
                ViewBag.varietyId = new SelectList(db.Varieties, "varietyId", "FullDescription");
                //ViewBag.assignedToId = new SelectList(db.Users, "userId", "FullName", tblactivity.userId);
                ViewBag.quality = listQuality(tblactivity.quality);

                return View(tblactivity);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error occurred while saving record: " + ex.Message);
                ViewBag.locationId = new SelectList(db.Locations, "locationId", "name", tblactivity.locationId);
                ViewBag.typeId = new SelectList(db.MediumPrepTypes, "typeId", "typename", tblactivity.typeId);
                ViewBag.activityDefinitionId = new SelectList(db.ActivityDefinitions, "activityDefinitionId", "name", tblactivity.activityDefinitionId);
                ViewBag.varietyId = new SelectList(db.Varieties, "varietyId", "FullDescription");
                //ViewBag.assignedToId = new SelectList(db.Users, "userId", "FullName", tblactivity.userId);
                ViewBag.quality = listQuality(tblactivity.quality);
                return View();
            }
        }

        //
        // GET: /Activity/Edit/5
        [Authorize(Roles = "Admin, CanEditActivity, Activity")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            ViewBag.locationId = new SelectList(db.Locations, "locationId", "name", activity.locationId);
            ViewBag.typeId = new SelectList(db.MediumPrepTypes, "typeId", "typename", activity.typeId);
            ViewBag.activityDefinitionId = new SelectList(db.ActivityDefinitions, "activityDefinitionId", "name", activity.activityDefinitionId);
            ViewBag.varietyId = new SelectList(db.Varieties, "varietyId", "FullDescription");
            //ViewBag.assignedToId = new SelectList(db.Users, "userId", "FullName", tblactivity.userId);
            ViewBag.quality = listQuality(activity.quality);
            return View(activity);
        }

        //
        // POST: /Activity/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanEditActivity, Activity")]
        public ActionResult Edit([Bind(Include = "activityId,locationId,typeId,activityDefinitionId,varietyId,quantity,quality,description,activityDate,status")] Activity tblactivity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        //db.Entry(tbllocation).State = EntityState.Modified;
                        var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                        var currentUser = manager.FindById(User.Identity.GetUserId());

                        var act = db.Activities.Where(c => c.activityId == tblactivity.activityId).FirstOrDefault();
                        act.activityDefinitionId = tblactivity.activityDefinitionId;
                        act.locationId = tblactivity.locationId;
                        act.typeId = tblactivity.typeId;
                        act.description = tblactivity.description;
                        act.varietyId = tblactivity.varietyId;
                        act.quality = tblactivity.quality;
                        act.quantity = tblactivity.quantity;
                        act.activityDate = tblactivity.activityDate;
                        act.status = tblactivity.status;

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
                        var databaseValues = (Activity)entry.GetDatabaseValues().ToObject();
                        var clientValues = (Activity)entry.Entity;
                        if (databaseValues.location.name != clientValues.location.name)
                            ModelState.AddModelError("Location", "Current value: " + databaseValues.location.name);
                        if (databaseValues.activityDefinition.name != clientValues.activityDefinition.name)
                            ModelState.AddModelError("Def. name", "Current value: " + databaseValues.activityDefinition.name);
                        if (databaseValues.mediumPrepType.typename != clientValues.mediumPrepType.typename)
                            ModelState.AddModelError("Type name", "Current value: " + databaseValues.mediumPrepType.typename);

                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                          + "was modified by another user after you got the original value. The "
                          + "edit operation was canceled and the current values in the database "
                          + "have been displayed. If you still want to edit this record, click "
                          + "the Save button again. Otherwise click the Back to List hyperlink.");

                        tblactivity.Timestamp = databaseValues.Timestamp;
                        ViewBag.locationId = new SelectList(db.Locations, "locationId", "name", tblactivity.locationId);
                        ViewBag.typeId = new SelectList(db.MediumPrepTypes, "typeId", "typename", tblactivity.typeId);
                        ViewBag.activityDefinitionId = new SelectList(db.ActivityDefinitions, "activityDefinitionId", "name", tblactivity.activityDefinitionId);
                        ViewBag.varietyId = new SelectList(db.Varieties, "varietyId", "FullDescription");
                        //ViewBag.assignedToId = new SelectList(db.Users, "userId", "FullName, tblactivity.userId");
                        ViewBag.quality = listQuality(tblactivity.quality);
                        return View();
                    }
                    return RedirectToAction("Index");
                }
                ViewBag.locationId = new SelectList(db.Locations, "locationId", "name", tblactivity.locationId);
                ViewBag.typeId = new SelectList(db.MediumPrepTypes, "typeId", "typename", tblactivity.typeId);
                ViewBag.activityDefinitionId = new SelectList(db.ActivityDefinitions, "activityDefinitionId", "name", tblactivity.activityDefinitionId);
                ViewBag.varietyId = new SelectList(db.Varieties, "varietyId", "FullDescription", tblactivity.varietyId);
                //ViewBag.assignedToId = new SelectList(db.Users, "userId", "FullName", tblactivity.userId);
                ViewBag.quality = listQuality(tblactivity.quality);
                return View(tblactivity);
            }
            catch
            {
                ViewBag.locationId = new SelectList(db.Locations, "locationId", "name", tblactivity.locationId);
                ViewBag.typeId = new SelectList(db.MediumPrepTypes, "typeId", "typename", tblactivity.typeId);
                ViewBag.activityDefinitionId = new SelectList(db.ActivityDefinitions, "activityDefinitionId", "name", tblactivity.activityDefinitionId);
                ViewBag.varietyId = new SelectList(db.Varieties, "varietyId", "FullDescription");
                //ViewBag.assignedToId = new SelectList(db.Users, "userId", "FullName", tblactivity.userId);
                ViewBag.quality = listQuality(tblactivity.quality);
                return View(tblactivity);
            }
        }

        //
        // GET: /Activity/Delete/5
        [Authorize(Roles = "Admin, CanDeleteActivity, Activity")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        //
        // POST: /Activity/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanDeleteActivity, Activity")]
        public ActionResult DeleteConfirmed(long? id, FormCollection collection)
        {
            try
            {
                Activity activity = db.Activities.Find(id);
                db.Activities.Remove(activity);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        // GET: /ActivityAssignment/Delete/5
        [Authorize(Roles = "Admin, CanDeleteActivity, Activity")]
        public ActionResult DeleteAssignee(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityAssignment tblactivityassignment = db.ActivityAssignments.Find(id);
            if (tblactivityassignment == null)
            {
                return HttpNotFound();
            }
            return View(tblactivityassignment);
        }

        // POST: /LocationUser/Delete/5
        [HttpPost, ActionName("DeleteAssignee")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanDeleteActivity, Activity")]
        public ActionResult DeleteAssigneeConfirmed(long id, long activityId)
        {
            ActivityAssignment tblactivityassignment = db.ActivityAssignments.Find(id);
            db.ActivityAssignments.Remove(tblactivityassignment);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = activityId });
        }

        // POST: /LocationUser/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanAddActivity, Activity")]
        public ActionResult AddAssignee([Bind(Include = "activityId,userId,todo")] ActivityAssignment tblactivityassignment)
        {
            if (ModelState.IsValid)
            {
                var activityassignment = db.ActivityAssignments.FirstOrDefault(p => p.userId == tblactivityassignment.userId
                    && p.activityId == tblactivityassignment.activityId
                    && p.todo == tblactivityassignment.todo);
                if (activityassignment == null)
                {
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var currentUser = manager.FindById(User.Identity.GetUserId());

                    if (currentUser != null)
                        tblactivityassignment.createdBy = currentUser.UserName;
                    else
                        tblactivityassignment.createdBy = User.Identity.Name;

                    tblactivityassignment.createdDate = DateTime.Now;

                    db.ActivityAssignments.Add(tblactivityassignment);
                    db.SaveChanges();
                }
                else
                {
                    var user = db.ActivityAssignments.SingleOrDefault(p => p.userId == tblactivityassignment.userId);
                    ModelState.AddModelError(string.Empty, "Activity already assigned to user: " + user.AssigneeFullName + " and activity: " + tblactivityassignment.activity.activityDefinition.name);
                    ViewBag.userId = new SelectList(db.Users, "Id", "FullName");
                    //return View(tbllocationsubordinate);
                }

                return RedirectToAction("Details", new { id = tblactivityassignment.activityId });
            }

            ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblactivityassignment.userId);
            return RedirectToAction("Details", new { id = tblactivityassignment.activityId });
        }

        // GET: /Activity/EditAssignment/5
        [Authorize(Roles = "Admin, CanEditActivity, Activity")]
        public ActionResult EditAssignee(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityAssignment tblactivityassignment = db.ActivityAssignments.Find(id);
            if (tblactivityassignment == null)
            {
                return HttpNotFound();
            }

            ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblactivityassignment.userId);
            return View(tblactivityassignment);
        }

        // POST: /Activity/EditAssignment/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanEditActivity, Activity")]
        public ActionResult EditAssignee([Bind(Include = "assignmentId,activityId,userId,todo")] ActivityAssignment tblactivityassignment)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());

            if (ModelState.IsValid)
            {
                try
                {
                    //db.Entry(tbllocationuser).State = EntityState.Modified;
                    var activityassignment = db.ActivityAssignments.Where(c => c.assignmentId == tblactivityassignment.assignmentId).FirstOrDefault();
                    activityassignment.userId = tblactivityassignment.userId;
                    activityassignment.todo = tblactivityassignment.todo;
                    if (currentUser != null)
                        activityassignment.updatedBy = currentUser.UserName;
                    else
                        activityassignment.updatedBy = User.Identity.Name;

                    activityassignment.updatedDate = DateTime.Now;
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var databaseValues = (ActivityAssignment)entry.GetDatabaseValues().ToObject();
                    var clientValues = (ActivityAssignment)entry.Entity;
                    if (databaseValues.activity.activityDefinition.name != clientValues.activity.activityDefinition.name)
                        ModelState.AddModelError("Activity", "Current value: " + databaseValues.activity.activityDefinition.name);
                    if (databaseValues.AssigneeFullName != clientValues.AssigneeFullName)
                        ModelState.AddModelError("Activity Assignee", "Current value: " + databaseValues.AssigneeFullName);

                    ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                      + "was modified by another user after you got the original value. The "
                      + "edit operation was canceled and the current values in the database "
                      + "have been displayed. If you still want to edit this record, click "
                      + "the Save button again. Otherwise click the Back to List hyperlink.");

                    tblactivityassignment.Timestamp = databaseValues.Timestamp;
                    ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblactivityassignment.userId);
                    return View();
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                    ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblactivityassignment.userId);
                    return View();
                }
                return RedirectToAction("Details", new { id = tblactivityassignment.activityId });
            }
            ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblactivityassignment.userId);
            return View(tblactivityassignment);
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
