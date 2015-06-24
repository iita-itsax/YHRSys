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
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using DYMO.Label.Framework;
using System.Text.RegularExpressions;
using System.Drawing.Printing;
using System.Printing;
using System.Management;
using DYMO;

namespace YHRSys.Controllers
{
    [Authorize]
    public class VarietyProcessFlowController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        SelectedValue selVal = new SelectedValue();

        private List<SelectListItem> listQuality(int quality)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "--Select--", Value = "", Selected = selVal.checkForSelectedValue(0, quality) });
            items.Add(new SelectListItem { Text = "100%", Value = "100", Selected = selVal.checkForSelectedValue(100, quality) });
            items.Add(new SelectListItem { Text = "80%", Value = "80", Selected = selVal.checkForSelectedValue(80, quality) });
            items.Add(new SelectListItem { Text = "60%", Value = "60", Selected = selVal.checkForSelectedValue(60, quality) });
            items.Add(new SelectListItem { Text = "40%", Value = "40", Selected = selVal.checkForSelectedValue(40, quality) });
            items.Add(new SelectListItem { Text = "20%", Value = "20", Selected = selVal.checkForSelectedValue(20, quality) });
            return items;
        }
        // GET: /VarietyProcessFlow/
        public ActionResult Index(string sortOrder, string currentFilter, string currentStartDateFilter, string currentEndDateFilter, string searchString, DateTime? searchStartProcessDate, DateTime? searchEndProcessDate, int? page, string btnSubmit, string labeltype, long? copies)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.FormSortParm = sortOrder == "Form" ? "form_desc" : "Form";
            ViewBag.OiCNameSortParm = sortOrder == "OiC" ? "oicname_desc" : "OiC";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.RankSortParm = sortOrder == "Rank" ? "rank_desc" : "Rank";
            ViewBag.RankSortParm = sortOrder == "Quality" ? "quality_desc" : "Quality";
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
                case "quality_desc":
                    varietyProcessFlow = varietyProcessFlow.OrderByDescending(rg => rg.quality);
                    break;
                case "Quality":
                    varietyProcessFlow = varietyProcessFlow.OrderBy(rg => rg.quality);
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

            if (TempData["msg"] != null && TempData["msg"].ToString().Length>0) { ViewBag.Message = TempData["msg"].ToString(); Session.Clear(); }
            /*
            if (btnSubmit == "Print Labels!")
            {
                PrintLabelList(varietyProcessFlow.ToList(), copies, labeltype);
            }*/
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            //ViewBag.printers = new SelectList(GetPrinters());
            
            return View(varietyProcessFlow.ToPagedList(pageNumber, pageSize));
            //var tblvarietyprocessflows = db.VarietyProcessFlows;//.Include(t => t.tblVariety);
            //return View(tblvarietyprocessflows.ToList());
        }

        List<string> GetPrinters()
        {
            PrintServer localPrintServer = new PrintServer();
            PrintQueueCollection printQueues = localPrintServer.GetPrintQueues(new[] { EnumeratedPrintQueueTypes.Local, EnumeratedPrintQueueTypes.Connections });
            var printerList = (from printer in printQueues select printer.Name).ToList();

            return printerList;
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

            if (TempData["msg"] != null && TempData["msg"].ToString().Length>0) { ViewBag.Message = TempData["msg"].ToString(); Session.Clear(); }

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
            ViewBag.quality = listQuality(0);
            return View();
        }

        // POST: /VarietyProcessFlow/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanAddVarietyProcessFlow, VarietyProcessFlow")]
        public ActionResult Create([Bind(Include="varietyId,form,processDate,userId,rank,description,quality")] VarietyProcessFlow tblvarietyprocessflow)
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
            ViewBag.quality = listQuality(tblvarietyprocessflow.quality);
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
            ViewBag.quality = listQuality(tblvarietyprocessflow.quality);
            ViewBag.barcode = tblvarietyprocessflow.barcodeImageUrl;
            return View(tblvarietyprocessflow);
        }

        // POST: /VarietyProcessFlow/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanEditVarietyProcessFlow, VarietyProcessFlow")]
        public ActionResult Edit([Bind(Include="processId,varietyId,form,processDate,userId,rank,description,quality")] VarietyProcessFlow tblvarietyprocessflow)
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
                        act.quality = tblvarietyprocessflow.quality;
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
                        if (databaseValues.quality != clientValues.quality)
                            ModelState.AddModelError("Quality", "Current value: " + databaseValues.quality);
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
                ViewBag.quality = listQuality(tblvarietyprocessflow.quality);
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
                ViewBag.quality = listQuality(tblvarietyprocessflow.quality);
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

        // POST: /VarietyProcessFlow/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanViewVarietyProcessFlow, VarietyProcessFlow")]
        public ActionResult PrintDetails(long? processId, string copies, string btnSubmit, string labeltype)
        {
            try { 
                int intOutput;
                bool isInt = false;
                isInt = int.TryParse(copies, out intOutput);
                if (!isInt) {
                    intOutput = 1;
                }
                switch (btnSubmit)
                {
                        case "Generate Label!":
                            return GenerateReport(processId, intOutput, "label");
                        //case "Print Labels!":
                        //    return PrintLabel(processId, intOutput, labeltype);
                        default://Print Details code goes here
                            return GenerateReport(processId, intOutput, "detail");
                }
            }
            catch (Exception ex)
            {
                Response.Write("Last Error: " + ex.InnerException + " <br/>  <br/> " + ex.Message + " <br/>  <br/> " + ex.StackTrace);
                return null;
            }
        }

        //GENERATE REPORT HERE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenerateReport(long? id, long? copies, string target)
        {
            if (copies > 1)
            {
                var query = from pa in db.VarietyProcessFlows.Where(pa => pa.processId == id)
                            select new
                                                       {
                                                           barcode = pa.barcode
                                                       };

                List<CustomVarietyProcessFlow> customVPF = new List<CustomVarietyProcessFlow>();
                for (int i = 0; i < copies; i++)
                {
                    //query.Concat(query.Where(pa => pa.processId == id));
                    foreach (var q in query) { 
                        CustomVarietyProcessFlow cvpf = new CustomVarietyProcessFlow();
                        cvpf.barcode = q.barcode;
                        customVPF.Add(cvpf);
                    }
                }

                ReportDocument read = new ReportDocument();

                //if (target == "label")
                //    read.Load(Path.Combine(Server.MapPath("~/Content/Reports"), "BarcodeLabels.rpt"));
                //else
                read.Load(Path.Combine(Server.MapPath("~/Content/Reports"), "vpflow_details.rpt"));
                    //read.Load(Path.Combine(Server.MapPath("~/Content/Reports"), "VPFlowDetails.rpt"));

                if (query!=null)
                    read.SetDataSource(customVPF.ToArray());

                //read.SetParameterValue("copies", Convert.ToDecimal(copies));
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                try
                {
                    if (query != null)
                    {
                        Stream stream = read.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                        stream.Seek(0, SeekOrigin.Begin);
                        //if (target == "label")
                        //    return File(stream, "application/pdf", "BarcodeLabel.pdf");
                        //else
                         return File(stream, "application/pdf", "VPFlowDetailsRpt.pdf");
                    }
                    else
                    {
                        //string message = "No matching record(s) found for your query!";
                        TempData["msg"] = "No matching record(s) found using your query to the expected generate report!";

                        return RedirectToAction("Details", "VarietyProcessFlow", new { id });
                    }
                }
                catch (Exception ex)
                {
                    TempData["msg"] = "ERROR! " + ex.Message;
                    return RedirectToAction("Details", "VarietyProcessFlow", new { id });
                }
            }
            else {
                var vpf = (from pa in db.VarietyProcessFlows.AsEnumerable().Where(pa => pa.processId == id)
                           select new CustomVarietyProcessFlow
                           {
                               variety = pa.variety.varietyDefinition.name,
                               OiC = pa.user.FirstName + " " + pa.user.LastName,
                               form = pa.form,
                               processDate = pa.processDate.HasValue ? pa.processDate.Value.ToString("dd/MM/yyyy") : String.Empty,
                               rank = pa.rank,
                               barcode = pa.barcode,
                               barcodeImageUrl = Convert.ToString(pa.barcodeImageUrl),
                               description = pa.description
                           }).ToArray();
                ReportDocument read = new ReportDocument();

                //if (target == "label")
                //    read.Load(Path.Combine(Server.MapPath("~/Content/Reports"), "BarcodeLabels.rpt"));
                //else
                read.Load(Path.Combine(Server.MapPath("~/Content/Reports"), "vpflow_details.rpt"));
                read.SetDataSource(vpf);

                //read.SetParameterValue("copies", Convert.ToDecimal(copies));
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                try
                {
                    if (vpf != null)
                    {
                        Stream stream = read.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                        stream.Seek(0, SeekOrigin.Begin);
                        //if (target == "label")
                        //    return File(stream, "application/pdf", "BarcodeLabel.pdf");
                        //else
                        return File(stream, "application/pdf", "VPFlowDetailsRpt.pdf");
                    }
                    else
                    {
                        //string message = "No matching record(s) found for your query!";
                        TempData["msg"] = "No matching record(s) found using your query to the expected generate report!";

                        return RedirectToAction("Details", "VarietyProcessFlow", new { id });
                    }
                }
                catch (Exception ex)
                {
                    TempData["msg"] = "ERROR! " + ex.Message;
                    return RedirectToAction("Details", "VarietyProcessFlow", new { id });
                }
            }
        }

        //Deprecated Code blocks as the server side label printing could not get to work
        /*
        //Printing label copies from details page
        [ValidateAntiForgeryToken]
        public ActionResult PrintLabel(long? id, long? copies, string labeltype)
        {
            List<CustomVarietyProcessFlow> customVPF = new List<CustomVarietyProcessFlow>();
            var query = from pa in db.VarietyProcessFlows.Where(pa => pa.processId == id)
                        select new
                        {
                            barcode = pa.barcode,//Regex.Match(pa.barcode, @"\d+").Value
                            processId = pa.processId
                        };
            if(query==null){
                TempData["msg"] = "No matching record(s) found using your query to the expected generate report!";
                return RedirectToAction("Details", "VarietyProcessFlow", new { id });
            }

            if (copies > 1)
            {
                //Create a number of copies for the array of Variety Process Flow
                for (int i = 0; i < copies; i++)
                {
                    //Create a single array of Variety Process Flow
                    foreach (var q in query)
                    {
                        CustomVarietyProcessFlow cvpf = new CustomVarietyProcessFlow();
                        cvpf.barcode = q.barcode;//Regex.Match(q.barcode, @"\d+").Value;
                        cvpf.processId = q.processId;
                        customVPF.Add(cvpf);
                    }
                }
            }
            else {
                //Create a single array of Variety Process Flow
                foreach (var q in query)
                {
                    CustomVarietyProcessFlow cvpf = new CustomVarietyProcessFlow();
                    cvpf.barcode = q.barcode;//Regex.Match(q.barcode, @"\d+").Value;
                    cvpf.processId = q.processId;
                    customVPF.Add(cvpf);
                }
            }

            //var label = Label.Open(@"c:\\Users\\koraegbunam\\My Documents\\DYMO Label\\Labels\\Printed Labels\\TestLabe.label");//c:\\Documents and Settings\\All Users\\Documents\\DYMO Label\\Label\\TestLabel.label
            var label = Label.Open(Path.Combine(Server.MapPath("~/Content/Labels"), "Barcode25x13mm.label"));
            if (labeltype == "Barcode51x19mm")
                label = Label.Open(Path.Combine(Server.MapPath("~/Content/Labels"), "Barcode51x19mm.label"));

            if (customVPF.Count > 0)
            {
                int doublePrint = customVPF.Count / 2;
                //var record = labelSet.AddRecord();
                for (int i = 0; i < doublePrint; i++)
                {
                    if (labeltype == "Barcode25x13mm")
                    {
                        string vpfID = customVPF[i].processId.ToString();
                        label.SetObjectText("topLabel", vpfID.PadLeft(4, '0').ToString());
                        label.SetObjectText("topText", customVPF[i].barcode.ToString());
                        label.SetObjectText("bottomLabel", vpfID.PadLeft(4, '0').ToString());
                        label.SetObjectText("bottomText", customVPF[i].barcode.ToString());
                    }
                    else {
                        label.SetObjectText("Barcode", customVPF[i].barcode);
                    }
                    //Try and catch any printer thrown errors
                    try
                    {
                        label.Print("DYMO LabelWriter 4XL");
                    }
                    catch (Exception e)
                    {
                        TempData["msg"] = "Printer Error - " + e.Message;
                        return RedirectToAction("Details", "VarietyProcessFlow", new { id });
                    }
                }
                if ((customVPF.Count % 2) > 0) {
                    if (labeltype == "Barcode25x13mm")
                    {
                        string vpfID = customVPF[customVPF.Count-1].processId.ToString();
                        label.SetObjectText("topLabel", vpfID.PadLeft(4, '0').ToString());
                        label.SetObjectText("topText", customVPF[customVPF.Count - 1].barcode.ToString());
                        label.SetObjectText("bottomLabel", "");
                        label.SetObjectText("bottomText", "");
                    }
                    else
                    {
                        label.SetObjectText("Barcode", customVPF[customVPF.Count - 1].barcode);
                    }
                    //Try and catch any printer thrown errors
                    try
                    {
                        label.Print("DYMO LabelWriter 4XL");
                    }
                    catch (Exception e)
                    {
                        TempData["msg"] = "Printer Error - " + e.Message;
                        return RedirectToAction("Details", "VarietyProcessFlow", new { id });
                    }
                }                
            }
            return RedirectToAction("Details", "VarietyProcessFlow", new { id });
        }

        //Printing list of labels from the index page
        [ValidateAntiForgeryToken]
        public ActionResult PrintLabelList(List<VarietyProcessFlow> vpf, long? copies, string labeltype)
        {
            List<CustomVarietyProcessFlow> customVPF = new List<CustomVarietyProcessFlow>();

            if (vpf == null)
            {
                TempData["msg"] = "No matching record(s) found using your query to the expected generate report! NULL";
                return RedirectToAction("Index", "VarietyProcessFlow");
            }
            else if (vpf.Count() <= 0)
            {
                TempData["msg"] = "No matching record(s) found using your query to the expected generate report! EMPTY";
                return RedirectToAction("Index", "VarietyProcessFlow");
            }

            if (copies > 1)
            {
                //Create a number of copies for the array of Variety Process Flow
                for (int i = 0; i < copies; i++)
                {
                    //Create a single array of Variety Process Flow
                    foreach (var q in vpf)
                    {
                        CustomVarietyProcessFlow cvpf = new CustomVarietyProcessFlow();
                        cvpf.barcode = q.barcode;//Regex.Match(q.barcode, @"\d+").Value;
                        cvpf.processId = q.processId;
                        customVPF.Add(cvpf);
                    }
                }
            }
            else
            {
                //Create a single array of Variety Process Flow
                foreach (var q in vpf)
                {
                    CustomVarietyProcessFlow cvpf = new CustomVarietyProcessFlow();
                    cvpf.barcode = q.barcode;//Regex.Match(q.barcode, @"\d+").Value;
                    cvpf.processId = q.processId;
                    customVPF.Add(cvpf);
                }
            }

            //var label = Label.Open(@"c:\\Users\\koraegbunam\\My Documents\\DYMO Label\\Labels\\Printed Labels\\TestLabe.label");//c:\\Documents and Settings\\All Users\\Documents\\DYMO Label\\Label\\TestLabel.label
            var label = Label.Open(Path.Combine(Server.MapPath("~/Content/Labels"), "Barcode25x13mm.label"));
            if (labeltype == "Barcode51x19mm")
                label = Label.Open(Path.Combine(Server.MapPath("~/Content/Labels"), "Barcode51x19mm.label"));

            if (customVPF.Count > 0 && copies > 1)
            {
                int doublePrint = customVPF.Count / 2;
                //var record = labelSet.AddRecord();
                for (int i = 0; i < doublePrint; i++)
                {
                    if (labeltype == "Barcode25x13mm")
                    {
                        string vpfID = customVPF[i].processId.ToString();
                        label.SetObjectText("topLabel", vpfID.PadLeft(4, '0').ToString());
                        label.SetObjectText("topText", customVPF[i].barcode.ToString());
                        label.SetObjectText("bottomLabel", vpfID.PadLeft(4, '0').ToString());
                        label.SetObjectText("bottomText", customVPF[i].barcode.ToString());
                    }
                    else
                    {
                        label.SetObjectText("Barcode", customVPF[i].barcode);
                    }
                    //Try and catch any printer thrown errors
                    try
                    {
                        label.Print("DYMO LabelWriter 4XL");
                    }
                    catch (Exception e)
                    {
                        TempData["msg"] = "Printer Error - " + e.Message;
                        return RedirectToAction("Index", "VarietyProcessFlow");
                    }
                }
                if ((customVPF.Count % 2) > 0)
                {
                    if (labeltype == "Barcode25x13mm")
                    {
                        string vpfID = customVPF[customVPF.Count - 1].processId.ToString();
                        label.SetObjectText("topLabel", vpfID.PadLeft(4, '0').ToString());
                        label.SetObjectText("topText", customVPF[customVPF.Count - 1].barcode.ToString());
                        label.SetObjectText("bottomLabel", "");
                        label.SetObjectText("bottomText", "");
                    }
                    else
                    {
                        label.SetObjectText("Barcode", customVPF[customVPF.Count - 1].barcode);
                    }
                    //Try and catch any printer thrown errors
                    try
                    {
                        label.Print("DYMO LabelWriter 4XL");
                    }
                    catch (Exception e)
                    {
                        TempData["msg"] = "Printer Error - " + e.Message;
                        return RedirectToAction("Index", "VarietyProcessFlow");
                    }
                }
            }
            //printing one copy each for the records in the list
            else if (customVPF.Count > 0)
            {
                for (int i = 0; i < customVPF.Count; i++)
                {
                    if (labeltype == "Barcode25x13mm")
                    {
                        //Check mod of i for us to write on the topLabel of the multilabel type
                        if ((i % 2) == 0)
                        {
                            string vpfID = customVPF[i].processId.ToString();
                            label.SetObjectText("topLabel", vpfID.PadLeft(4, '0').ToString());
                            label.SetObjectText("topText", customVPF[i].barcode.ToString());
                        }
                        //Check if array index exist for us to write on the bottomLabel of the multilabel type
                        if (customVPF.Count > (i + 1))
                        {
                            string vpfID = customVPF[i + 1].processId.ToString();
                            label.SetObjectText("bottomLabel", vpfID.PadLeft(4, '0').ToString());
                            label.SetObjectText("bottomText", customVPF[i + 1].barcode.ToString());
                            i++;
                        }
                        else
                        {
                            label.SetObjectText("bottomLabel", "");
                            label.SetObjectText("bottomText", "");
                        }
                    }
                    else
                    {
                        label.SetObjectText("Barcode", customVPF[i].barcode);
                    }
                    //Try and catch any printer thrown errors
                    try
                    {
                        //DYMO.Label.Framework.Framework.GetLabelWriterPrinters();
                        label.Print("DYMO LabelWriter 4XL");
                    }
                    catch (Exception e)
                    {
                        TempData["msg"] = "Printer Error - " + e.Message;
                        return RedirectToAction("Index", "VarietyProcessFlow");
                    }
                }
            }
            else { TempData["msg"] = "No records to print!"; }
            return RedirectToAction("Index", "VarietyProcessFlow");
        }*/

        //Printing list of labels from the index page
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult IndexDataList(string searchString, DateTime? searchStartProcessDate, DateTime? searchEndProcessDate, long? copies, long? id = 0)
        {
            List<CustomVarietyProcessFlow> customVPF = new List<CustomVarietyProcessFlow>();
            var varietyProcessFlow = from r in db.VarietyProcessFlows select r;

            if (id == 0)
            {
                if (searchStartProcessDate != null)
                    searchStartProcessDate = DateTime.Parse(searchStartProcessDate.ToString());
                if (searchEndProcessDate != null)
                    searchEndProcessDate = DateTime.Parse(searchEndProcessDate.ToString());

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
            }
            else { 
                varietyProcessFlow = varietyProcessFlow.Where(rg => (rg.processId == id));
            }

            var vpf = varietyProcessFlow.ToList();

            if (vpf == null)
            {
                return Json("No matching record(s) found using your query to the expected generate report! NULL");
            }
            else if (vpf.Count() <= 0)
            {
                return Json("No matching record(s) found using your query to the expected generate report! EMPTY");
            }

            if (copies > 1)
            {
                //Create a number of copies for the array of Variety Process Flow
                for (int i = 0; i < copies; i++)
                {
                    //Create a single array of Variety Process Flow
                    foreach (var q in vpf)
                    {
                        CustomVarietyProcessFlow cvpf = new CustomVarietyProcessFlow();
                        cvpf.barcode = q.barcode;//Regex.Match(q.barcode, @"\d+").Value;
                        cvpf.processId = q.processId;
                        customVPF.Add(cvpf);
                    }
                }
            }
            else
            {
                //Create a single array of Variety Process Flow
                foreach (var q in vpf)
                {
                    CustomVarietyProcessFlow cvpf = new CustomVarietyProcessFlow();
                    cvpf.barcode = q.barcode;//Regex.Match(q.barcode, @"\d+").Value;
                    cvpf.processId = q.processId;
                    customVPF.Add(cvpf);
                }
            }
            return Json(customVPF);
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
