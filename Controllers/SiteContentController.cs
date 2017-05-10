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
using DropdownSelect.Models;
using PagedList;
using System.IO;

namespace YHRSys.Controllers
{
    [Authorize]
    public class SiteContentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /SiteContent/
        [Authorize(Roles = "Admin, CanViewSiteContents, SiteContent")]
        public ActionResult Index(string sortOrder, string currentFilter, string currentStartDateFilter, string currentEndDateFilter, string searchString, DateTime? searchStartDate, DateTime? searchEndDate, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.CreatedDateSortParm = sortOrder == "CreatedDate" ? "createddate_desc" : "CreatedDate";
            ViewBag.StatusSortParm = sortOrder == "Status" ? "status_desc" : "Status";
            ViewBag.CaptionSortParm = sortOrder == "Caption" ? "caption_desc" : "Caption";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (searchStartDate != null)
            {
                page = 1;
            }
            else
            {
                if (currentStartDateFilter != null)
                    searchStartDate = DateTime.Parse(currentStartDateFilter.ToString());
            }
            ViewBag.CurrentStartDateFilter = searchStartDate;

            if (searchEndDate != null)
            {
                page = 1;
            }
            else
            {
                if (currentEndDateFilter != null)
                    searchEndDate = DateTime.Parse(currentEndDateFilter.ToString());
            }
            ViewBag.CurrentEndDateFilter = searchEndDate;

            if (searchStartDate != null)
                searchStartDate = DateTime.Parse(searchStartDate.ToString());
            if (searchEndDate != null)
                searchEndDate = DateTime.Parse(searchEndDate.ToString());

            var sitecontents = from r in db.SiteContents select r;

            if (!String.IsNullOrEmpty(searchString))
            {
                if (searchStartDate != null && searchEndDate != null)
                {
                    sitecontents = sitecontents.Where(rg => (rg.user.LastName.Contains(searchString) || rg.user.FirstName.Contains(searchString)
                                          || rg.user.UserName.Contains(searchString) || rg.fullArticle.Contains(searchString)) && (rg.createdDate >= (DateTime)searchStartDate && rg.createdDate <= (DateTime)searchEndDate));
                }
                else if (searchStartDate != null)
                {
                    sitecontents = sitecontents.Where(rg => (rg.user.LastName.Contains(searchString) || rg.user.FirstName.Contains(searchString)
                                          || rg.user.UserName.Contains(searchString) || rg.fullArticle.Contains(searchString)) && (rg.createdDate == (DateTime)searchStartDate));
                }
                else if (searchEndDate != null)
                {
                    sitecontents = sitecontents.Where(rg => (rg.user.LastName.Contains(searchString) || rg.user.FirstName.Contains(searchString)
                                          || rg.user.UserName.Contains(searchString) || rg.fullArticle.Contains(searchString)) && (rg.createdDate == (DateTime)searchEndDate));
                }
                else
                {
                    sitecontents = sitecontents.Where(rg => rg.user.LastName.Contains(searchString) || rg.user.FirstName.Contains(searchString)
                                          || rg.user.UserName.Contains(searchString) || rg.fullArticle.Contains(searchString));
                }
            }
            else
            {
                if (searchStartDate != null && searchEndDate != null)
                {
                    sitecontents = sitecontents.Where(rg => (rg.createdDate >= (DateTime)searchStartDate && rg.createdDate <= (DateTime)searchEndDate));
                }
                else if (searchStartDate != null)
                {
                    sitecontents = sitecontents.Where(rg => rg.createdDate == (DateTime)searchStartDate);
                }
                else if (searchEndDate != null)
                {
                    sitecontents = sitecontents.Where(rg => rg.createdDate <= (DateTime)searchEndDate);
                }
            }

            switch (sortOrder)
            {
                case "name_desc":
                    sitecontents = sitecontents.OrderByDescending(rg => rg.user.LastName);
                    break;
                case "CreatedDate":
                    sitecontents = sitecontents.OrderBy(rg => rg.createdDate);
                    break;
                case "createddate_desc":
                    sitecontents = sitecontents.OrderByDescending(rg => rg.createdDate);
                    break;
                case "Status":
                    sitecontents = sitecontents.OrderBy(rg => rg.status);
                    break;
                case "status_desc":
                    sitecontents = sitecontents.OrderByDescending(rg => rg.createdDate);
                    break;
                case "Caption":
                    sitecontents = sitecontents.OrderBy(rg => rg.caption);
                    break;
                case "caption_desc":
                    sitecontents = sitecontents.OrderByDescending(rg => rg.caption);
                    break;
                default:  // Name ascending 
                    sitecontents = sitecontents.OrderBy(rg => rg.user.LastName);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(sitecontents.ToPagedList(pageNumber, pageSize));
        }

        // GET: /SiteContent/Details/5
        [Authorize(Roles = "Admin, CanViewSiteContents, SiteContent")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteContent sitecontent = db.SiteContents.Find(id);
            if (sitecontent == null)
            {
                return HttpNotFound();
            }
            return View(sitecontent);
        }

        // GET: /SiteContent/Create
        [Authorize(Roles = "Admin, CanAddSiteContents, SiteContent")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /SiteContent/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanAddSiteContents, SiteContent")]
        [ValidateInput(false)]
        //[AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([Bind(Include = "caption,summary,fullArticle,status")] SiteContent sitecontent)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var wal = db.SiteContents.FirstOrDefault(p => p.caption == sitecontent.caption);
                    if (wal == null && (sitecontent.caption != null || sitecontent.caption!=""))
                    {
                        var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                        var currentUser = manager.FindById(User.Identity.GetUserId());

                        if (currentUser != null){
                            sitecontent.createdBy = currentUser.UserName;
                            sitecontent.userId = currentUser.Id;
                        }else{
                            sitecontent.createdBy = User.Identity.Name;
                        }

                        sitecontent.createdDate = DateTime.Now;

                        db.SiteContents.Add(sitecontent);
                        db.SaveChanges();
                        return RedirectToAction("Index", "SiteContent");
                    }
                }
                else
                {
                    //ModelState.AddModelError(string.Empty, "Site Content already entered: " + sitecontent.caption + ". Or your empty caption field is not allowed!");
                    ViewBag.Error = "Site Content already entered: " + sitecontent.caption + ". Or your empty caption field is not allowed!";
                    return View(sitecontent);//sitecontent
                }

                return View(sitecontent);//sitecontent
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error occurred while saving record: " + ex.Message);
                ViewBag.Error = "Error occurred while saving record: " + ex.Message;
                return View(sitecontent);
            }
        }

        // GET: /SiteContent/Edit/5
        [Authorize(Roles = "Admin, CanEditSiteContents, SiteContent")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteContent sitecontent = db.SiteContents.Find(id);
            if (sitecontent == null)
            {
                return HttpNotFound();
            }

            return View(sitecontent);
        }

        // POST: /SiteContent/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanEditSiteContents, SiteContent")]
        [ValidateInput(false)]
        //[AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit([Bind(Include = "id,caption,summary,fullArticle,status")] SiteContent sitecontent)
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

                        var act = db.SiteContents.Where(c => c.id == sitecontent.id).FirstOrDefault();
                        act.fullArticle = sitecontent.fullArticle;
                        act.summary = sitecontent.summary;
                        act.caption = sitecontent.caption;
                        act.status = sitecontent.status;

                        if (currentUser != null)
                        {
                            act.updatedBy = currentUser.UserName;
                        }
                        else
                            act.updatedBy = User.Identity.Name;

                        act.updatedDate = DateTime.Now;

                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        var entry = ex.Entries.Single();
                        var databaseValues = (SiteContent)entry.GetDatabaseValues().ToObject();
                        var clientValues = (SiteContent)entry.Entity;
                        if (databaseValues.caption != clientValues.caption)
                            ModelState.AddModelError("Caption", "Current value: " + databaseValues.caption);
                        if (databaseValues.summary != clientValues.summary)
                            ModelState.AddModelError("Summary", "Current value: " + databaseValues.summary);
                        if (databaseValues.fullArticle != clientValues.fullArticle)
                            ModelState.AddModelError("Full Article", "Current value: " + databaseValues.fullArticle);
                        if (databaseValues.status != clientValues.status)
                            ModelState.AddModelError("Status", "Current value: " + databaseValues.status);

                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                          + "was modified by another user after you got the original value. The "
                          + "edit operation was canceled and the current values in the database "
                          + "have been displayed. If you still want to edit this record, click "
                          + "the Save button again. Otherwise click the Back to List hyperlink.");

                        sitecontent.Timestamp = databaseValues.Timestamp;
                        return View(sitecontent);
                    }
                    return RedirectToAction("Details", "SiteContent", new { id = sitecontent.id });
                }

                return View(sitecontent);
            }
            catch
            {
                return View(sitecontent);
            }
        }

        // GET: /SiteContent/Delete/5
        [Authorize(Roles = "Admin, CanDeleteSiteContents, SiteContent")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteContent sitecontent = db.SiteContents.Find(id);
            if (sitecontent == null)
            {
                return HttpNotFound();
            }
            return View(sitecontent);
        }

        // POST: /SiteContent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanDeleteSiteContents, SiteContent")]
        public ActionResult DeleteConfirmed(int id)
        {
            SiteContent sitecontent = db.SiteContents.Find(id);
            db.SiteContents.Remove(sitecontent);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UploadFromEditor(HttpPostedFileBase upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            string url; // url to return
            string message; // message to display (optional)

            using (Stream file = System.IO.File.Create(Path.Combine(Server.MapPath("~/Content/uploads"), upload.FileName)))
            {
                upload.InputStream.CopyTo(file);
            }

            url = Url.Content("~/Content/uploads/" + upload.FileName);

            // passing message success/failure
            message = "";

            // since it is an ajax request it requires this string
            string output = @"<html><body><script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" +
              url + "\", \"" + message + "\");</script></body></html>";
            return Content(output);
        }

        public ActionResult ImageBrowser()
        {
            var images = new List<string>();
            foreach (string image in Directory.GetFiles(Server.MapPath("~/Content/uploads")))
            {
                images.Add(new FileInfo(image).Name);
            }

            return View(images);
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
