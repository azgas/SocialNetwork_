using System.Threading;
using System.Web.Mvc;
using System.Web.UI;
using SocNet_EPX.TemplateResources;
using SocNet_EPX.TemplateUtilities.Authorization;
using SocNet_EPX.TemplateUtilities.Localization;

namespace SocNet_EPX.Controllers.Template
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