using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PostSharp.Aspects;

namespace EPX_Template1.TemplateUtilities.MethodCaching
{
    [Serializable]
    public class CacheableResultAttribute : MethodInterceptionAspect
    {
        /// <summary>
        /// Cached value expiration period in seconds.
        /// </summary>
        public int ExpirationPeriod { get; set; }

        public override void OnInvoke(MethodInterceptionArgs args)
        {
            var cache = MethodResultCache.GetCache(args.Method, this.ExpirationPeriod);
            var result = cache.GetCachedResult(args.Arguments);
            if (result != null)
            {
                args.ReturnValue = result;
                return;
            }

            base.OnInvoke(args);

            cache.CacheCallResult(args.ReturnValue, args.Arguments);
        }
    }
}