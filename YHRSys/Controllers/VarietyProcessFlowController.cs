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
using BarCode.Models;
using DropdownSelect.Models;
using PagedList;

namespace YHRSys.Controllers
{
    [Authorize]
    public class VarietyProcessFlowController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        SelectedValue selVal = new SelectedValue();

        // GET: /VarietyProcessFlow/
        public ActionResult Index(string sortOrder, string currentFilter, string currentStartDateFilter, string currentEndDateFilter, string searchString, DateTime? searchStartProcessDate, DateTime? searchEndProcessDate, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.FormSortParm = sortOrder == "Form" ? "form_desc" : "Form";
            ViewBag.OiCNameSortParm = sortOrder == "OiC" ? "oicname_desc" : "OiC";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.RankSortParm = sortOrder == "Rank" ? "rank_desc" : "Rank";
            ViewBag.BarcodeSortParm = sortOrder == "Barcode" ? "barcode_desc" : "Barcode";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            if (searchStartProcessDate != null)
            {
                page = 1;
            }
            else
            {
                if (currentStartDateFilter != null)
                    searchStartProcessDate = DateTime.Parse(currentStartDateFilter.ToString());
            }
            ViewBag.CurrentStartDateFilter = searchStartProcessDate;

            if (searchEndProcessDate != null)
            {
                page = 1;
            }
            else
            {
                if (currentEndDateFilter != null)
                    searchEndProcessDate = DateTime.Parse(currentEndDateFilter.ToString());
            }
            ViewBag.CurrentEndDateFilter = searchEndProcessDate;

            if (searchStartProcessDate != null)
                searchStartProcessDate = DateTime.Parse(searchStartProcessDate.ToString());
            if (searchEndProcessDate != null)
                searchEndProcessDate = DateTime.Parse(searchEndProcessDate.ToString());

            var varietyProcessFlow = from r in db.VarietyProcessFlows select r;

            if (!String.IsNullOrEmpty(searchString))
            {
                if (searchStartProcessDate != null && searchEndProcessDate != null)
                {
                    varietyProcessFlow = varietyProcessFlow.Where(rg => (rg.variety.varietyDefinition.name.Contains(searchString)
                                       || rg.variety.sampleNumber.Contains(searchString) || rg.form.Contains(searchString) || rg.user.LastName.Contains(searchString) || rg.user.FirstName.Contains(searchString)
                                        || rg.rank.Contains(searchString) || rg.barcode.Contains(searchString)
                                         || rg.description.Contains(searchString)) && (rg.processDate >= (DateTime)searchStartProcessDate && rg.processDate <= (DateTime)searchEndProcessDate));
                }
                else if (searchStartProcessDate != null)
                {
                    varietyProcessFlow = varietyProcessFlow.Where(rg => (rg.variety.varietyDefinition.name.Contains(searchString)
                                          || rg.variety.sampleNumber.Contains(searchString) || rg.form.Contains(searchString) || rg.user.LastName.Contains(searchString) || rg.user.FirstName.Contains(searchString)
                                        || rg.rank.Contains(searchString) || rg.barcode.Contains(searchString)
                                         || rg.description.Contains(searchString)) && (rg.processDate == (DateTime)searchStartProcessDate));
                }
                else if (searchEndProcessDate != null)
                {
                    varietyProcessFlow = varietyProcessFlow.Where(rg => (rg.variety.varietyDefinition.name.Contains(searchString)
                                          || rg.variety.sampleNumber.Contains(searchString) || rg.form.Contains(searchString) || rg.user.LastName.Contains(searchString) || rg.user.FirstName.Contains(searchString)
                                        || rg.rank.Contains(searchString) || rg.barcode.Contains(searchString)
                                         || rg.description.Contains(searchString)) && (rg.processDate == (DateTime)searchEndProcessDate));
                }
                else
                {
                    varietyProcessFlow = varietyProcessFlow.Where(rg => rg.variety.varietyDefinition.name.Contains(searchString)
                                          || rg.variety.sampleNumber.Contains(searchString) || rg.form.Contains(searchString) || rg.user.LastName.Contains(searchString) || rg.user.FirstName.Contains(searchString)
                                        || rg.rank.Contains(searchString) || rg.barcode.Contains(searchString)
                                         || rg.description.Contains(searchString));
                }
            }
            else
            {
                if (searchStartProcessDate != null && searchEndProcessDate != null)
                {
                    varietyProcessFlow = varietyProcessFlow.Where(rg => (rg.processDate >= (DateTime)searchStartProcessDate && rg.processDate <= (DateTime)searchEndProcessDate));
                }
                else if (searchStartProcessDate != null)
                {
                    varietyProcessFlow = varietyProcessFlow.Where(rg => rg.processDate == (DateTime)searchStartProcessDate);
                }
                else if (searchEndProcessDate != null)
                {
                    varietyProcessFlow = varietyProcessFlow.Where(rg => rg.processDate == (DateTime)searchEndProcessDate);
                }
            }

            switch (sortOrder)
            {
                case "name_desc":
                    varietyProcessFlow = varietyProcessFlow.OrderByDescending(rg => rg.variety.sampleNumber).ThenBy(rg => rg.variety.varietyDefinition.name);
                    break;
                case "form_desc":
                    varietyProcessFlow = varietyProcessFlow.OrderByDescending(rg => rg.form);
                    break;
                case "Form":
                    varietyProcessFlow = varietyProcessFlow.OrderBy(rg => rg.form);
                    break;
                case "oicname_desc":
                    varietyProcessFlow = varietyProcessFlow.OrderByDescending(rg => rg.user.LastName).ThenBy(rg => rg.user.FirstName);
                    break;
                case "OiC":
                    varietyProcessFlow = varietyProcessFlow.OrderBy(rg => rg.user.LastName).ThenBy(rg => rg.user.FirstName);
                    break;
                case "Date":
                    varietyProcessFlow = varietyProcessFlow.OrderBy(rg => rg.processDate);
                    break;
                case "date_desc":
                    varietyProcessFlow = varietyProcessFlow.OrderByDescending(rg => rg.processDate);
                    break;
                case "rank_desc":
                    varietyProcessFlow = varietyProcessFlow.OrderByDescending(rg => rg.rank);
                    break;
                case "Rank":
                    varietyProcessFlow = varietyProcessFlow.OrderBy(rg => rg.rank);
                    break;
                case "barcode_desc":
                    varietyProcessFlow = varietyProcessFlow.OrderByDescending(rg => rg.barcode);
                    break;
                case "Barcode":
                    varietyProcessFlow = varietyProcessFlow.OrderBy(rg => rg.barcode);
                    break;
                default:  // Name ascending 
                    varietyProcessFlow = varietyProcessFlow.OrderBy(rg => rg.variety.sampleNumber).ThenBy(rg => rg.variety.varietyDefinition.name);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(varietyProcessFlow.ToPagedList(pageNumber, pageSize));
            //var tblvarietyprocessflows = db.VarietyProcessFlows;//.Include(t => t.tblVariety);
            //return View(tblvarietyprocessflows.ToList());
        }

        // GET: /VarietyProcessFlow/Details/5
        [Authorize(Roles = "Admin, CanViewVarietyProcessFlow, VarietyProcessFlow")]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VarietyProcessFlow tblvarietyprocessflow = db.VarietyProcessFlows.Find(id);
            if (tblvarietyprocessflow == null)
            {
                return HttpNotFound();
            }
            ViewBag.barcode = tblvarietyprocessflow.barcodeImageUrl;
            return View(tblvarietyprocessflow);
        }

        private List<SelectListItem> listForm(string form)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "--Select--", Value = "" });
            items.Add(new SelectListItem { Text = "Bioreactor", Value = "Bioreactor", Selected = selVal.checkForSelectedValue("Bioreactor", form) });
            items.Add(new SelectListItem { Text = "Tube", Value = "Tube", Selected = selVal.checkForSelectedValue("Tube", form) });
            items.Add(new SelectListItem { Text = "Tuber", Value = "Tuber", Selected = selVal.checkForSelectedValue("Tuber", form) });
            return items;
        }

        private List<SelectListItem> listRank(string rank)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "--Select--", Value = "" });
            items.Add(new SelectListItem { Text = "1", Value = "1", Selected = selVal.checkForSelectedValue("1", rank) });
            items.Add(new SelectListItem { Text = "2", Value = "2", Selected = selVal.checkForSelectedValue("2", rank) });
            items.Add(new SelectListItem { Text = "3", Value = "3", Selected = selVal.checkForSelectedValue("3", rank) });
            return items;
        }


        // GET: /VarietyProcessFlow/Create
        [Authorize(Roles = "Admin, CanAddVarietyProcessFlow, VarietyProcessFlow")]
        public ActionResult Create()
        {
            var varieties = db.Varieties.ToList()
            .Select(v => new
            {
                varietyId = v.varietyId,
                description = string.Format("{0}--{1}", v.varietyDefinition.name, v.sampleNumber)
            });
            ViewBag.varietyId = new SelectList(varieties, "varietyId", "description");
            ViewBag.userId = new SelectList(db.Users, "Id", "FullName");
            ViewBag.form = listForm(null);
            ViewBag.rank = listRank(null);
            return View();
        }

        // POST: /VarietyProcessFlow/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanAddVarietyProcessFlow, VarietyProcessFlow")]
        public ActionResult Create([Bind(Include="varietyId,form,processDate,userId,rank,description")] VarietyProcessFlow tblvarietyprocessflow)
        {
            if (ModelState.IsValid)
            {
                var locuser = db.VarietyProcessFlows.FirstOrDefault(p => p.varietyId == tblvarietyprocessflow.varietyId && p.barcode == tblvarietyprocessflow.barcode);
                if (locuser == null)
                {
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var currentUser = manager.FindById(User.Identity.GetUserId());
                    barcodecs objbar = new barcodecs();

                    if (currentUser != null)
                        tblvarietyprocessflow.createdBy = currentUser.UserName;
                    else
                        tblvarietyprocessflow.createdBy = User.Identity.Name;

                    tblvarietyprocessflow.createdDate = DateTime.Now;

                    //Get variety entity with entity Id
                    var variety = db.Varieties.FirstOrDefault(v => v.varietyId == tblvarietyprocessflow.varietyId);
                    if(variety==null){
                        ModelState.AddModelError(string.Empty, "Selected Variety could not be located!");
                        return View(tblvarietyprocessflow);
                    }
                    tblvarietyprocessflow.barcode = objbar.generateBarcode(variety.varietyDefinition.name, variety.sampleNumber, 
                        tblvarietyprocessflow.rank);
                    //
                    tblvarietyprocessflow.barcodeImageUrl = objbar.getBarcodeImage(objbar.generateBarcode(variety.varietyDefinition.name, 
                        variety.sampleNumber, tblvarietyprocessflow.rank),
                        variety.varietyDefinition.name + variety.sampleNumber);

                    db.VarietyProcessFlows.Add(tblvarietyprocessflow);
                    db.SaveChanges();
                }
                else
                {
                    var user = db.VarietyProcessFlows.SingleOrDefault(p => p.userId == tblvarietyprocessflow.userId);
                    ModelState.AddModelError(string.Empty, "Variety process flow already added for variety: " + tblvarietyprocessflow.FullIdentifier + " and by OiC: " + tblvarietyprocessflow.user.FullName);
                    return View(tblvarietyprocessflow);
                }
                return RedirectToAction("Index");
            }

            var varieties = db.Varieties.ToList()
            .Select(v => new
            {
                varietyId = v.varietyId,
                description = string.Format("{0}--{1}", v.varietyDefinition.name, v.sampleNumber)
            });

            ViewBag.varietyId = new SelectList(varieties, "varietyId", "description", tblvarietyprocessflow.varietyId);
            ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblvarietyprocessflow.userId);
            ViewBag.form = listForm(tblvarietyprocessflow.form);
            ViewBag.rank = listRank(tblvarietyprocessflow.rank);
            return View(tblvarietyprocessflow);
        }

        // GET: /VarietyProcessFlow/Edit/5
        [Authorize(Roles = "Admin, CanEditVarietyProcessFlow, VarietyProcessFlow")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VarietyProcessFlow tblvarietyprocessflow = db.VarietyProcessFlows.Find(id);
            if (tblvarietyprocessflow == null)
            {
                return HttpNotFound();
            }
            var varieties = db.Varieties.ToList()
            .Select(v => new
            {
                varietyId = v.varietyId,
                description = string.Format("{0}--{1}", v.varietyDefinition.name, v.sampleNumber)
            });

            ViewBag.varietyId = new SelectList(varieties, "varietyId", "description", tblvarietyprocessflow.varietyId);
            ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblvarietyprocessflow.userId);
            ViewBag.form = listForm(tblvarietyprocessflow.form);
            ViewBag.rank = listRank(tblvarietyprocessflow.rank);
            ViewBag.barcode = tblvarietyprocessflow.barcodeImageUrl;
            return View(tblvarietyprocessflow);
        }

        // POST: /VarietyProcessFlow/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanEditVarietyProcessFlow, VarietyProcessFlow")]
        public ActionResult Edit([Bind(Include="processId,varietyId,form,processDate,userId,rank,description")] VarietyProcessFlow tblvarietyprocessflow)
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
                        barcodecs objbar = new barcodecs();

                        var act = db.VarietyProcessFlows.Where(c => c.processId == tblvarietyprocessflow.processId).FirstOrDefault();
                        act.varietyId = tblvarietyprocessflow.varietyId;
                        act.form = tblvarietyprocessflow.form;
                        act.processDate = tblvarietyprocessflow.processDate;
                        act.userId = tblvarietyprocessflow.userId;
                        act.rank = tblvarietyprocessflow.rank;
                        act.description = tblvarietyprocessflow.description;

                        if (currentUser != null)
                            act.updatedBy = currentUser.UserName;
                        else
                            act.updatedBy = User.Identity.Name;

                        act.updatedDate = DateTime.Now;

                        //Get variety entity with entity Id
                        var variety = db.Varieties.FirstOrDefault(v => v.varietyId == tblvarietyprocessflow.varietyId);
                        if (variety == null)
                        {
                            ModelState.AddModelError(string.Empty, "Selected Variety could not be located!");
                            return View(tblvarietyprocessflow);
                        }
                        act.barcode = objbar.generateBarcode(variety.varietyDefinition.name, variety.sampleNumber,
                            tblvarietyprocessflow.rank);
                        //
                        act.barcodeImageUrl = objbar.getBarcodeImage(objbar.generateBarcode(variety.varietyDefinition.name,
                            variety.sampleNumber, tblvarietyprocessflow.rank),
                            variety.varietyDefinition.name + variety.sampleNumber);

                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        var entry = ex.Entries.Single();
                        var databaseValues = (VarietyProcessFlow)entry.GetDatabaseValues().ToObject();
                        var clientValues = (VarietyProcessFlow)entry.Entity;
                        if (databaseValues.barcode != clientValues.barcode)
                            ModelState.AddModelError("Barcode", "Current value: " + databaseValues.barcode);
                        if (databaseValues.form != clientValues.form)
                            ModelState.AddModelError("Form", "Current value: " + databaseValues.form);
                        if (databaseValues.rank != clientValues.rank)
                            ModelState.AddModelError("Rank", "Current value: " + databaseValues.rank);
                        if (databaseValues.processDate != clientValues.processDate)
                            ModelState.AddModelError("Process Date", "Current value: " + databaseValues.processDate);

                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                          + "was modified by another user after you got the original value. The "
                          + "edit operation was canceled and the current values in the database "
                          + "have been displayed. If you still want to edit this record, click "
                          + "the Save button again. Otherwise click the Back to List hyperlink.");

                        tblvarietyprocessflow.Timestamp = databaseValues.Timestamp;
                        return View();
                    }
                    return RedirectToAction("Index");
                }
                var varieties = db.Varieties.ToList()
                .Select(v => new
                {
                    varietyId = v.varietyId,
                    description = string.Format("{0}--{1}", v.varietyDefinition.name, v.sampleNumber)
                });
                ViewBag.varietyId = new SelectList(varieties, "varietyId", "description", tblvarietyprocessflow.varietyId);
                ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblvarietyprocessflow.userId);
                ViewBag.form = listForm(tblvarietyprocessflow.form);
                ViewBag.rank = listRank(tblvarietyprocessflow.rank);
                ViewBag.barcode = tblvarietyprocessflow.barcodeImageUrl;
                return View(tblvarietyprocessflow);
            }
            catch
            {
                var varieties = db.Varieties.ToList()
                .Select(v => new
                {
                    varietyId = v.varietyId,
                    description = string.Format("{0}--{1}", v.varietyDefinition.name, v.sampleNumber)
                });
                ViewBag.varietyId = new SelectList(varieties, "varietyId", "description", tblvarietyprocessflow.varietyId);
                ViewBag.userId = new SelectList(db.Users, "Id", "FullName", tblvarietyprocessflow.userId);
                ViewBag.form = listForm(tblvarietyprocessflow.form);
                ViewBag.rank = listRank(tblvarietyprocessflow.rank);
                ViewBag.barcode = tblvarietyprocessflow.barcodeImageUrl;
                return View();
            }
        }

        // GET: /VarietyProcessFlow/Delete/5
        [Authorize(Roles = "Admin, CanDeleteVarietyProcessFlow, VarietyProcessFlow")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VarietyProcessFlow tblvarietyprocessflow = db.VarietyProcessFlows.Find(id);
            if (tblvarietyprocessflow == null)
            {
                return HttpNotFound();
            }
            ViewBag.barcode = tblvarietyprocessflow.barcodeImageUrl;
            return View(tblvarietyprocessflow);
        }

        // POST: /VarietyProcessFlow/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanDeleteVarietyProcessFlow, VarietyProcessFlow")]
        public ActionResult DeleteConfirmed(long id)
        {
            VarietyProcessFlow tblvarietyprocessflow = db.VarietyProcessFlows.Find(id);
            db.VarietyProcessFlows.Remove(tblvarietyprocessflow);
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
