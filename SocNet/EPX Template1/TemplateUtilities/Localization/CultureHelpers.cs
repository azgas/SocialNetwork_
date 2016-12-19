using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPX_Template1.TemplateUtilities.Localization
{
    public static class CultureHelpers
    {
        private static readonly IList<string> validCultures =
            new List<string>
            {
                "pl",
                "pl-PL",
                "en",
                "en-GB",
                "en-US"
            };

        private static readonly IList<string> cultures =
            new List<string>
            {
                "pl-PL",
                "en-GB",
                "en-US"
            };

        private static readonly IList<Language> availableLanguages =
            new List<Language>
            {
                new Language() { Value = "pl-PL", ResourceName = "Polish" },
                new Language() { Value = "en-US", ResourceName = "English" }
            };

        public static IList<string> GetCultures()
        {
            return cultures;
        }

        public static string GetImplementedCulture(string cultureName)
        {
            if (string.IsNullOrEmpty(cultureName))
            {
                return GetDefaultCulture();
            }

            if (validCultures.Where(c => c.Equals(cultureName, StringComparison.InvariantCultureIgnoreCase)).Count() == 0)
            {
                return GetDefaultCulture();
            }

            if (cultures.Where(c => c.Equals(cultureName, StringComparison.InvariantCultureIgnoreCase)).Count() > 0)
            {
                return cultureName;
            }

            string neutralCultureName = GetNeutralCulture(cultureName);

            foreach (string culture in cultures)
            {
                if (culture.StartsWith(neutralCultureName))
                {
                    return culture;
                }
            }

            return GetDefaultCulture();
        }

        private static string GetDefaultCulture()
        {
            return cultures[0];
        }

        public static string GetNeutralCulture(string name)
        {
            if (!name.Contains("-"))
            {
                return name;
            }

            return name.Split('-')[0];
        }

        public static IList<Language> GetAvailableLanguages()
        {
            return availableLanguages;
        }
    }
}
