using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocNet_EPX.TemplateModels.User;
using SocNet_EPX.TemplateModels.Project;
using SocNet_EPX.TemplateModels.Application;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(SocNet_EPX.TemplateMappingsConfig), "SetUpMappings")]

namespace SocNet_EPX
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