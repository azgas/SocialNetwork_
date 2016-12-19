using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPXSocNet.TemplateUtilities.ColorTheme
{
    public interface IThemeProvider
    {
        string ThemeName { get; }

        string GetThemeStyleBundleName();
    }
}