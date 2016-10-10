using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SocNetEPX.TemplateUtilities.PlatformRouting;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(SocNetEPX.TemplateRouteConfig), "RegisterRoutes")]

namespace SocNetEPX
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