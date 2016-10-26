using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tweetinvi;
using TwitterServices.Models;

namespace TwitterServices.Controllers
{
    public class TwitterNetworkAPIController : ApiController
    {
        private Networkv3Entities network = new Networkv3Entities();
        //dodawanie nowych wierzchołków do bayz danych
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
        //POST: /api/TwitterNetworkAPI/TwitterFollowers
        [HttpPost]
        public IHttpActionResult TwitterFollowers([FromBody] FormTwitterNetwork form)
        {
            return Ok();
        }
        //POST: /api/TwitterNetworkAPI/TwitterFriends
        [HttpPost]
        public IHttpActionResult TwitterFriends([FromBody] FormTwitterNetwork form)
        {
            return Ok();
        }
        //POST: /api/TwitterNetworkAPI/TwitterFollowersNetwork
        [HttpPost]
        public IHttpActionResult TwitterFollowersNetwork([FromBody] FormTwitterNetwork form)
        {
            return Ok();
        }
        //POST: /api/TwitterNetworkAPI/TwitterFriendsNetwork
        [HttpPost]
        public IHttpActionResult TwitterFriendsNetwork([FromBody] FormTwitterNetwork form)
        {
            return Ok();
        }
        //POST: /api/TwitterNetworkAPI/TwitterNetwork
        [HttpPost]
        public IHttpActionResult TwitterNetwork([FromBody] FormTwitterNetwork form)
        {
            //data utworzenia krawedzi
            var data = DateTime.Now;
            //ilosc zapytań
            int queries = 0;
            //akredytacja
            var credentials = network.Credentials.Where<Credentials>(x => x.ServiceDb.name == "Twitter").ToList();
            Auth.SetApplicationOnlyCredentials(credentials[0].key, credentials[0].secret, true);
            //okiełznanie limitów twitter
            RateLimit.RateLimitTrackerMode = RateLimitTrackerMode.TrackOnly;
            TweetinviEvents.QueryBeforeExecute += (sender, args2) =>
            {
                var queryRateLimits = RateLimit.GetQueryRateLimit(args2.QueryURL);

                // Some methods are not RateLimited. Invoking such a method will result in the queryRateLimits to be null
                if (queryRateLimits != null)
                {
                    if (queryRateLimits.Remaining > 0)
                    {
                        // We have enough resource to execute the query
                        return;
                    }
                    else
                    {
                        while (queryRateLimits.Remaining < 1)
                        {
                            foreach (var cred in network.Credentials.Where<Credentials>(x => x.ServiceDb.name == "Twitter").ToList())
                            {
                                var credential = Auth.SetApplicationOnlyCredentials(cred.key, cred.secret, true);
                                queryRateLimits = RateLimit.GetQueryRateLimit(args2.QueryURL, credential);
                                if (queryRateLimits.Remaining > 0)
                                {
                                    args2.TwitterQuery.TwitterCredentials = Auth.SetApplicationOnlyCredentials(cred.key, cred.secret, true);
                                    break;
                                }
                            }
                        }
                    }
                }
            };
            //wybor id serwisu
            var serviceObj = network.ServiceDb.Where<ServiceDb>(x => x.name == "Twitter").ToList();
            var serviceId = serviceObj[0].id;
            //utworzenie uzytkownika poczatkowego twittera
            var userName = Tweetinvi.User.GetUserFromId(form.InitialVertex);
            if (userName != null)
            {
                IsUserIdInVertex(form.InitialVertex, serviceObj[0].id, userName.ScreenName);
            }
            else
            {
                IsUserIdInVertex(form.InitialVertex, serviceObj[0].id);
            }
            var userId = network.VertexDb.Where<VertexDb>(x => x.identifier == form.InitialVertex.ToString() && x.service_id == serviceId).ToList();
            //tworzenie listy followersow i friendsow wierzcholka poczatkowego
            var twitterFollowers = Tweetinvi.User.GetFollowerIds(form.InitialVertex, form.QueryLimit);
            var twitterFriends = Tweetinvi.User.GetFriendIds(form.InitialVertex, form.QueryLimit);
            queries++;
            //zapis followersow dobazy danych
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
                //pobranie id wiersza bazy z wierzcholkami
                var followerId = network.VertexDb.Where<VertexDb>(x => x.identifier == follower.ToString() && x.service_id == serviceId).ToList();
                var link = new LinkDb();
                link.date_modified = data;
                link.network_id = form.NetowrkID;
                link.source_id = followerId[0].id;
                link.target_id = userId[0].id;
                network.LinkDb.Add(link);
            }
            network.SaveChanges();
            //zapis friendsow do bazy danych
            foreach (var friend in twitterFriends)
            {
                //dodawnie nowego uzytkownika do wierzcholkow
                var followerName = Tweetinvi.User.GetUserFromId(friend);
                if (followerName != null)
                {
                    IsUserIdInVertex(friend, serviceObj[0].id, followerName.ScreenName);
                }
                else
                {
                    IsUserIdInVertex(friend, serviceObj[0].id);
                }
                //pobranie id wiersza b azy z wierzcholkami
                var friendId = network.VertexDb.Where<VertexDb>(x => x.identifier == friend.ToString() && x.service_id == serviceId).ToList();
                var link = new LinkDb();
                link.date_modified = data;
                link.network_id = form.NetowrkID;
                link.source_id = userId[0].id;
                link.target_id = friendId[0].id;
                network.LinkDb.Add(link);
            }
            network.SaveChanges();
            //utworzenie list uzytkownikow
            //List<long> usersTwitter = twitterFollowers.Union(twitterFriends).ToList();
            List<long> twitterFollowersList = twitterFollowers.ToList();
            List<long> twitterFriendsList = twitterFriends.ToList();
            List<long> usedUsers = new List<long>();
            usedUsers.Add(form.InitialVertex);
            //rozpoczynamy pobieranie dla kolejnych wierzcholkow
            while (queries < form.NumberQueries && (twitterFriendsList.Any() || twitterFollowersList.Any()))
            {
                var rand = new Random();
                bool bool1 = (twitterFriendsList.Any()) ? true : false;
                bool bool2 = (twitterFollowersList.Any()) ? true : false;
                if (rand.Next(100) % 2 == 1 && bool1 == true)
                {
                    queries++;
                    var newInitialVertexId = twitterFriendsList[0];
                    userId = network.VertexDb.Where<VertexDb>(x => x.identifier == newInitialVertexId.ToString() && x.service_id == serviceId).ToList();
                    usedUsers.Add(newInitialVertexId);
                    var newUsersFriends = Tweetinvi.User.GetFriendIds(newInitialVertexId, form.QueryLimit);
                    var newUsersFollowers = Tweetinvi.User.GetFollowerIds(newInitialVertexId, form.QueryLimit);
                    twitterFriendsList = twitterFriendsList.Union(newUsersFriends).Except(usedUsers).ToList();
                    twitterFollowersList = twitterFollowersList.Union(newUsersFollowers).Except(usedUsers).ToList();
                    //zapis followersow dobazy danych
                    foreach (var follower in newUsersFollowers)
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
                        link.network_id = form.NetowrkID;
                        link.source_id = followerId[0].id;
                        link.target_id = userId[0].id;
                        network.LinkDb.Add(link);
                    }
                    network.SaveChanges();
                    //zapis friendsow do bazy danych
                    foreach (var friend in newUsersFriends)
                    {
                        //dodawnie nowego uzytkownika do wierzcholkow
                        var followerName = Tweetinvi.User.GetUserFromId(friend);
                        if (followerName != null)
                        {
                            IsUserIdInVertex(friend, serviceObj[0].id, followerName.ScreenName);
                        }
                        else
                        {
                            IsUserIdInVertex(friend, serviceObj[0].id);
                        }
                        //pobranie id wiersza b azy z wierzcholkami
                        var friendId = network.VertexDb.Where<VertexDb>(x => x.identifier == friend.ToString() && x.service_id == serviceId).ToList();
                        var link = new LinkDb();
                        link.date_modified = data;
                        link.network_id = form.NetowrkID;
                        link.source_id = userId[0].id;
                        link.target_id = friendId[0].id;
                        network.LinkDb.Add(link);
                    }
                    network.SaveChanges();
                }
                else if (rand.Next(100) % 2 == 0 && bool2 == true)
                {
                    queries++;
                    var newInitialVertexId = twitterFollowersList[0];
                    userId = network.VertexDb.Where<VertexDb>(x => x.identifier == newInitialVertexId.ToString() && x.service_id == serviceId).ToList();
                    usedUsers.Add(newInitialVertexId);
                    var newUsersFriends = Tweetinvi.User.GetFriendIds(newInitialVertexId, form.QueryLimit);
                    var newUsersFollowers = Tweetinvi.User.GetFollowerIds(newInitialVertexId, form.QueryLimit);
                    twitterFriendsList = twitterFriendsList.Union(newUsersFriends).Except(usedUsers).ToList();
                    twitterFollowersList = twitterFollowersList.Union(newUsersFollowers).Except(usedUsers).ToList();
                    //zapis followersow dobazy danych
                    foreach (var follower in newUsersFollowers)
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
                        link.network_id = form.NetowrkID;
                        link.source_id = followerId[0].id;
                        link.target_id = userId[0].id;
                        network.LinkDb.Add(link);
                    }
                    network.SaveChanges();
                    //zapis friendsow do bazy danych
                    foreach (var friend in newUsersFriends)
                    {
                        //dodawnie nowego uzytkownika do wierzcholkow
                        var followerName = Tweetinvi.User.GetUserFromId(friend);
                        if (followerName != null)
                        {
                            IsUserIdInVertex(friend, serviceObj[0].id, followerName.ScreenName);
                        }
                        else
                        {
                            IsUserIdInVertex(friend, serviceObj[0].id);
                        }
                        //pobranie id wiersza b azy z wierzcholkami
                        var friendId = network.VertexDb.Where<VertexDb>(x => x.identifier == friend.ToString() && x.service_id == serviceId).ToList();
                        var link = new LinkDb();
                        link.date_modified = data;
                        link.network_id = form.NetowrkID;
                        link.source_id = userId[0].id;
                        link.target_id = friendId[0].id;
                        network.LinkDb.Add(link);
                    }
                    network.SaveChanges();
                }
            }
            return Ok();
        }

    }
}
