using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using SocNet_EPX.TemplateUtilities.ColorTheme;

namespace SocNet_EPX
{
    public class LocalTemplateDIConfig
    {
        public static void Register(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<ThemesManager>().As<IThemesManager>();
        }
    }
}