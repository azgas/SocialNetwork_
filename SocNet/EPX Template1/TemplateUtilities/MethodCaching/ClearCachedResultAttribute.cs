using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PostSharp.Aspects;

namespace EPX_Template1.TemplateUtilities.MethodCaching
{
    [Serializable]
    public class ClearCachedResultAttribute : MethodInterceptionAspect
    {
        /// <summary>
        /// Full cached metod name with namespace, e.g. Namespace.Class.Method
        /// </summary>
        public string MethodNameWithNamespace { get; set; }

        /// <summary>
        /// Full list of comma-separated cached method parameters with mapping to cache invalidating method attributes e.g. "cached_method_arg0 > invalidating_method_arg1,cached_method_arg1 > invalidating_method_arg3,". The order of parameters in list must match the order of parameters in the cached method definition.  
        /// </summary>
        public string MethodParameterMapping { get; set; }

        public override void OnInvoke(MethodInterceptionArgs args)
        {
            var cache = MethodResultCache.GetCacheIfExists(MethodNameWithNamespace);
            if (cache != null)
            {
                var methodParams = args.Method.GetParameters().Select(p => new { p.Position, p.Name }).OrderBy(p => p.Position).ToDictionary(e => e.Name, e => e.Position);
                IList<string> attributeParameterNames = this.MethodParameterMapping.Split(',').Select(n => n.Trim()).ToList();

                var attributeParameterEntries = new List<AttributeParameterEntry>();

                foreach (var apm in attributeParameterNames)
                {
                    var map = apm.Split('>').Select(e => e.Trim()).ToList();

                    if (map.Count != 2)
                    {
                        throw new ArgumentException("Invalid parameter mapping definition (\"" + apm + "\") in ClearCachedResultAttribute");
                    }

                    var attributeName = map[1].Split('.')[0];

                    if (methodParams.ContainsKey(attributeName))
                    {
                        //attributeParameterIndexes.Add(methodParams[map[1]]);
                        attributeParameterEntries.Add(new AttributeParameterEntry() { Index = methodParams[attributeName], ParameterDescription = map[1] });
                    }
                    else
                    {
                        throw new ArgumentException("Argument \"" + apm[1] + "\" not found in method definition");
                    }
                }

                var methodArgumentValues = new List<object>();
                foreach (var entry in attributeParameterEntries)
                {
                    object argumentValue = args.Arguments[entry.Index];
                    if (entry.ParameterProperties.Count > 0)
                    {
                        foreach (var property in entry.ParameterProperties)
                        {
                            try
                            {
                                argumentValue = argumentValue.GetType().GetProperty(property).GetValue(argumentValue);
                            }
                            catch
                            {
                                throw new ArgumentException("Invalid property access definition (\"" + entry.ParameterDescription + "\") in ClearCachedResultAttribute");
                            }

                        }
                    }
                    methodArgumentValues.Add(argumentValue);
                }


                cache.RemoveCallResult(methodArgumentValues);
            }
        }

        private sealed class AttributeParameterEntry
        {
            public int Index { get; set; }

            public string ParameterDescription { get; set; }

            public string ParameterName { get { return this.ParameterDescription.Split('.')[0]; } }

            public IList<string> ParameterProperties
            {
                get
                {
                    var splittedDescription = this.ParameterDescription.Split('.');
                    if (splittedDescription.Length > 1)
                    {
                        return splittedDescription.Skip(1).ToList();
                    }
                    else
                    {
                        return new List<string>();
                    }

                }
            }
        }
    }
}