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
using DropdownSelect.Models;


namespace YHRSys.Controllers
{
    [Authorize]
    public class VarietyController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        SelectedValue selVal = new SelectedValue();

        private List<SelectListItem> listReleaseStatus(string status)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "--Select--", Value = "", Selected = selVal.checkForSelectedValue("", status) });
            items.Add(new SelectListItem { Text = "Near Release", Value = "Near Release", Selected = selVal.checkForSelectedValue("Near Release", status) });
            items.Add(new SelectListItem { Text = "Released", Value = "Released", Selected = selVal.checkForSelectedValue("Released", status) });
            items.Add(new SelectListItem { Text = "Not Released", Value = "Not Released", Selected = selVal.checkForSelectedValue("Not Released", status) });
            return items;
        }

        // GET: /Variety/
        public ActionResult Index(string sortOrder, string currentFilter, string currentStartDateFilter, string currentEndDateFilter, string searchString, DateTime? searchStartTestDate, DateTime? searchEndTestDate, int? page)
        {
            //DateTime startDate, endDate;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.SampleNumSortParm = sortOrder == "SampleNum" ? "samplenum_desc" : "SampleNum";
            ViewBag.OiCNameSortParm = sortOrder == "OiC" ? "oicname_desc" : "OiC";
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


            if (searchStartTestDate != null)
            {
                page = 1;
            }
            else
            {
                if (currentStartDateFilter != null)
                    searchStartTestDate = DateTime.Parse(currentStartDateFilter.ToString());
            }
            ViewBag.CurrentStartDateFilter = searchStartTestDate;

            if (searchEndTestDate != null)
            {
                page = 1;
            }
            else
            {
                if(currentEndDateFilter != null)
                    searchEndTestDate = DateTime.Parse(currentEndDateFilter.ToString());
            }
            ViewBag.CurrentEndDateFilter = searchEndTestDate;

            if (searchStartTestDate != null)
                searchStartTestDate = DateTime.Parse(searchStartTestDate.ToString());
            if (searchEndTestDate != null)
                searchEndTestDate = DateTime.Parse(searchEndTestDate.ToString());

            var varieties = from r in db.Varieties select r;

            if (!String.IsNullOrEmpty(searchString))
            {
                if (searchStartTestDate != null && searchEndTestDate != null)
                {
                    varieties = varieties.Where(rg => (rg.varietyDefinition.name.Contains(searchString)
                                       || rg.sampleNumber.Contains(searchString) || rg.user.LastName.Contains(searchString) || rg.user.FirstName.Contains(searchString)
                                        || rg.releaseStatus.Contains(searchString) || rg.resistance.Contains(searchString)
                                         || rg.stability.Contains(searchString) || rg.storability.Contains(searchString)
                                         || rg.species.name.Contains(searchString) || rg.uniformity.Contains(searchString)
                                          || rg.poundability.Contains(searchString) || rg.location.name.Contains(searchString)
                                           || rg.distinctness.Contains(searchString)) && (rg.testDate >= (DateTime)searchStartTestDate && rg.testDate <= (DateTime)searchEndTestDate));
                }
                else if (searchStartTestDate != null)
                {
                    varieties = varieties.Where(rg => (rg.varietyDefinition.name.Contains(searchString)
                                          || rg.sampleNumber.Contains(searchString) || rg.user.LastName.Contains(searchString) || rg.user.FirstName.Contains(searchString)
                                           || rg.releaseStatus.Contains(searchString) || rg.resistance.Contains(searchString)
                                            || rg.stability.Contains(searchString) || rg.storability.Contains(searchString)
                                            || rg.species.name.Contains(searchString) || rg.uniformity.Contains(searchString)
                                             || rg.poundability.Contains(searchString) || rg.location.name.Contains(searchString)
                                              || rg.distinctness.Contains(searchString)) && (rg.testDate == (DateTime)searchStartTestDate));
                }
                else if (searchEndTestDate != null)
                {
                    varieties = varieties.Where(rg => (rg.varietyDefinition.name.Contains(searchString)
                                          || rg.sampleNumber.Contains(searchString) || rg.user.LastName.Contains(searchString) || rg.user.FirstName.Contains(searchString)
                                           || rg.releaseStatus.Contains(searchString) || rg.resistance.Contains(searchString)
                                            || rg.stability.Contains(searchString) || rg.storability.Contains(searchString)
                                            || rg.species.name.Contains(searchString) || rg.uniformity.Contains(searchString)
                                             || rg.poundability.Contains(searchString) || rg.location.name.Contains(searchString)
                                              || rg.distinctness.Contains(searchString)) && (rg.testDate == (DateTime)searchEndTestDate));
                }
                else
                {
                    varieties = varieties.Where(rg => rg.varietyDefinition.name.Contains(searchString)
                                          || rg.sampleNumber.Contains(searchString) || rg.user.LastName.Contains(searchString) || rg.user.FirstName.Contains(searchString)
                                           || rg.releaseStatus.Contains(searchString) || rg.resistance.Contains(searchString)
                                            || rg.stability.Contains(searchString) || rg.storability.Contains(searchString)
                                            || rg.species.name.Contains(searchString) || rg.uniformity.Contains(searchString)
                                             || rg.poundability.Contains(searchString) || rg.location.name.Contains(searchString)
                                              || rg.distinctness.Contains(searchString));
                }
            }
            else {
                if (searchStartTestDate != null && searchEndTestDate != null)
                {
                    varieties = varieties.Where(rg => (rg.testDate >= (DateTime)searchStartTestDate && rg.testDate <= (DateTime)searchEndTestDate));
                }
                else if (searchStartTestDate!=null)
                {
                    varieties = varieties.Where(rg => rg.testDate == (DateTime)searchStartTestDate);
                }
                else if (searchEndTestDate != null)
                {
                    varieties = varieties.Where(rg => rg.testDate == (DateTime)searchEndTestDate);
                }
            }
            
            switch (sortOrder)
            {
                case "name_desc":
                    varieties = varieties.OrderByDescending(rg => rg.varietyDefinition.name);
                    break;
                case "samplenum_desc":
                    varieties = varieties.OrderByDescending(rg => rg.sampleNumber);
                    break;
                case "SampleNum":
                    varieties = varieties.OrderBy(rg => rg.sampleNumber);
                    break;
                case "oicname_desc":
                    varieties = varieties.OrderByDescending(rg => rg.user.LastName).ThenBy(rg => rg.user.FirstName);
                    break;
                case "OiC":
                    varieties = varieties.OrderBy(rg => rg.user.LastName).ThenBy(rg => rg.user.FirstName);
                    break;
                case "Date":
                    varieties = varieties.OrderBy(rg => rg.createdDate);
                    break;
                case "date_desc":
                    varieties = varieties.OrderByDescending(rg => rg.createdDate);
                    break;
                default:  // Name ascending 
                    varieties = varieties.OrderBy(rg => rg.varietyDefinition.name);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(varieties.ToPagedList(pageNumber, pageSize));
        }

        // GET: /Variety/Details/5
        [Authorize(Roles = "Admin, CanViewVariety, Variety")]
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
        [Authorize(Roles = "Admin, CanAddVariety, Variety")]
        public ActionResult Create()
        {
            //ViewBag.activityId = new SelectList(db.Activities, "activityId", "name");
            ViewBag.varietyDefinitionId = new SelectList(db.VarietyDefinitions, "varietyDefinitionId", "name");
            ViewBag.locationId = new SelectList(db.Locations, "locationId", "name");
            ViewBag.specieId = new SelectList(db.Species, "specieId", "name");
            ViewBag.userId = new SelectList(db.Users, "Id", "FullName");
            ViewBag.releaseStatus = listReleaseStatus("");
            ViewBag.uomId = new SelectList(db.Measurements, "measurementId", "name");
            return View();
        }

        // POST: /Variety/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanAddVariety, Variety")]
        public ActionResult Create([Bind(Include="varietyDefinitionId,sampleNumber,userId,testDate,releaseStatus,resistance,storability,poundability,specieId,uniformity,stability,distinctness,value,locationId,availableQuantity,totalWeight,uomId")] Variety tblvariety)
        {
            if (ModelState.IsValid)
            {
                var med = db.Varieties.FirstOrDefault(p => p.varietyDefinitionId == tblvariety.varietyDefinitionId && p.sampleNumber == tblvariety.sampleNumber);
                if (med == null)
                {
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var currentUser = manager.FindById(User.Identity.GetUserId());

                    if (currentUser != null)
                        tblvariety.createdBy = currentUser.UserName;
                    else
                        tblvariety.createdBy = User.Identity.Name;

                    tblvariety.createdDate = DateTime.Now;
                    db.Varieties.Add(tblvariety);
                    db.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Variety already registered: " + med.varietyDefinition.name);
                    return View(tblvariety);
                }
                return RedirectToAction("Index");
            }

            //ViewBag.activityId = new SelectList(db.Activities, "activityId", "name", tblvariety.activityId);
            ViewBag.varietyDefinitionId = new SelectList(db.VarietyDefinitions, "varietyDefinitionId", "name", tblvariety.varietyDefinitionId);
            ViewBag.locationId = new SelectList(db.Locations, "locationId", "name", tblvariety.locationId);
            ViewBag.specieId = new SelectList(db.Species, "specieId", "name", tblvariety.specieId);
            ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblvariety.userId);
            ViewBag.releaseStatus = listReleaseStatus(tblvariety.releaseStatus);
            ViewBag.uomId = new SelectList(db.Measurements, "measurementId", "name");
            return View(tblvariety);
        }

        // GET: /Variety/Edit/5
        [Authorize(Roles = "Admin, CanEditVariety, Variety")]
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
            //ViewBag.activityId = new SelectList(db.Activities, "activityId", "name", tblvariety.activityId);
            ViewBag.varietyDefinitionId = new SelectList(db.VarietyDefinitions, "varietyDefinitionId", "name", tblvariety.varietyDefinitionId);
            ViewBag.locationId = new SelectList(db.Locations, "locationId", "name", tblvariety.locationId);
            ViewBag.specieId = new SelectList(db.Species, "specieId", "name", tblvariety.specieId);
            ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblvariety.userId);
            ViewBag.releaseStatus = listReleaseStatus(tblvariety.releaseStatus);
            ViewBag.uomId = new SelectList(db.Measurements, "measurementId", "name", tblvariety.uomId);
            return View(tblvariety);
        }

        // POST: /Variety/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanEditVariety, Variety")]
        public ActionResult Edit([Bind(Include = "varietyId,varietyDefinitionId,sampleNumber,userId,testDate,releaseStatus,resistance,storability,poundability,specieId,uniformity,stability,distinctness,value,locationId,availableQuantity,totalWeight,uomId")] Variety tblvariety)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //db.Entry(tbllocation).State = EntityState.Modified;
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var currentUser = manager.FindById(User.Identity.GetUserId());

                    var med = db.Varieties.Where(c => c.varietyId == tblvariety.varietyId).FirstOrDefault();
                    med.varietyDefinitionId = tblvariety.varietyDefinitionId;
                    med.sampleNumber = tblvariety.sampleNumber;
                    med.userId = tblvariety.userId;
                    med.testDate = tblvariety.testDate;
                    med.releaseStatus = tblvariety.releaseStatus;
                    med.resistance = tblvariety.resistance;
                    med.storability = tblvariety.storability;
                    med.poundability = tblvariety.poundability;
                    med.specieId = tblvariety.specieId;
                    med.uniformity = tblvariety.uniformity;
                    med.stability = tblvariety.stability;
                    med.distinctness = tblvariety.distinctness;
                    med.value = tblvariety.value;
                    med.locationId = tblvariety.locationId;

                    med.availableQuantity = tblvariety.availableQuantity;
                    med.totalWeight = tblvariety.totalWeight;
                    med.uomId = tblvariety.uomId;

                    if (currentUser != null)
                        med.updatedBy = currentUser.UserName;
                    else
                        med.updatedBy = User.Identity.Name;

                    med.updatedDate = DateTime.Now;

                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var databaseValues = (Variety)entry.GetDatabaseValues().ToObject();
                    var clientValues = (Variety)entry.Entity;
                    if (databaseValues.varietyDefinition.name != clientValues.varietyDefinition.name)
                        ModelState.AddModelError("Variety", "Current value: " + databaseValues.varietyDefinition.name);

                    ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                      + "was modified by another user after you got the original value. The "
                      + "edit operation was canceled and the current values in the database "
                      + "have been displayed. If you still want to edit this record, click "
                      + "the Save button again. Otherwise click the Back to List hyperlink.");

                    tblvariety.Timestamp = databaseValues.Timestamp;
                    return View();
                }
                return RedirectToAction("Index");
            }
            //ViewBag.activityId = new SelectList(db.Activities, "activityId", "name", tblvariety.activityId);
            ViewBag.varietyDefinitionId = new SelectList(db.VarietyDefinitions, "varietyDefinitionId", "name", tblvariety.varietyDefinitionId);
            ViewBag.locationId = new SelectList(db.Locations, "locationId", "name", tblvariety.locationId);
            ViewBag.specieId = new SelectList(db.Species, "specieId", "name", tblvariety.specieId);
            ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblvariety.userId);
            ViewBag.releaseStatus = listReleaseStatus(tblvariety.releaseStatus);
            ViewBag.uomId = new SelectList(db.Measurements, "measurementId", "name", tblvariety.uomId);
            return View(tblvariety);
        }

        // GET: /Variety/Delete/5
        [Authorize(Roles = "Admin, CanDeleteVariety, Variety")]
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
        [Authorize(Roles = "Admin, CanDeleteVariety, Variety")]
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
