using FlickrServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FlickrServices.Controllers
{
    public class FlickrFollowersAPIController : ApiController
    {
        //POST: /api/FlickrFollowersAPI/FlickrFollowers
        [HttpPost]
        public IHttpActionResult FlickrFollowers([FromBody] FormFlickrFollowers form)
        {
            return Ok();
        }
        //POST: /api/FlickrFollowersAPI/FlickrNetwork
        [HttpPost]
        public IHttpActionResult FlickrNetwork([FromBody] FormFlickrFollowers form)
        {
            return Ok();
        }
    }
}
