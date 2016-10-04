using SocNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace SocNet.Controllers
{
    public class GetFlickrUsersFollowersController : Controller
    {
        // GET: GetFlickrUsersFollowers
        public ActionResult Index()
        {
            return View();
        }
        // POST: GetFlickrUsersFollowers
        [HttpPost]
        public ActionResult Index(AskApiFlickrFollowers form)
        {
            HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://localhost:60701/");
            client.BaseAddress = new Uri("http://localhost:666/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.Timeout = new TimeSpan(0, 30, 0);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.PostAsJsonAsync("api/ApiFlickrFollowers/UsersFlickrFollowers", form).Result;
            return RedirectToAction("Index", "LinkDbs");
        }
    }
}