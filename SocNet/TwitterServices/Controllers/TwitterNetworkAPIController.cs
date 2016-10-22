using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TwitterServices.Models;

namespace TwitterServices.Controllers
{
    public class TwitterNetworkAPIController : ApiController
    {
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
            return Ok();
        }

    }
}
