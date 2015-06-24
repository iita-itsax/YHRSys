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
    public class MediumPrepTypeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /MediumPrepType/
        [Authorize(Roles = "Admin, CanViewMediumPrepType, MediumPrepTypes")]
        public ActionResult Index()
        {
            return View(db.MediumPrepTypes.ToList());
        }

        // GET: /MediumPrepType/Details/5
        [Authorize(Roles = "Admin, CanViewMediumPrepType, MediumPrepTypes")]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MediumPrepType tblmediumpreptype = db.MediumPrepTypes.Find(id);
            if (tblmediumpreptype == null)
            {
                return HttpNotFound();
            }
            return View(tblmediumpreptype);
        }

        // GET: /MediumPrepType/Create
        [Authorize(Roles = "Admin, CanAddMediumPrepType, MediumPrepTypes")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /MediumPrepType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanAddMediumPrepType, MediumPrepTypes")]
        public ActionResult Create([Bind(Include="typename")] MediumPrepType tblmediumpreptype)
        {
            if (ModelState.IsValid)
            {
                var med = db.MediumPrepTypes.FirstOrDefault(p => p.typename == tblmediumpreptype.typename);
                if (med == null)
                {
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var currentUser = manager.FindById(User.Identity.GetUserId());

                    if (currentUser != null)
                        tblmediumpreptype.createdBy = currentUser.UserName;
                    else
                        tblmediumpreptype.createdBy = User.Identity.Name;

                    tblmediumpreptype.createdDate = DateTime.Now;

                    db.MediumPrepTypes.Add(tblmediumpreptype);
                    db.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Medium Prep. Type already registered: " + tblmediumpreptype.typename);
                    return View(tblmediumpreptype);
                }
                return RedirectToAction("Index");
            }

            return View(tblmediumpreptype);
        }

        // GET: /MediumPrepType/Edit/5
        [Authorize(Roles = "Admin, CanEditMediumPrepType, MediumPrepTypes")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MediumPrepType tblmediumpreptype = db.MediumPrepTypes.Find(id);
            if (tblmediumpreptype == null)
            {
                return HttpNotFound();
            }
            return View(tblmediumpreptype);
        }

        // POST: /MediumPrepType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanEditMediumPrepType, MediumPrepTypes")]
        public ActionResult Edit([Bind(Include="typeId,typename")] MediumPrepType tblmediumpreptype)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //db.Entry(tbllocation).State = EntityState.Modified;
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var currentUser = manager.FindById(User.Identity.GetUserId());

                    var med = db.MediumPrepTypes.Where(c => c.typeId == tblmediumpreptype.typeId).FirstOrDefault();
                    med.typename = tblmediumpreptype.typename;

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
                    var databaseValues = (MediumPrepType)entry.GetDatabaseValues().ToObject();
                    var clientValues = (MediumPrepType)entry.Entity;
                    if (databaseValues.typename != clientValues.typename)
                        ModelState.AddModelError("Medium Prep. Type", "Current value: " + databaseValues.typename);

                    ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                      + "was modified by another user after you got the original value. The "
                      + "edit operation was canceled and the current values in the database "
                      + "have been displayed. If you still want to edit this record, click "
                      + "the Save button again. Otherwise click the Back to List hyperlink.");

                    tblmediumpreptype.Timestamp = databaseValues.Timestamp;
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View(tblmediumpreptype);
        }

        // GET: /MediumPrepType/Delete/5
        [Authorize(Roles = "Admin, CanDeleteMediumPrepType, MediumPrepTypes")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MediumPrepType tblmediumpreptype = db.MediumPrepTypes.Find(id);
            if (tblmediumpreptype == null)
            {
                return HttpNotFound();
            }
            return View(tblmediumpreptype);
        }

        // POST: /MediumPrepType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanDeleteMediumPrepType, MediumPrepTypes")]
        public ActionResult DeleteConfirmed(long id)
        {
            MediumPrepType tblmediumpreptype = db.MediumPrepTypes.Find(id);
            db.MediumPrepTypes.Remove(tblmediumpreptype);
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
