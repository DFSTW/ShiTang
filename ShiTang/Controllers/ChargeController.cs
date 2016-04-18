using ShiTang.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShiTang.Controllers
{
    [Authorize(Users = "14546, 00114546,3001377,3002769")]
    public class ChargeController : Controller
    {
        //
        // GET: /Charge/

        public ActionResult Index()
        {
            var cm = ShitangService.GetCharge();
            ViewBag.Charge = cm.Charge;
            ViewBag.Reset = cm.Reset;
            return View(cm);
        }

        [HttpPost]
        public ActionResult Index(ChargeModel model)
        {
            model.Comp = "东汽";
            ShitangService.UpdateCharge(model);
            var cm = ShitangService.GetCharge();
            return View(cm);
        }

        public ActionResult White()
        {
            return View();
        }

        [HttpPost]
        public ActionResult White(string id, int select, int charge)
        {
            string status = "挂失";
            switch (select)
            {
                case 0:
                    status = "挂失"; break;
                case 2:
                    status = "产假"; break;
                case 1:
                case 3:
                case 5:
                    status = "正常"; break;
                case 4:
                    status = "冻结"; break;
            }
            if (select == 3 || select == 5)
            {
                ShitangService.UpdateCharge(id, charge);
            }
            ShitangService.UpdateUserStatus(id, status);
            var x = ShitangService.GetUserInfo(id);
            if (!string.IsNullOrEmpty(x.dq_cardid))
            {

                string fileName = Server.MapPath("~/W.exe");
                Process p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = fileName;
                p.StartInfo.CreateNoWindow = true;
                if (status == "正常")
                {
                    p.StartInfo.Arguments = "-a0 " + id;
                }
                else
                {
                    p.StartInfo.Arguments = "-d0 " + id;
                }
                p.StartInfo.WorkingDirectory = Server.MapPath("~/");
                p.Start();
                p.WaitForExit();

                //ViewBag.output = p.StandardOutput.ReadToEnd();

                //ViewBag.dq_id = x.dq_id;
                //ViewBag.dq_name = x.dq_name;
                //ViewBag.dq_depart = x.dq_depart;
                //ViewBag.dq_state = x.dq_state;
                //ViewBag.dq_company = x.dq_company;
                //ViewBag.dq_cardid = x.dq_cardid;
                //ViewBag.dq_charge = x.dq_charge;
                
            }
            else
            {
                ViewBag.dq_id = "用户不存在";
            }
            ViewBag.ID = id;

            return View();
        }

        public ActionResult CJ()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CJ(string id, int select)
        {

            return View();
        }

        public ActionResult NEW()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NEW(string id, string name, string cardid, int charge)
        {
            var user = ShitangService.CreateNewUser(id, name, cardid, charge);
            var x = ShitangService.GetUserInfo(id);
            if (!string.IsNullOrEmpty(x.dq_cardid))
            {
                string fileName = Server.MapPath("~/W.exe");
                Process p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = fileName;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.Arguments = "-a0 " + id;
                p.StartInfo.WorkingDirectory = Server.MapPath("~/");
                p.Start();
                p.WaitForExit();

                ViewBag.dq_id = x.dq_id;
                ViewBag.dq_name = x.dq_name;
                ViewBag.dq_depart = x.dq_depart;
                ViewBag.dq_state = x.dq_state;
                ViewBag.dq_company = x.dq_company;
                ViewBag.dq_cardid = x.dq_cardid;
                ViewBag.dq_charge = x.dq_charge;
            }
            else
            {
                ViewBag.dq_id = "用户添加失败";
            }
            return View();
        }

        public ActionResult ZX()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ZX(string id, int select)
        {

            return View();
        }

    }
}
