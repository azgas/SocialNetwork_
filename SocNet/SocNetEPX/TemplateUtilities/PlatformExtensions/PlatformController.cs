using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using System.Globalization;
using System.Security.Claims;
using EPXOS.Contracts.Logic.DataAccess;
using SocNetEPX.TemplateUtilities.Exceptions;

namespace SocNetEPX.TemplateUtilities.PlatformExtensions
{
    public class PlatformController : Controller
    {
        private IApplicationService applicationService;

        private const string PlatformRouteKey = "platform";
        private const string UserClaimType = "sub";
        private const string LocalizationClaimType = "locale";

        public PlatformController()
        {
            applicationService = AutofacDependencyResolver.Current.ApplicationContainer.Resolve<IApplicationService>();
        }

        protected string Platform
        {
            get
            {
                var routeData = Request.RequestContext.RouteData;

                if (routeData.Values.ContainsKey(PlatformRouteKey))
                    return (string)routeData.Values[PlatformRouteKey];
                else
                    return null;
            }
        }

        protected string UserId
        {
            get
            {
                var subClaim = (User as ClaimsPrincipal).FindFirst(UserClaimType);

                if (subClaim == null)
                    throw new ContextDataNotAvailableException("User id is not available");

                return subClaim.Value;
            }
        }

        protected string CultureName
        {
            get
            {
                var localizationClaim = (User as ClaimsPrincipal).FindFirst(LocalizationClaimType);

                if (localizationClaim == null)
                    throw new ContextDataNotAvailableException("Culture name is not available");

                return localizationClaim.Value;
            }
        }

        protected string UserApplicationRole
        {
            get
            {
                var user = User as ClaimsPrincipal;
                var access_token = user.FindFirst("access_token");
                return applicationService.GetUserApplicationRole(UserId, Platform,
                    SocNetEPX.TemplateUtilities.AppConfiguration.AppConfig.GetAppConfig.ClientId, access_token != null ? access_token.Value : null);
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {

        }
    }
}