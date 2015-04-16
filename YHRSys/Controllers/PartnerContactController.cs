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
using DropdownSelect.Models;
using PagedList;

namespace YHRSys.Controllers
{
    [Authorize]
    public class PartnerContactController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        SelectedValue selVal = new SelectedValue();

        private List<SelectListItem> listTitle(string title)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "--Title--", Value = "", Selected = selVal.checkForSelectedValue("", title) });
            items.Add(new SelectListItem { Text = "Mr", Value = "Mr", Selected = selVal.checkForSelectedValue("Mr", title) });
            items.Add(new SelectListItem { Text = "Mrs", Value = "Mrs", Selected = selVal.checkForSelectedValue("Mrs", title) });
            items.Add(new SelectListItem { Text = "Ms", Value = "Ms", Selected = selVal.checkForSelectedValue("Ms", title) });
            items.Add(new SelectListItem { Text = "Engr", Value = "Engr", Selected = selVal.checkForSelectedValue("Engr", title) });
            items.Add(new SelectListItem { Text = "Dr", Value = "Dr", Selected = selVal.checkForSelectedValue("Dr", title) });
            items.Add(new SelectListItem { Text = "Prof", Value = "Prof", Selected = selVal.checkForSelectedValue("Prof", title) });
            items.Add(new SelectListItem { Text = "Rev", Value = "Rev", Selected = selVal.checkForSelectedValue("Rev", title) });
            items.Add(new SelectListItem { Text = "Chief", Value = "Chief", Selected = selVal.checkForSelectedValue("Chief", title) });
            items.Add(new SelectListItem { Text = "Other", Value = "Other", Selected = selVal.checkForSelectedValue("Other", title) });
            return items;
        }

        private List<SelectListItem> listGender(string gender)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "--Gender--", Value = "", Selected = selVal.checkForSelectedValue("", gender) });
            items.Add(new SelectListItem { Text = "Female", Value = "Female", Selected = selVal.checkForSelectedValue("Female", gender) });
            items.Add(new SelectListItem { Text = "Male", Value = "Male", Selected = selVal.checkForSelectedValue("Male", gender) });
            return items;
        }

        // GET: /PartnerContact/
        public ActionResult Index(string sortOrder, string currentFilter, string currentStartDateFilter, string currentEndDateFilter, string searchString, DateTime? searchStartCreatedDate, DateTime? searchEndCreatedDate, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.ContactSortParm = sortOrder == "Contact" ? "contact_desc" : "Contact";
            ViewBag.GenderSortParm = sortOrder == "Gender" ? "gender_desc" : "Gender";
            ViewBag.PersonSortParm = sortOrder == "Person" ? "person_desc" : "Person";
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

            var partners = from r in db.PartnerContactPersons select r;

            if (!String.IsNullOrEmpty(searchString))
            {
                if (searchStartCreatedDate != null && searchEndCreatedDate != null)
                {
                    partners = partners.Where(rg => (rg.firstName.Contains(searchString) || rg.lastName.Contains(searchString) || rg.otherNames.Contains(searchString) || rg.personTitle.Contains(searchString) || rg.gender.Contains(searchString)
                                       || rg.contactAddress.Contains(searchString) || rg.contactCity.Contains(searchString) || rg.contactState.Contains(searchString) || rg.createdBy.Contains(searchString)
                                        || rg.contactCountry.Contains(searchString) || rg.emailAddress.Contains(searchString) || rg.webAddress.Contains(searchString)) && (rg.createdDate >= (DateTime)searchStartCreatedDate && rg.createdDate <= (DateTime)searchEndCreatedDate));
                }
                else if (searchStartCreatedDate != null)
                {
                    partners = partners.Where(rg => (rg.firstName.Contains(searchString) || rg.lastName.Contains(searchString) || rg.otherNames.Contains(searchString) || rg.personTitle.Contains(searchString) || rg.gender.Contains(searchString)
                                       || rg.contactAddress.Contains(searchString) || rg.contactCity.Contains(searchString) || rg.contactState.Contains(searchString) || rg.createdBy.Contains(searchString)
                                        || rg.contactCountry.Contains(searchString) || rg.emailAddress.Contains(searchString) || rg.webAddress.Contains(searchString)) && (rg.createdDate == (DateTime)searchStartCreatedDate));
                }
                else if (searchEndCreatedDate != null)
                {
                    partners = partners.Where(rg => (rg.firstName.Contains(searchString) || rg.lastName.Contains(searchString) || rg.otherNames.Contains(searchString) || rg.personTitle.Contains(searchString) || rg.gender.Contains(searchString)
                                       || rg.contactAddress.Contains(searchString) || rg.contactCity.Contains(searchString) || rg.contactState.Contains(searchString) || rg.createdBy.Contains(searchString)
                                        || rg.contactCountry.Contains(searchString) || rg.emailAddress.Contains(searchString) || rg.webAddress.Contains(searchString)) && (rg.createdDate == (DateTime)searchEndCreatedDate));
                }
                else
                {
                    partners = partners.Where(rg => (rg.firstName.Contains(searchString) || rg.lastName.Contains(searchString) || rg.otherNames.Contains(searchString) || rg.personTitle.Contains(searchString) || rg.gender.Contains(searchString)
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

            switch (sortOrder)
            {
                case "name_desc":
                    partners = partners.OrderByDescending(rg => rg.partner.name);
                    break;
                case "person_desc":
                    partners = partners.OrderByDescending(rg => rg.lastName).ThenBy(rg => rg.firstName);
                    break;
                case "Person":
                    partners = partners.OrderBy(rg => rg.lastName).ThenBy(rg => rg.firstName);
                    break;
                case "contact_desc":
                    partners = partners.OrderByDescending(rg => rg.contactAddress);
                    break;
                case "Contact":
                    partners = partners.OrderBy(rg => rg.contactAddress);
                    break;
                case "gender_desc":
                    partners = partners.OrderByDescending(rg => rg.gender);
                    break;
                case "Gender":
                    partners = partners.OrderBy(rg => rg.gender);
                    break;
                case "Date":
                    partners = partners.OrderBy(rg => rg.createdDate);
                    break;
                case "date_desc":
                    partners = partners.OrderByDescending(rg => rg.createdDate);
                    break;
                default:  // Name ascending 
                    partners = partners.OrderBy(rg => rg.partner.name);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(partners.ToPagedList(pageNumber, pageSize));
        }

        // GET: /PartnerContact/Details/5
        [Authorize(Roles = "Admin, CanViewPartnerContact, PartnerContact")]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartnerContact tblpartnercontact = db.PartnerContactPersons.Find(id);
            if (tblpartnercontact == null)
            {
                return HttpNotFound();
            }
            return View(tblpartnercontact);
        }

        // GET: /PartnerContact/Create
        [Authorize(Roles = "Admin, CanAddPartnerContact, PartnerContact")]
        public ActionResult Create()
        {
            ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name");
            ViewBag.personTitle = listTitle(null);
            ViewBag.gender = listGender(null);
            return View();
        }

        // POST: /PartnerContact/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanAddPartnerContact, PartnerContact")]
        public ActionResult Create([Bind(Include = "partnerId,firstName,otherNames,lastName,gender,phoneNumber,emailAddress,contactAddress,contactCity,contactState,contactCountry,webAddress,geoLongitude,geoLatitude,personTitle")] PartnerContact tblpartnercontact)
        {
            if (ModelState.IsValid)
            {
                var r = db.PartnerContactPersons.FirstOrDefault(p => p.partnerId == tblpartnercontact.partnerId && p.firstName == tblpartnercontact.firstName && p.lastName == tblpartnercontact.lastName && p.emailAddress == tblpartnercontact.emailAddress);
                if (r == null)
                {
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var currentUser = manager.FindById(User.Identity.GetUserId());

                    if (currentUser != null)
                        tblpartnercontact.createdBy = currentUser.UserName;
                    else
                        tblpartnercontact.createdBy = User.Identity.Name;

                    tblpartnercontact.createdDate = DateTime.Now;

                    db.PartnerContactPersons.Add(tblpartnercontact);
                    db.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Partner contact already registered: " + tblpartnercontact.partner.name + ", " + tblpartnercontact.firstName + " " + tblpartnercontact.lastName);
                    ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartnercontact.partnerId);
                    ViewBag.personTitle = listTitle(tblpartnercontact.personTitle);
                    ViewBag.gender = listGender(tblpartnercontact.gender);
                    return View(tblpartnercontact);
                }
                return RedirectToAction("Index");
            }

            ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartnercontact.partnerId);
            ViewBag.personTitle = listTitle(tblpartnercontact.personTitle);
            ViewBag.gender = listGender(tblpartnercontact.gender);
            return View(tblpartnercontact);
        }

        // GET: /PartnerContact/Edit/5
        [Authorize(Roles = "Admin, CanEditPartnerContact, PartnerContact")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartnerContact tblpartnercontact = db.PartnerContactPersons.Find(id);
            if (tblpartnercontact == null)
            {
                return HttpNotFound();
            }
            ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartnercontact.partnerId);
            ViewBag.personTitle = listTitle(tblpartnercontact.personTitle);
            ViewBag.gender = listGender(tblpartnercontact.gender);
            return View(tblpartnercontact);
        }

        // POST: /PartnerContact/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanEditPartnerContact, PartnerContact")]
        public ActionResult Edit([Bind(Include = "contactId,partnerId,firstName,otherNames,lastName,gender,phoneNumber,emailAddress,contactAddress,contactCity,contactState,contactCountry,webAddress,geoLongitude,geoLatitude,personTitle")] PartnerContact tblpartnercontact)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());

            if (ModelState.IsValid)
            {
                //db.SaveChanges();
                try
                {
                    //db.Entry(tbllocationuser).State = EntityState.Modified;
                    var r = db.PartnerContactPersons.Where(c => c.contactId == tblpartnercontact.contactId).FirstOrDefault();
                    r.partnerId = tblpartnercontact.partnerId;
                    r.personTitle = tblpartnercontact.personTitle;
                    r.otherNames = tblpartnercontact.otherNames;
                    r.lastName = tblpartnercontact.lastName;
                    r.contactAddress = tblpartnercontact.contactAddress;
                    r.contactCity = tblpartnercontact.contactCity;
                    r.contactState = tblpartnercontact.contactState;
                    r.contactCountry = tblpartnercontact.contactCountry;

                    r.phoneNumber = tblpartnercontact.phoneNumber;
                    r.emailAddress = tblpartnercontact.emailAddress;
                    r.webAddress = tblpartnercontact.webAddress;
                    r.geoLongitude = tblpartnercontact.geoLongitude;
                    r.geoLatitude = tblpartnercontact.geoLatitude;
                    r.gender = tblpartnercontact.gender;

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
                    var databaseValues = (PartnerContact)entry.GetDatabaseValues().ToObject();
                    var clientValues = (PartnerContact)entry.Entity;
                    if (databaseValues.firstName != clientValues.firstName || databaseValues.lastName != clientValues.lastName)
                        ModelState.AddModelError("Partner Contact", "Current value: " + databaseValues.firstName + " " + databaseValues.lastName);
                    if (databaseValues.contactAddress != clientValues.contactAddress)
                        ModelState.AddModelError("Contact Address", "Current value: " + databaseValues.contactAddress);
                    if (databaseValues.emailAddress != clientValues.emailAddress)
                        ModelState.AddModelError("Email Address", "Current value: " + databaseValues.emailAddress);

                    ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                      + "was modified by another user after you got the original value. The "
                      + "edit operation was canceled and the current values in the database "
                      + "have been displayed. If you still want to edit this record, click "
                      + "the Save button again. Otherwise click the Back to List hyperlink.");

                    tblpartnercontact.Timestamp = databaseValues.Timestamp;
                    ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartnercontact.partnerId);
                    ViewBag.personTitle = listTitle(tblpartnercontact.personTitle);
                    ViewBag.gender = listGender(tblpartnercontact.gender);
                    return View();
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                    ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartnercontact.partnerId);
                    ViewBag.personTitle = listTitle(tblpartnercontact.personTitle);
                    ViewBag.gender = listGender(tblpartnercontact.gender);
                    return View();
                }
                return RedirectToAction("Index");
            }
            ViewBag.partnerId = new SelectList(db.Partners, "partnerId", "name", tblpartnercontact.partnerId);
            ViewBag.personTitle = listTitle(tblpartnercontact.personTitle);
            ViewBag.gender = listGender(tblpartnercontact.gender);
            return View(tblpartnercontact);
        }

        // GET: /PartnerContact/Delete/5
        [Authorize(Roles = "Admin, CanDeletePartnerContact, PartnerContact")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartnerContact tblpartnercontact = db.PartnerContactPersons.Find(id);
            if (tblpartnercontact == null)
            {
                return HttpNotFound();
            }
            return View(tblpartnercontact);
        }

        // POST: /PartnerContact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanDeletePartnerContact, PartnerContact")]
        public ActionResult DeleteConfirmed(long id)
        {
            PartnerContact tblpartnercontact = db.PartnerContactPersons.Find(id);
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
