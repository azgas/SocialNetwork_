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
    public class GetTwitterUserFollowersController : Controller
    {
        // GET: GetTwitterUserFollowers
        public ActionResult Index()
        {
            return View();
        }
        // POST: GetTwitterUserFollowers
        [HttpPost]
        public ActionResult Index(AskApiTwitterFollowerFriend form)
        {
            HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://localhost:60701/");
            client.BaseAddress = new Uri("http://localhost:666/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.Timeout = new TimeSpan(0, 30, 0);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.PostAsJsonAsync("api/ApiTwitterFollower/UserTwitterFollowers", form).Result;
            return RedirectToAction("Index", "LinkDbs");
        }
    }
}