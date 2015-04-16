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
using PagedList;

namespace YHRSys.Controllers
{
    [Authorize]
    public class InternalReagentUsageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private int diffInQty = 0;

        // GET: /InternalReagentUsage/
        public ActionResult Index(string sortOrder, string currentFilter, string currentStartDateFilter, string currentEndDateFilter, string searchString, DateTime? searchStartDispatchDate, DateTime? searchEndDispatchDate, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.OiCNameSortParm = sortOrder == "OiC" ? "oicname_desc" : "OiC";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.QuantitySortParm = sortOrder == "Quantity" ? "quantity_desc" : "Quantity";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (searchStartDispatchDate != null)
            {
                page = 1;
            }
            else
            {
                if (currentStartDateFilter != null)
                    searchStartDispatchDate = DateTime.Parse(currentStartDateFilter.ToString());
            }
            ViewBag.CurrentStartDateFilter = searchStartDispatchDate;

            if (searchEndDispatchDate != null)
            {
                page = 1;
            }
            else
            {
                if (currentEndDateFilter != null)
                    searchEndDispatchDate = DateTime.Parse(currentEndDateFilter.ToString());
            }
            ViewBag.CurrentEndDateFilter = searchEndDispatchDate;

            if (searchStartDispatchDate != null)
                searchStartDispatchDate = DateTime.Parse(searchStartDispatchDate.ToString());
            if (searchEndDispatchDate != null)
                searchEndDispatchDate = DateTime.Parse(searchEndDispatchDate.ToString());

            var internalReagentUsages = from r in db.InternalReagentUsages select r;

            if (!String.IsNullOrEmpty(searchString))
            {
                if (searchStartDispatchDate != null && searchStartDispatchDate != null)
                {
                    internalReagentUsages = internalReagentUsages.Where(rg => (rg.reagent.name.Contains(searchString)
                                       || rg.note.Contains(searchString) || rg.user.LastName.Contains(searchString)
                                          || rg.user.FirstName.Contains(searchString)) && (rg.dispatchDate >= (DateTime)searchStartDispatchDate && rg.dispatchDate <= (DateTime)searchEndDispatchDate));
                }
                else if (searchStartDispatchDate != null)
                {
                    internalReagentUsages = internalReagentUsages.Where(rg => (rg.reagent.name.Contains(searchString)
                                       || rg.note.Contains(searchString) || rg.user.LastName.Contains(searchString)
                                          || rg.user.FirstName.Contains(searchString)) && (rg.dispatchDate == (DateTime)searchStartDispatchDate));
                }
                else if (searchEndDispatchDate != null)
                {
                    internalReagentUsages = internalReagentUsages.Where(rg => (rg.reagent.name.Contains(searchString)
                                       || rg.note.Contains(searchString) || rg.user.LastName.Contains(searchString)
                                          || rg.user.FirstName.Contains(searchString)) && (rg.dispatchDate == (DateTime)searchEndDispatchDate));
                }
                else
                {
                    internalReagentUsages = internalReagentUsages.Where(rg => (rg.reagent.name.Contains(searchString)
                                       || rg.note.Contains(searchString) || rg.user.LastName.Contains(searchString)
                                          || rg.user.FirstName.Contains(searchString)));
                }
            }
            else
            {
                if (searchStartDispatchDate != null && searchEndDispatchDate != null)
                {
                    internalReagentUsages = internalReagentUsages.Where(rg => (rg.dispatchDate >= (DateTime)searchStartDispatchDate && rg.dispatchDate <= (DateTime)searchEndDispatchDate));
                }
                else if (searchStartDispatchDate != null)
                {
                    internalReagentUsages = internalReagentUsages.Where(rg => rg.dispatchDate == (DateTime)searchStartDispatchDate);
                }
                else if (searchEndDispatchDate != null)
                {
                    internalReagentUsages = internalReagentUsages.Where(rg => rg.dispatchDate == (DateTime)searchEndDispatchDate);
                }
            }

            switch (sortOrder)
            {
                case "name_desc":
                    internalReagentUsages = internalReagentUsages.OrderByDescending(rg => rg.reagent.name);
                    break;
                case "oicname_desc":
                    internalReagentUsages = internalReagentUsages.OrderByDescending(rg => rg.user.LastName).ThenBy(rg => rg.user.FirstName);
                    break;
                case "OiC":
                    internalReagentUsages = internalReagentUsages.OrderBy(rg => rg.user.LastName).ThenBy(rg => rg.user.FirstName);
                    break;
                case "quantity_desc":
                    internalReagentUsages = internalReagentUsages.OrderByDescending(rg => rg.quantity);
                    break;
                case "Quantity":
                    internalReagentUsages = internalReagentUsages.OrderBy(rg => rg.quantity);
                    break;
                case "Date":
                    internalReagentUsages = internalReagentUsages.OrderBy(rg => rg.dispatchDate);
                    break;
                case "date_desc":
                    internalReagentUsages = internalReagentUsages.OrderByDescending(rg => rg.dispatchDate);
                    break;
                default:  // Name ascending 
                    internalReagentUsages = internalReagentUsages.OrderBy(rg => rg.reagent.name);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(internalReagentUsages.ToPagedList(pageNumber, pageSize));

            //var tblreagentusages = db.InternalReagentUsages;//.Include(t => t.tblReagent);
            //return View(tblreagentusages.ToList());
        }

        // GET: /InternalReagentUsage/Details/5
        [Authorize(Roles = "Admin, CanViewInternalReagentUsage, InternalReagentUsage")]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InternalReagentUsage tblreagentusage = db.InternalReagentUsages.Find(id);
            if (tblreagentusage == null)
            {
                return HttpNotFound();
            }
            return View(tblreagentusage);
        }

        // GET: /Inventory/Create
        [Authorize(Roles = "Admin, CanAddInternalReagentUsage, InternalReagentUsage")]
        public ActionResult Create()
        {
            ViewBag.reagentId = new SelectList(db.Reagents, "reagentId", "name");
            ViewBag.userId = new SelectList(db.Users, "Id", "FullName");
            return View();
        }

        // POST: /Inventory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanAddInternalReagentUsage, InternalReagentUsage")]
        public ActionResult Create([Bind(Include = "reagentId,quantity,dispatchDate,userId,note")] InternalReagentUsage tblreagentusage)
        {
            if (ModelState.IsValid)
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var loc = db.InternalReagentUsages.FirstOrDefault(p => p.reagent.reagentId == tblreagentusage.reagentId && p.quantity == tblreagentusage.quantity && p.note == tblreagentusage.note && p.userId == tblreagentusage.userId && p.dispatchDate == tblreagentusage.dispatchDate);
                        if (loc == null)
                        {
                            //Call stock tracker table for STOCK-IN TRANSACTION
                            var stocklevel = db.Stocks.Where(c => c.reagentId == tblreagentusage.reagentId).FirstOrDefault();
                            if (stocklevel != null)
                            {
                                if ((stocklevel.totalIn - tblreagentusage.quantity)>=0) {
                                    stocklevel.reagentId = tblreagentusage.reagentId;
                                    stocklevel.totalIn = stocklevel.totalIn - tblreagentusage.quantity;
                                    db.SaveChanges();

                                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                                    var currentUser = manager.FindById(User.Identity.GetUserId());

                                    if (currentUser != null)
                                        tblreagentusage.createdBy = currentUser.UserName;
                                    else
                                        tblreagentusage.createdBy = User.Identity.Name;

                                    tblreagentusage.createdDate = DateTime.Now;

                                    db.InternalReagentUsages.Add(tblreagentusage);
                                    db.SaveChanges();

                                    dbContextTransaction.Commit();
                                }else{
                                    ModelState.AddModelError(string.Empty, "Reagent quantity entered: {" + tblreagentusage.quantity + "} is more than Stock level: {" + stocklevel.totalIn + "}!. "
                                    + "Reduce quantity entered and try again.");
                                    ViewBag.reagentId = new SelectList(db.Reagents, "reagentId", "name", tblreagentusage.reagentId);
                                    ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblreagentusage.userId);
                                    return View(tblreagentusage);
                                }
                            }
                            else {
                                ModelState.AddModelError(string.Empty, "Reagent selected could not be found in the system!. " 
                                    + "Please try again or contact the System Administrator.");
                                ViewBag.reagentId = new SelectList(db.Reagents, "reagentId", "name", tblreagentusage.reagentId);
                                ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblreagentusage.userId);
                                return View(tblreagentusage);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Reagent usage already entered & also deducted from the stock: " + tblreagentusage.reagent.name);
                            ViewBag.reagentId = new SelectList(db.Reagents, "reagentId", "name", tblreagentusage.reagentId);
                            ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblreagentusage.userId);
                            return View(tblreagentusage);
                        }
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        if (dbContextTransaction != null) dbContextTransaction.Rollback();
                        ModelState.AddModelError(string.Empty, "Error occurred saving internal reagent usage. " + "\n\nError message: " + ex.Message);
                        ViewBag.reagentId = new SelectList(db.Reagents, "reagentId", "name", tblreagentusage.reagentId);
                        ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblreagentusage.userId);
                        return View(tblreagentusage);
                    }
                }
            }

            ViewBag.reagentId = new SelectList(db.Reagents, "reagentId", "name", tblreagentusage.reagentId);
            ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblreagentusage.userId);
            return View(tblreagentusage);
        }

        // GET: /Inventory/Edit/5
        [Authorize(Roles = "Admin, CanEditInternalReagentUsage, InternalReagentUsage")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InternalReagentUsage tblreagentusage = db.InternalReagentUsages.Find(id);
            if (tblreagentusage == null)
            {
                return HttpNotFound();
            }
            //ViewBag.reagentId = new SelectList(db.Reagents, "reagentId", "name", tblinventory.reagentId);
            ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblreagentusage.userId);
            return View(tblreagentusage);
        }

        // POST: /Inventory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanEditInternalReagentUsage, InternalReagentUsage")]
        public ActionResult Edit([Bind(Include = "usageId,reagentId,quantity,dispatchDate,userId,note")] InternalReagentUsage tblreagentusage)
        {
            if (ModelState.IsValid)
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //db.Entry(tbllocation).State = EntityState.Modified;
                        var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                        var currentUser = manager.FindById(User.Identity.GetUserId());

                        var loc = db.InternalReagentUsages.Where(c => c.usageId == tblreagentusage.usageId).FirstOrDefault();
                        if (loc != null)
                            diffInQty = tblreagentusage.quantity - loc.quantity;

                        if (diffInQty > 0)//Update the reagent usage and stock tables
                        {
                            //Call stock tracker table for STOCK-IN TRANSACTION
                            var stocklevel = db.Stocks.Where(c => c.reagentId == tblreagentusage.reagentId).FirstOrDefault();
                            if (stocklevel != null)
                            {
                                if (stocklevel.totalIn > diffInQty)
                                {
                                    loc.reagentId = tblreagentusage.reagentId;
                                    loc.quantity = tblreagentusage.quantity;
                                    loc.dispatchDate = tblreagentusage.dispatchDate;
                                    loc.userId = tblreagentusage.userId;
                                    loc.note = tblreagentusage.note;

                                    if (currentUser != null)
                                        loc.updatedBy = currentUser.UserName;
                                    else
                                        loc.updatedBy = User.Identity.Name;

                                    loc.updatedDate = DateTime.Now;

                                    db.SaveChanges();

                                    stocklevel.reagentId = tblreagentusage.reagentId;
                                    stocklevel.totalIn = stocklevel.totalIn - diffInQty;
                                    db.SaveChanges();

                                    dbContextTransaction.Commit();
                                }
                                else
                                {
                                    ModelState.AddModelError(string.Empty, "Balance of Reagent quantity entered to be deducted from the stock level: {" + diffInQty + "} is more than Stock level: {" + stocklevel.totalIn + "}!. "
                                    + "Reduce quantity entered and try again.");
                                    ViewBag.reagentId = new SelectList(db.Reagents, "reagentId", "name", tblreagentusage.reagentId);
                                    ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblreagentusage.userId);
                                    return View(tblreagentusage);
                                }                                
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Reagent selected to be updated could not be found in the system!. "
                                    + "Please try again or contact the System Administrator.");
                                ViewBag.reagentId = new SelectList(db.Reagents, "reagentId", "name", tblreagentusage.reagentId);
                                ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblreagentusage.userId);
                                return View(tblreagentusage);
                            }
                        }
                        else if (diffInQty == 0)//Update the inventory table but no update on the stock table
                        {
                            loc.reagentId = tblreagentusage.reagentId;
                            loc.quantity = tblreagentusage.quantity;
                            loc.dispatchDate = tblreagentusage.dispatchDate;
                            loc.userId = tblreagentusage.userId;
                            loc.note = tblreagentusage.note;

                            if (currentUser != null)
                                loc.updatedBy = currentUser.UserName;
                            else
                                loc.updatedBy = User.Identity.Name;

                            loc.updatedDate = DateTime.Now;

                            db.SaveChanges();

                            dbContextTransaction.Commit();
                        }
                        else if (diffInQty < 0)//check indices to make sure the stock table will not end up with minus value after this update
                        {
                            //Get stock level
                            var stocklevel = db.Stocks.Where(c => c.reagentId == tblreagentusage.reagentId).FirstOrDefault();
                            if (stocklevel != null)
                            {
                                loc.reagentId = tblreagentusage.reagentId;
                                loc.quantity = tblreagentusage.quantity;
                                loc.dispatchDate = tblreagentusage.dispatchDate;
                                loc.userId = tblreagentusage.userId;
                                loc.note = tblreagentusage.note;

                                if (currentUser != null)
                                    loc.updatedBy = currentUser.UserName;
                                else
                                    loc.updatedBy = User.Identity.Name;

                                loc.updatedDate = DateTime.Now;

                                db.SaveChanges();

                                //Call stock tracker table for STOCK-IN TRANSACTION
                                stocklevel.totalIn = stocklevel.totalIn - (diffInQty);
                                db.SaveChanges();

                                dbContextTransaction.Commit();
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Reagent selected to be updated could not be found in the system!. "
                                    + "Please try again or contact the System Administrator.");
                                ViewBag.reagentId = new SelectList(db.Reagents, "reagentId", "name", tblreagentusage.reagentId);
                                ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblreagentusage.userId);
                                return View(tblreagentusage);
                            }
                        }
                        return RedirectToAction("Index");
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        var entry = ex.Entries.Single();
                        var databaseValues = (InternalReagentUsage)entry.GetDatabaseValues().ToObject();
                        var clientValues = (InternalReagentUsage)entry.Entity;
                        if (databaseValues.reagent.name != clientValues.reagent.name)
                            ModelState.AddModelError("Reagent", "Current value: " + databaseValues.reagent.name);
                        if (databaseValues.quantity != clientValues.quantity)
                            ModelState.AddModelError("Dispatch Qty", "Current value: " + databaseValues.quantity);

                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                          + "was modified by another user after you got the original value. The "
                          + "edit operation was canceled and the current values in the database "
                          + "have been displayed. If you still want to edit this record, click "
                          + "the Save button again. Otherwise click the Back to List hyperlink.");

                        if (dbContextTransaction != null) dbContextTransaction.Rollback();

                        tblreagentusage.Timestamp = databaseValues.Timestamp;
                        ViewBag.reagentId = new SelectList(db.Reagents, "reagentId", "name", tblreagentusage.reagentId);
                        ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblreagentusage.userId);
                        return View(tblreagentusage);
                    }
                    catch (Exception ex)
                    {
                        if (dbContextTransaction != null) dbContextTransaction.Rollback();
                        ModelState.AddModelError(string.Empty, "Error occurred saving inventory. " + "\n\nError message: " + ex.Message);
                        ViewBag.reagentId = new SelectList(db.Reagents, "reagentId", "name", tblreagentusage.reagentId);
                        ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblreagentusage.userId);
                        return View(tblreagentusage);
                    }
                }
            }
            ViewBag.reagentId = new SelectList(db.Reagents, "reagentId", "name", tblreagentusage.reagentId);
            ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblreagentusage.userId);
            return View(tblreagentusage);
        }

        // GET: /Inventory/Delete/5
        [Authorize(Roles = "Admin, CanDeleteInternalReagentUsage, InternalReagentUsage")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InternalReagentUsage tblreagentusage = db.InternalReagentUsages.Find(id);
            if (tblreagentusage == null)
            {
                return HttpNotFound();
            }
            return View(tblreagentusage);
        }

        // POST: /Inventory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanDeleteInternalReagentUsage, InternalReagentUsage")]
        public ActionResult DeleteConfirmed(long id)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    InternalReagentUsage tblreagentusage = db.InternalReagentUsages.Find(id);
                    if (tblreagentusage != null)
                    {
                        //Get stock level
                        var stocklevel = db.Stocks.Where(c => c.reagentId == tblreagentusage.reagentId).FirstOrDefault();
                        if (stocklevel != null)
                        {
                            //Call stock tracker table for STOCK-IN TRANSACTION
                            stocklevel.totalIn = stocklevel.totalIn + tblreagentusage.quantity;
                            db.SaveChanges();

                            db.InternalReagentUsages.Remove(tblreagentusage);
                            db.SaveChanges();

                            dbContextTransaction.Commit();
                        }
                        else
                        {
                            db.InternalReagentUsages.Remove(tblreagentusage);
                            db.SaveChanges();

                            dbContextTransaction.Commit();
                        }
                    }
                }
                catch (Exception)
                {
                    if (dbContextTransaction != null) dbContextTransaction.Rollback();
                }
            }
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