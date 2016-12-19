using System.Threading;
using System.Web.Mvc;
using System.Web.UI;
using EPXSocNet.TemplateResources;
using EPXSocNet.TemplateUtilities.Authorization;
using EPXSocNet.TemplateUtilities.Localization;

namespace EPXSocNet.Controllers.Template
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