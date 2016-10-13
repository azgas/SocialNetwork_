using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocNet_EPX.TemplateUtilities.ColorTheme
{
    public class NoDefaultThemeProviderSpecifiedException : Exception
    {
        public NoDefaultThemeProviderSpecifiedException()
            : base()
        { }

        public NoDefaultThemeProviderSpecifiedException(string message)
            : base(message)
        { }

        public NoDefaultThemeProviderSpecifiedException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}