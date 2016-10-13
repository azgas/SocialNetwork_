using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocNet.Models
{
    public class AskApiFlickrFollowers
    {
        public int networkID { get; set; }
        public string initialVertex { get; set; }
        public int numberOfQueries { get; set; }
    }
}