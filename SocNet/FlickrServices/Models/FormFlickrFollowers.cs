﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlickrServices.Models
{
    public class FormFlickrFollowers
    {
        public string InitialVertex { get; set; }
        public int DonwloadID { get; set; }
        public bool DownloadNetwork { get; set; }
        public int NumberQueries { get; set; }
    }
}