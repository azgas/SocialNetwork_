using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using EPX_Template1.TemplateUtilities.ColorTheme;

namespace EPX_Template1
{
    public class LocalTemplateDIConfig
    {
        public static void Register(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<ThemesManager>().As<IThemesManager>();
        }
    }
}