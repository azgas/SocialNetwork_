using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EPXSocNet.TemplateModels.User
{
    public class User
    {
        public string Id { get; set; }

        [Display(Name = "FirstName", ResourceType = typeof(EPXSocNet.TemplateResources.User.ManageAccount))]
        public string FirstName { get; set; }

        [Display(Name = "LastName", ResourceType = typeof(EPXSocNet.TemplateResources.User.ManageAccount))]
        public string LastName { get; set; }

        [Display(Name = "Email", ResourceType = typeof(EPXSocNet.TemplateResources.User.ManageAccount))]
        public string Email { get; set; }

        [Display(Name = "PhoneNumber", ResourceType = typeof(EPXSocNet.TemplateResources.User.ManageAccount))]
        public string PhoneNumber { get; set; }

        [Display(Name = "Language", ResourceType = typeof(EPXSocNet.TemplateResources.User.ManageAccount))]
        [Required(ErrorMessageResourceName = "LanguageSelectionIsRequired", ErrorMessageResourceType = typeof(EPXSocNet.TemplateResources.User.ManageAccount))]
        public string Language { get; set; }
    }
}