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
    public class RanksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Ranks
        public ActionResult Index()
        {
            return View(db.Ranks.ToList());
        }

        // GET: Ranks/Details/5
        [Authorize(Roles = "Admin, CanViewRanks, Ranks")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ranks ranks = db.Ranks.Find(id);
            if (ranks == null)
            {
                return HttpNotFound();
            }
            return View(ranks);
        }

        // GET: Ranks/Create
        [Authorize(Roles = "Admin, CanAddRanks, Ranks")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ranks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanAddRanks, Ranks")]
        public ActionResult Create([Bind(Include = "name")] Ranks ranks)
        {
            if (ModelState.IsValid)
            {
                var r = db.Ranks.FirstOrDefault(p => p.name == ranks.name);
                if (r == null)
                {
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var currentUser = manager.FindById(User.Identity.GetUserId());

                    if (currentUser != null)
                        ranks.createdBy = currentUser.UserName;
                    else
                        ranks.createdBy = User.Identity.Name;

                    ranks.createdDate = DateTime.Now;

                    db.Ranks.Add(ranks);
                    db.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Rank already registered: " + ranks.name);
                    return View(ranks);
                }
                return RedirectToAction("Index");
            }

            return View(ranks);
        }

        // GET: Ranks/Edit/5
        [Authorize(Roles = "Admin, CanEditRanks, Ranks")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ranks ranks = db.Ranks.Find(id);
            if (ranks == null)
            {
                return HttpNotFound();
            }
            return View(ranks);
        }

        // POST: Ranks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanEditRanks, Ranks")]
        public ActionResult Edit([Bind(Include = "rankId,name")] Ranks ranks)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var currentUser = manager.FindById(User.Identity.GetUserId());

                    var r = db.Ranks.Where(c => c.rankId == ranks.rankId).FirstOrDefault();
                    r.name = ranks.name;

                    if (currentUser != null)
                        r.updatedBy = currentUser.UserName;
                    else
                        r.updatedBy = User.Identity.Name;

                    r.updatedDate = DateTime.Now;

                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var databaseValues = (Ranks)entry.GetDatabaseValues().ToObject();
                    var clientValues = (Ranks)entry.Entity;
                    if (databaseValues.name != clientValues.name)
                        ModelState.AddModelError("Name", "Current value: " + databaseValues.name);

                    ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                      + "was modified by another user after you got the original value. The "
                      + "edit operation was canceled and the current values in the database "
                      + "have been displayed. If you still want to edit this record, click "
                      + "the Save button again. Otherwise click the Back to List hyperlink.");

                    ranks.Timestamp = databaseValues.Timestamp;
                    return View();
                }
                return RedirectToAction("Index");
            }

            return View(ranks);
        }

        // GET: Ranks/Delete/5
        [Authorize(Roles = "Admin, CanDeleteRanks, Ranks")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ranks ranks = db.Ranks.Find(id);
            if (ranks == null)
            {
                return HttpNotFound();
            }
            return View(ranks);
        }

        // POST: Ranks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanDeleteRanks, Ranks")]
        public ActionResult DeleteConfirmed(int id)
        {
            Ranks ranks = db.Ranks.Find(id);
            db.Ranks.Remove(ranks);
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
