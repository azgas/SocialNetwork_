using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

using DomainProject = EPXOS.Domain.Objects.Project;
using ProjectModel = EPXSocNet.TemplateModels.Project.Project;

namespace EPXSocNet.TemplateModels.Project
{
    public class ProjectMappings
    {
        public static void SetUpMappings()
        {
            Mapper.CreateMap<DomainProject, ProjectModel>();
        }
    }
}