using MVC5App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5App.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult SurgeViewer()
        {
            ViewBag.Message = "Your Surge VIewer Page";
            return View();
        }
        
        public ActionResult Tests()
        {
            ViewBag.Message = "Your test page.";

            return View();
        }
        public ActionResult UploadPage()
        {
            ViewBag.Message = "Your upload page.";

            return View();
        }
        public ActionResult CompanyQueryByTopic()
        {
            ViewBag.Message = "Your company query by topic page.";

            return View();
        }
        public ActionResult TopicQueryByCompany()
        {
            ViewBag.Message = "Your topic query by Company page.";

            return View();
        }
    }
}