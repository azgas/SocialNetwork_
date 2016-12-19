using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Globalization;
using EPX_Template1.TemplateUtilities.PlatformExtensions;

namespace EPX_Template1.TemplateUtilities.Localization
{
    public class LocalizedController : PlatformController
    {
        protected CultureInfo Culture
        {
            get
            {
                return new CultureInfo(CultureHelpers.GetImplementedCulture(CultureName));
            }
        }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            if (User.Identity.IsAuthenticated)
            {
                Thread.CurrentThread.CurrentCulture = Culture;
                Thread.CurrentThread.CurrentUICulture = Culture;
            }

            return base.BeginExecuteCore(callback, state);
        }
    }
}