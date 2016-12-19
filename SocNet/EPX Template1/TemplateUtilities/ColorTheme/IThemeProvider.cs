using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPX_Template1.TemplateUtilities.ColorTheme
{
    public interface IThemeProvider
    {
        string ThemeName { get; }

        string GetThemeStyleBundleName();
    }
}