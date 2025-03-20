using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GulfSymbolProject.Controllers
{
    public class ServicesController : Controller
    {
        // GET: Services
        public ActionResult Services()
        {
            ViewBag.NavClassServices = "active";

            return View();
        }
    }
}