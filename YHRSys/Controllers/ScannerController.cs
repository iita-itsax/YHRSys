using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YHRSys.Models;

namespace YHRSys.Controllers
{
    public class ScannerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //
        // GET: /Scanner/
        public ActionResult Index(string barcodeText)
        {
            if (barcodeText != null)
            {
                var varietyProcessFlow = from r in db.VarietyProcessFlows select r;
                varietyProcessFlow = varietyProcessFlow.Where(rg => rg.barcode.Contains(barcodeText));
                if (varietyProcessFlow == null)
                {
                    return HttpNotFound();
                }
                VarietyProcessFlow vpf = varietyProcessFlow.FirstOrDefault();
                if (vpf != null)
                    return RedirectToAction("Details", "VarietyProcessFlow", new { id = vpf.processId });
                else
                    ViewBag.Message = "No record found!";
            }
            return View();
        }
	}
}