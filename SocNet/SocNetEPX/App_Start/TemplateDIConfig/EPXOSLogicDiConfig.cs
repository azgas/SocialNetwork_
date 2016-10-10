using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using EPXOS.Logic.DataAccess;
using EPXOS.Contracts.Logic.DataAccess;

namespace SocNetEPX
{
    public class EPXOSLogicDiConfig
    {
        public static void Register(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<UserService>().As<IUserService>();

            containerBuilder.RegisterType<ProjectService>().As<IProjectService>();

            containerBuilder.RegisterType<ApplicationService>().As<IApplicationService>();

            containerBuilder.RegisterType<PlatformService>().As<IPlatformService>();
        }
    }
}