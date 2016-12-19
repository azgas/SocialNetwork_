using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace EPXSocNet.TemplateUtilities.PlatformRouting
{
    public static class RouteExtensions
    {
        public static void MapPlatformRoute(this RouteCollection routes, string name, string url, object defaults = null, object constraints = null)
        {
            routes.Add(name, new PlatformRoute(url)
            {
                Defaults = new RouteValueDictionary(defaults),
                Constraints = new RouteValueDictionary(constraints),
                DataTokens = new RouteValueDictionary()
            });
        }
    }
}