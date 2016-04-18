using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShiTang.Controllers
{
    [Authorize(Users = "14546, 00114546,3001377,3002769")]
    public class ManageController : Controller
    {
        //
        // GET: /Manage/

        public ActionResult Index()
        {
            return View();
        }

    }
}
