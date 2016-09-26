using FlickrNet;
using SocNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SocNet.Controllers
{
    public class ApiFlickrFollowersController : ApiController
    {
        private Networkv3Entities1 network = new Networkv3Entities1();
        public void IsUserIdInVertex(string VertexId, int serviceID, string VertexName=null)
        {
            var existUser = network.VertexDb.Where<VertexDb>(x => x.identifier == VertexId && x.service_id == serviceID).ToList();
            if (existUser.Count == 0 && VertexName!=null)
            {
                var vertex = new VertexDb();
                vertex.identifier = VertexId;
                vertex.service_id = serviceID;
                vertex.name = VertexName;
                network.VertexDb.Add(vertex);
                network.SaveChanges();
            }else if(existUser.Count == 0 && VertexName == null)
            {
                var vertex = new VertexDb();
                vertex.identifier = VertexId;
                vertex.service_id = serviceID;
                network.VertexDb.Add(vertex);
                network.SaveChanges();
            }
        }
        // POST: api/ApiFlickrFollowers/UsersFlickrFollowers
        //pobieranie wszystkich followersów odpowiedniej liczby uzytkownikow flickr
        [HttpPost]
        public IHttpActionResult UsersFlickrFollowers([FromBody]AskApiFlickrFollowers form)
        {
            //data rozpoczęcia pobierania
            var data = DateTime.Now;
            //liczba wykonanych zapytan
            int queries = 0;
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
            int numberOfPages = flickrFollowers.Pages;
            queries++;
            //dodanie uzytkownika poczatkowego do tabeli vertex
            IsUserIdInVertex(form.initialVertex, serviceObj[0].id);
            //pobranie id uzytkonika poczatkowego w tabeli vertex
            var userId = network.VertexDb.Where<VertexDb>(x => x.identifier == form.initialVertex && x.service_id == serviceId).ToList();
            //zapis pierwszej partii do bazy danych
            foreach (var follower in flickrFollowers)
            {
                IsUserIdInVertex(follower.UserId, serviceObj[0].id, follower.UserName);
                var followerId = network.VertexDb.Where<VertexDb>(x => x.identifier == follower.UserId && x.service_id == serviceId).ToList();
                var link = new LinkDb();
                link.date_modified = data;
                link.network_id = form.networkID;
                link.source_id = followerId[0].id;
                link.target_id = userId[0].id;
                network.LinkDb.Add(link);
            }
            network.SaveChanges();
            //zapis reszty danych
            for (int i = 2; i <= numberOfPages; i++)
            {
                flickrFollowers = user.ContactsGetPublicList(form.initialVertex, i, 1000);
                foreach (var follower in flickrFollowers)
                {
                    IsUserIdInVertex(follower.UserId, serviceObj[0].id, follower.UserName);
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
            //utworzenie listy followersow
            var followersList = flickrFollowers.ToList();
            //lista uzytych followersow
            List<Contact> usedFollowers = new List<Contact>();
            //usedFollowers.Add(form.initialVertex);
            while (queries<=form.numberOfQueries && followersList.Any())
            {
                queries++;
                var newInitialVertex = followersList[0];
                if (newInitialVertex.UserId != form.initialVertex)
                { 
                userId = network.VertexDb.Where<VertexDb>(x => x.identifier == newInitialVertex.UserId && x.service_id == serviceId).ToList();
                flickrFollowers = user.ContactsGetPublicList(newInitialVertex.UserId);
                usedFollowers.Add(newInitialVertex);
                followersList = followersList.Union(flickrFollowers).Except(usedFollowers).ToList();
                foreach(var follower in flickrFollowers)
                {
                    IsUserIdInVertex(follower.UserId, serviceObj[0].id, follower.UserName);
                    var followerId = network.VertexDb.Where<VertexDb>(x => x.identifier == follower.UserId && x.service_id == serviceId).ToList();
                    var link = new LinkDb();
                    link.date_modified = data;
                    link.network_id = form.networkID;
                    link.source_id = followerId[0].id;
                    link.target_id = userId[0].id;
                    network.LinkDb.Add(link);
                }
                network.SaveChanges();

                //zapis kolejnych partii danych
                numberOfPages = flickrFollowers.Pages;
                    for (int i = 2; i <= numberOfPages; i++)
                    {
                        flickrFollowers = user.ContactsGetPublicList(newInitialVertex.UserId, i, 1000);
                        followersList = followersList.Union(flickrFollowers).Except(usedFollowers).ToList();
                        foreach (var follower in flickrFollowers)
                        {
                            IsUserIdInVertex(follower.UserId, serviceObj[0].id, follower.UserName);
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
                }
            }
            return Ok();
        }
    }
}
