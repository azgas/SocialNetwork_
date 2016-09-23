using SocNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SocNet.Controllers
{
    public class AskApiFlickrFollowersController : ApiController
    {
        private Networkv3Entities1 network = new Networkv3Entities1();
        public void IsUserIdInVertex(string VertexId, int serviceID)
        {
            var existUser = network.VertexDb.Where<VertexDb>(x => x.identifier == VertexId && x.service_id == serviceID).ToList();
            if (existUser.Count == 0)
            {
                var vertex = new VertexDb();
                vertex.identifier = VertexId;
                vertex.service_id = serviceID;
                network.VertexDb.Add(vertex);
                network.SaveChanges();
            }
        }
        // POST: api/ApiFlickrFollower/UsersFlickrFollowers
        //pobieranie wszystkich followersów odpowiedniej liczby uzytkownikow flickr
        [HttpPost]
        public IHttpActionResult UsersFlickrFollowers([FromBody]AskApiFlickrFollowers form)
        {
            return Ok();
        }
    }
}
