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
    public class UsersController : Controller
    {
        private NGOEntities1 db = new NGOEntities1();

        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Donation).Include(u => u.UserType1);
            return View(users.ToList());
        }

        public ActionResult Login()
        {
            return View();
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost]
        public ActionResult Login(User user)
        {
            User users = db.Users.Where(x=>x.Username == user.Username && x.Password == user.Password).FirstOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index", "Home");
        }



        // GET: Users/Create
        public ActionResult Registration()
        {
            ViewBag.UserId = new SelectList(db.Donations, "DonationId", "Notes");
            ViewBag.UserType = new SelectList(db.UserTypes, "UsertypeId", "UserType1");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Include = "FirstName,LastName,Username,Email,PhoneNumber,Address,UserType,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Created_Date = DateTime.Now;
                user.Updated_Date = DateTime.Now;
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            ViewBag.UserId = new SelectList(db.Donations, "DonationId", "Notes", user.UserId);
            ViewBag.UserType = new SelectList(db.UserTypes, "UsertypeId", "UserType1", user.UserType);
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Donations, "DonationId", "Notes", user.UserId);
            ViewBag.UserType = new SelectList(db.UserTypes, "UsertypeId", "UserType1", user.UserType);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,FirstName,LastName,Username,Email,PhoneNumber,Address,UserType,Password,Created_Date,Updated_Date")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Donations, "DonationId", "Notes", user.UserId);
            ViewBag.UserType = new SelectList(db.UserTypes, "UsertypeId", "UserType1", user.UserType);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
