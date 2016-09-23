using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocNet.Models;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SocNet.Controllers
{
    public class GetFlickrUserFollowersController : Controller
    {
        // GET: GetFlickrUserFollowers
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        // POST: GetFlickrUserFollowers
        [HttpPost]
        public ActionResult Index(AskApiFlickrFollower form)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:60701/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.Timeout = new TimeSpan(0, 30, 0);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.PostAsJsonAsync("api/ApiFlickrFollower/UserFlickrFollowers", form).Result;
            return RedirectToAction("Index", "LinkDbs");
        }
    }
}