using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using SocNet_EPX.TemplateUtilities.Localization;
using SocNet_EPX.TemplateUtilities.Authorization;

namespace SocNet_EPX.Controllers
{
    public class HomeController : LocalizedController
    {
        public ActionResult Index()
        {
            ViewBag.Client = SocNet_EPX.TemplateUtilities.AppConfiguration.AppConfig.GetAppConfig.ClientId;
            ViewBag.Lang = CultureName;
            ViewBag.Platform = Platform;
            ViewBag.ApplicationRole = UserApplicationRole;

            return View();
        }
    }
}