using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocNetEPX.TemplateUtilities.Authorization;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(SocNetEPX.TemplateFilterConfig), "RegisterFilters")]

namespace SocNetEPX
{
    public class TemplateFilterConfig
    {
        public static void RegisterFilters()
        {
            GlobalFilters.Filters.Add(new AuthorizeAttribute());
        }
    }
}