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
            if (
                (form.DownloadNetwork == true && form.DownloadFollowers == true && form.DownloadFriends == true) ||
                (form.DownloadNetwork == true && form.DownloadFollowers == false && form.DownloadFriends == false)
                )
            {//pobieramy całą sieć
                HttpResponseMessage response = client.PostAsJsonAsync("api/TwitterNetworkAPI/TwitterNetwork", form).Result;
            }
            else if (form.DownloadNetwork == true && form.DownloadFollowers == true && form.DownloadFriends == false)
            {//pobieramy siec followersow
                HttpResponseMessage response = client.PostAsJsonAsync("api/TwitterNetworkAPI/TwitterFollowersNetwork", form).Result;
            }
            else if (form.DownloadNetwork == true && form.DownloadFollowers == false && form.DownloadFriends == true)
            {//pobieramy siec friendsow
                HttpResponseMessage response = client.PostAsJsonAsync("api/TwitterNetworkAPI/TwitterFriendsNetwork", form).Result;
            }
            else if (form.DownloadNetwork == false && form.DownloadFollowers == true && form.DownloadFriends == true)
            {//pobieramy 1xFriends i 1xFollowers
                HttpResponseMessage response = client.PostAsJsonAsync("api/TwitterNetworkAPI/TwitterFollowers", form).Result;
                HttpResponseMessage response2 = client.PostAsJsonAsync("api/TwitterNetworkAPI/TwitterFriends", form).Result;
            }
            else if (form.DownloadNetwork == false && form.DownloadFollowers == false && form.DownloadFriends == true)
            {//pobieramy 1xFriends
                HttpResponseMessage response = client.PostAsJsonAsync("api/TwitterNetworkAPI/TwitterFriends", form).Result;
            }
            else if (form.DownloadNetwork == false && form.DownloadFollowers == true && form.DownloadFriends == false)
            {//pobieramy 1xFollowers
                HttpResponseMessage response = client.PostAsJsonAsync("api/TwitterNetworkAPI/TwitterFollowers", form).Result;
            }
            return RedirectToAction("Index", "VisualisationController");
        }
    }
}