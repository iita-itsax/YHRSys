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

namespace YHRSys.Controllers
{
    [Authorize]
    public class ActivityDefinitionController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /MediumPrepType/
        [Authorize(Roles = "Admin, CanViewActivityDefinition, ActivityDefinition")]
        public ActionResult Index()
        {
            return View(db.ActivityDefinitions.ToList());
        }

        // GET: /MediumPrepType/Details/5
        [Authorize(Roles = "Admin, CanViewActivityDefinition, ActivityDefinition")]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityDefinition tblactivitydefinition = db.ActivityDefinitions.Find(id);
            if (tblactivitydefinition == null)
            {
                return HttpNotFound();
            }
            return View(tblactivitydefinition);
        }

        // GET: /MediumPrepType/Create
        [Authorize(Roles = "Admin, CanAddActivityDefinition, ActivityDefinition")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /MediumPrepType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanAddActivityDefinition, ActivityDefinition")]
        public ActionResult Create([Bind(Include = "name")] ActivityDefinition tblactivitydefinition)
        {
            if (ModelState.IsValid)
            {
                var med = db.ActivityDefinitions.FirstOrDefault(p => p.name == tblactivitydefinition.name);
                if (med == null)
                {
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var currentUser = manager.FindById(User.Identity.GetUserId());

                    if (currentUser != null)
                        tblactivitydefinition.createdBy = currentUser.UserName;
                    else
                        tblactivitydefinition.createdBy = User.Identity.Name;

                    tblactivitydefinition.createdDate = DateTime.Now;

                    db.ActivityDefinitions.Add(tblactivitydefinition);
                    db.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Activity definition already registered: " + tblactivitydefinition.name);
                    return View(tblactivitydefinition);
                }
                return RedirectToAction("Index");
            }

            return View(tblactivitydefinition);
        }

        // GET: /MediumPrepType/Edit/5
        [Authorize(Roles = "Admin, CanEditActivityDefinition, ActivityDefinition")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityDefinition tblactivitydefinition = db.ActivityDefinitions.Find(id);
            if (tblactivitydefinition == null)
            {
                return HttpNotFound();
            }
            return View(tblactivitydefinition);
        }

        // POST: /MediumPrepType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanEditActivityDefinition, ActivityDefinition")]
        public ActionResult Edit([Bind(Include = "activityDefinitionId,name")] ActivityDefinition tblactivitydefinition)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //db.Entry(tbllocation).State = EntityState.Modified;
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var currentUser = manager.FindById(User.Identity.GetUserId());

                    var med = db.ActivityDefinitions.Where(c => c.activityDefinitionId == tblactivitydefinition.activityDefinitionId).FirstOrDefault();
                    med.name = tblactivitydefinition.name;

                    if (currentUser != null)
                        med.updatedBy = currentUser.UserName;
                    else
                        med.updatedBy = User.Identity.Name;

                    med.updatedDate = DateTime.Now;

                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var databaseValues = (ActivityDefinition)entry.GetDatabaseValues().ToObject();
                    var clientValues = (ActivityDefinition)entry.Entity;
                    if (databaseValues.name != clientValues.name)
                        ModelState.AddModelError("Activity Definition", "Current value: " + databaseValues.name);

                    ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                      + "was modified by another user after you got the original value. The "
                      + "edit operation was canceled and the current values in the database "
                      + "have been displayed. If you still want to edit this record, click "
                      + "the Save button again. Otherwise click the Back to List hyperlink.");

                    tblactivitydefinition.Timestamp = databaseValues.Timestamp;
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View(tblactivitydefinition);
        }

        // GET: /MediumPrepType/Delete/5
        [Authorize(Roles = "Admin, CanDeleteActivityDefinition, ActivityDefinition")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityDefinition tblactivitydefinition = db.ActivityDefinitions.Find(id);
            if (tblactivitydefinition == null)
            {
                return HttpNotFound();
            }
            return View(tblactivitydefinition);
        }

        // POST: /MediumPrepType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanDeleteActivityDefinition, ActivityDefinition")]
        public ActionResult DeleteConfirmed(long id)
        {
            ActivityDefinition tblactivitydefinition = db.ActivityDefinitions.Find(id);
            db.ActivityDefinitions.Remove(tblactivitydefinition);
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