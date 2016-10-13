using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using EPXOS.Contracts.Logic.DataAccess;
using SocNet_EPX.TemplateUtilities.ColorTheme;
using SocNet_EPX.TemplateModels.Application;
using SocNet_EPX.TemplateUtilities.Authorization;
using SocNet_EPX.TemplateUtilities.Localization;
using System.Security.Claims;

namespace SocNet_EPX.Controllers.Template
{
    public class ApplicationController : LocalizedController
    {
        private IApplicationService applicationService;
        private IThemesManager themesManager;

        public ApplicationController(IApplicationService applicationService, IThemesManager themesManager)
        {
            this.applicationService = applicationService;
            this.themesManager = themesManager;

            themesManager.SetDefaultThemeProvider(new GrayThemeProvider());
            themesManager.AddThemeProvider(new DefaultThemeProvider());
            themesManager.AddThemeProvider(new BlueThemeProvider());
            themesManager.AddThemeProvider(new BrownThemeProvider());
            themesManager.AddThemeProvider(new GrayThemeProvider());
            themesManager.AddThemeProvider(new GreenThemeProvider());
            themesManager.AddThemeProvider(new OrangeThemeProvider());
            themesManager.AddThemeProvider(new PinkThemeProvider());
            themesManager.AddThemeProvider(new PrusiaThemeProvider());
            themesManager.AddThemeProvider(new PurpleThemeProvider());
            themesManager.AddThemeProvider(new RedThemeProvider());
            themesManager.AddThemeProvider(new YellowThemeProvider());
        }

        [SkipPlatformAuthorization]
        public ActionResult Error()
        {
            return PartialView("~/TemplateViews/Application/Error.cshtml");
        }

        [SkipPlatformAuthorization]
        public ActionResult PageNotFound()
        {
            return PartialView("~/TemplateViews/Application/PageNotFound.cshtml");
        }

        [SkipPlatformAuthorization]
        public ActionResult NoPlatformSelected()
        {
            return View("~/TemplateViews/Application/NoPlatformSelected.cshtml");
        }

        [SkipPlatformAuthorization]
        public ActionResult PlatformNotAllowed()
        {
            return View("~/TemplateViews/Application/PlatformNotAllowed.cshtml");
        }

        [SkipPlatformAuthorization]
        public ActionResult UserRoleNotAllowed()
        {
            return View("~/TemplateViews/Application/UserRoleNotAllowed.cshtml");
        }

        [SkipPlatformAuthorization]
        public ActionResult UserHasNoAccess()
        {
            return PartialView("~/TemplateViews/Application/UserHasNoAccess.cshtml");
        }

        [SkipPlatformAuthorization]
        public ActionResult GetApplicationName()
        {
            return PartialView("~/TemplateViews/Application/GetApplicationName.cshtml",
                Properties.Settings.Default.ApplicationName);
        }

        [SkipPlatformAuthorization]
        public ActionResult GetPlatformStyleBundle()
        {
            return PartialView("~/TemplateViews/Application/GetPlatformStyleBundle.cshtml",
                themesManager.GetPlatformThemeProvider(Platform).GetThemeStyleBundleName());
        }

        public ActionResult GetPlatformApplications()
        {
            var user = User as ClaimsPrincipal;
            var access_token = user.FindFirst("access_token");
            var applications = Mapper.Map<List<Application>>(applicationService.GetPlatformApplications(Platform, access_token != null ? access_token.Value : null));
            foreach (var app in applications)
            {
                if (app.Url.Contains("*"))
                    app.Url = app.Url.Replace("*", Platform);
            }
            return PartialView("~/TemplateViews/Application/GetPlatformApplications.cshtml", applications);
        }
    }
}