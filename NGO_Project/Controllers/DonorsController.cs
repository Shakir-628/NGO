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
    public class DonorsController : Controller
    {
        private NGOEntities db = new NGOEntities();

        // GET: Donors/Home
        public ActionResult Index()
        {
            int currentNGOId = Convert.ToInt32(Session["Userid"]);

            var donorDetails = db.Donors
                .Where(d => d.NGOId == currentNGOId)
                .OrderByDescending(d => d.CreatedDate)
                .ToList();

            return View(donorDetails);
        }

        // GET: Donors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donor donor = db.Donors.Find(id);
            if (donor == null)
            {
                return HttpNotFound();
            }
            return View(donor);
        }

        // GET: Donors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Donors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,FullName,Email,PhoneNumber,Location,PasswordHash,RegistrationDate")] Donor donor)
        {
            if (ModelState.IsValid)
            {
                donor.CreatedDate = DateTime.Now;
                db.Donors.Add(donor);
                db.SaveChanges();
                return RedirectToAction("Home");
            }

            return View(donor);
        }

        [HttpPost]
        public JsonResult SaveDonor(Donor model)
        {
            if (ModelState.IsValid)
            {
                // Save donor into CRM (DB)
                model.CreatedDate = DateTime.Now;
                db.Donors.Add(model);
                db.SaveChanges();

                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Invalid data." });
        }

        // GET: Donors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donor donor = db.Donors.Find(id);
            if (donor == null)
            {
                return HttpNotFound();
            }
            return View(donor);
        }

        // POST: Donors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,FullName,Email,PhoneNumber,Location,PasswordHash,RegistrationDate")] Donor donor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Home");
            }
            return View(donor);
        }

        // GET: Donors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donor donor = db.Donors.Find(id);
            if (donor == null)
            {
                return HttpNotFound();
            }
            return View(donor);
        }

        // POST: Donors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Donor donor = db.Donors.Find(id);
            db.Donors.Remove(donor);
            db.SaveChanges();
            return RedirectToAction("Home");
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