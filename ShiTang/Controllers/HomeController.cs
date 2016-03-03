using ShiTang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ShiTang.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        public ActionResult Index(string id)
        {
            if (User.Identity.Name == "14546" || User.Identity.Name == "00114546" || User.Identity.Name == "3001377" || User.Identity.Name == "3002769")
                return RedirectToAction("Index", "Admin", new { name = User.Identity.Name });
            if (string.IsNullOrEmpty(id)) id = DateTime.Now.ToString("yyyyMM");
            var Users = Membership.GetUser();
            var Remains = ShitangService.GetRemains(User.Identity.Name);
            if (Remains.StartsWith("无此胸卡号") && Users.Comment != null)
            {

                ViewBag.Remains = ShitangService.GetRemains(Users.Comment);
                return View(ShitangService.GetDetails(Users.Comment, id));
            }
            else
            {
                ViewBag.Remains = Remains;
                return View(ShitangService.GetDetails(User.Identity.Name, id));
            }
        }
    }
}
