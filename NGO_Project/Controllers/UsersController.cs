using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using NGO_Project;
using NGO_Project.Libs; // Import encryption helper

namespace NGO_Project.Controllers
{
    public class UsersController : Controller
    {
        private NGOEntities db = new NGOEntities();

        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users;
            return View(users.ToList());
        }

        // GET: Users/Login
        public ActionResult Login()
        {
            return View("Login");
        }

        // POST: Users/Login
        [HttpPost]
        public ActionResult Login(User user)
        {
            var encryptedPassword = Encryption.Encrypt(user.Password);

            var existingUser = db.Users
                .FirstOrDefault(x => x.Username == user.Username && x.Password == encryptedPassword);

            if (existingUser == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View("Login");
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            User user = db.Users.Find(id);
            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        // GET: Users/Registration
        public ActionResult Registration()
        {
            ViewBag.UserTypelist = new SelectList(db.UserTypes, "TypeId", "Type");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Include = "FirstName,LastName,Username,Email,PhoneNumber,Address,City,CNIC,Type,Password")] User user)
        {
            if (db.Users.Any(x => x.Username == user.Username))
                ModelState.AddModelError("Username", "Username already exists. Please choose another one.");

            if (db.Users.Any(x => x.Email == user.Email))
                ModelState.AddModelError("Email", "Email already exists. Please use another one.");

            if (!ModelState.IsValid)
            {
                ViewBag.UserTypelist = new SelectList(db.UserTypes, "TypeId", "Type", user.Type);
                return View(user);
            }

            user.Password = Encryption.Encrypt(user.Password);
            user.Created_Date = DateTime.Now;
            user.Updated_Date = DateTime.Now;

            db.Users.Add(user);
            try
            {
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                ViewBag.UserTypelist = new SelectList(db.UserTypes, "TypeId", "Type", user.Type);
                return View(user);
            }

            TempData["SuccessMessage"] = "Account created successfully!";
            return RedirectToAction("Login");
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            User user = db.Users.Find(id);
            if (user == null)
                return HttpNotFound();

            ViewBag.UserTypelist = new SelectList(db.UserTypes, "TypeId", "Type", user.Type);
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,FirstName,LastName,Username,Email,PhoneNumber,Address,City,CNIC,Type,Password,Created_Date,Updated_Date")] User user)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.UserTypelist = new SelectList(db.UserTypes, "TypeId", "Type", user.Type);
                return View(user);
            }

            user.Password = Encryption.Encrypt(user.Password);
            user.Updated_Date = DateTime.Now;
            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                ViewBag.UserTypelist = new SelectList(db.UserTypes, "TypeId", "Type", user.Type);
                return View(user);
            }

            return RedirectToAction("Index");
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            User user = db.Users.Find(id);
            if (user == null)
                return HttpNotFound();

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
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}
