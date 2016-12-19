using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using AutoMapper;
using EPXOS.Contracts.Logic.DataAccess;
using EPXSocNet.TemplateModels.Project;
using EPXSocNet.TemplateUtilities.Localization;

namespace EPXSocNet.Controllers.Template
{
    public class ProjectController : LocalizedController
    {
        private IProjectService projectService;

        public ProjectController(IProjectService projectService)
        {
            this.projectService = projectService;
        }

        [HttpGet]
        public ActionResult GetUserSelectedProject()
        {
            var user = User as ClaimsPrincipal;
            var userId = user.FindFirst("sub").Value;
            var access_token = user.FindFirst("access_token");
            var userSelectedProject = projectService.GetUserSelectedProject(Platform, userId, access_token != null ? access_token.Value : null);
            var selectedProjectName = userSelectedProject != null ? userSelectedProject.Name : null;

            return PartialView("~/TemplateViews/Project/GetUserSelectedProject.cshtml", selectedProjectName);
        }

        [HttpGet]
        public ActionResult GetUserAllowedProjects()
        {
            var user = User as ClaimsPrincipal;
            var userId = user.FindFirst("sub").Value;
            var access_token = user.FindFirst("access_token");
            var projects = Mapper.Map<List<Project>>(projectService.GetUserAllowedProjects(Platform, userId, access_token != null ? access_token.Value : null));

            return PartialView("~/TemplateViews/Project/GetUserAllowedProjects.cshtml", projects);
        }
    }
}