using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShiTang.Controllers
{
    public class CompController : Controller
    {
        //
        // GET: /Comp/

        public ActionResult Index()
        {
            if (User.Identity.Name == "001DQSY")
            {
                ViewBag.Comp = "东汽实业";
            }
            else if (User.Identity.Name == "001JHSY")
            {
                ViewBag.Comp = "佳虹实业";
            }
            else if (User.Identity.Name == "001XEJ")
            {
                ViewBag.Comp = "肖恩记";
            }
            return View();
        }

    }
}
