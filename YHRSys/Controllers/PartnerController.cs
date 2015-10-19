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
    public class PartnerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        AccessRoleCheck accessRoleCheck = new AccessRoleCheck();
        string[] groups = new string[] { "Partner" };
        
        // GET: /Partner/
        [Authorize(Roles = "Admin, CanViewPartner, CanViewOwnPartner, Partner")]
        public ActionResult Index(string sortOrder, string currentFilter, string currentStartDateFilter, string currentEndDateFilter, string searchString, DateTime? searchStartCreatedDate, DateTime? searchEndCreatedDate, int? page)
        {
            string perms = "CanViewOwnPartner";
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            ApplicationUser accessUser = manager.FindById(User.Identity.GetUserId());
            Boolean OwnAccess = accessRoleCheck.checkOwnPerm(groups, accessUser, perms);
            //Boolean GeneralAccess = checkOwnPerm(groups, accessUser, perms);

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.ContactSortParm = sortOrder == "Contact" ? "contact_desc" : "Contact";
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

            if (searchStartCreatedDate != null)
            {
                page = 1;
            }
            else
            {
                if (currentStartDateFilter != null)
                    searchStartCreatedDate = DateTime.Parse(currentStartDateFilter.ToString());
            }
            ViewBag.CurrentStartDateFilter = searchStartCreatedDate;

            if (searchEndCreatedDate != null)
            {
                page = 1;
            }
            else
            {
                if (currentEndDateFilter != null)
                    searchEndCreatedDate = DateTime.Parse(currentEndDateFilter.ToString());
            }
            ViewBag.CurrentEndDateFilter = searchEndCreatedDate;

            if (searchStartCreatedDate != null)
                searchStartCreatedDate = DateTime.Parse(searchStartCreatedDate.ToString());
            if (searchEndCreatedDate != null)
                searchEndCreatedDate = DateTime.Parse(searchEndCreatedDate.ToString());

            var partners = from r in db.Partners select r;

            if (!String.IsNullOrEmpty(searchString))
            {
                if (searchStartCreatedDate != null && searchEndCreatedDate != null)
                {
                    partners = partners.Where(rg => (rg.name.Contains(searchString)
                                       || rg.contactAddress.Contains(searchString) || rg.contactCity.Contains(searchString) || rg.contactState.Contains(searchString) || rg.createdBy.Contains(searchString)
                                        || rg.contactCountry.Contains(searchString) || rg.emailAddress.Contains(searchString) || rg.webAddress.Contains(searchString)) && (rg.createdDate >= (DateTime)searchStartCreatedDate && rg.createdDate <= (DateTime)searchEndCreatedDate));
                }
                else if (searchStartCreatedDate != null)
                {
                    partners = partners.Where(rg => (rg.name.Contains(searchString)
                                       || rg.contactAddress.Contains(searchString) || rg.contactCity.Contains(searchString) || rg.contactState.Contains(searchString) || rg.createdBy.Contains(searchString)
                                        || rg.contactCountry.Contains(searchString) || rg.emailAddress.Contains(searchString) || rg.webAddress.Contains(searchString)) && (rg.createdDate == (DateTime)searchStartCreatedDate));
                }
                else if (searchEndCreatedDate != null)
                {
                    partners = partners.Where(rg => (rg.name.Contains(searchString)
                                       || rg.contactAddress.Contains(searchString) || rg.contactCity.Contains(searchString) || rg.contactState.Contains(searchString) || rg.createdBy.Contains(searchString)
                                        || rg.contactCountry.Contains(searchString) || rg.emailAddress.Contains(searchString) || rg.webAddress.Contains(searchString)) && (rg.createdDate == (DateTime)searchEndCreatedDate));
                }
                else
                {
                    partners = partners.Where(rg => (rg.name.Contains(searchString)
                                       || rg.contactAddress.Contains(searchString) || rg.contactCity.Contains(searchString) || rg.contactState.Contains(searchString) || rg.createdBy.Contains(searchString)
                                        || rg.contactCountry.Contains(searchString) || rg.emailAddress.Contains(searchString) || rg.webAddress.Contains(searchString)));
                }
            }
            else
            {
                if (searchStartCreatedDate != null && searchEndCreatedDate != null)
                {
                    partners = partners.Where(rg => (rg.createdDate >= (DateTime)searchStartCreatedDate && rg.createdDate <= (DateTime)searchEndCreatedDate));
                }
                else if (searchStartCreatedDate != null)
                {
                    partners = partners.Where(rg => rg.createdDate == (DateTime)searchStartCreatedDate);
                }
                else if (searchEndCreatedDate != null)
                {
                    partners = partners.Where(rg => rg.createdDate <= (DateTime)searchEndCreatedDate);
                }
            }

            if (OwnAccess)
            {
                partners = partners.Where(rg => rg.createdBy == accessUser.UserName);
            }
            
            switch (sortOrder)
            {
                case "name_desc":
                    partners = partners.OrderByDescending(rg => rg.name);
                    break;
                case "contact_desc":
                    partners = partners.OrderByDescending(rg => rg.contactAddress);
                    break;
                case "Contact":
                    partners = partners.OrderBy(rg => rg.contactAddress);
                    break;
                case "Date":
                    partners = partners.OrderBy(rg => rg.createdDate);
                    break;
                case "date_desc":
                    partners = partners.OrderByDescending(rg => rg.createdDate);
                    break;
                default:  // Name ascending 
                    partners = partners.OrderBy(rg => rg.name);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(partners.ToPagedList(pageNumber, pageSize));
        }

        // GET: /Partner/Details/5
        [Authorize(Roles = "Admin, CanViewPartner, CanViewOwnPartner, Partner")]
        public ActionResult Details(long? id)
        {
            string perms = "CanViewOwnPartner";
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            ApplicationUser accessUser = manager.FindById(User.Identity.GetUserId());
            Boolean OwnAccess = accessRoleCheck.checkOwnPerm(groups, accessUser, perms);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Partner tblpartner = db.Partners.Find(id);
            if (OwnAccess)
            {
                tblpartner = (Partner)db.Partners.Where(p => p.partnerId == id && p.createdBy == accessUser.UserName);
            }

            if (tblpartner == null)
            {
                return HttpNotFound();
            }
            return View(tblpartner);
        }

        // GET: /Partner/Create
        [Authorize(Roles = "Admin, CanAddPartner, CanAddOwnPartner, Partner")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Partner/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanAddPartner, CanAddOwnPartner, Partner")]
        public ActionResult Create([Bind(Include="name,contactAddress,contactCity,contactState,contactCountry,phoneNumber,emailAddress,webAddress,geoLongitude,geoLatitude")] Partner tblpartner)
        {
            string perms = "CanAddOwnPartner";
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            ApplicationUser accessUser = manager.FindById(User.Identity.GetUserId());
            Boolean OwnAccess = accessRoleCheck.checkOwnPerm(groups, accessUser, perms);

            if (ModelState.IsValid)
            {
                var r = db.Partners.FirstOrDefault(p => p.name == tblpartner.name);

                if(OwnAccess){
                    r = db.Partners.FirstOrDefault(p => p.createdBy == accessUser.UserName);
                }

                if (r == null)
                {
                    //var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var currentUser = manager.FindById(User.Identity.GetUserId());

                    if (currentUser != null)
                        tblpartner.createdBy = currentUser.UserName;
                    else
                        tblpartner.createdBy = User.Identity.Name;

                    tblpartner.createdDate = DateTime.Now;

                    db.Partners.Add(tblpartner);
                    db.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Partner already registered or you have added a partner earlier than now: " + r.name);
                    return View(tblpartner);
                }
                return RedirectToAction("Index");
            }

            return View(tblpartner);
        }

        // GET: /Partner/Edit/5
        [Authorize(Roles = "Admin, CanEditPartner, CanEditOwnPartner, Partner")]
        public ActionResult Edit(long? id)
        {
            string perms = "CanEditOwnPartner";
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            ApplicationUser accessUser = manager.FindById(User.Identity.GetUserId());
            Boolean OwnAccess = accessRoleCheck.checkOwnPerm(groups, accessUser, perms);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partner tblpartner = db.Partners.Find(id);

            if (OwnAccess)
            {
                tblpartner = (Partner)db.Partners.Where(p => p.partnerId == id && p.createdBy == accessUser.UserName);
            }

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
        [Authorize(Roles = "Admin, CanEditPartner, CanEditOwnPartner, Partner")]
        public ActionResult Edit([Bind(Include = "partnerId,name,contactAddress,contactCity,contactState,contactCountry,phoneNumber,emailAddress,webAddress,geoLongitude,geoLatitude")] Partner tblpartner)
        {
            string perms = "CanEditOwnPartner";
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            ApplicationUser currentUser = manager.FindById(User.Identity.GetUserId());
            Boolean OwnAccess = accessRoleCheck.checkOwnPerm(groups, currentUser, perms);

            //var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            //var currentUser = manager.FindById(User.Identity.GetUserId());

            if (ModelState.IsValid)
            {
                //db.SaveChanges();
                try
                {
                    //db.Entry(tbllocationuser).State = EntityState.Modified;
                    var r = db.Partners.Where(c => c.partnerId == tblpartner.partnerId).FirstOrDefault();

                    if (OwnAccess)
                    {
                        r = (Partner)db.Partners.Where(p => p.partnerId == tblpartner.partnerId && p.createdBy == currentUser.UserName);
                    }

                    r.name = tblpartner.name;
                    r.contactAddress = tblpartner.contactAddress;

                    r.phoneNumber = tblpartner.phoneNumber;
                    r.emailAddress = tblpartner.emailAddress;
                    r.webAddress = tblpartner.webAddress;
                    r.geoLongitude = tblpartner.geoLongitude;
                    r.geoLatitude = tblpartner.geoLatitude;
                    r.contactCity = tblpartner.contactCity;
                    r.contactState = tblpartner.contactState;
                    r.contactCountry = tblpartner.contactCountry;

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
                    var databaseValues = (Partner)entry.GetDatabaseValues().ToObject();
                    var clientValues = (Partner)entry.Entity;
                    if (databaseValues.name != clientValues.name)
                        ModelState.AddModelError("Partner", "Current value: " + databaseValues.name);
                    if (databaseValues.contactAddress != clientValues.contactAddress)
                        ModelState.AddModelError("Contact Address", "Current value: " + databaseValues.contactAddress);

                    ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                      + "was modified by another user after you got the original value. The "
                      + "edit operation was canceled and the current values in the database "
                      + "have been displayed. If you still want to edit this record, click "
                      + "the Save button again. Otherwise click the Back to List hyperlink.");

                    tblpartner.Timestamp = databaseValues.Timestamp;
                    return View();
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View(tblpartner);
        }

        // GET: /Partner/Delete/5
        [Authorize(Roles = "Admin, CanDeletePartner, CanDeleteOwnPartner, Partner")]
        public ActionResult Delete(long? id)
        {
            string perms = "CanDeleteOwnPartner";
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            ApplicationUser currentUser = manager.FindById(User.Identity.GetUserId());
            Boolean OwnAccess = accessRoleCheck.checkOwnPerm(groups, currentUser, perms);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partner tblpartner = db.Partners.Find(id);

            if (OwnAccess)
            {
                tblpartner = (Partner)db.Partners.Where(p => p.partnerId == id && p.createdBy == currentUser.UserName);
            }

            if (tblpartner == null)
            {
                return HttpNotFound();
            }
            return View(tblpartner);
        }

        // POST: /Partner/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanDeletePartner, CanDeleteOwnPartner, Partner")]
        public ActionResult DeleteConfirmed(long id)
        {
            string perms = "CanDeleteOwnPartner";
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            ApplicationUser currentUser = manager.FindById(User.Identity.GetUserId());
            Boolean OwnAccess = accessRoleCheck.checkOwnPerm(groups, currentUser, perms);

            Partner tblpartner = db.Partners.Find(id);
            if (OwnAccess)
            {
                tblpartner = (Partner)db.Partners.Where(p => p.partnerId == id && p.createdBy == currentUser.UserName);
            }

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
