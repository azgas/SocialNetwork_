using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using EPXOS.Contracts.Logic.DataAccess;
using EPXOS.Logic.DataAccess;
using EPXSocNet.TemplateUtilities.Localization;
using EPXSocNet.TemplateUtilities.ModalWindows;
using EPXSocNet.TemplateUtilities.Authorization;
using EPXSocNet.TemplateModels.User;

using DomainUser = EPXOS.Domain.Objects.User;
using System.Security.Claims;

namespace EPXSocNet.Controllers.Template
{
    public class UserController : LocalizedController
    {
        private IUserService userService;
        private IProjectService projectService;

        public UserController(IUserService userService, IProjectService projectService)
        {
            this.userService = userService;
            this.projectService = projectService;
        }

        [SkipPlatformAuthorization]
        public ActionResult Login()
        {
            return Redirect("/");
        }

        [SkipPlatformAuthorization]
        public ActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut();

            return Redirect("/");
        }

        [SkipPlatformAuthorization]
        public ActionResult GetRepresentation()
        {
            var userId = (User as ClaimsPrincipal).FindFirst("sub").Value;

            var userRepresentation = userService.GetUserRepresentation(userId);

            return PartialView("~/TemplateViews/User/GetRepresentation.cshtml", userRepresentation);
        }

        [SkipPlatformAuthorization]
        [HttpGet]
        public ActionResult ManageAccount()
        {
            var user = User as ClaimsPrincipal;

            var userId = user.FindFirst("sub").Value;
            var access_token = user.FindFirst("access_token");
            var userModel = Mapper.Map<User>(userService.GetUser(userId, access_token != null ? access_token.Value : null));

            ViewBag.Languages = GetLanguagesSelectListItems();

            return PartialView("~/TemplateViews/User/ManageAccount.cshtml", userModel);
        }

        [SkipPlatformAuthorization]
        [HttpPost]
        public ActionResult ManageAccount(User userModel)
        {
            var user = User as ClaimsPrincipal;

            var userId = user.FindFirst("sub").Value;
            var access_token = user.FindFirst("access_token");
            if (ModelState.IsValid)
            {
                userService.UpdateUser(userId, Mapper.Map<DomainUser>(userModel), access_token != null ? access_token.Value : null);

                return this.RedirectToActionForModal("Index", "Home");
            }

            ViewBag.Languages = GetLanguagesSelectListItems();

            return PartialView("~/TemplateViews/User/ManageAccount.cshtml", userModel);
        }

        private List<SelectListItem> GetLanguagesSelectListItems()
        {
            return CultureHelpers.GetAvailableLanguages().Select(l =>
                {
                    return new SelectListItem()
                    {
                        Text = EPXSocNet.TemplateResources.Languages.ResourceManager.GetString(l.ResourceName, Culture),
                        Value = l.Value
                    };
                }).ToList();
        }

        [HttpPost]
        public ActionResult SetUserProject(string projectUri)
        {
            var user = User as ClaimsPrincipal;
            var access_token = user.FindFirst("access_token");
            projectService.SetUserProject(UserId, Platform, projectUri, access_token != null ? access_token.Value : null);

            return Json(Url.Action("Index", "Home"));
        }
    }
}