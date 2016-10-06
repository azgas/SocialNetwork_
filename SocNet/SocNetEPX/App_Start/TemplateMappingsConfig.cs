using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocNetEPX.TemplateModels.User;
using SocNetEPX.TemplateModels.Project;
using SocNetEPX.TemplateModels.Application;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(SocNetEPX.TemplateMappingsConfig), "SetUpMappings")]

namespace SocNetEPX
{
    public class TemplateMappingsConfig
    {
        public static void SetUpMappings()
        {
            SetUpLocalMappings();
        }

        private static void SetUpLocalMappings()
        {
            UserMappings.SetUpMappings();

            ProjectMappings.SetUpMappings();

            ApplicationMappings.SetUpMappings();
        }
    }
}