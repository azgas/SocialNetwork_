using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using EPXOS.Api.Infrastructure;
using EPXOS.Api;
using EPXOS.Contracts.Api;

namespace SocNetEPX
{
    public class EPXOSApiDiConfig
    {
        public static void Register(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<ApiClientManager>().As<IApiClientManager>();

            containerBuilder.RegisterType<UserApi>().As<IUserApi>();

            containerBuilder.RegisterType<ApplicationApi>().As<IApplicationApi>();

            containerBuilder.RegisterType<PlatformApi>().As<IPlatformApi>();

            containerBuilder.RegisterType<ProjectApi>().As<IProjectApi>();

            containerBuilder.RegisterType<ClaimApi>().As<IClaimApi>();
        }
    }
}