using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NGO_Project;

namespace NGO_Project.Controllers
{
    public class AidRequestsController : Controller
    {
        private NGOEntities db = new NGOEntities();

        // GET: AidRequests
        public ActionResult Index()
        {
            var aidRequests = db.AidRequests.Include(a => a.RequestedItem);
            return View(aidRequests.ToList());
        }

        // GET: AidRequests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AidRequest aidRequest = db.AidRequests.Find(id);
            if (aidRequest == null)
            {
                return HttpNotFound();
            }
            return View(aidRequest);
        }

        // GET: AidRequests/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName");
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName");
            ViewBag.RequestId = new SelectList(db.RequestedItems, "RequestedItemId", "ItemName");
            return View();
        }

        // POST: AidRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RequestId,UserId,RequestTitle,Description,Category,UrgencyLevel,Location,PostDate,IsActive")] AidRequest aidRequest)
        {
            if (ModelState.IsValid)
            {
                db.AidRequests.Add(aidRequest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", aidRequest.UserId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", aidRequest.UserId);
            ViewBag.RequestId = new SelectList(db.RequestedItems, "RequestedItemId", "ItemName", aidRequest.RequestId);
            return View(aidRequest);
        }

        // GET: AidRequests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AidRequest aidRequest = db.AidRequests.Find(id);
            if (aidRequest == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", aidRequest.UserId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", aidRequest.UserId);
            ViewBag.RequestId = new SelectList(db.RequestedItems, "RequestedItemId", "ItemName", aidRequest.RequestId);
            return View(aidRequest);
        }

        // POST: AidRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RequestId,UserId,RequestTitle,Description,Category,UrgencyLevel,Location,PostDate,IsActive")] AidRequest aidRequest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aidRequest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", aidRequest.UserId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", aidRequest.UserId);
            ViewBag.RequestId = new SelectList(db.RequestedItems, "RequestedItemId", "ItemName", aidRequest.RequestId);
            return View(aidRequest);
        }

        // GET: AidRequests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AidRequest aidRequest = db.AidRequests.Find(id);
            if (aidRequest == null)
            {
                return HttpNotFound();
            }
            return View(aidRequest);
        }

        // POST: AidRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AidRequest aidRequest = db.AidRequests.Find(id);
            db.AidRequests.Remove(aidRequest);
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
