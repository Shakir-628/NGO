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
            // Fetch all active aid requests and pass them to the view.
            var aidRequests = db.AidRequests.Where(r => r.IsActive == true).ToList();
            return View(aidRequests);
        }

        // POST: AidRequests/PostRequest
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostRequest([Bind(Include = "RequestTitle,Description,Category,UrgencyLevel,Location,RequestedItems")] AidRequest aidRequest, string itemsNeeded)
        {
            // Assuming UserId is retrieved from the logged-in session.
            // For this example, we'll use a hardcoded value.
            // In a real application, you'd get this from a User session object.
            aidRequest.UserId = 1; // Example: Set a hardcoded UserID

            if (ModelState.IsValid)
            {
                aidRequest.PostDate = DateTime.Now;
                aidRequest.IsActive = true; // Set to true for new requests

                // This part depends on how you handle requested items.
                // Assuming you have a separate table for RequestedItems.
                // You would need to split the 'itemsNeeded' string and create
                // new entries in the RequestedItems table, then associate them
                // with the new AidRequest.

                db.AidRequests.Add(aidRequest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // If the model is not valid, you might want to return an error or handle it gracefully.
            // For now, we'll just redirect to the index page.
            return RedirectToAction("Index");
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
            // Assuming your User entity has FirstName
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName");
            return View();
        }

        // POST: AidRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RequestId,UserId,RequestTitle,Description,Category,UrgencyLevel,Location,PostDate,IsActive")] AidRequest aidRequest)
        {
            if (ModelState.IsValid)
            {
                aidRequest.PostDate = DateTime.Now;
                db.AidRequests.Add(aidRequest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", aidRequest.UserId);
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
            return View(aidRequest);
        }

        // POST: AidRequests/Edit/5
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