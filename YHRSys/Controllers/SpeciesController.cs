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
    public class SpeciesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /MediumPrepType/
        [Authorize(Roles = "Admin, CanViewSpecies, Species")]
        public ActionResult Index()
        {
            return View(db.Species.ToList());
        }

        // GET: /MediumPrepType/Details/5
        [Authorize(Roles = "Admin, CanViewSpecies, Species")]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Species tblspecies = db.Species.Find(id);
            if (tblspecies == null)
            {
                return HttpNotFound();
            }
            return View(tblspecies);
        }

        // GET: /MediumPrepType/Create
        [Authorize(Roles = "Admin, CanAddSpecies, Species")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /MediumPrepType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanAddSpecies, Species")]
        public ActionResult Create([Bind(Include = "name")] Species tblspecies)
        {
            if (ModelState.IsValid)
            {
                var med = db.Species.FirstOrDefault(p => p.name == tblspecies.name);
                if (med == null)
                {
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var currentUser = manager.FindById(User.Identity.GetUserId());

                    if (currentUser != null)
                        tblspecies.createdBy = currentUser.UserName;
                    else
                        tblspecies.createdBy = User.Identity.Name;

                    tblspecies.createdDate = DateTime.Now;

                    db.Species.Add(tblspecies);
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.Message = "Specie already registered: " + tblspecies.name;
                    return View(tblspecies);
                }
                return RedirectToAction("Index");
            }

            return View(tblspecies);
        }

        // GET: /MediumPrepType/Edit/5
        [Authorize(Roles = "Admin, CanEditSpecies, Species")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Species tblspecies = db.Species.Find(id);
            if (tblspecies == null)
            {
                return HttpNotFound();
            }
            return View(tblspecies);
        }

        // POST: /MediumPrepType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanEditSpecies, Species")]
        public ActionResult Edit([Bind(Include = "specieId,name")] Species tblspecies)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //db.Entry(tbllocation).State = EntityState.Modified;
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var currentUser = manager.FindById(User.Identity.GetUserId());

                    var med = db.Species.Where(c => c.specieId == tblspecies.specieId).FirstOrDefault();
                    med.name = tblspecies.name;

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
                    var databaseValues = (Species)entry.GetDatabaseValues().ToObject();
                    var clientValues = (Species)entry.Entity;
                    if (databaseValues.name != clientValues.name)
                        ModelState.AddModelError("Specie", "Current value: " + databaseValues.name);

                    ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                      + "was modified by another user after you got the original value. The "
                      + "edit operation was canceled and the current values in the database "
                      + "have been displayed. If you still want to edit this record, click "
                      + "the Save button again. Otherwise click the Back to List hyperlink.");

                    tblspecies.Timestamp = databaseValues.Timestamp;
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View(tblspecies);
        }

        // GET: /MediumPrepType/Delete/5
        [Authorize(Roles = "Admin, CanDeleteSpecies, Species")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Species tblspecies = db.Species.Find(id);
            if (tblspecies == null)
            {
                return HttpNotFound();
            }
            return View(tblspecies);
        }

        // POST: /MediumPrepType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanDeleteSpecies, Species")]
        public ActionResult DeleteConfirmed(long id)
        {
            Species tblspecies = db.Species.Find(id);
            db.Species.Remove(tblspecies);
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