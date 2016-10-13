using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocNet.Models
{
    public class AskApiTwitterNetwork
    {
        public int networkID { get; set; }
        public int initialVertex { get; set; }
        public int queryLimit { get; set; }
        public int numberOfQueries { get; set; }
    }
}