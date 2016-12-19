using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Security.Claims;
using Autofac;
using Autofac.Integration.Mvc;
using EPXOS.Contracts.Logic.DataAccess;

namespace EPX_Template1.TemplateUtilities.Authorization
{
    public class PlatformAuthorizationAttribute : FilterAttribute, IAuthorizationFilter
    {
        private IPlatformService platformService;

        public PlatformAuthorizationAttribute()
        {
            this.platformService = AutofacDependencyResolver.Current.ApplicationContainer.Resolve<IPlatformService>();
        }

        public void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            var routeValues = filterContext.RequestContext.RouteData.Values;

            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(SkipPlatformAuthorizationAttribute), false).Any())
            {
                return;
            }

            if (!routeValues.ContainsKey("platform"))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Action = "NoPlatformSelected", Controller = "Application" }));
                return;
            }

            var platform = routeValues["platform"] as string;
            var user = filterContext.HttpContext.User as ClaimsPrincipal;
            var userId = user.FindFirst("sub").Value;
            var access_token = user.FindFirst("access_token");
            if (!platformService.CheckIfUserHasAccessToPlatform(platform, userId, access_token != null ? access_token.Value : null))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Action = "PlatformNotAllowed", Controller = "Application" }));
            }
        }
    }
}