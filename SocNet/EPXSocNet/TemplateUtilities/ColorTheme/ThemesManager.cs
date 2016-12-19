using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPXOS.Contracts.Logic.DataAccess;

namespace EPXSocNet.TemplateUtilities.ColorTheme
{
    public class ThemesManager : IThemesManager
    {
        private IPlatformService platformService;

        private IThemeProvider defaultThemeProvider;
        private Dictionary<string, IThemeProvider> providers;

        public ThemesManager(IPlatformService platformService)
        {
            this.platformService = platformService;

            providers = new Dictionary<string, IThemeProvider>();
        }

        public void SetDefaultThemeProvider(IThemeProvider themeProvider)
        {
            if (themeProvider == null)
                throw new ArgumentNullException("themeProvider parameter is null");

            defaultThemeProvider = themeProvider;
        }

        public void AddThemeProvider(IThemeProvider themeProvider)
        {
            if (themeProvider == null)
                throw new ArgumentNullException("themeProvider parameter is null");

            providers.Add(themeProvider.ThemeName, themeProvider);
        }

        public void ClearThemes()
        {
            defaultThemeProvider = null;
            providers.Clear();
        }

        public IThemeProvider GetPlatformThemeProvider(string platformUri)
        {
            var themeName = platformService.GetPlatformColorThemeName(platformUri);

            if (string.IsNullOrEmpty(themeName))
                return GetDefaultTheme();

            if (!providers.ContainsKey(themeName))
                return GetDefaultTheme();

            IThemeProvider selectedThemeProvider = providers[themeName];
            return selectedThemeProvider;
        }

        private IThemeProvider GetDefaultTheme()
        {
            if (defaultThemeProvider == null)
                throw new NoDefaultThemeProviderSpecifiedException("Default theme provider is not specified");

            return defaultThemeProvider;
        }
    }
}