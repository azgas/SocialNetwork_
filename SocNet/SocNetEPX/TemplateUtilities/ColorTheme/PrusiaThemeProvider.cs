using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocNetEPX.TemplateUtilities.ColorTheme
{
    public class PrusiaThemeProvider : IThemeProvider
    {
        private const string PrusiaThemeName = "prusia";
        private const string PrusiaThemeStyleBundleName = @"~/content/template/core/prusia";

        public string ThemeName
        {
            get { return PrusiaThemeName; }
        }

        public string GetThemeStyleBundleName()
        {
            return PrusiaThemeStyleBundleName;
        }
    }
}