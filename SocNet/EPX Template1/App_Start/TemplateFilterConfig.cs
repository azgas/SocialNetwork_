using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPX_Template1.TemplateUtilities.Authorization;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(EPX_Template1.TemplateFilterConfig), "RegisterFilters")]

namespace EPX_Template1
{
    public class TemplateFilterConfig
    {
        public static void RegisterFilters()
        {
            GlobalFilters.Filters.Add(new AuthorizeAttribute());
        }
    }
}