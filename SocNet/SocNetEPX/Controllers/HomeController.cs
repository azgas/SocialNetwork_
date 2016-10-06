using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using SocNetEPX.TemplateUtilities.Localization;
using SocNetEPX.TemplateUtilities.Authorization;

namespace SocNetEPX.Controllers
{
    public class HomeController : LocalizedController
    {
        public ActionResult Index()
        {
            ViewBag.Client = SocNetEPX.TemplateUtilities.AppConfiguration.AppConfig.GetAppConfig.ClientId;
            ViewBag.Lang = CultureName;
            ViewBag.Platform = Platform;
            ViewBag.ApplicationRole = UserApplicationRole;

            return View();
        }
    }
}