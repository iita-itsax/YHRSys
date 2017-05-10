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
    public class SeedlingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Seedlings
        public ActionResult Index()
        {
            return View(db.Seedlings.ToList());
        }

        // GET: Seedlings/Details/5
        [Authorize(Roles = "Admin, CanViewSeedling, Seedling")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seedling seedling = db.Seedlings.Find(id);
            if (seedling == null)
            {
                return HttpNotFound();
            }
            return View(seedling);
        }

        // GET: Seedlings/Create
        [Authorize(Roles = "Admin, CanAddSeedling, Seedling")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Seedlings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanAddSeedling, Seedling")]
        public ActionResult Create([Bind(Include = "name")] Seedling seedling)
        {
            if (ModelState.IsValid)
            {
                var f = db.Seedlings.FirstOrDefault(p => p.name == seedling.name);
                if (f == null)
                {
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var currentUser = manager.FindById(User.Identity.GetUserId());

                    if (currentUser != null)
                        seedling.createdBy = currentUser.UserName;
                    else
                        seedling.createdBy = User.Identity.Name;

                    seedling.createdDate = DateTime.Now;

                    db.Seedlings.Add(seedling);
                    db.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Seedling already registered: " + seedling.name);
                    return View(seedling);
                }
                return RedirectToAction("Index");
            }

            return View(seedling);
        }

        // GET: Seedlings/Edit/5
        [Authorize(Roles = "Admin, CanEditSeedling, Seedling")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seedling seedling = db.Seedlings.Find(id);
            if (seedling == null)
            {
                return HttpNotFound();
            }
            return View(seedling);
        }

        // POST: Seedlings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanEditSeedling, Seedling")]
        public ActionResult Edit([Bind(Include = "seedlingId,name")] Seedling seedling)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var currentUser = manager.FindById(User.Identity.GetUserId());

                    var f = db.Seedlings.Where(c => c.seedlingId == seedling.seedlingId).FirstOrDefault();
                    f.name = seedling.name;

                    if (currentUser != null)
                        f.updatedBy = currentUser.UserName;
                    else
                        f.updatedBy = User.Identity.Name;

                    f.updatedDate = DateTime.Now;

                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var databaseValues = (Forms)entry.GetDatabaseValues().ToObject();
                    var clientValues = (Forms)entry.Entity;
                    if (databaseValues.name != clientValues.name)
                        ModelState.AddModelError("Name", "Current value: " + databaseValues.name);

                    ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                      + "was modified by another user after you got the original value. The "
                      + "edit operation was canceled and the current values in the database "
                      + "have been displayed. If you still want to edit this record, click "
                      + "the Save button again. Otherwise click the Back to List hyperlink.");

                    seedling.Timestamp = databaseValues.Timestamp;
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View(seedling);
        }

        // GET: Seedlings/Delete/5
        [Authorize(Roles = "Admin, CanDeleteSeedling, Seedling")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seedling seedling = db.Seedlings.Find(id);
            if (seedling == null)
            {
                return HttpNotFound();
            }
            return View(seedling);
        }

        // POST: Seedlings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanDeleteSeedling, Seedling")]
        public ActionResult DeleteConfirmed(int id)
        {
            Seedling seedling = db.Seedlings.Find(id);
            db.Seedlings.Remove(seedling);
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
