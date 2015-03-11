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
    public class VarietyController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Variety/
        public ActionResult Index()
        {
            var tblvarieties = db.Varieties;//.Include(t => t.tblActivity);
            return View(tblvarieties.ToList());
        }

        // GET: /Variety/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Variety tblvariety = db.Varieties.Find(id);
            if (tblvariety == null)
            {
                return HttpNotFound();
            }
            return View(tblvariety);
        }

        // GET: /Variety/Create
        public ActionResult Create()
        {
            ViewBag.activityId = new SelectList(db.Activities, "activityId", "name");
            return View();
        }

        // POST: /Variety/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="varietyId,name,sampleNumber,repUser,testDate,releaseStatus,resistance,storability,poundability,species,uniformity,stability,distinctness,value,activityId,locationId,userId,createdBy,createdDate,updatedBy,updatedDate")] Variety tblvariety)
        {
            if (ModelState.IsValid)
            {
                db.Varieties.Add(tblvariety);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.activityId = new SelectList(db.Activities, "activityId", "name", tblvariety.activityId);
            return View(tblvariety);
        }

        // GET: /Variety/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Variety tblvariety = db.Varieties.Find(id);
            if (tblvariety == null)
            {
                return HttpNotFound();
            }
            ViewBag.activityId = new SelectList(db.Activities, "activityId", "name", tblvariety.activityId);
            return View(tblvariety);
        }

        // POST: /Variety/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="varietyId,name,sampleNumber,repUser,testDate,releaseStatus,resistance,storability,poundability,species,uniformity,stability,distinctness,value,activityId,locationId,userId,createdBy,createdDate,updatedBy,updatedDate")] Variety tblvariety)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblvariety).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.activityId = new SelectList(db.Activities, "activityId", "name", tblvariety.activityId);
            return View(tblvariety);
        }

        // GET: /Variety/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Variety tblvariety = db.Varieties.Find(id);
            if (tblvariety == null)
            {
                return HttpNotFound();
            }
            return View(tblvariety);
        }

        // POST: /Variety/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Variety tblvariety = db.Varieties.Find(id);
            db.Varieties.Remove(tblvariety);
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
