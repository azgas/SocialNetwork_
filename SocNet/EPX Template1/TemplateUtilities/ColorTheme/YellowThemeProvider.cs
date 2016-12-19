using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPX_Template1.TemplateUtilities.ColorTheme
{
    public class YellowThemeProvider : IThemeProvider
    {
        private const string YellowThemeName = "yellow";
        private const string YellowThemeStyleBundleName = @"~/content/template/core/yellow";

        public string ThemeName
        {
            get { return YellowThemeName; }
        }

        public string GetThemeStyleBundleName()
        {
            return YellowThemeStyleBundleName;
        }
    }
}