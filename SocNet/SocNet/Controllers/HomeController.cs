using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocNet.Models;

namespace SocNet.Controllers
{
    public class HomeController : Controller
    {
        //cokolwiek
        //private Credentials asd = new Credentials();
        //private NetworkDb ada =new NetworkDb();
        //private LinkDb lin = new LinkDb();
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
    }
}