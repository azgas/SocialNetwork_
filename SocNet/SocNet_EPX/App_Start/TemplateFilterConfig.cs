using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocNet_EPX.TemplateUtilities.Authorization;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(SocNet_EPX.TemplateFilterConfig), "RegisterFilters")]

namespace SocNet_EPX
{
    public class TemplateFilterConfig
    {
        public static void RegisterFilters()
        {
            GlobalFilters.Filters.Add(new AuthorizeAttribute());
        }
    }
}