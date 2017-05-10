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
    public class VarietyDefinitionController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /MediumPrepType/
        [Authorize(Roles = "Admin, CanViewVarietyDefinition, VarietyDefinition")]
        public ActionResult Index()
        {
            return View(db.VarietyDefinitions.ToList());
        }

        // GET: /MediumPrepType/Details/5
        [Authorize(Roles = "Admin, CanViewVarietyDefinition, VarietyDefinition")]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VarietyDefinition tblvarietydefinition = db.VarietyDefinitions.Find(id);
            if (tblvarietydefinition == null)
            {
                return HttpNotFound();
            }
            return View(tblvarietydefinition);
        }

        // GET: /MediumPrepType/Create
        [Authorize(Roles = "Admin, CanAddVarietyDefinition, VarietyDefinition")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /MediumPrepType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanAddVarietyDefinition, VarietyDefinition")]
        public ActionResult Create([Bind(Include = "name")] VarietyDefinition tblvarietydefinition)
        {
            if (ModelState.IsValid)
            {
                var med = db.VarietyDefinitions.FirstOrDefault(p => p.name == tblvarietydefinition.name);
                if (med == null)
                {
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var currentUser = manager.FindById(User.Identity.GetUserId());

                    if (currentUser != null)
                        tblvarietydefinition.createdBy = currentUser.UserName;
                    else
                        tblvarietydefinition.createdBy = User.Identity.Name;

                    tblvarietydefinition.createdDate = DateTime.Now;

                    db.VarietyDefinitions.Add(tblvarietydefinition);
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.Message = "Activity definition already registered: " + tblvarietydefinition.name;
                    return View(tblvarietydefinition);
                }
                return RedirectToAction("Index");
            }

            return View(tblvarietydefinition);
        }

        // GET: /MediumPrepType/Edit/5
        [Authorize(Roles = "Admin, CanEditVarietyDefinition, VarietyDefinition")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VarietyDefinition tblvarietydefinition = db.VarietyDefinitions.Find(id);
            if (tblvarietydefinition == null)
            {
                return HttpNotFound();
            }
            return View(tblvarietydefinition);
        }

        // POST: /MediumPrepType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanEditVarietyDefinition, VarietyDefinition")]
        public ActionResult Edit([Bind(Include = "varietyDefinitionId,name")] VarietyDefinition tblvarietydefinition)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //db.Entry(tbllocation).State = EntityState.Modified;
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var currentUser = manager.FindById(User.Identity.GetUserId());

                    var med = db.VarietyDefinitions.Where(c => c.varietyDefinitionId == tblvarietydefinition.varietyDefinitionId).FirstOrDefault();
                    med.name = tblvarietydefinition.name;

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

                    tblvarietydefinition.Timestamp = databaseValues.Timestamp;
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View(tblvarietydefinition);
        }

        // GET: /MediumPrepType/Delete/5
        [Authorize(Roles = "Admin, CanDeleteVarietyDefinition, VarietyDefinition")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VarietyDefinition tblvarietydefinition = db.VarietyDefinitions.Find(id);
            if (tblvarietydefinition == null)
            {
                return HttpNotFound();
            }
            return View(tblvarietydefinition);
        }

        // POST: /MediumPrepType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanDeleteVarietyDefinition, VarietyDefinition")]
        public ActionResult DeleteConfirmed(long id)
        {
            VarietyDefinition tblactivityvarietydefinition = db.VarietyDefinitions.Find(id);
            db.VarietyDefinitions.Remove(tblactivityvarietydefinition);
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