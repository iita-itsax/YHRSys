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

namespace YHRSys.Controllers
{
    public class ReagentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: /Reagent/
        public ActionResult Index()
        {
            return View(db.Reagents.ToList());
        }

        // GET: /Reagent/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reagent tblreagent = db.Reagents.Find(id);
            if (tblreagent == null)
            {
                return HttpNotFound();
            }
            return View(tblreagent);
        }

        // GET: /Reagent/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Reagent/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="name,uom")] Reagent tblreagent)
        {
            if (ModelState.IsValid)
            {
                String user = Membership.GetUser().ProviderUserKey.ToString();
                //var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();

                //long userId = Convert.ToInt32(user);
                tblreagent.baseUserEntity.createdBy = user;
                tblreagent.baseDateEntity.createdDate = DateTime.Now;
                db.Reagents.Add(tblreagent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblreagent);
        }

        // GET: /Reagent/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reagent tblreagent = db.Reagents.Find(id);
            if (tblreagent == null)
            {
                return HttpNotFound();
            }
            return View(tblreagent);
        }

        // POST: /Reagent/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="reagentId,name,uom,createdBy")] Reagent tblreagent)
        {
            if (ModelState.IsValid)
            {
                //MembershipUser user = Membership.GetUser(User.Identity.Name);
                String user = Membership.GetUser().ProviderUserKey.ToString();
               // long userId = Convert.ToInt32(user.ProviderUserKey);
                tblreagent.baseUserEntity.updatedBy = user;
                tblreagent.baseDateEntity.updatedDate = DateTime.Now;
                db.Entry(tblreagent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblreagent);
        }

        // GET: /Reagent/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reagent tblreagent = db.Reagents.Find(id);
            if (tblreagent == null)
            {
                return HttpNotFound();
            }
            return View(tblreagent);
        }

        // POST: /Reagent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Reagent tblreagent = db.Reagents.Find(id);
            db.Reagents.Remove(tblreagent);
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
