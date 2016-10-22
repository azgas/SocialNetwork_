using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocNet.Models
{
    public class FormTwitterNetwork
    {
        public long InitialVertex { get; set; }
        public int NetworkID { get; set; }
        public int NumberQueries { get; set; }
        public int QueryLimit { get; set; }
        public bool DownloadNetwork { get; set; }
        public bool DownloadFriends { get; set; }
        public bool DownloadFollowers { get; set; }
    }
}