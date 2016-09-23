using SocNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tweetinvi;

namespace SocNet.Controllers
{
    public class ApiTwitterFollowerController : ApiController
    {
        private Networkv3Entities1 network = new Networkv3Entities1();
        public void IsUserIdInVertex(long VertexId, int serviceID, string VertexName = null)
        {
            string vertexIdStr = VertexId.ToString();
            var existUser = network.VertexDb.Where<VertexDb>(x => x.identifier == vertexIdStr && x.service_id == serviceID).ToList();
            if (existUser.Count == 0 && VertexName != null)
            {
                var vertex = new VertexDb();
                vertex.identifier = vertexIdStr;
                vertex.name = VertexName;
                vertex.service_id = serviceID;
                network.VertexDb.Add(vertex);
                network.SaveChanges();
            }
            else if (existUser.Count == 0 && VertexName == null)
            {
                var vertex = new VertexDb();
                vertex.identifier = vertexIdStr;
                vertex.service_id = serviceID;
                network.VertexDb.Add(vertex);
                network.SaveChanges();
            }
        }

        // POST: api/ApiTwitterFollower/UserTwitterFollowers
        //pobieranie wszystkich followersów użytkownika Twitter
        [HttpPost]
        public IHttpActionResult UserTwitterFollowers([FromBody]AskApiTwitterFollowerFriend form)
        {
            //data utworzenia krawedzi
            var data = DateTime.Now;
            //akredytacja POPRAWIC
            var credentials= network.Credentials.Where<Credentials>(x => x.ServiceDb.name == "Twitter").ToList();
            Auth.SetApplicationOnlyCredentials(credentials[0].key, credentials[0].secret, true);
            //wybor id serwisu
            var serviceObj = network.ServiceDb.Where<ServiceDb>(x => x.name == "Twitter").ToList();
            var serviceId = serviceObj[0].id;
            //utworzenie uzytkownika poczatkowego twittera
            var userName = Tweetinvi.User.GetUserFromId(form.initialVertex);
            if (userName != null)
            {
                IsUserIdInVertex(form.initialVertex, serviceObj[0].id, userName.ScreenName);
            }
            else
            {
                IsUserIdInVertex(form.initialVertex, serviceObj[0].id);
            }

            var userId = network.VertexDb.Where<VertexDb>(x => x.identifier == form.initialVertex.ToString() && x.service_id == serviceId).ToList();
            //utworzenie listy followersow uzytkownika twittera
            var twitterFollowers = Tweetinvi.User.GetFollowerIds(form.initialVertex, form.queryLimit);
            //zapis krawędzi i wierzchołków do bazy danych
            foreach (var follower in twitterFollowers)
            {
                //dodawnie nowego uzytkownika do wierzcholkow
                var followerName = Tweetinvi.User.GetUserFromId(follower);
                if (followerName != null)
                {
                    IsUserIdInVertex(follower, serviceObj[0].id, followerName.ScreenName);
                }
                else
                {
                    IsUserIdInVertex(follower, serviceObj[0].id);
                }
                //pobranie id wiersza b azy z wierzcholkami
                var followerId = network.VertexDb.Where<VertexDb>(x => x.identifier == follower.ToString() && x.service_id == serviceId).ToList();
                var link = new LinkDb();
                link.date_modified = data;
                link.network_id = form.networkID;
                link.source_id = followerId[0].id;
                link.target_id = userId[0].id;
                network.LinkDb.Add(link);
            }
            network.SaveChanges();
            return Ok();
        }
    }
}
