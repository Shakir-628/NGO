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
    public class InventoryItemsController : Controller
    {
        private NGOEntities db = new NGOEntities();

        // GET: InventoryItems
        public ActionResult Index()
        {
            var viewModel = new InventoryDashboardViewModel
            {
                NewItem = new InventoryItem(),
                InventoryItems = db.InventoryItems.ToList()
            };
            // pass units as distinct list from Category table
            ViewBag.Units = new SelectList(db.Categories.Where(x => x.CategoryName != null)
                                           .Select(c => c.Unit)
                                           .Distinct()
                                           .ToList());
            ViewBag.Categories = new SelectList(db.Categories.Where(x => x.CategoryName != null), "CategoryId", "CategoryName");
            return View(viewModel);
        }

        // POST: InventoryItems/Create (for AJAX)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InventoryDashboardViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (Session["UserId"] != null && !string.IsNullOrEmpty(Session["UserId"].ToString()))
                {
                    viewModel.NewItem.CreatedBy = Convert.ToInt16(Session["UserId"]);
                    viewModel.NewItem.QualityCheckStatus = true;
                    viewModel.NewItem.LastUpdated = DateTime.Now;

                    db.InventoryItems.Add(viewModel.NewItem);
                    db.SaveChanges();
                    // pass units as distinct list from Category table
                    ViewBag.Units = new SelectList(db.Categories.Where(x => x.CategoryName != null)
                                                   .Select(c => c.Unit)
                                                   .Distinct()
                                                   .ToList());
                    ViewBag.Categories = new SelectList(db.Categories.Where(x => x.CategoryName != null), "CategoryId", "CategoryName");
                    return Json(new { success = true });
                }
            }

            return Json(new { success = false, errors = "Invalid data submitted or user not logged in." });
        }

        // GET: InventoryItems/GetInventoryItems (Action for AJAX to refresh the table)
        public ActionResult GetInventoryItems()
        {
            var inventoryItems = db.InventoryItems.ToList();
            return PartialView("_InventoryTableRows", inventoryItems);
        }

        // GET: InventoryItems/GetSummaryData (Action for AJAX to refresh summary cards)
        public ActionResult GetSummaryData()
        {
            var inventoryItems = db.InventoryItems.ToList();
            var total = inventoryItems.Count();
            var available = inventoryItems.Count(i => i.Quantity > 5);
            var lowStock = inventoryItems.Count(i => i.Quantity <= 5 && i.Quantity > 0);

            // Return JSON data for the summary cards
            return Json(new { total = total, available = available, lowStock = lowStock }, JsonRequestBehavior.AllowGet);
        }

        // The other CRUD actions (Details, Edit, Delete) are included for completeness.

        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            InventoryItem inventoryItem = db.InventoryItems.Find(id);
            if (inventoryItem == null) return HttpNotFound();
            return View(inventoryItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ItemId,ItemName,Category,Quantity,Unit,QualityCheckStatus,ExpirationDate,LastUpdated,CreatedBy")] InventoryItem inventoryItem)
        {
            if (ModelState.IsValid)
            {
                if (Session["UserId"] != null && !string.IsNullOrEmpty(Session["UserId"].ToString()))
                {
                    inventoryItem.LastUpdated = DateTime.Now;
                    db.Entry(inventoryItem).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(inventoryItem);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            InventoryItem inventoryItem = db.InventoryItems.Find(id);
            if (inventoryItem == null) return HttpNotFound();
            return View(inventoryItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InventoryItem inventoryItem = db.InventoryItems.Find(id);
            db.InventoryItems.Remove(inventoryItem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCategory([Bind(Include = "CategoryName, Unit")] Category viewModel)
        {
            if (ModelState.IsValid)
            {
                if (Session["UserId"] != null && !string.IsNullOrEmpty(Session["UserId"].ToString()))
                {
                    var existingCategories = db.Categories.FirstOrDefault(x =>
                        x.CategoryName == viewModel.CategoryName);

                    if (existingCategories == null)
                    {
                        db.Categories.Add(viewModel);
                        db.SaveChanges();
                        // pass units as distinct list from Category table
                        ViewBag.Units = new SelectList(db.Categories.Where(x => x.CategoryName != null)
                                                       .Select(c => c.Unit)
                                                       .Distinct()
                                                       .ToList());
                        ViewBag.Categories = new SelectList(db.Categories.Where(x => x.CategoryName != null), "CategoryId", "CategoryName");
                        return Json(new { success = true, message = "Category created successfully!" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Category already exists." });
                    }
                }
            }

            return Json(new { success = false, message = "Invalid data submitted or user not logged in." });
        }
        public ActionResult GetUnitByCategory(int categoryId)
        {
            var unit = db.Categories
                         .Where(c => c.CategoryId == categoryId)
                         .Select(c => c.Unit)
                         .FirstOrDefault();

            return Json(new { unit = unit }, JsonRequestBehavior.AllowGet);
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