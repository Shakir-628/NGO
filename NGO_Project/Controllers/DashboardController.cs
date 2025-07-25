using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NGO_Project.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            if (Session["UserId"] == null) 
            {

                return RedirectToAction("Login","Users");
            }
            return View();
        }

        public ActionResult NGO()
        {
            return View("NGO");
        }
        public ActionResult Donor()
        {
            if (Session["UserId"] == null)
            {

                return RedirectToAction("Login", "Users");
                   

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.UtcNow.AddSeconds(-1));
                Response.Cache.SetNoStore();
            }
            return View();
        }
    }
}