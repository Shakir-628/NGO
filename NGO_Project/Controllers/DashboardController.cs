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

        private NGOEntities db = new NGOEntities();
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
            if (Session["UserId"] == null)
            {

                return RedirectToAction("Login", "Users");


                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.UtcNow.AddSeconds(-1));
                Response.Cache.SetNoStore();
            }
            return View();
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
            // Join AidRequests with Users on the UserId column to get the user (NGO) details.
            var aidRequestsWithUsers = (from ar in db.AidRequests
                                        join u in db.Users on ar.UserId equals u.UserId
                                        where ar.IsActive == true
                                        orderby ar.PostDate descending
                                        select new
                                        {
                                            AidRequest = ar,
                                            User = u
                                        }).ToList();

            // Pass the list of anonymous objects to the view.
            // The view will now access data through this new structure (e.g., item.AidRequest.RequestTitle).
            return View(aidRequestsWithUsers);
        }
        //public ActionResult Donor()
        //{
        //    if (Session["UserId"] == null)
        //    {

        //        return RedirectToAction("Login", "Users");
                   

        //        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //        Response.Cache.SetExpires(DateTime.UtcNow.AddSeconds(-1));
        //        Response.Cache.SetNoStore();
        //    }
        //    return View();
        //}
    }
}