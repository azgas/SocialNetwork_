using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using EPXSocNet.TemplateUtilities.ColorTheme;

namespace EPXSocNet
{
    public class LocalTemplateDIConfig
    {
        public static void Register(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<ThemesManager>().As<IThemesManager>();
        }
    }
}