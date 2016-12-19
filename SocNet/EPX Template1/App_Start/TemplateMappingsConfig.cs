using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPX_Template1.TemplateModels.User;
using EPX_Template1.TemplateModels.Project;
using EPX_Template1.TemplateModels.Application;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(EPX_Template1.TemplateMappingsConfig), "SetUpMappings")]

namespace EPX_Template1
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