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
    public class PartnerActivityController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /PartnerActivity/
        public ActionResult Index()
        {
            var tblpartneractivities = db.PartnerActivities;
            return View(tblpartneractivities.ToList());
        }

        // GET: /PartnerActivity/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartnerActivity tblpartneractivity = db.PartnerActivities.Find(id);
            if (tblpartneractivity == null)
            {
                return HttpNotFound();
            }
            return View(tblpartneractivity);
        }

        // GET: /PartnerActivity/Create
        public ActionResult Create()
        {
            ViewBag.reagentId = new SelectList(db.Reagents, "reagentId", "name");
            ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name");
            return View();
        }

        // POST: /PartnerActivity/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="partnerActivityId,partnerId,reagentId,backStopping,tcPlantletsGiven,bioreactorplantsGiven,tubersGiven,tcPlantletsAvailable,tibPlantletsAvailable,tubersAvailable,officerInCharge,activityDate,createdBy,createdDate,updatedBy,updatedDate")] PartnerActivity tblpartneractivity)
        {
            if (ModelState.IsValid)
            {
                db.PartnerActivities.Add(tblpartneractivity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.reagentId = new SelectList(db.Reagents, "reagentId", "name", tblpartneractivity.reagentId);
            ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.partnerId);
            return View(tblpartneractivity);
        }

        // GET: /PartnerActivity/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartnerActivity tblpartneractivity = db.PartnerActivities.Find(id);
            if (tblpartneractivity == null)
            {
                return HttpNotFound();
            }
            ViewBag.reagentId = new SelectList(db.Reagents, "reagentId", "name", tblpartneractivity.reagentId);
            ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.partnerId);
            return View(tblpartneractivity);
        }

        // POST: /PartnerActivity/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="partnerActivityId,partnerId,reagentId,backStopping,tcPlantletsGiven,bioreactorplantsGiven,tubersGiven,tcPlantletsAvailable,tibPlantletsAvailable,tubersAvailable,officerInCharge,activityDate,createdBy,createdDate,updatedBy,updatedDate")] PartnerActivity tblpartneractivity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblpartneractivity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.reagentId = new SelectList(db.Reagents, "reagentId", "name", tblpartneractivity.reagentId);
            ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartneractivity.partnerId);
            return View(tblpartneractivity);
        }

        // GET: /PartnerActivity/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartnerActivity tblpartneractivity = db.PartnerActivities.Find(id);
            if (tblpartneractivity == null)
            {
                return HttpNotFound();
            }
            return View(tblpartneractivity);
        }

        // POST: /PartnerActivity/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            PartnerActivity tblpartneractivity = db.PartnerActivities.Find(id);
            db.PartnerActivities.Remove(tblpartneractivity);
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
