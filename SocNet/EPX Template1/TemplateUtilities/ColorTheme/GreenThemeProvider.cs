using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPX_Template1.TemplateUtilities.ColorTheme
{
    public class GreenThemeProvider : IThemeProvider
    {
        private const string GreenThemeName = "green";
        private const string GreenThemeStyleBundleName = @"~/content/template/core/green";

        public string ThemeName
        {
            get { return GreenThemeName; }
        }

        public string GetThemeStyleBundleName()
        {
            return GreenThemeStyleBundleName;
        }
    }
}