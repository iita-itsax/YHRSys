using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YHRSys.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace YHRSys.Controllers
{
    public class ActivityController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        //
        // GET: /Activity/
        public ActionResult Index()
        {
            var activities = db.Activities.ToList();
            return View(activities);
        }

        //
        // GET: /Activity/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        //
        // GET: /Activity/Create
        public ActionResult Create()
        {
            ViewBag.locationId = new SelectList(db.Locations, "locationId", "name");
            ViewBag.typeId = new SelectList(db.MediumPrepTypes, "typeId", "typename");
            return View();
        }

        //
        // POST: /Activity/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection collection)
        {
            Activity activity = new Activity();
            try
            {
                if (ModelState.IsValid)
                {
                    activity.baseDateEntity.createdDate = new DateTime();
                    activity.name = collection["name"];
                    activity.baseUserEntity.createdBy = User.Identity.GetUserId();
                    activity.locationId = Convert.ToInt32(collection["locationId"]);
                    activity.typeId = Convert.ToInt32(collection["typeId"]);
                    activity.description = collection["description"];

                    db.Activities.Add(activity);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.locationId = new SelectList(db.Locations, "locationId", "name", activity.locationId);
                ViewBag.typeId = new SelectList(db.MediumPrepTypes, "typeId", "typename", activity.typeId);

                return View(activity);
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Activity/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            ViewBag.locationId = new SelectList(db.Locations, "locationId", "name", activity.locationId);
            ViewBag.typeId = new SelectList(db.MediumPrepTypes, "typeId", "typename", activity.typeId);
            return View(activity);
        }

        //
        // POST: /Activity/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(long? id, FormCollection collection, Activity activity)
        {
            try
            {
                //activityId,name,locationId,typeId,description,updatedBy,updatedDate
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    db.Entry(activity).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.locationId = new SelectList(db.Locations, "locationId", "name", activity.locationId);
                ViewBag.typeId = new SelectList(db.MediumPrepTypes, "typeId", "typename", activity.typeId);
                return View(activity);
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Activity/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        //
        // POST: /Activity/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long? id, FormCollection collection)
        {
            try
            {
                Activity activity = db.Activities.Find(id);
                db.Activities.Remove(activity);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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
