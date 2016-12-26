using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPXSocNet.Models;
using System.Net.Http;
using System.Net.Http.Headers;

namespace EPXSocNet.Controllers
{
    public class AcquisitionController : Controller
    {
        //cokolwiek
        //private Credentials asd = new Credentials();
        //private NetworkDb ada =new NetworkDb();
        //private LinkDb lin = new LinkDb();
        public ActionResult Index()
        {
            return View();
        }
        //GET: /Home/FlickrFollowers
        [HttpGet]
        public ActionResult FlickrFollowers()
        {
            return View();
        }
        //POST: /Home/FlickrFollowers
        [HttpPost]
        public ActionResult FlickrFollowers(FormFlickrFollowers form)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://gnanalysis:444/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.PostAsJsonAsync("api/FlickrFollowersAPI/FlickrNetwork", form).Result;
            return RedirectToAction("Index", "Visualisation");
        }
        //GET: /Home/TwitterFollowers
        [HttpGet]
        public ActionResult TwitterNetwork()
        {
            return View();
        }
        //POST: /Home/TwitterFollowers
        [HttpPost]
        public ActionResult TwitterNetwork(FormTwitterNetwork form)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://gnanalysis:4451/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.PostAsJsonAsync("api/TwitterNetworkAPI/TwitterNetwork", form).Result;
            return RedirectToAction("Index", "Visualisation");
        }
    }
}