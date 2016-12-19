using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EPXSocNet.TemplateUtilities.PlatformRouting
{
    public class PlatformRoute : Route
    {
        public PlatformRoute(string url) : base(url, new MvcRouteHandler()) { }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var routeData = base.GetRouteData(httpContext);
            if (routeData == null) return null;
            string subdomain = httpContext.Request.Params["platform"];
            if (subdomain == null)
            {
                string host = httpContext.Request.Headers["Host"];
                int index = host.IndexOf('.');
                if (index >= 0)
                    subdomain = host.Substring(0, index);
            }
            if (subdomain != null)
                routeData.Values["platform"] = subdomain;
            return routeData;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            object subdomainParam = requestContext.HttpContext.Request.Params["platform"];
            if (subdomainParam != null)
                values["platform"] = subdomainParam;
            return base.GetVirtualPath(requestContext, values);
        }
    }
}