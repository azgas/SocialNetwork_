using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

using DomainProject = EPXOS.Domain.Objects.Project;
using ProjectModel = EPX_Template1.TemplateModels.Project.Project;

namespace EPX_Template1.TemplateModels.Project
{
    public class ProjectMappings
    {
        public static void SetUpMappings()
        {
            Mapper.CreateMap<DomainProject, ProjectModel>();
        }
    }
}