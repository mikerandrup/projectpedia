using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectPediaWebAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Browse()
        {
            return View("~/Views/Home/Index.cshtml");
        }
    }
}
