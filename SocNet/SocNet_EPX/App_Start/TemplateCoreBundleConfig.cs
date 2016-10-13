using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(SocNet_EPX.TemplateCoreBundleConfig), "RegisterBundles")]

namespace SocNet_EPX
{
    public class TemplateCoreBundleConfig
    {
        public static void RegisterBundles()
        {
            #region Script bundles
            // Jquery bundle
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/template/jquery").Include(
                "~/TemplateScripts/Jquery/jquery-2.1.1.js"));

            // Jquery plugins bundle
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/template/jquery/plugins").Include(
                "~/TemplateScripts/Jquery/Plugins/jquery.cookie.js",
                "~/TemplateScripts/Jquery/Plugins/jPushMenu.js",
                "~/TemplateScripts/Jquery/Plugins/jquery.nanoscroller.js",
                "~/TemplateScripts/Jquery/Plugins/jquery.gritter.js",
                "~/TemplateScripts/Jquery/Plugins/jquery-ui.js"));

            // Core template scripts bundle
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/template/core").Include(
                "~/TemplateScripts/Core/core.js"));

            // Bootstrap bundle
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/template/bootstrap").Include(
                "~/TemplateScripts/Bootstrap/bootstrap-modified.js"));

            // Localization bundle
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/template/localization").Include(
                "~/TemplateScripts/Localization/get-resources.js"));
            #endregion

            #region Style bundles
            // Basic style bundle
            BundleTable.Bundles.Add(new StyleBundle("~/content/template/base").Include(
                "~/TemplateContent/Base/bootstrap.css",
                "~/TemplateContent/Base/font-awesome.css"));

            // Plugins style bundle
            BundleTable.Bundles.Add(new StyleBundle("~/content/template/plugins").Include(
                "~/TemplateContent/Plugins/jquery.gritter.css",
                "~/TemplateContent/Plugins/nanoscroller.css",
                "~/TemplateContent/Plugins/jPushMenu.css"));

            // Template core default theme style bundle
            BundleTable.Bundles.Add(new StyleBundle("~/content/template/core").Include(
                "~/TemplateContent/Core/style.css"));

            // Template core blue theme style bundle
            BundleTable.Bundles.Add(new StyleBundle("~/content/template/core/blue").Include(
                "~/TemplateContent/Core/skin-blue.css"));

            // Template core brown theme style bundle
            BundleTable.Bundles.Add(new StyleBundle("~/content/template/core/brown").Include(
                "~/TemplateContent/Core/skin-brown.css"));

            // Template core gray theme style bundle
            BundleTable.Bundles.Add(new StyleBundle("~/content/template/core/gray").Include(
                "~/TemplateContent/Core/skin-gray.css"));

            // Template core green theme style bundle
            BundleTable.Bundles.Add(new StyleBundle("~/content/template/core/green").Include(
                "~/TemplateContent/Core/skin-green.css"));

            // Template core orange theme style bundle
            BundleTable.Bundles.Add(new StyleBundle("~/content/template/core/orange").Include(
                "~/TemplateContent/Core/skin-orange.css"));

            // Template core pink theme style bundle
            BundleTable.Bundles.Add(new StyleBundle("~/content/template/core/pink").Include(
                "~/TemplateContent/Core/skin-pink.css"));

            // Template core prusia theme style bundle
            BundleTable.Bundles.Add(new StyleBundle("~/content/template/core/prusia").Include(
                "~/TemplateContent/Core/skin-prusia.css"));

            // Template core purple theme style bundle
            BundleTable.Bundles.Add(new StyleBundle("~/content/template/core/purple").Include(
                "~/TemplateContent/Core/skin-purple.css"));

            // Template core red theme style bundle
            BundleTable.Bundles.Add(new StyleBundle("~/content/template/core/red").Include(
                "~/TemplateContent/Core/skin-red.css"));

            // Template core yellow theme style bundle
            BundleTable.Bundles.Add(new StyleBundle("~/content/template/core/yellow").Include(
                "~/TemplateContent/Core/skin-yellow.css"));

            // Template commons style bundle
            BundleTable.Bundles.Add(new StyleBundle("~/content/template/commons").Include(
                "~/TemplateContent/Core/commons.css"));
            #endregion
        }
    }
}