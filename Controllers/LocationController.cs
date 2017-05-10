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
    public class LocationController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Location/
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.Locations.ToList());
        }

        // GET: /Location/Details/5
        [Authorize(Roles = "Admin, CanViewLocation, Location")]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location tbllocation = db.Locations.Find(id);
            if (tbllocation == null)
            {
                return HttpNotFound();
            }
            return View(tbllocation);
        }

        // GET: /Location/Create
        [Authorize(Roles = "Admin, CanAddLocation, Location")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Location/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanAddLocation, Location")]
        public ActionResult Create([Bind(Include="name,locNumber,source,description")] Location tbllocation)
        {
            if (ModelState.IsValid)
            {
                var loc = db.Locations.FirstOrDefault(p => p.name == tbllocation.name);
                if (loc == null)
                {
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var currentUser = manager.FindById(User.Identity.GetUserId());

                    if (currentUser != null)
                        tbllocation.createdBy = currentUser.UserName;
                    else
                        tbllocation.createdBy = User.Identity.Name;

                    tbllocation.createdDate = DateTime.Now;

                    db.Locations.Add(tbllocation);
                    db.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Location name already registered: " + tbllocation.name);
                    return View(tbllocation);
                }
                return RedirectToAction("Index");
            }
            else {
                var errors = ModelState.SelectMany(x => x.Value.Errors.Select(z => z.Exception));
            }

            return View(tbllocation);
        }

        // GET: /Location/Edit/5
        [Authorize(Roles = "Admin, CanEditLocation, Location")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location tbllocation = db.Locations.Find(id);
            if (tbllocation == null)
            {
                return HttpNotFound();
            }
            return View(tbllocation);
        }

        // POST: /Location/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanEditLocation, Location")]
        public ActionResult Edit([Bind(Include="locationId,name,locNumber,source,description,Timestamp")] Location tbllocation)
        {
           if (ModelState.IsValid)
            {
                try
                {
                    //db.Entry(tbllocation).State = EntityState.Modified;
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var currentUser = manager.FindById(User.Identity.GetUserId());

                    var loc = db.Locations.Where(c => c.locationId == tbllocation.locationId).FirstOrDefault();
                    loc.name = tbllocation.name;
                    loc.source = tbllocation.source;
                    loc.locNumber = tbllocation.locNumber;
                    loc.description = tbllocation.description;

                    if (currentUser != null)
                        loc.updatedBy = currentUser.UserName;
                    else
                        loc.updatedBy = User.Identity.Name;

                    loc.updatedDate = DateTime.Now;

                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var databaseValues = (Location)entry.GetDatabaseValues().ToObject();
                    var clientValues = (Location)entry.Entity;
                    if (databaseValues.name != clientValues.name)
                        ModelState.AddModelError("Name", "Current value: " + databaseValues.name);
                    if (databaseValues.locNumber != clientValues.locNumber)
                        ModelState.AddModelError("LocNo", "Current value: " + databaseValues.locNumber);

                    ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                      + "was modified by another user after you got the original value. The "
                      + "edit operation was canceled and the current values in the database "
                      + "have been displayed. If you still want to edit this record, click "
                      + "the Save button again. Otherwise click the Back to List hyperlink.");

                    tbllocation.Timestamp = databaseValues.Timestamp;
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View(tbllocation);
        }

        // GET: /Location/Delete/5
        [Authorize(Roles = "Admin, CanDeleteLocation, Location")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location tbllocation = db.Locations.Find(id);
            if (tbllocation == null)
            {
                return HttpNotFound();
            }
            return View(tbllocation);
        }

        // POST: /Location/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanDeleteLocation, Location")]
        public ActionResult DeleteConfirmed(long id)
        {
            Location tbllocation = db.Locations.Find(id);
            db.Locations.Remove(tbllocation);
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
