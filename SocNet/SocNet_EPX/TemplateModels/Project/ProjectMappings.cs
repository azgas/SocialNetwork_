using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

using DomainProject = EPXOS.Domain.Objects.Project;
using ProjectModel = SocNet_EPX.TemplateModels.Project.Project;

namespace SocNet_EPX.TemplateModels.Project
{
    public class ProjectMappings
    {
        public static void SetUpMappings()
        {
            Mapper.CreateMap<DomainProject, ProjectModel>();
        }
    }
}