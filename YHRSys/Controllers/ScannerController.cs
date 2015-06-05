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
                int x = 0;
                if (Int32.TryParse(barcodeText, out x))
                    x = Int32.Parse(barcodeText);

                var varietyProcessFlow = from r in db.VarietyProcessFlows select r;
                varietyProcessFlow = varietyProcessFlow.Where(rg => rg.barcode.Contains(barcodeText) || rg.processId==x);
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