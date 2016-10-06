using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocNetEPX.TemplateUtilities.ColorTheme
{
    public class OrangeThemeProvider : IThemeProvider
    {
        private const string OrangeThemeName = "orange";
        private const string OrangeThemeStyleBundleName = @"~/content/template/core/orange";

        public string ThemeName
        {
            get { return OrangeThemeName; }
        }

        public string GetThemeStyleBundleName()
        {
            return OrangeThemeStyleBundleName;
        }
    }
}