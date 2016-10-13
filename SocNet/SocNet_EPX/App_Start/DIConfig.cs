using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using Autofac;
using Autofac.Integration.Mvc;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(SocNet_EPX.DIConfig), "SetUp")]

namespace SocNet_EPX
{
    public partial class DIConfig
    {
        private static IContainer container;

        public static void SetUp()
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();

            Register(containerBuilder);

            if (container == null)
            {
                container = containerBuilder.Build();
            }
            else
            {
                containerBuilder.Update(container);
            }

            DependencyResolver.SetResolver(new Autofac.Integration.Mvc.AutofacDependencyResolver(container));
        }

        private static void Register(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterControllers(Assembly.GetExecutingAssembly());

            FilterDIConfig.Register(containerBuilder);

            containerBuilder.RegisterFilterProvider();

            EPXOSApiDiConfig.Register(containerBuilder);
            EPXOSLogicDiConfig.Register(containerBuilder);

            RegisterLocalDependencies(containerBuilder);

            RegisterApplicationDependencies(containerBuilder);
        }

        private static void RegisterLocalDependencies(ContainerBuilder containerBuilder)
        {
            LocalTemplateDIConfig.Register(containerBuilder);
        }
    }
}