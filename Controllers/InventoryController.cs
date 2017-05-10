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
    public class InventoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private int diffInQty = 0;

        // GET: /Inventory/
        public ActionResult Index(string sortOrder, string currentFilter, string currentStartDateFilter, string currentEndDateFilter, string searchString, DateTime? searchStartStockDate, DateTime? searchEndStockDate, int? page)
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

            if (searchStartStockDate != null)
            {
                page = 1;
            }
            else
            {
                if (currentStartDateFilter != null)
                    searchStartStockDate = DateTime.Parse(currentStartDateFilter.ToString());
            }
            ViewBag.CurrentStartDateFilter = searchStartStockDate;

            if (searchEndStockDate != null)
            {
                page = 1;
            }
            else
            {
                if (currentEndDateFilter != null)
                    searchEndStockDate = DateTime.Parse(currentEndDateFilter.ToString());
            }
            ViewBag.CurrentEndDateFilter = searchEndStockDate;

            if (searchStartStockDate != null)
                searchStartStockDate = DateTime.Parse(searchStartStockDate.ToString());
            if (searchEndStockDate != null)
                searchEndStockDate = DateTime.Parse(searchEndStockDate.ToString());

            var inventories = from r in db.Inventories select r;

            if (!String.IsNullOrEmpty(searchString))
            {
                if (searchStartStockDate != null && searchEndStockDate != null)
                {
                    inventories = inventories.Where(rg => (rg.reagent.name.Contains(searchString)
                                       || rg.note.Contains(searchString) || rg.user.LastName.Contains(searchString)
                                          || rg.user.FirstName.Contains(searchString)) && (rg.stockDate >= (DateTime)searchStartStockDate && rg.stockDate <= (DateTime)searchEndStockDate));
                }
                else if (searchStartStockDate != null)
                {
                    inventories = inventories.Where(rg => (rg.reagent.name.Contains(searchString)
                                       || rg.note.Contains(searchString) || rg.user.LastName.Contains(searchString)
                                          || rg.user.FirstName.Contains(searchString)) && (rg.stockDate == (DateTime)searchStartStockDate));
                }
                else if (searchEndStockDate != null)
                {
                    inventories = inventories.Where(rg => (rg.reagent.name.Contains(searchString)
                                       || rg.note.Contains(searchString) || rg.user.LastName.Contains(searchString)
                                          || rg.user.FirstName.Contains(searchString)) && (rg.stockDate == (DateTime)searchEndStockDate));
                }
                else
                {
                    inventories = inventories.Where(rg => (rg.reagent.name.Contains(searchString)
                                       || rg.note.Contains(searchString) || rg.user.LastName.Contains(searchString)
                                          || rg.user.FirstName.Contains(searchString)));
                }
            }
            else
            {
                if (searchStartStockDate != null && searchEndStockDate != null)
                {
                    inventories = inventories.Where(rg => (rg.stockDate >= (DateTime)searchStartStockDate && rg.stockDate <= (DateTime)searchEndStockDate));
                }
                else if (searchStartStockDate != null)
                {
                    inventories = inventories.Where(rg => rg.stockDate == (DateTime)searchStartStockDate);
                }
                else if (searchEndStockDate != null)
                {
                    inventories = inventories.Where(rg => rg.stockDate == (DateTime)searchEndStockDate);
                }
            }

            switch (sortOrder)
            {
                case "name_desc":
                    inventories = inventories.OrderByDescending(rg => rg.reagent.name);
                    break;
                case "oicname_desc":
                    inventories = inventories.OrderByDescending(rg => rg.user.LastName).ThenBy(rg => rg.user.FirstName);
                    break;
                case "OiC":
                    inventories = inventories.OrderBy(rg => rg.user.LastName).ThenBy(rg => rg.user.FirstName);
                    break;
                case "quantity_desc":
                    inventories = inventories.OrderByDescending(rg => rg.quantity);
                    break;
                case "Quantity":
                    inventories = inventories.OrderBy(rg => rg.quantity);
                    break;
                case "Date":
                    inventories = inventories.OrderBy(rg => rg.stockDate);
                    break;
                case "date_desc":
                    inventories = inventories.OrderByDescending(rg => rg.stockDate);
                    break;
                default:  // Name ascending 
                    inventories = inventories.OrderBy(rg => rg.reagent.name);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(inventories.ToPagedList(pageNumber, pageSize));

            //var tblinventories = db.Inventories;//.Include(t => t.tblReagent);
            //return View(tblinventories.ToList());
        }

        // GET: /Inventory/Details/5
        [Authorize(Roles = "Admin, CanViewInventory, Inventory")]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory tblinventory = db.Inventories.Find(id);
            if (tblinventory == null)
            {
                return HttpNotFound();
            }
            return View(tblinventory);
        }

        // GET: /Inventory/Create
        [Authorize(Roles = "Admin, CanAddInventory, Inventory")]
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
        [Authorize(Roles = "Admin, CanAddInventory, Inventory")]
        public ActionResult Create([Bind(Include="reagentId,quantity,stockDate,userId,note")] Inventory tblinventory)
        {
            if (ModelState.IsValid)
            {
                using (var dbContextTransaction = db.Database.BeginTransaction()) {
                    try 
                    { 
                        var loc = db.Inventories.FirstOrDefault(p => p.reagent.reagentId == tblinventory.reagentId && p.quantity == tblinventory.quantity && p.note == tblinventory.note && p.userId == tblinventory.userId && p.stockDate == tblinventory.stockDate);
                        if (loc == null)
                        {
                            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                            var currentUser = manager.FindById(User.Identity.GetUserId());

                            if (currentUser != null)
                                tblinventory.createdBy = currentUser.UserName;
                            else
                                tblinventory.createdBy = User.Identity.Name;

                            tblinventory.createdDate = DateTime.Now;

                            db.Inventories.Add(tblinventory);
                            db.SaveChanges();

                            //Call stock tracker table for STOCK-IN TRANSACTION
                            var stocklevel = db.Stocks.Where(c => c.reagentId == tblinventory.reagentId).FirstOrDefault();
                            if(stocklevel != null){
                                stocklevel.reagentId = tblinventory.reagentId;
                                stocklevel.totalIn = stocklevel.totalIn + tblinventory.quantity;
                            }else{
                                Stock stk = new Stock();
                                stk.reagentId = tblinventory.reagentId;
                                stk.totalIn = tblinventory.quantity;
                                db.Stocks.Add(stk);
                            }
                            db.SaveChanges();

                            dbContextTransaction.Commit();
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Reagent already added to the stock: " + tblinventory.reagent.name);
                            ViewBag.reagentId = new SelectList(db.Reagents, "reagentId", "name", tblinventory.reagentId);
                            ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblinventory.userId);
                            return View(tblinventory);
                        }
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        if (dbContextTransaction != null) dbContextTransaction.Rollback();
                        ModelState.AddModelError(string.Empty, "Error occurred saving inventory. " +"\n\nError message: " + ex.Message);
                    }
                }
            }

            ViewBag.reagentId = new SelectList(db.Reagents, "reagentId", "name", tblinventory.reagentId);
            ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblinventory.userId);
            return View(tblinventory);
        }

        // GET: /Inventory/Edit/5
        [Authorize(Roles = "Admin, CanEditInventory, Inventory")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory tblinventory = db.Inventories.Find(id);
            if (tblinventory == null)
            {
                return HttpNotFound();
            }
            //ViewBag.reagentId = new SelectList(db.Reagents, "reagentId", "name", tblinventory.reagentId);
            ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblinventory.userId);
            return View(tblinventory);
        }

        // POST: /Inventory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanEditInventory, Inventory")]
        public ActionResult Edit([Bind(Include="inventoryId,reagentId,quantity,stockDate,userId,note")] Inventory tblinventory)
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

                        var loc = db.Inventories.Where(c => c.inventoryId == tblinventory.inventoryId).FirstOrDefault();
                        if(loc != null)
                            diffInQty = tblinventory.quantity - loc.quantity;

                        if (diffInQty > 0)//Update the inventory and stock tables
                        {
                            loc.reagentId = tblinventory.reagentId;
                            loc.quantity = tblinventory.quantity;
                            loc.stockDate = tblinventory.stockDate;
                            loc.userId = tblinventory.userId;
                            loc.note = tblinventory.note;

                            if (currentUser != null)
                                loc.updatedBy = currentUser.UserName;
                            else
                                loc.updatedBy = User.Identity.Name;

                            loc.updatedDate = DateTime.Now;

                            db.SaveChanges();

                            //Call stock tracker table for STOCK-IN TRANSACTION
                            var stocklevel = db.Stocks.Where(c => c.reagentId == tblinventory.reagentId).FirstOrDefault();
                            if (stocklevel != null)
                            {
                                stocklevel.reagentId = tblinventory.reagentId;
                                stocklevel.totalIn = stocklevel.totalIn + diffInQty;
                            }
                            else {
                                Stock stk = new Stock();
                                stocklevel.reagentId = tblinventory.reagentId;
                                stocklevel.totalIn = tblinventory.quantity;
                            }
                            db.SaveChanges();

                            dbContextTransaction.Commit();
                        }
                        else if (diffInQty == 0)//Update the inventory table but no update on the stock table
                        {
                            loc.reagentId = tblinventory.reagentId;
                            loc.quantity = tblinventory.quantity;
                            loc.stockDate = tblinventory.stockDate;
                            loc.userId = tblinventory.userId;
                            loc.note = tblinventory.note;

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
                            var stocklevel = db.Stocks.Where(c => c.reagentId == tblinventory.reagentId).FirstOrDefault();
                            if (stocklevel != null)
                            {
                                if ((stocklevel.totalIn + diffInQty) < 0)
                                {
                                    ModelState.AddModelError(string.Empty, "Transaction not allowed. Stock level for "
                                        + tblinventory.reagent.name.ToUpper()
                                        + " will be left with " + (stocklevel.totalIn + diffInQty)
                                        + " if this transaction was allowed to be completed");

                                    if (dbContextTransaction != null) dbContextTransaction.Rollback();
                                    ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblinventory.userId);
                                    return View(tblinventory);
                                }
                                else {
                                    loc.reagentId = tblinventory.reagentId;
                                    loc.quantity = tblinventory.quantity;
                                    loc.stockDate = tblinventory.stockDate;
                                    loc.userId = tblinventory.userId;
                                    loc.note = tblinventory.note;

                                    if (currentUser != null)
                                        loc.updatedBy = currentUser.UserName;
                                    else
                                        loc.updatedBy = User.Identity.Name;

                                    loc.updatedDate = DateTime.Now;

                                    db.SaveChanges();

                                    //Call stock tracker table for STOCK-IN TRANSACTION
                                    stocklevel.totalIn = stocklevel.totalIn + diffInQty;
                                    db.SaveChanges();

                                    dbContextTransaction.Commit();
                                }
                            }
                            else {
                                loc.reagentId = tblinventory.reagentId;
                                loc.quantity = tblinventory.quantity;
                                loc.stockDate = tblinventory.stockDate;
                                loc.userId = tblinventory.userId;
                                loc.note = tblinventory.note;

                                if (currentUser != null)
                                    loc.updatedBy = currentUser.UserName;
                                else
                                    loc.updatedBy = User.Identity.Name;

                                loc.updatedDate = DateTime.Now;

                                db.SaveChanges();

                                //Call stock tracker table for STOCK-IN TRANSACTION
                                Stock stk = new Stock();
                                stk.reagentId = tblinventory.reagentId;
                                stk.totalIn = tblinventory.quantity;
                                db.Stocks.Add(stk);
                                db.SaveChanges();

                                dbContextTransaction.Commit();
                            }
                        }
                        return RedirectToAction("Index");
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        var entry = ex.Entries.Single();
                        var databaseValues = (Inventory)entry.GetDatabaseValues().ToObject();
                        var clientValues = (Inventory)entry.Entity;
                        if (databaseValues.reagent.name != clientValues.reagent.name)
                            ModelState.AddModelError("Reagent", "Current value: " + databaseValues.reagent.name);
                        if (databaseValues.quantity != clientValues.quantity)
                            ModelState.AddModelError("Pur. Qty", "Current value: " + databaseValues.quantity);

                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                          + "was modified by another user after you got the original value. The "
                          + "edit operation was canceled and the current values in the database "
                          + "have been displayed. If you still want to edit this record, click "
                          + "the Save button again. Otherwise click the Back to List hyperlink.");

                        if (dbContextTransaction != null) dbContextTransaction.Rollback();

                        tblinventory.Timestamp = databaseValues.Timestamp;
                        ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblinventory.userId);
                        return View(tblinventory);
                    }
                    catch (Exception ex)
                    {
                        if (dbContextTransaction != null) dbContextTransaction.Rollback();
                        ModelState.AddModelError(string.Empty, "Error occurred saving inventory. " + "\n\nError message: " + ex.Message);
                    }
                }
            }
            //ViewBag.reagentId = new SelectList(db.Reagents, "reagentId", "name", tblinventory.reagentId);
            ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblinventory.userId);
            return View(tblinventory);
        }

        // GET: /Inventory/Delete/5
        [Authorize(Roles = "Admin, CanDeleteInventory, Inventory")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory tblinventory = db.Inventories.Find(id);
            if (tblinventory == null)
            {
                return HttpNotFound();
            }
            return View(tblinventory);
        }

        // POST: /Inventory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanDeleteInventory, Inventory")]
        public ActionResult DeleteConfirmed(long id)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    Inventory tblinventory = db.Inventories.Find(id);
                    if (tblinventory != null) 
                    { 
                        //Get stock level
                        var stocklevel = db.Stocks.Where(c => c.reagentId == tblinventory.reagentId).FirstOrDefault();
                        if (stocklevel != null)
                        {
                            if ((stocklevel.totalIn - tblinventory.quantity) < 0)
                            {
                                ModelState.AddModelError(string.Empty, "Transaction not allowed. Stock level for "
                                    + tblinventory.reagent.name.ToUpper()
                                    + " will be left with " + (stocklevel.totalIn + diffInQty)
                                    + " if this transaction was allowed to be completed");

                                if (dbContextTransaction != null) dbContextTransaction.Rollback();
                                return View(tblinventory);
                            }
                            else
                            {
                                //Call stock tracker table for STOCK-IN TRANSACTION
                                stocklevel.totalIn = stocklevel.totalIn - tblinventory.quantity;
                                db.SaveChanges();

                                db.Inventories.Remove(tblinventory);
                                db.SaveChanges();

                                dbContextTransaction.Commit();
                            }
                        }
                        else
                        {
                            db.Inventories.Remove(tblinventory);
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
