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
using System.Web.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Infrastructure;
using System.Security.Claims;
using PagedList;

namespace YHRSys.Controllers
{
    [Authorize]
    public class ReagentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //PrincipalUser pUser = new PrincipalUser();
        
        // GET: /Reagent/
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var reagents = from r in db.Reagents join s in db.Stocks on r.reagentId equals s.reagentId into reagentInfo from reagentStock in reagentInfo.DefaultIfEmpty() 
                           select new ReagentStockModel
                           {
                               reagentId = r.reagentId,
                               reagentName = r.name,
                               measurementName = r.measurements.name,
                               stockBalance = (reagentStock.totalIn == null ? 0 : reagentStock.totalIn),
                               reOrderLevel = (r.reOrderLevel == null ? 0 : r.reOrderLevel),
                               description = r.description,
                               createdBy = r.createdBy,
                               createdDate = r.createdDate,
                               updatedBy = r.updatedBy,
                               updatedDate = r.updatedDate
                           };
            if (!String.IsNullOrEmpty(searchString))
            {
                reagents = reagents.Where(rg => rg.reagentName.Contains(searchString)
                                       || rg.description.Contains(searchString) || rg.measurementName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    reagents = reagents.OrderByDescending(rg => rg.reagentName);
                    break;
                case "Date":
                    reagents = reagents.OrderBy(rg => rg.createdDate);
                    break;
                case "date_desc":
                    reagents = reagents.OrderByDescending(rg => rg.createdDate);
                    break;
                default:  // Name ascending 
                    reagents = reagents.OrderBy(rg => rg.reagentName);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(reagents.ToPagedList(pageNumber, pageSize));
        }

        // GET: /Reagent/Details/5
        [Authorize(Roles = "Admin, CanViewReagent, Reagent")]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reagent tblreagent = db.Reagents.Find(id);
            if (tblreagent == null)
            {
                return HttpNotFound();
            }
            return View(tblreagent);
        }

        // GET: /Reagent/Create
        [Authorize(Roles = "Admin, CanAddReagent, Reagent")]
        public ActionResult Create()
        {
            ViewBag.measurementId = new SelectList(db.Measurements, "measurementId", "name");
            return View();
        }

        // POST: /Reagent/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanAddReagent, Reagent")]
        public ActionResult Create([Bind(Include = "name,measurementId,description,reOrderLevel")] Reagent tblreagent)
        {
            if (ModelState.IsValid)
            {
                var r = db.Reagents.FirstOrDefault(p => p.name == tblreagent.name);
                if (r == null)
                {
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var currentUser = manager.FindById(User.Identity.GetUserId());

                    if (currentUser != null)
                        tblreagent.createdBy = currentUser.UserName;
                    else
                        tblreagent.createdBy = User.Identity.Name;

                    tblreagent.createdDate = DateTime.Now;

                    db.Reagents.Add(tblreagent);
                    db.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Reagent already registered: " + tblreagent.name);
                    ViewBag.measurementId = new SelectList(db.Measurements, "measurementId", "name");
                    return View(tblreagent);
                }
                return RedirectToAction("Index");
            }
            ViewBag.measurementId = new SelectList(db.Measurements, "measurementId", "name");
            return View(tblreagent);
        }

        // GET: /Reagent/Edit/5
        [Authorize(Roles = "Admin, CanEditReagent, Reagent")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reagent tblreagent = db.Reagents.Find(id);
            if (tblreagent == null)
            {
                return HttpNotFound();
            }
            ViewBag.measurementId = new SelectList(db.Measurements, "measurementId", "name", "uom");
            return View(tblreagent);
        }

        // POST: /Reagent/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanEditReagent, Reagent")]
        public ActionResult Edit([Bind(Include = "reagentId,name,measurementId,description,reOrderLevel")] Reagent tblreagent)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());

            if (ModelState.IsValid)
            {
                try
                {
                    //db.Entry(tbllocationuser).State = EntityState.Modified;
                    var r = db.Reagents.Where(c => c.reagentId == tblreagent.reagentId).FirstOrDefault();
                    r.measurementId = tblreagent.measurementId;
                    r.name = tblreagent.name;
                    r.description = tblreagent.description;
                    r.reOrderLevel = tblreagent.reOrderLevel;
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
                    var databaseValues = (Reagent)entry.GetDatabaseValues().ToObject();
                    var clientValues = (Reagent)entry.Entity;
                    if (databaseValues.name != clientValues.name)
                        ModelState.AddModelError("Reagent", "Current value: " + databaseValues.name);
                    if (databaseValues.measurements.name != clientValues.measurements.name)
                        ModelState.AddModelError("UoM", "Current value: " + databaseValues.measurements.name);

                    ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                      + "was modified by another user after you got the original value. The "
                      + "edit operation was canceled and the current values in the database "
                      + "have been displayed. If you still want to edit this record, click "
                      + "the Save button again. Otherwise click the Back to List hyperlink.");

                    tblreagent.Timestamp = databaseValues.Timestamp;
                    ViewBag.measurementId = new SelectList(db.Measurements, "measurementId", "name");
                    return View();
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                    ViewBag.measurementId = new SelectList(db.Measurements, "measurementId", "name");
                    return View();
                }
                return RedirectToAction("Index");
            }
            ViewBag.measurementId = new SelectList(db.Measurements, "measurementId", "name");
            return View(tblreagent);
        }

        // GET: /Reagent/Delete/5
        [Authorize(Roles = "Admin, CanDeleteReagent, Reagent")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reagent tblreagent = db.Reagents.Find(id);
            if (tblreagent == null)
            {
                return HttpNotFound();
            }
            return View(tblreagent);
        }

        // POST: /Reagent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanDeleteReagent, Reagent")]
        public ActionResult DeleteConfirmed(long id)
        {
            Reagent tblreagent = db.Reagents.Find(id);
            Stock tblstock = db.Stocks.Find(tblreagent.reagentId);
            
            db.Stocks.Remove(tblstock);

            db.Reagents.Remove(tblreagent);

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
