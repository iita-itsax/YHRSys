using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using YHRSys.Models;

namespace YHRSys.Controllers
{
    public class AvailableVarietyController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AvailableVariety
        public ActionResult Index(string sortOrder, string currentFilter, string currentStartDateFilter, string currentEndDateFilter, string searchString, int? page)
        {
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

            var varieties = from r in db.Varieties select r;

            if (!String.IsNullOrEmpty(searchString))
            {
                varieties = varieties.Where(rg => rg.varietyDefinition.name.Contains(searchString)
                                          || rg.sampleNumber.Contains(searchString) || rg.user.LastName.Contains(searchString) || rg.user.FirstName.Contains(searchString)
                                           || rg.releaseStatus.Contains(searchString) || rg.resistance.Contains(searchString)
                                            || rg.stability.Contains(searchString) || rg.storability.Contains(searchString)
                                            || rg.species.name.Contains(searchString) || rg.uniformity.Contains(searchString)
                                             || rg.poundability.Contains(searchString) || rg.location.name.Contains(searchString)
                                              || rg.distinctness.Contains(searchString));
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

        // GET: AvailableVariety
        public ActionResult OrderNow()
        {
            return View();
        }

        public ActionResult AddToCart()
        {
            return View();
        }

        // GET: AvailableVariety
        public ActionResult RequestConfirmed()
        {
            return View();
        }
    }
}