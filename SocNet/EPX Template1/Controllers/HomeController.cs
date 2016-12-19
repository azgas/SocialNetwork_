using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using EPX_Template1.TemplateUtilities.Localization;
using EPX_Template1.TemplateUtilities.Authorization;

namespace EPX_Template1.Controllers
{
    public class HomeController : LocalizedController
    {
        public ActionResult Index()
        {
            ViewBag.Client = EPX_Template1.TemplateUtilities.AppConfiguration.AppConfig.GetAppConfig.ClientId;
            ViewBag.Lang = CultureName;
            ViewBag.Platform = Platform;
            ViewBag.ApplicationRole = UserApplicationRole;

            return View();
        }
    }
}