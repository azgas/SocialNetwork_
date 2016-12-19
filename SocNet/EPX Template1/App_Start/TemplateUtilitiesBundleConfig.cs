using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(EPX_Template1.TemplateUtilitiesBundleConfig), "RegisterBundles")]

namespace EPX_Template1
{
    public class TemplateUtilitiesBundleConfig
    {
        public static void RegisterBundles()
        {
            #region Script bundles
            // Modal windows bundle
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/template/utilities/modal-windows").Include(
                "~/TemplateScripts/Utilities/ModalWindows/modal-window.js",
                "~/TemplateScripts/Utilities/ModalWindows/constant-modal-window.js"));
            #endregion
        }
    }
}