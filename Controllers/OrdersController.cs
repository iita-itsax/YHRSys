using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YHRSys.Models;
using PagedList;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Infrastructure;

namespace YHRSys.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Orders
        [Authorize(Roles = "Admin, CanViewOrder, Order")]
        public ActionResult Index(string sortOrder, string currentFilter, string currentStartDateFilter, string currentEndDateFilter, string searchString, DateTime? searchStartOrderDate, DateTime? searchEndOrderDate, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.LastNameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.CitySortParm = sortOrder == "City" ? "city_desc" : "City";
            ViewBag.StateSortParm = sortOrder == "State" ? "state_desc" : "State";
            ViewBag.OrderDateSortParm = sortOrder == "OrderDate" ? "orderdate_desc" : "OrderDate";
            ViewBag.CountrySortParm = sortOrder == "Country" ? "country_desc" : "Country";
            ViewBag.StatusSortParm = sortOrder == "Status" ? "status_desc" : "Status";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (searchStartOrderDate != null)
            {
                page = 1;
            }
            else
            {
                if (currentStartDateFilter != null)
                    searchStartOrderDate = DateTime.Parse(currentStartDateFilter.ToString());
            }
            ViewBag.CurrentStartDateFilter = searchStartOrderDate;

            if (searchEndOrderDate != null)
            {
                page = 1;
            }
            else
            {
                if (currentEndDateFilter != null)
                    searchEndOrderDate = DateTime.Parse(currentEndDateFilter.ToString());
            }
            ViewBag.CurrentEndDateFilter = searchEndOrderDate;

            if (searchStartOrderDate != null)
                searchStartOrderDate = DateTime.Parse(searchStartOrderDate.ToString());
            if (searchEndOrderDate != null)
                searchEndOrderDate = DateTime.Parse(searchEndOrderDate.ToString());

            var orders = from r in db.Orders select r;

            if (!String.IsNullOrEmpty(searchString))
            {
                if (searchStartOrderDate != null && searchEndOrderDate != null)
                {
                    orders = orders.Where(rg => (rg.LastName.Contains(searchString)
                                       || rg.FirstName.Contains(searchString)
                                       || rg.City.Contains(searchString) || rg.State.Contains(searchString) || rg.Country.Contains(searchString) || rg.Email.Contains(searchString) || rg.status.Contains(searchString) || rg.Address.Contains(searchString)) && (rg.OrderDate.Date >= (DateTime)searchStartOrderDate && rg.OrderDate.Date <= (DateTime)searchEndOrderDate));
                }
                else if (searchStartOrderDate != null)
                {
                    orders = orders.Where(rg => (rg.LastName.Contains(searchString)
                                       || rg.FirstName.Contains(searchString)
                                       || rg.City.Contains(searchString) || rg.State.Contains(searchString) || rg.Country.Contains(searchString) || rg.Email.Contains(searchString) || rg.status.Contains(searchString) || rg.Address.Contains(searchString)) && (rg.OrderDate.Date == (DateTime)searchStartOrderDate));
                }
                else if (searchEndOrderDate != null)
                {
                    orders = orders.Where(rg => (rg.LastName.Contains(searchString)
                                       || rg.FirstName.Contains(searchString)
                                       || rg.City.Contains(searchString) || rg.State.Contains(searchString) || rg.Country.Contains(searchString) || rg.Email.Contains(searchString) || rg.status.Contains(searchString) || rg.Address.Contains(searchString)) && (rg.OrderDate.Date == (DateTime)searchEndOrderDate));
                }
                else
                {
                    orders = orders.Where(rg => rg.LastName.Contains(searchString)
                                       || rg.FirstName.Contains(searchString)
                                       || rg.City.Contains(searchString) || rg.State.Contains(searchString) || rg.Country.Contains(searchString) || rg.Email.Contains(searchString) || rg.status.Contains(searchString) || rg.Address.Contains(searchString));
                }
            }
            else
            {
                if (searchStartOrderDate != null && searchEndOrderDate != null)
                {
                    orders = orders.Where(rg => (rg.OrderDate.Date >= (DateTime)searchStartOrderDate && rg.OrderDate <= (DateTime)searchEndOrderDate));
                }
                else if (searchStartOrderDate != null)
                {
                    orders = orders.Where(rg => rg.OrderDate.Date == (DateTime)searchStartOrderDate);
                }
                else if (searchEndOrderDate != null)
                {
                    orders = orders.Where(rg => rg.OrderDate.Date <= (DateTime)searchEndOrderDate);
                }
            }

            switch (sortOrder)
            {
                case "name_desc":
                    orders = orders.OrderByDescending(rg => rg.LastName).ThenBy(rg => rg.FirstName);
                    break;
                case "city_desc":
                    orders = orders.OrderByDescending(rg => rg.City);
                    break;
                case "City":
                    orders = orders.OrderBy(rg => rg.City);
                    break;
                case "state_desc":
                    orders = orders.OrderByDescending(rg => rg.State);
                    break;
                case "State":
                    orders = orders.OrderBy(rg => rg.State);
                    break;
                case "country_desc":
                    orders = orders.OrderByDescending(rg => rg.Country);
                    break;
                case "Country":
                    orders = orders.OrderBy(rg => rg.Country);
                    break;
                case "status_desc":
                    orders = orders.OrderByDescending(rg => rg.status);
                    break;
                case "Status":
                    orders = orders.OrderBy(rg => rg.status);
                    break;
                case "OrderDate":
                    orders = orders.OrderBy(rg => rg.OrderDate);
                    break;
                case "orderdate_desc":
                    orders = orders.OrderByDescending(rg => rg.OrderDate);
                    break;
                default:  // Name ascending 
                    orders = orders.OrderBy(rg => rg.LastName).ThenBy(rg => rg.FirstName);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(orders.ToPagedList(pageNumber, pageSize));
        }

        // GET: Orders/Details/5
        [Authorize(Roles = "Admin, CanViewOrder, Order")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        [Authorize(Roles = "Admin, CanEditOrder, Order")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanEditOrder, Order")]
        public ActionResult Edit([Bind(Include = "FirstName,LastName,Address,City,State,Country,PostalCode,Phone,Email,status")] Order order, int orderId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        //db.Entry(tbllocation).State = EntityState.Modified;
                        var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                        var currentUser = manager.FindById(User.Identity.GetUserId());

                        var o = db.Orders.Where(c => c.OrderId == orderId).FirstOrDefault();
                        if (o != null)
                        {
                            o.FirstName = order.FirstName;
                            o.LastName = order.LastName;
                            o.Address = order.Address;
                            o.City = order.City;
                            o.State = order.State;
                            o.Country = order.Country;
                            o.PostalCode = order.PostalCode;
                            o.Phone = order.Phone;
                            o.Email = order.Email;
                            o.status = order.status;

                            if (currentUser != null)
                                o.updatedBy = currentUser.UserName;
                            else
                                o.updatedBy = User.Identity.Name;

                            o.updatedDate = DateTime.Now;

                            db.SaveChanges();


                            //SEND MAIL TO ORDER REQUESTER
                            if (order.status == ORDERSTATUS.COMPLETED.ToString()) {
                                OrderDetailProcess oDP = new OrderDetailProcess();
                                oDP.SendCompletedAlert(o, orderId);
                            }
                        }
                        else {
                            ModelState.AddModelError(string.Empty, "Submitted Order could not be located for update!");
                            return View(order);
                        }
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        var entry = ex.Entries.Single();
                        var databaseValues = (Order)entry.GetDatabaseValues().ToObject();
                        var clientValues = (Order)entry.Entity;
                        if (databaseValues.LastName != clientValues.LastName)
                            ModelState.AddModelError("Last name", "Current value: " + databaseValues.LastName);
                        if (databaseValues.FirstName != clientValues.FirstName)
                            ModelState.AddModelError("First name", "Current value: " + databaseValues.FirstName);
                        if (databaseValues.status != clientValues.status)
                            ModelState.AddModelError("Status", "Current value: " + databaseValues.status);

                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                          + "was modified by another user after you got the original value. The "
                          + "edit operation was canceled and the current values in the database "
                          + "have been displayed. If you still want to edit this record, click "
                          + "the Save button again. Otherwise click the Back to List hyperlink.");

                        order.Timestamp = databaseValues.Timestamp;
                        return View();
                    }
                    return RedirectToAction("Index");
                }
                return View(order);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error occurred: "+ex.Message);
                return View(order);
            }
        }

        // GET: Orders/Delete/5
        [Authorize(Roles = "Admin, CanDeleteOrder, Order")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanDeleteOrder, Order")]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: /Orders/VarietyOrderDelete/5
        [Authorize(Roles = "Admin, CanDeleteOrder, Order")]
        public ActionResult DeleteVarietyOrder(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail od = db.OrderDetails.Find(id);
            if (od == null)
            {
                return HttpNotFound();
            }
            return View(od);
        }

        // POST: /Orders/VarietyOrderDelete/5
        [HttpPost, ActionName("DeleteVarietyOrder")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanDeleteOrder, Order")]
        public ActionResult DeleteVarietyOrderConfirmed(long id)
        {
            OrderDetail od = db.OrderDetails.Find(id);
            if (od != null)
            {
                db.OrderDetails.Remove(od);
                db.SaveChanges();
            }
            return RedirectToAction("Details", new { id = id });
        }

        // GET: /Orders/EditVarietyOrder/5
        [Authorize(Roles = "Admin, CanEditOrder, Order")]
        public ActionResult EditVarietyOrder(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail od = db.OrderDetails.Find(id);
            if (od == null)
            {
                return HttpNotFound();
            }

            ViewBag.VarietyId = new SelectList(db.Varieties, "varietyId", "FullDescription", od.VarietyId);
            return View(od);
        }

        // POST: /Orders/EditVarietyOrder/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanEditOrder, Order")]
        public ActionResult EditVarietyOrder([Bind(Include = "OrderDetailId,OrderId,VarietyId,Quantity")] OrderDetail orderDetail)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());

            if (ModelState.IsValid)
            {
                try
                {
                    //db.Entry(tbllocationuser).State = EntityState.Modified;
                    var od = db.OrderDetails.Where(c => c.OrderDetailId == orderDetail.OrderDetailId).FirstOrDefault();
                    od.VarietyId = orderDetail.VarietyId;
                    od.Quantity = orderDetail.Quantity;
                    if (currentUser != null)
                        od.updatedBy = currentUser.UserName;
                    else
                        od.updatedBy = User.Identity.Name;

                    od.updatedDate = DateTime.Now;
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var databaseValues = (OrderDetail)entry.GetDatabaseValues().ToObject();
                    var clientValues = (OrderDetail)entry.Entity;
                    if (databaseValues.VarietyId != clientValues.VarietyId)
                        ModelState.AddModelError("Variety", "Current value: " + databaseValues.Variety.FullDescription);
                    if (databaseValues.Quantity != clientValues.Quantity)
                        ModelState.AddModelError("Variety Quantity", "Current value: " + databaseValues.Quantity);

                    ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                      + "was modified by another user after you got the original value. The "
                      + "edit operation was canceled and the current values in the database "
                      + "have been displayed. If you still want to edit this record, click "
                      + "the Save button again. Otherwise click the Back to List hyperlink.");

                    orderDetail.Timestamp = databaseValues.Timestamp;
                    ViewBag.VarietyId = new SelectList(db.Varieties, "varietyId", "FullDescription", orderDetail.VarietyId);
                    return View();
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                    ViewBag.VarietyId = new SelectList(db.Varieties, "varietyId", "FullDescription", orderDetail.VarietyId);
                    return View();
                }
                return RedirectToAction("Details", new { id = orderDetail.OrderId });
            }
            ViewBag.VarietyId = new SelectList(db.Varieties, "varietyId", "FullDescription", orderDetail.VarietyId);
            return View(orderDetail);
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
