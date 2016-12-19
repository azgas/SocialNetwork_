using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPXSocNet.TemplateModels.User;
using EPXSocNet.TemplateModels.Project;
using EPXSocNet.TemplateModels.Application;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(EPXSocNet.TemplateMappingsConfig), "SetUpMappings")]

namespace EPXSocNet
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