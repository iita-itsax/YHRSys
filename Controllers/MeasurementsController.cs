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
    public class MeasurementsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /MediumPrepType/
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.Measurements.ToList());
        }

        // GET: /MediumPrepType/Details/5
        [Authorize(Roles = "Admin, CanViewMeasurements, Measurements")]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Measurements tblmeasurement = db.Measurements.Find(id);
            if (tblmeasurement == null)
            {
                return HttpNotFound();
            }
            return View(tblmeasurement);
        }

        // GET: /MediumPrepType/Create
        [Authorize(Roles = "Admin, CanAddMeasurements, Measurements")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /MediumPrepType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanAddMeasurements, Measurements")]
        public ActionResult Create([Bind(Include = "name,uom")] Measurements tblmeasurement)
        {
            if (ModelState.IsValid)
            {
                var loc = db.Measurements.FirstOrDefault(p => p.name == tblmeasurement.name);
                if (loc == null)
                {
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var currentUser = manager.FindById(User.Identity.GetUserId());

                    if (currentUser != null)
                        tblmeasurement.createdBy = currentUser.UserName;
                    else
                        tblmeasurement.createdBy = User.Identity.Name;

                    tblmeasurement.createdDate = DateTime.Now;

                    db.Measurements.Add(tblmeasurement);
                    db.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Unit of Measurement already registered: " + tblmeasurement.name);
                    return View(tblmeasurement);
                }
                return RedirectToAction("Index");
            }

            return View(tblmeasurement);
        }

        // GET: /MediumPrepType/Edit/5
        [Authorize(Roles = "Admin, CanEditMeasurements, Measurements")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Measurements tblmeasurement = db.Measurements.Find(id);
            if (tblmeasurement == null)
            {
                return HttpNotFound();
            }
            return View(tblmeasurement);
        }

        // POST: /MediumPrepType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanEditMeasurements, Measurements")]
        public ActionResult Edit([Bind(Include = "measurementId,name,uom")] Measurements tblmeasurement)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //db.Entry(tbllocation).State = EntityState.Modified;
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var currentUser = manager.FindById(User.Identity.GetUserId());

                    var uom = db.Measurements.Where(c => c.measurementId == tblmeasurement.measurementId).FirstOrDefault();
                    uom.name = tblmeasurement.name;
                    uom.uom = tblmeasurement.uom;

                    if (currentUser != null)
                        uom.updatedBy = currentUser.UserName;
                    else
                        uom.updatedBy = User.Identity.Name;

                    uom.updatedDate = DateTime.Now;

                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var databaseValues = (Measurements)entry.GetDatabaseValues().ToObject();
                    var clientValues = (Measurements)entry.Entity;
                    if (databaseValues.name != clientValues.name)
                        ModelState.AddModelError("Name", "Current value: " + databaseValues.name);
                    if (databaseValues.uom != clientValues.uom)
                        ModelState.AddModelError("Abbreviation", "Current value: " + databaseValues.uom);

                    ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                      + "was modified by another user after you got the original value. The "
                      + "edit operation was canceled and the current values in the database "
                      + "have been displayed. If you still want to edit this record, click "
                      + "the Save button again. Otherwise click the Back to List hyperlink.");

                    tblmeasurement.Timestamp = databaseValues.Timestamp;
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View(tblmeasurement);
        }

        // GET: /MediumPrepType/Delete/5
        [Authorize(Roles = "Admin, CanDeleteMeasurements, Measurements")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Measurements tblmeasurement = db.Measurements.Find(id);
            if (tblmeasurement == null)
            {
                return HttpNotFound();
            }
            return View(tblmeasurement);
        }

        // POST: /MediumPrepType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanDeleteMeasurements, Measurements")]
        public ActionResult DeleteConfirmed(long id)
        {
            Measurements tblmeasurement = db.Measurements.Find(id);
            db.Measurements.Remove(tblmeasurement);
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
