using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPXSocNet.TemplateUtilities.ColorTheme
{
    public class RedThemeProvider : IThemeProvider
    {
        private const string RedThemeName = "red";
        private const string RedThemeStyleBundleName = @"~/content/template/core/red";

        public string ThemeName
        {
            get { return RedThemeName; }
        }

        public string GetThemeStyleBundleName()
        {
            return RedThemeStyleBundleName;
        }
    }
}