using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocNet_EPX.TemplateUtilities.ColorTheme
{
    public class BlueThemeProvider : IThemeProvider
    {
        private const string BlueThemeName = "blue";
        private const string BlueThemeStyleBundleName = @"~/content/template/core/blue";

        public string ThemeName
        {
            get { return BlueThemeName; }
        }

        public string GetThemeStyleBundleName()
        {
            return BlueThemeStyleBundleName;
        }
    }
}