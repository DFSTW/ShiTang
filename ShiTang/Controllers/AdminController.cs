using ShiTang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ShiTang.Controllers
{
    [Authorize(Users = "14546, 00114546,3001377,3002769")]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Index(string id, string name)
        {
            if (string.IsNullOrEmpty(id)) id = DateTime.Now.ToString("yyyyMM");
            if (string.IsNullOrEmpty(name)) name = User.Identity.Name;
            var Users = Membership.GetUser(name);
            var Remains = ShitangService.GetRemains(name);
            if (Remains.StartsWith("无此胸卡号") && Users != null && Users.Comment != null)
            {

                ViewBag.Remains = ShitangService.GetRemains(Users.Comment);
                ViewBag.Name = Users.Comment;
                return View(ShitangService.GetDetails(Users.Comment, id));
            }
            else
            {
                ViewBag.Remains = Remains;
                ViewBag.Name = name;
                return View(ShitangService.GetDetails(name, id));
            }
        }
    }
}
