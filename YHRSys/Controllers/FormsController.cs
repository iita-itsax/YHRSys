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
    public class FormsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Forms
        public ActionResult Index()
        {
            return View(db.Forms.ToList());
        }

        // GET: Forms/Details/5
        [Authorize(Roles = "Admin, CanViewForms, Forms")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Forms forms = db.Forms.Find(id);
            if (forms == null)
            {
                return HttpNotFound();
            }
            return View(forms);
        }

        // GET: Forms/Create
        [Authorize(Roles = "Admin, CanAddForms, Forms")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Forms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanAddForms, Forms")]
        public ActionResult Create([Bind(Include = "name")] Forms forms)
        {
            if (ModelState.IsValid)
            {
                var f = db.Forms.FirstOrDefault(p => p.name == forms.name);
                if (f == null)
                {
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var currentUser = manager.FindById(User.Identity.GetUserId());

                    if (currentUser != null)
                        forms.createdBy = currentUser.UserName;
                    else
                        forms.createdBy = User.Identity.Name;

                    forms.createdDate = DateTime.Now;

                    db.Forms.Add(forms);
                    db.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Form already registered: " + forms.name);
                    return View(forms);
                }
                return RedirectToAction("Index");
            }

            return View(forms);
        }

        // GET: Forms/Edit/5
        [Authorize(Roles = "Admin, CanEditForms, Forms")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Forms forms = db.Forms.Find(id);
            if (forms == null)
            {
                return HttpNotFound();
            }
            return View(forms);
        }

        // POST: Forms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanEditForms, Forms")]
        public ActionResult Edit([Bind(Include = "formId,name")] Forms forms)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var currentUser = manager.FindById(User.Identity.GetUserId());

                    var f = db.Forms.Where(c => c.formId == forms.formId).FirstOrDefault();
                    f.name = forms.name;

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

                    forms.Timestamp = databaseValues.Timestamp;
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View(forms);
        }

        // GET: Forms/Delete/5
        [Authorize(Roles = "Admin, CanDeleteForms, Forms")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Forms forms = db.Forms.Find(id);
            if (forms == null)
            {
                return HttpNotFound();
            }
            return View(forms);
        }

        // POST: Forms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanDeleteForms, Forms")]
        public ActionResult DeleteConfirmed(int id)
        {
            Forms forms = db.Forms.Find(id);
            db.Forms.Remove(forms);
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
