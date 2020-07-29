using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mBikeEcommerce.Controllers
{
    [Authorize( Roles = "admin" )]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EditAccounts()
        {
            return View();
        }
    }
}