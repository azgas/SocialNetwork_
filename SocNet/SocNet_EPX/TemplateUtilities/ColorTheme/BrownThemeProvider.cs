using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocNet_EPX.TemplateUtilities.ColorTheme
{
    public class BrownThemeProvider : IThemeProvider
    {
        private const string BrownThemeName = "brown";
        private const string BrownThemeStyleBundleName = @"~/content/template/core/brown";

        public string ThemeName
        {
            get { return BrownThemeName; }
        }

        public string GetThemeStyleBundleName()
        {
            return BrownThemeStyleBundleName;
        }
    }
}