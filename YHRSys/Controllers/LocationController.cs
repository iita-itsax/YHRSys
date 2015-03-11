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

namespace YHRSys.Controllers
{
    public class LocationController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Location/
        public ActionResult Index()
        {
            return View(db.Locations.ToList());
        }

        // GET: /Location/Details/5
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Location/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="locationId,name,locNumber,source,description,createdBy,createdDate,updatedBy,updatedDate")] Location tbllocation)
        {
            if (ModelState.IsValid)
            {
                tbllocation.baseDateEntity.createdDate = DateTime.Now;
                tbllocation.baseUserEntity.createdBy = User.Identity.GetUserName();
                db.Locations.Add(tbllocation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbllocation);
        }

        // GET: /Location/Edit/5
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
        public ActionResult Edit([Bind(Include="locationId,name,locNumber,source,description,createdBy,createdDate,updatedBy,updatedDate")] Location tbllocation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbllocation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbllocation);
        }

        // GET: /Location/Delete/5
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
