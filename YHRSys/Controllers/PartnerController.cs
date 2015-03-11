using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YHRSys.Models;

namespace YHRSys.Controllers
{
    public class PartnerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Partner/
        public ActionResult Index()
        {
            return View(db.Partners.ToList());
        }

        // GET: /Partner/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partner tblpartner = db.Partners.Find(id);
            if (tblpartner == null)
            {
                return HttpNotFound();
            }
            return View(tblpartner);
        }

        // GET: /Partner/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Partner/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="partnerId,name,contactAddress,phoneNumber,emailAddress,webAddress,geoLongitude,geoLatitude,createdBy,createdDate,updatedBy,updatedDate")] Partner tblpartner)
        {
            if (ModelState.IsValid)
            {
                db.Partners.Add(tblpartner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblpartner);
        }

        // GET: /Partner/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partner tblpartner = db.Partners.Find(id);
            if (tblpartner == null)
            {
                return HttpNotFound();
            }
            return View(tblpartner);
        }

        // POST: /Partner/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="partnerId,name,contactAddress,phoneNumber,emailAddress,webAddress,geoLongitude,geoLatitude,createdBy,createdDate,updatedBy,updatedDate")] Partner tblpartner)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblpartner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblpartner);
        }

        // GET: /Partner/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partner tblpartner = db.Partners.Find(id);
            if (tblpartner == null)
            {
                return HttpNotFound();
            }
            return View(tblpartner);
        }

        // POST: /Partner/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Partner tblpartner = db.Partners.Find(id);
            db.Partners.Remove(tblpartner);
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
