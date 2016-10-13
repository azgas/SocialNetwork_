using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocNet_EPX.TemplateUtilities.ColorTheme
{
    public class PurpleThemeProvider : IThemeProvider
    {
        private const string PurpleThemeName = "purple";
        private const string PurpleThemeStyleBundleName = @"~/content/template/core/purple";

        public string ThemeName
        {
            get { return PurpleThemeName; }
        }

        public string GetThemeStyleBundleName()
        {
            return PurpleThemeStyleBundleName;
        }
    }
}