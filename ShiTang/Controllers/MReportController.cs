using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShiTang.Controllers
{
    public class MReportController : Controller
    {
        //
        // GET: /MReport/
        public string baseUrl = "http://172.18.50.156:8123";
        public ActionResult Index()
        {
            ViewBag.HeaerTitle = "东汽食堂消费报表";
            ViewBag.Url = baseUrl + "/api/MReport/";
            ViewBag.Param = "";
            return View();
        }

        public ActionResult SY()
        {
            ViewBag.HeaerTitle = "东汽实业食堂消费报表";
            ViewBag.Url = baseUrl + "/api/Comp/";
            ViewBag.Param = "?comp=东汽实业";
            return View("Index");
        }

        public ActionResult XK()
        {
            ViewBag.HeaerTitle = "西科食堂消费报表";
            ViewBag.Url = baseUrl + "/api/Comp/";
            ViewBag.Param = "?comp=西科";
            return View("Index");
        }
        public ActionResult BY()
        {
            ViewBag.HeaerTitle = "八角一食堂消费报表";
            ViewBag.Url = baseUrl + "/api/STReport/";
            ViewBag.Param = "?st=八角一食堂";
            return View("Index");
        }
        public ActionResult BE()
        {
            ViewBag.HeaerTitle = "八角二食堂消费报表";
            ViewBag.Url = baseUrl + "/api/STReport/";
            ViewBag.Param = "?st=八角二食堂";
            return View("Index");
        }
        public ActionResult BS()
        {
            ViewBag.HeaerTitle = "八角三食堂消费报表";
            ViewBag.Url = baseUrl + "/api/STReport/";
            ViewBag.Param = "?st=八角三食堂";
            return View("Index");
        }
        public ActionResult BP()
        {
            ViewBag.HeaerTitle = "八角配餐中心消费报表";
            ViewBag.Url = baseUrl + "/api/STReport/";
            ViewBag.Param = "?st=八角配餐";
            return View("Index");
        }
        public ActionResult EF()
        {
            ViewBag.HeaerTitle = "二分厂食堂消费报表";
            ViewBag.Url = baseUrl + "/api/STReport/";
            ViewBag.Param = "?st=二分厂阶梯楼";
            return View("Index");
        }
        public ActionResult FD()
        {
            ViewBag.HeaerTitle = "风电食堂消费报表";
            ViewBag.Url = baseUrl + "/api/STReport/";
            ViewBag.Param = "?st=风电";
            return View("Index");
        }
        public ActionResult MZ()
        {
            ViewBag.HeaerTitle = "绵竹食堂消费报表";
            ViewBag.Url = baseUrl + "/api/STReport/";
            ViewBag.Param = "?st=绵竹";
            return View("Index");
        }
        public ActionResult FJ()
        {
            ViewBag.HeaerTitle = "辅机食堂消费报表";
            ViewBag.Url = baseUrl + "/api/STReport/";
            ViewBag.Param = "?st=辅机";
            return View("Index");
        }
        public ActionResult HD()
        {
            ViewBag.HeaerTitle = "河东食堂消费报表";
            ViewBag.Url = baseUrl + "/api/STReport/";
            ViewBag.Param = "?st=河东";
            return View("Index");
        }
        public ActionResult YY()
        {
            ViewBag.HeaerTitle = "运业食堂消费报表";
            ViewBag.Url = baseUrl + "/api/STReport/";
            ViewBag.Param = "?st=运业";
            return View("Index");
        }

        public ActionResult PM()
        {
            return View();
        }

        public ActionResult PMM(string index, string loc)
        {
            ViewBag.HeaerTitle = index + "号刷卡机--（" + loc+"）";
            ViewBag.Url = baseUrl + "/api/PosReport/";
            ViewBag.Param = "?index=" + index;
            return View("Index");
        }
        
    }
}
