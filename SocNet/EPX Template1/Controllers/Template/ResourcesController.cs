using System.Threading;
using System.Web.Mvc;
using System.Web.UI;
using EPX_Template1.TemplateResources;
using EPX_Template1.TemplateUtilities.Authorization;
using EPX_Template1.TemplateUtilities.Localization;

namespace EPX_Template1.Controllers.Template
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