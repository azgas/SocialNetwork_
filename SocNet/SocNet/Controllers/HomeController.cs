using System;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
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
            client.BaseAddress = new Uri("http://localhost:56662/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.Timeout = new TimeSpan(0, 30, 0);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (form.DownloadNetwork == false)
            {
                HttpResponseMessage response = client.PostAsJsonAsync("api/FlickrFollowersAPI/FlickrFollowers", form).Result;
            }
            else if (form.DownloadNetwork == true)
            {
                HttpResponseMessage response = client.PostAsJsonAsync("api/FlickrFollowersAPI/FlickrNetwork", form).Result;
            }
            return RedirectToAction("Index", "VisualisationController");
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
            client.BaseAddress = new Uri("http://localhost:56837/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.Timeout = new TimeSpan(0, 30, 0);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.PostAsJsonAsync("api/TwitterNetworkAPI/TwitterNetwork", form).Result;
            return RedirectToAction("Index", "VisualisationController");
        }
    }
}