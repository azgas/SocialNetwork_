using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SocNet.Models;
using FlickrNet;

namespace SocNet.Controllers
{
    public class ApiFlickrFollowerController : ApiController
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

        // POST: api/ApiFlickrFollower/UserFlickrFollowers
        //pobieranie wszystkich followersów użytkownika flickr
        [HttpPost]
        public IHttpActionResult UserFlickrFollowers([FromBody]AskApiFlickrFollower form)
        {
            //data rozpoczęcia pobierania
            var data = DateTime.Now;
            //dodac pobieranie id serwisu z bazy danych
            var serviceObj = network.ServiceDb.Where<ServiceDb>(x => x.name == "Flickr").ToList();
            var serviceId = serviceObj[0].id;
            //utworzenie uzytkownika Flickr
            Flickr user = new Flickr();
            //podmienic pozniej na credentials z bazy danych
            var credentials = network.Credentials.Where<Credentials>(x => x.service_id == serviceId).ToList();
            user.ApiKey = credentials[0].key;
            user.ApiSecret = credentials[0].secret;
            //pobieranie followersow uzytkownika z formularza
            var flickrFollowers = user.ContactsGetPublicList(form.initialVertex);
            //dodanie uzytkownika poczatkowego do tabeli vertex
            IsUserIdInVertex(form.initialVertex, serviceObj[0].id);
            //pobranie id uzytkonika poczatkowego w tabeli vertex
            var userId = network.VertexDb.Where<VertexDb>(x => x.identifier == form.initialVertex && x.service_id == serviceId).ToList();
            int numberOfPages = flickrFollowers.Pages;
            //zapis pierwszej partii do bazy danych
            foreach (var follower in flickrFollowers)
            {
                IsUserIdInVertex(follower.UserId, serviceObj[0].id);
                var followerId = network.VertexDb.Where<VertexDb>(x => x.identifier == follower.UserId && x.service_id == serviceId).ToList();
                var link = new LinkDb();
                link.date_modified = data;
                link.network_id = form.networkID;
                link.source_id = followerId[0].id;
                link.target_id= userId[0].id;
                network.LinkDb.Add(link);
            }
            network.SaveChanges();
            //zapis reszty danych
            for (int i = 2; i <= numberOfPages; i++)
            {
                flickrFollowers = user.ContactsGetPublicList(form.initialVertex, i, 1000);
                foreach (var follower in flickrFollowers)
                {
                    IsUserIdInVertex(follower.UserId, serviceObj[0].id);
                    var followerId = network.VertexDb.Where<VertexDb>(x => x.identifier == follower.UserId && x.service_id == serviceId).ToList();
                    var link = new LinkDb();
                    link.date_modified = data;
                    link.network_id = form.networkID;
                    link.source_id = followerId[0].id;
                    link.target_id = userId[0].id;
                    network.LinkDb.Add(link);
                }
                network.SaveChanges();
            }
            return Ok();
        }
    }
}
