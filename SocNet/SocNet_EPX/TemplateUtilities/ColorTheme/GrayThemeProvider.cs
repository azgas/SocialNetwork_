﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocNet_EPX.TemplateUtilities.ColorTheme
{
    public class GrayThemeProvider : IThemeProvider
    {
        private const string GrayThemeName = "gray";
        private const string GrayThemeStyleBundleName = @"~/content/template/core/gray";

        public string ThemeName
        {
            get { return GrayThemeName; }
        }

        public string GetThemeStyleBundleName()
        {
            return GrayThemeStyleBundleName;
        }
    }
}