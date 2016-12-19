using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using EPXSocNet.TemplateUtilities.Localization;
using EPXSocNet.TemplateUtilities.Authorization;

namespace EPXSocNet.Controllers
{
    public class HomeController : LocalizedController
    {
        public ActionResult Index()
        {
            ViewBag.Client = EPXSocNet.TemplateUtilities.AppConfiguration.AppConfig.GetAppConfig.ClientId;
            ViewBag.Lang = CultureName;
            ViewBag.Platform = Platform;
            ViewBag.ApplicationRole = UserApplicationRole;

            return View();
        }
    }
}