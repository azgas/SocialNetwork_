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
using SocNetEPX.TemplateUtilities.Exceptions;

namespace SocNetEPX.TemplateUtilities.Authorization
{
    public class ApplicationRoleAuthorizationAttribute : FilterAttribute, IAuthorizationFilter
    {
        private IApplicationService applicationService;

        private const string PlatformRouteKey = "platform";
        private const string UserClaimType = "sub";

        public ApplicationRoleAuthorizationAttribute()
        {
            this.applicationService = AutofacDependencyResolver.Current.ApplicationContainer.Resolve<IApplicationService>();
        }

        public string ApplicationRole { get; set; }

        public void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            if (string.IsNullOrEmpty(ApplicationRole))
                return;

            var user = filterContext.HttpContext.User as ClaimsPrincipal;

            var subClaim = user.FindFirst(UserClaimType);
            if (subClaim == null)
                throw new ContextDataNotAvailableException("User id is not available");

            var userId = subClaim.Value;

            string platform = null;

            var routeData = filterContext.RequestContext.RouteData;
            if (routeData.Values.ContainsKey(PlatformRouteKey))
                platform = (string)routeData.Values[PlatformRouteKey];
            else
                throw new ContextDataNotAvailableException("Platform not available");
            var access_token = user.FindFirst("access_token");
            string userApplicationRole = applicationService.GetUserApplicationRole(userId, platform,
                SocNetEPX.TemplateUtilities.AppConfiguration.AppConfig.GetAppConfig.ClientId, access_token != null ? access_token.Value : null);

            if (userApplicationRole != ApplicationRole)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Action = "UserRoleNotAllowed", Controller = "Application" }));
                return;
            }
        }
    }
}