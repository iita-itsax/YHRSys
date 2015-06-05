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
    public class LocationUserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /LocationUser/
        public ActionResult Index()
        {
            var tbllocationusers = db.LocationUsers;//.Include(t => t.tblLocation);
            ViewBag.Users = db.Users;
            return View(tbllocationusers.ToList());
        }

        // GET: /LocationUser/Details/5
        [Authorize(Roles = "Admin, CanViewLocationUser, LocationUser")]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LocationUser tbllocationuser = db.LocationUsers.Find(id);
            if (tbllocationuser == null)
            {
                return HttpNotFound();
            }
            ViewBag.userSubordinateId = new SelectList(db.Users, "Id", "FullName");
            return View(tbllocationuser);
        }

        // GET: /LocationUser/Create
        [Authorize(Roles = "Admin, CanAddLocationUser, LocationUser")]
        public ActionResult Create()
        {
            ViewBag.locationId = new SelectList(db.Locations, "locationId", "name");
            ViewBag.userId = new SelectList(db.Users, "Id", "FullName");
            return View();
        }

        // POST: /LocationUser/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanAddLocationUser, LocationUser")]
        public ActionResult Create([Bind(Include="userId,locationId,startDate,endDate,status")] LocationUser tbllocationuser)
        {
            if (ModelState.IsValid)
            {
                var locuser = db.LocationUsers.FirstOrDefault(p => p.locationId == tbllocationuser.locationId && p.userId == tbllocationuser.userId);
                    if (locuser == null)
                    {
                        var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                        var currentUser = manager.FindById(User.Identity.GetUserId());

                        if (currentUser != null)
                            tbllocationuser.createdBy = currentUser.UserName;
                        else
                            tbllocationuser.createdBy = User.Identity.Name;

                        tbllocationuser.createdDate = DateTime.Now;

                        db.LocationUsers.Add(tbllocationuser);
                        db.SaveChanges();
                    }
                    else
                    {
                        var user = db.LocationUsers.SingleOrDefault(p => p.userId == tbllocationuser.userId);
                        ModelState.AddModelError(string.Empty, "Location user already registered for user: " + user.FullName + " and location: " + tbllocationuser.location.name);
                        ViewBag.locationId = new SelectList(db.Locations, "locationId", "name");
                        ViewBag.userId = new SelectList(db.Users, "Id", "FullName");
                        return View(tbllocationuser);
                    }
                
                return RedirectToAction("Index");
            }

            ViewBag.locationId = new SelectList(db.Locations, "locId", "name", tbllocationuser.locationId);
            ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tbllocationuser.userId);
            return View(tbllocationuser);
        }

        // GET: /LocationUser/Edit/5
        [Authorize(Roles = "Admin, CanEditLocationUser, LocationUser")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LocationUser tbllocationuser = db.LocationUsers.Find(id);
            if (tbllocationuser == null)
            {
                return HttpNotFound();
            }
            ViewBag.locationId = new SelectList(db.Locations, "locationId", "name", tbllocationuser.locationId);
            ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tbllocationuser.userId);
            return View(tbllocationuser);
        }

        // POST: /LocationUser/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanEditLocationUser, LocationUser")]
        public ActionResult Edit([Bind(Include="locationUserId,userId,locationId,startDate,endDate,status")] LocationUser tbllocationuser)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());

            if (ModelState.IsValid)
            {
                try
                {
                    //db.Entry(tbllocationuser).State = EntityState.Modified;
                    var locuser = db.LocationUsers.Where(c => c.locationUserId == tbllocationuser.locationUserId).FirstOrDefault();
                    locuser.userId = tbllocationuser.userId;
                    locuser.locationId = tbllocationuser.locationId;
                    locuser.startDate = tbllocationuser.startDate;
                    locuser.endDate = tbllocationuser.endDate;
                    locuser.status = tbllocationuser.status;
                    if (currentUser != null)
                        locuser.updatedBy = currentUser.UserName;
                    else
                        locuser.updatedBy = User.Identity.Name;

                    locuser.updatedDate = DateTime.Now;
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var databaseValues = (LocationUser)entry.GetDatabaseValues().ToObject();
                    var clientValues = (LocationUser)entry.Entity;
                    if (databaseValues.location.name != clientValues.location.name)
                        ModelState.AddModelError("Location", "Current value: " + databaseValues.location.name);
                    if (databaseValues.FullName != clientValues.FullName)
                        ModelState.AddModelError("Loc. OiC", "Current value: " + databaseValues.FullName);

                    ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                      + "was modified by another user after you got the original value. The "
                      + "edit operation was canceled and the current values in the database "
                      + "have been displayed. If you still want to edit this record, click "
                      + "the Save button again. Otherwise click the Back to List hyperlink.");

                    tbllocationuser.Timestamp = databaseValues.Timestamp;
                    ViewBag.locationId = new SelectList(db.Locations, "locationId", "name", tbllocationuser.locationId);
                    ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tbllocationuser.userId);
                    return View();
                }catch(Exception e){
                    ModelState.AddModelError(string.Empty, e.Message);
                    ViewBag.locationId = new SelectList(db.Locations, "locationId", "name", tbllocationuser.locationId);
                    ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tbllocationuser.userId);
                    return View();
                }
                return RedirectToAction("Index");
            }
            ViewBag.locationId = new SelectList(db.Locations, "locationId", "name", tbllocationuser.locationId);
            ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tbllocationuser.userId);
            return View(tbllocationuser);
        }

        // GET: /LocationUser/Delete/5
        [Authorize(Roles = "Admin, CanDeleteLocationUser, LocationUser")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LocationUser tbllocationuser = db.LocationUsers.Find(id);
            if (tbllocationuser == null)
            {
                return HttpNotFound();
            }
            return View(tbllocationuser);
        }

        // POST: /LocationUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanDeleteLocationUser, LocationUser")]
        public ActionResult DeleteConfirmed(long id)
        {
            LocationUser tbllocationuser = db.LocationUsers.Find(id);
            db.LocationUsers.Remove(tbllocationuser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: /LocationSubordinate/Delete/5
        [Authorize(Roles = "Admin, CanDeleteLocationUser, LocationUser")]
        public ActionResult DeleteSubordinate(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LocationSubordinate tbllocationsubordinate = db.LocationSubordinates.Find(id);
            if (tbllocationsubordinate == null)
            {
                return HttpNotFound();
            }
            return View(tbllocationsubordinate);
        }

        // POST: /LocationUser/Delete/5
        [HttpPost, ActionName("DeleteSubordinate")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanDeleteLocationUser, LocationUser")]
        public ActionResult DeleteSubordinateConfirmed(long id, long locationUserId)
        {
            LocationSubordinate tbllocationuser = db.LocationSubordinates.Find(id);
            db.LocationSubordinates.Remove(tbllocationuser);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = locationUserId });
        }

        // POST: /LocationUser/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanAddLocationUser, LocationUser")]
        public ActionResult AddSubordinate([Bind(Include = "locationUserId,userSubordinateId,status,workBrief")] LocationSubordinate tbllocationsubordinate)
        {
            if (ModelState.IsValid)
            {
                var locsubordinate = db.LocationSubordinates.FirstOrDefault(p => p.userSubordinateId == tbllocationsubordinate.userSubordinateId
                    && p.locationUserId == tbllocationsubordinate.locationUserId
                    && p.status == tbllocationsubordinate.status && p.workBrief == tbllocationsubordinate.workBrief);
                if (locsubordinate == null)
                {
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var currentUser = manager.FindById(User.Identity.GetUserId());

                   if (currentUser != null)
                        tbllocationsubordinate.createdBy = currentUser.UserName;
                    else
                        tbllocationsubordinate.createdBy = User.Identity.Name;

                    tbllocationsubordinate.createdDate = DateTime.Now;

                    db.LocationSubordinates.Add(tbllocationsubordinate);
                    db.SaveChanges();
                }
                else
                {
                    var user = db.LocationSubordinates.SingleOrDefault(p => p.userSubordinateId == tbllocationsubordinate.userSubordinateId);
                    ModelState.AddModelError(string.Empty, "Location user already registered for user: " + user.SubordinateFullName + " and location: " + tbllocationsubordinate.locationUser.location.name);
                    ViewBag.userSubordinateId = new SelectList(db.Users, "Id", "FullName");
                    //return View(tbllocationsubordinate);
                }

                return RedirectToAction("Details", new { id = tbllocationsubordinate.locationUserId });
            }

            ViewBag.userSubordinateId = new SelectList(db.Users, "Id", "FullName", tbllocationsubordinate.userSubordinateId);
            return RedirectToAction("Details", new { id = tbllocationsubordinate.locationUserId });
        }

        // GET: /LocationUser/Edit/5
        [Authorize(Roles = "Admin, CanEditLocationUser, LocationUser")]
        public ActionResult EditSubordinate(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LocationSubordinate tbllocationsubordinate = db.LocationSubordinates.Find(id);
            if (tbllocationsubordinate == null)
            {
                return HttpNotFound();
            }

            ViewBag.userSubordinateId = new SelectList(db.Users, "Id", "FullName", tbllocationsubordinate.userSubordinateId);
            return View(tbllocationsubordinate);
        }

        // POST: /LocationUser/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanEditLocationUser, LocationUser")]
        public ActionResult EditSubordinate([Bind(Include = "subordinateId,locationUserId,userSubordinateId,status,workBrief")] LocationSubordinate tbllocationsubordinate)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());

            if (ModelState.IsValid)
            {
                try
                {
                    //db.Entry(tbllocationuser).State = EntityState.Modified;
                    var locsubord = db.LocationSubordinates.Where(c => c.subordinateId == tbllocationsubordinate.subordinateId).FirstOrDefault();
                    locsubord.userSubordinateId = tbllocationsubordinate.userSubordinateId;
                    locsubord.status = tbllocationsubordinate.status;
                    locsubord.workBrief = tbllocationsubordinate.workBrief;
                    if (currentUser != null)
                        locsubord.updatedBy = currentUser.UserName;
                    else
                        locsubord.updatedBy = User.Identity.Name;

                    locsubord.updatedDate = DateTime.Now;
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var databaseValues = (LocationSubordinate)entry.GetDatabaseValues().ToObject();
                    var clientValues = (LocationSubordinate)entry.Entity;
                    if (databaseValues.locationUser.location.name != clientValues.locationUser.location.name)
                        ModelState.AddModelError("Location", "Current value: " + databaseValues.locationUser.location.name);
                    if (databaseValues.SubordinateFullName != clientValues.SubordinateFullName)
                        ModelState.AddModelError("Loc. Subordinate", "Current value: " + databaseValues.SubordinateFullName);

                    ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                      + "was modified by another user after you got the original value. The "
                      + "edit operation was canceled and the current values in the database "
                      + "have been displayed. If you still want to edit this record, click "
                      + "the Save button again. Otherwise click the Back to List hyperlink.");

                    tbllocationsubordinate.Timestamp = databaseValues.Timestamp;
                    ViewBag.userSubordinateId = new SelectList(db.Users, "Id", "FullName", tbllocationsubordinate.userSubordinateId);
                    return View();
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                    ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tbllocationsubordinate.userSubordinateId);
                    return View();
                }
                return RedirectToAction("Details", new { id = tbllocationsubordinate.locationUserId});
            }
            ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tbllocationsubordinate.userSubordinateId);
            return View(tbllocationsubordinate);
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
