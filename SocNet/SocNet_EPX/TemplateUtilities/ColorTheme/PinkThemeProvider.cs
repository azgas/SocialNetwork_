﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocNet_EPX.TemplateUtilities.ColorTheme
{
    public class PinkThemeProvider : IThemeProvider
    {
        private const string PinkThemeName = "pink";
        private const string PinkThemeStyleBundleName = @"~/content/template/core/pink";

        public string ThemeName
        {
            get { return PinkThemeName; }
        }

        public string GetThemeStyleBundleName()
        {
            return PinkThemeStyleBundleName;
        }
    }
}