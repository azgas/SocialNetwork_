using System.Threading;
using System.Web.Mvc;
using System.Web.UI;
using SocNetEPX.TemplateResources;
using SocNetEPX.TemplateUtilities.Authorization;
using SocNetEPX.TemplateUtilities.Localization;

namespace SocNetEPX.Controllers.Template
{
    public class ResourcesController : LocalizedController
    {
        [SkipPlatformAuthorization]
        [OutputCache(Duration = 60, VaryByParam = "none", Location = OutputCacheLocation.Client)]
        public JsonResult GetResources()
        {
            return Json(ResourceCollection.GetResources(Thread.CurrentThread.CurrentCulture),
                JsonRequestBehavior.AllowGet);
        }
    }
}