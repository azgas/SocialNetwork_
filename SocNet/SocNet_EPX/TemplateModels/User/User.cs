using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SocNet_EPX.TemplateModels.User
{
    public class User
    {
        public string Id { get; set; }

        [Display(Name = "FirstName", ResourceType = typeof(SocNet_EPX.TemplateResources.User.ManageAccount))]
        public string FirstName { get; set; }

        [Display(Name = "LastName", ResourceType = typeof(SocNet_EPX.TemplateResources.User.ManageAccount))]
        public string LastName { get; set; }

        [Display(Name = "Email", ResourceType = typeof(SocNet_EPX.TemplateResources.User.ManageAccount))]
        public string Email { get; set; }

        [Display(Name = "PhoneNumber", ResourceType = typeof(SocNet_EPX.TemplateResources.User.ManageAccount))]
        public string PhoneNumber { get; set; }

        [Display(Name = "Language", ResourceType = typeof(SocNet_EPX.TemplateResources.User.ManageAccount))]
        [Required(ErrorMessageResourceName = "LanguageSelectionIsRequired", ErrorMessageResourceType = typeof(SocNet_EPX.TemplateResources.User.ManageAccount))]
        public string Language { get; set; }
    }
}