using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocNet_EPX.TemplateUtilities.ColorTheme
{
    public interface IThemesManager
    {
        void SetDefaultThemeProvider(IThemeProvider themeProvider);

        void AddThemeProvider(IThemeProvider themeProvider);

        void ClearThemes();

        IThemeProvider GetPlatformThemeProvider(string platformUri);
    }
}
