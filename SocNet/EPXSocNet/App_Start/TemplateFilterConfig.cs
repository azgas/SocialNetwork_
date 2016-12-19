using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPXSocNet.TemplateUtilities.Authorization;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(EPXSocNet.TemplateFilterConfig), "RegisterFilters")]

namespace EPXSocNet
{
    public class TemplateFilterConfig
    {
        public static void RegisterFilters()
        {
            GlobalFilters.Filters.Add(new AuthorizeAttribute());
        }
    }
}