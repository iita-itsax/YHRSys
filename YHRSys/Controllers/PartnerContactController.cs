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
    public class PartnerContactController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /PartnerContact/
        public ActionResult Index()
        {
            var tblpartnercontacts = db.PartnerContactPersons;
            return View(tblpartnercontacts.ToList());
        }

        // GET: /PartnerContact/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartnerContactPerson tblpartnercontact = db.PartnerContactPersons.Find(id);
            if (tblpartnercontact == null)
            {
                return HttpNotFound();
            }
            return View(tblpartnercontact);
        }

        // GET: /PartnerContact/Create
        public ActionResult Create()
        {
            ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name");
            return View();
        }

        // POST: /PartnerContact/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "contactId,partnerId,firstName,otherNames,lastName,phoneNumber,emailAddress,contactAddress,webAddress,geoLongitude,geoLatitude,createdBy,createdDate,updatedBy,updatedDate")] PartnerContactPerson tblpartnercontact)
        {
            if (ModelState.IsValid)
            {
                db.PartnerContactPersons.Add(tblpartnercontact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartnercontact.partnerId);
            return View(tblpartnercontact);
        }

        // GET: /PartnerContact/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartnerContactPerson tblpartnercontact = db.PartnerContactPersons.Find(id);
            if (tblpartnercontact == null)
            {
                return HttpNotFound();
            }
            ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartnercontact.partnerId);
            return View(tblpartnercontact);
        }

        // POST: /PartnerContact/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "contactId,partnerId,firstName,otherNames,lastName,phoneNumber,emailAddress,contactAddress,webAddress,geoLongitude,geoLatitude,createdBy,createdDate,updatedBy,updatedDate")] PartnerContactPerson tblpartnercontact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblpartnercontact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartnercontact.partnerId);
            return View(tblpartnercontact);
        }

        // GET: /PartnerContact/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartnerContactPerson tblpartnercontact = db.PartnerContactPersons.Find(id);
            if (tblpartnercontact == null)
            {
                return HttpNotFound();
            }
            return View(tblpartnercontact);
        }

        // POST: /PartnerContact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            PartnerContactPerson tblpartnercontact = db.PartnerContactPersons.Find(id);
            db.PartnerContactPersons.Remove(tblpartnercontact);
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
