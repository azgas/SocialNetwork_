using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using EPXOS.Contracts.Logic.DataAccess;
using EPX_Template1.TemplateUtilities.Authorization;

namespace EPX_Template1
{
    public class FilterDIConfig
    {
        public static void Register(ContainerBuilder containerBuilder)
        {
            containerBuilder.Register(c => new PlatformAuthorizationAttribute())
                .AsAuthorizationFilterFor<Controller>().InstancePerRequest();
        }
    }
}