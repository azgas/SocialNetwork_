using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocNetEPX.TemplateUtilities.ColorTheme
{
    public class DefaultThemeProvider : IThemeProvider
    {
        private const string DefaultThemeName = "default";
        private const string DefaultThemeStyleBundleName = @"~/content/template/core";

        public string ThemeName
        {
            get { return DefaultThemeName; }
        }

        public string GetThemeStyleBundleName()
        {
            return DefaultThemeStyleBundleName;
        }
    }
}