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

        [HttpGet]
        public ActionResult CompanyQueryByTopic(string topicName)
        {
            ViewBag.Message = "Your company query by topic page.";
            ViewData["topicName"] = topicName;

            return View();
        }

        public ActionResult TopicQueryByCompany()
        {
            ViewBag.Message = "Your topic query by Company page.";

            return View();
        }

        [HttpGet]
        public ActionResult TopicQueryByCompany(string companyName)
        {
            ViewBag.Message = "Your topic query by Company page.";
            ViewData["companyName"] = companyName;

            return View();
        }

        [HttpGet]
        public ActionResult SinglePage(string companyName)
        {
            ViewData["companyName"] = companyName;

            return View("SinglePage");
        }

        [HttpGet]
        public ActionResult SinglePageCompanies(string topicName)
        {
            ViewData["topicName"] = topicName;

            return View("SinglePageCompanies");
        }

        public ActionResult FindCompany()
        {
            ViewBag.Message = "Your find company page.";

            return View();
        }
        public ActionResult FindTopic()
        {
            ViewBag.Message = "Your find topic page.";

            return View();
        }
        public ActionResult FindSurge()
        {
            ViewBag.Message = "Your find surge page.";

            return View();
        }

        //https://msdn.microsoft.com/en-us/library/dd410596(v=vs.100).aspx
        [HttpGet]
        public ActionResult PlotData(string companyName, string topicName, string companyName2 = "", string topicName2 = "")
        {
            ViewData["companyName"] = companyName;
            ViewData["topicName"] = topicName;
            ViewData["companyName2"] = companyName2;
            ViewData["topicName2"] = topicName2;

            return View("SurgeViewer");
        }

        public ActionResult SinglePage()
        {
            return View();
        }

        public ActionResult SinglePageCompanies()
        {
            return View();
        }
    }
}