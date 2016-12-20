﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(EPX_Template1.TemplateAppBarBundleConfig), "RegisterBundles")]

namespace EPX_Template1
{
    public class TemplateAppBarBundleConfig
    {
        public static void RegisterBundles()
        {
            #region Script bundles
            // Application bar script bundles
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/template/application-bar").Include(
                "~/TemplateScripts/ApplicationBar/template-utilities.js",
                "~/TemplateScripts/ApplicationBar/projects.js",
                "~/TemplateScripts/ApplicationBar/applications.js"));
            #endregion
        }
    }
}