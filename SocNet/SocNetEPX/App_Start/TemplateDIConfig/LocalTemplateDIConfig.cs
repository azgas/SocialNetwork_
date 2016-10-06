using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using SocNetEPX.TemplateUtilities.ColorTheme;

namespace SocNetEPX
{
    public class LocalTemplateDIConfig
    {
        public static void Register(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<ThemesManager>().As<IThemesManager>();
        }
    }
}