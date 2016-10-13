using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SocNet_EPX.TemplateUtilities.PlatformRouting;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(SocNet_EPX.TemplateRouteConfig), "RegisterRoutes")]

namespace SocNet_EPX
{
    public class TemplateRouteConfig
    {
        public static void RegisterRoutes()
        {
            var routes = RouteTable.Routes;

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapPlatformRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}