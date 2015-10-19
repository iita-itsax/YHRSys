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

using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Office.Tools;
using Microsoft.Office.Interop.Excel;
using System.IO;

namespace YHRSys.Controllers
{
    [Authorize]
    public class CummulativeReportingController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CummulativeReporting
        [Authorize(Roles = "Admin, CanViewCummulativeReport, PartnerReporting")]
        public ActionResult Index(string sortOrder, string currentFilter, string currentStartDateFilter, string currentEndDateFilter, string searchString, DateTime? searchStartActivityDate, DateTime? searchEndActivityDate, int? page, string btnSubmit)
        {
            //var partnerReportings = db.PartnerReportings.Include(p => p.activity);

            switch (btnSubmit)
            {
                case "Export Report!":
                    return ExportActivity(searchString, searchStartActivityDate, searchEndActivityDate);
                default:
                    ViewBag.CurrentSort = sortOrder;
                    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                    ViewBag.ReagentSortParm = sortOrder == "Reagent" ? "reagent_desc" : "Reagent";
                    ViewBag.VarietySortParm = sortOrder == "Variety" ? "variety_desc" : "Variety";
                    ViewBag.GiverSortParm = sortOrder == "Giver" ? "giver_desc" : "Giver";
                    ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                    ViewBag.OiCNameSortParm = sortOrder == "OiC" ? "oicnaame_desc" : "OiC";
                    ViewBag.QuantitySortParm = sortOrder == "Quantity" ? "quantity_desc" : "Quantity";
                    //ViewBag.VarQuantitySortParm = sortOrder == "VarQuantity" ? "varquantity_desc" : "VarQuantity";

                    if (searchString != null)
                    {
                        page = 1;
                    }
                    else
                    {
                        searchString = currentFilter;
                    }

                    ViewBag.CurrentFilter = searchString;

                    if (searchStartActivityDate != null)
                    {
                        page = 1;
                    }
                    else
                    {
                        if (currentStartDateFilter != null)
                            searchStartActivityDate = DateTime.Parse(currentStartDateFilter.ToString());
                    }
                    ViewBag.CurrentStartDateFilter = searchStartActivityDate;

                    if (searchEndActivityDate != null)
                    {
                        page = 1;
                    }
                    else
                    {
                        if (currentEndDateFilter != null)
                            searchEndActivityDate = DateTime.Parse(currentEndDateFilter.ToString());
                    }
                    ViewBag.CurrentEndDateFilter = searchEndActivityDate;

                    if (searchStartActivityDate != null)
                        searchStartActivityDate = DateTime.Parse(searchStartActivityDate.ToString());
                    if (searchEndActivityDate != null)
                        searchEndActivityDate = DateTime.Parse(searchEndActivityDate.ToString());

                    var partnerReportings = from r in db.PartnerReportings select r;
                    
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        if (searchStartActivityDate != null && searchEndActivityDate != null)
                        {
                            partnerReportings = partnerReportings.Where(rg => (rg.activity.giver.name.Contains(searchString)
                                               || rg.activity.partner.name.Contains(searchString)
                                               || rg.activity.variety.varietyDefinition.name.Contains(searchString)
                                               || rg.activity.reagent.name.Contains(searchString) || rg.activity.oic.LastName.Contains(searchString)
                                                  || rg.activity.oic.FirstName.Contains(searchString) || rg.activity.backStopping.Contains(searchString)) && (rg.reportDate >= (DateTime)searchStartActivityDate && rg.reportDate <= (DateTime)searchEndActivityDate));
                        }
                        else if (searchStartActivityDate != null)
                        {
                            partnerReportings = partnerReportings.Where(rg => (rg.activity.giver.name.Contains(searchString)
                                               || rg.activity.partner.name.Contains(searchString)
                                               || rg.activity.variety.varietyDefinition.name.Contains(searchString)
                                               || rg.activity.reagent.name.Contains(searchString) || rg.activity.oic.LastName.Contains(searchString)
                                                  || rg.activity.oic.FirstName.Contains(searchString) || rg.activity.backStopping.Contains(searchString)) && (rg.reportDate == (DateTime)searchStartActivityDate));
                        }
                        else if (searchEndActivityDate != null)
                        {
                            partnerReportings = partnerReportings.Where(rg => (rg.activity.giver.name.Contains(searchString)
                                               || rg.activity.partner.name.Contains(searchString)
                                               || rg.activity.variety.varietyDefinition.name.Contains(searchString)
                                               || rg.activity.reagent.name.Contains(searchString) || rg.activity.oic.LastName.Contains(searchString)
                                                  || rg.activity.oic.FirstName.Contains(searchString) || rg.activity.backStopping.Contains(searchString)) && (rg.reportDate == (DateTime)searchEndActivityDate));
                        }
                        else
                        {
                            partnerReportings = partnerReportings.Where(rg => rg.activity.giver.name.Contains(searchString)
                                               || rg.activity.partner.name.Contains(searchString)
                                               || rg.activity.variety.varietyDefinition.name.Contains(searchString)
                                               || rg.activity.reagent.name.Contains(searchString) || rg.activity.oic.LastName.Contains(searchString)
                                                  || rg.activity.oic.FirstName.Contains(searchString) || rg.activity.backStopping.Contains(searchString));
                        }
                    }
                    else
                    {
                        if (searchStartActivityDate != null && searchEndActivityDate != null)
                        {
                            partnerReportings = partnerReportings.Where(rg => (rg.reportDate >= (DateTime)searchStartActivityDate && rg.reportDate <= (DateTime)searchEndActivityDate));
                        }
                        else if (searchStartActivityDate != null)
                        {
                            partnerReportings = partnerReportings.Where(rg => rg.reportDate == (DateTime)searchStartActivityDate);
                        }
                        else if (searchEndActivityDate != null)
                        {
                            partnerReportings = partnerReportings.Where(rg => rg.reportDate <= (DateTime)searchEndActivityDate);
                        }
                    }

                    switch (sortOrder)
                    {
                        case "name_desc":
                            partnerReportings = partnerReportings.OrderByDescending(rg => rg.activity.partner.name);
                            break;
                        case "giver_desc":
                            partnerReportings = partnerReportings.OrderByDescending(rg => rg.activity.giver.name);
                            break;
                        case "Giver":
                            partnerReportings = partnerReportings.OrderBy(rg => rg.activity.giver.name);
                            break;
                        case "reagent_desc":
                            partnerReportings = partnerReportings.OrderByDescending(rg => rg.activity.reagent.name);
                            break;
                        case "Reagent":
                            partnerReportings = partnerReportings.OrderBy(rg => rg.activity.reagent.name);
                            break;
                        case "variety_desc":
                            partnerReportings = partnerReportings.OrderByDescending(rg => rg.activity.variety.varietyDefinition.name);
                            break;
                        case "Variety":
                            partnerReportings = partnerReportings.OrderBy(rg => rg.activity.variety.varietyDefinition.name);
                            break;
                        case "oicname_desc":
                            partnerReportings = partnerReportings.OrderByDescending(rg => rg.activity.oic.LastName).ThenBy(rg => rg.activity.oic.FirstName);
                            break;
                        case "OiC":
                            partnerReportings = partnerReportings.OrderBy(rg => rg.activity.oic.LastName).ThenBy(rg => rg.activity.oic.FirstName);
                            break;
                        case "quantity_desc":
                            partnerReportings = partnerReportings.OrderByDescending(rg => rg.reagentQty);
                            break;
                        case "Quantity":
                            partnerReportings = partnerReportings.OrderBy(rg => rg.reagentQty);
                            break;
                        case "Date":
                            partnerReportings = partnerReportings.OrderBy(rg => rg.reportDate);
                            break;
                        case "date_desc":
                            partnerReportings = partnerReportings.OrderByDescending(rg => rg.reportDate);
                            break;
                        default:  // Name ascending 
                            partnerReportings = partnerReportings.OrderBy(rg => rg.activity.partner.name);
                            break;
                    }
            
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(partnerReportings.ToPagedList(pageNumber, pageSize));
            }
            //return View(partnerReportings.ToList());
        }

        [Authorize(Roles = "Admin, CanViewCummulativeReport, PartnerReporting")]
        [HttpPost]
        public ActionResult ExportActivity(string searchString, DateTime? searchStartActivityDate, DateTime? searchEndActivityDate)
        {
            var partnerReportings = from r in db.PartnerReportings
                             join a in db.PartnerActivities on r.activityId equals a.partnerActivityId into aj
                             from a in aj.DefaultIfEmpty()
                             select new ActivityCummulativeReport
                             {
                                 ActivityDescr = r.activity.backStopping,
                                 ActivityDate = r.activity.activityDate,
                                 PartnerName = r.activity.partner.name,
                                 ReagentName = r.activity.reagent.name,
                                 ReagentQtyGiven = r.activity.reagentQty,
                                 VarietyName = r.activity.variety.varietyDefinition.name + "/" + r.activity.variety.sampleNumber,
                                 VarietyQtyGiven = r.activity.bioreactorplantsGiven + r.activity.seedsGiven + r.activity.tcPlantletsGiven + r.activity.tubersGiven,
                                 ActivityOicName = r.activity.oic.LastName + ", " + r.activity.oic.FirstName,
                                 UploadedBy = r.createdBy,
                                 SPQtyAvailable = r.spQty,
                                 BioRPQtyAvailable = r.bioRPQty,
                                 TCPQtyAvailable = r.tcpQty,
                                 TPQtyAvailable = r.tpQty,
                                 ReportDate = r.reportDate,
                                 ReportComment = r.comment
                             };
            //We load the data
            if (!String.IsNullOrEmpty(searchString))
                    {
                        if (searchStartActivityDate != null && searchEndActivityDate != null)
                        {
                            partnerReportings = partnerReportings.Where(rg => (rg.Activity.giver.name.Contains(searchString)
                                               || rg.Activity.partner.name.Contains(searchString)
                                               || rg.Activity.variety.varietyDefinition.name.Contains(searchString)
                                               || rg.Activity.reagent.name.Contains(searchString) || rg.Activity.oic.LastName.Contains(searchString)
                                                  || rg.Activity.oic.FirstName.Contains(searchString) || rg.Activity.backStopping.Contains(searchString)) && (rg.ReportDate >= (DateTime)searchStartActivityDate && rg.ReportDate <= (DateTime)searchEndActivityDate));
                        }
                        else if (searchStartActivityDate != null)
                        {
                            partnerReportings = partnerReportings.Where(rg => (rg.Activity.giver.name.Contains(searchString)
                                               || rg.Activity.partner.name.Contains(searchString)
                                               || rg.Activity.variety.varietyDefinition.name.Contains(searchString)
                                               || rg.Activity.reagent.name.Contains(searchString) || rg.Activity.oic.LastName.Contains(searchString)
                                                  || rg.Activity.oic.FirstName.Contains(searchString) || rg.Activity.backStopping.Contains(searchString)) && (rg.ReportDate == (DateTime)searchStartActivityDate));
                        }
                        else if (searchEndActivityDate != null)
                        {
                            partnerReportings = partnerReportings.Where(rg => (rg.Activity.giver.name.Contains(searchString)
                                               || rg.Activity.partner.name.Contains(searchString)
                                               || rg.Activity.variety.varietyDefinition.name.Contains(searchString)
                                               || rg.Activity.reagent.name.Contains(searchString) || rg.Activity.oic.LastName.Contains(searchString)
                                                  || rg.Activity.oic.FirstName.Contains(searchString) || rg.Activity.backStopping.Contains(searchString)) && (rg.ReportDate == (DateTime)searchEndActivityDate));
                        }
                        else
                        {
                            partnerReportings = partnerReportings.Where(rg => rg.Activity.giver.name.Contains(searchString)
                                               || rg.Activity.partner.name.Contains(searchString)
                                               || rg.Activity.variety.varietyDefinition.name.Contains(searchString)
                                               || rg.Activity.reagent.name.Contains(searchString) || rg.Activity.oic.LastName.Contains(searchString)
                                                  || rg.Activity.oic.FirstName.Contains(searchString) || rg.Activity.backStopping.Contains(searchString));
                        }
                    }
                    else
                    {
                        if (searchStartActivityDate != null && searchEndActivityDate != null)
                        {
                            partnerReportings = partnerReportings.Where(rg => (rg.ReportDate >= (DateTime)searchStartActivityDate && rg.ReportDate <= (DateTime)searchEndActivityDate));
                        }
                        else if (searchStartActivityDate != null)
                        {
                            partnerReportings = partnerReportings.Where(rg => rg.ReportDate == (DateTime)searchStartActivityDate);
                        }
                        else if (searchEndActivityDate != null)
                        {
                            partnerReportings = partnerReportings.Where(rg => rg.ReportDate <= (DateTime)searchEndActivityDate);
                        }
                    }

            GridView gv = new GridView();
            gv.DataSource = partnerReportings.ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            var fName = string.Format("PartnerActivityReportings-{0}", DateTime.Now.ToString("s"));
            Response.AddHeader("content-disposition", "attachment; filename=" + fName + ".xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

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
