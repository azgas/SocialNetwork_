using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterServices.Models
{
    public class FormTwitterNetwork
    {
        public long InitialVertex { get; set; }
        public int NetworkID { get; set; }
        public int NumberQueries { get; set; }
        public int QueryLimit { get; set; }
    }
}