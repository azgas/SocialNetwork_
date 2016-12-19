using EPXSocNet.TemplateUtilities.Exceptions;
using EPXSocNet.TemplateUtilities.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace EPXSocNet.TemplateResources
{
    public static class ResourceCollection
    {
        private static readonly Dictionary<string, Dictionary<string, string>> resources;

        static ResourceCollection()
        {
            resources = new Dictionary<string, Dictionary<string, string>>();

            CultureInfo originalCulture = Thread.CurrentThread.CurrentUICulture;

            foreach (var cultureName in CultureHelpers.GetCultures())
            {
                InitializeResourcesForCulture(cultureName);
            }

            Thread.CurrentThread.CurrentUICulture = originalCulture;
        }

        private static void InitializeResourcesForCulture(string cultureName)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureName);

            Assembly assembly = Assembly.GetCallingAssembly();

            System.CodeDom.Compiler.GeneratedCodeAttribute attribute = null;
            IEnumerable<Type> resourceTypes =
                from t in assembly.GetTypes()
                where (attribute = (System.CodeDom.Compiler.GeneratedCodeAttribute)Attribute.GetCustomAttribute(t,
                    typeof(System.CodeDom.Compiler.GeneratedCodeAttribute))) != null
                && attribute.Tool == "System.Resources.Tools.StronglyTypedResourceBuilder"
                select t;

            string assemblyName = assembly.GetName().Name;
            var cultureResources = new Dictionary<string, string>();
            foreach (Type type in resourceTypes)
            {
                string fullTypeName = type.FullName;
                string keyBase = null;

                int assemblyNameSubstringIndex = fullTypeName.IndexOf(assemblyName);
                keyBase = assemblyNameSubstringIndex < 0
                    ? fullTypeName
                    : fullTypeName.Remove(assemblyNameSubstringIndex, assemblyName.Length);

                if (keyBase.StartsWith("."))
                {
                    keyBase = keyBase.Remove(0, 1);
                }

                if (keyBase.StartsWith("Resources."))
                {
                    keyBase = keyBase.Remove(0, 10);
                }
                else if (keyBase.StartsWith("TemplateResources."))
                {
                    keyBase = keyBase.Remove(8, 9);
                }

                IEnumerable<PropertyInfo> currentTypeResources = type.GetProperties()
                    .Where(p => !p.Name.Equals("Resources") && !p.Name.Equals("Culture") && !p.Name.Equals("ResourceManager"));

                foreach (var resource in currentTypeResources)
                {
                    string key = String.Format("{0}_{1}", keyBase, resource.Name).Replace('.', '_');

                    if (cultureResources.ContainsKey(key))
                    {
                        throw new DuplicateResourceNameException(string.Format("Duplicate resource key found: {0}", key));
                    }

                    cultureResources.Add(key, resource.GetValue(null) as string);
                }
            }

            resources.Add(cultureName, cultureResources);
        }

        public static IDictionary<string, string> GetResources(CultureInfo culture)
        {
            return resources[CultureHelpers.GetImplementedCulture(culture.Name)];
        }

        public static string GetResource(string key, CultureInfo culture)
        {
            string cultureName = CultureHelpers.GetImplementedCulture(culture.Name);

            if (resources[cultureName].ContainsKey(key))
            {
                return resources[cultureName][key];
            }

            return null;
        }
    }
}
