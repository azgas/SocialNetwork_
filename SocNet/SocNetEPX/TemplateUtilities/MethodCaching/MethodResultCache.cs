using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Runtime.Caching;

namespace SocNetEPX.TemplateUtilities.MethodCaching
{
    public class MethodResultCache
    {
        private readonly string _methodName;
        private MemoryCache _cache;
        private readonly TimeSpan _expirationPeriod;
        private static readonly Dictionary<string, MethodResultCache> MethodCaches =
                new Dictionary<string, MethodResultCache>();

        public MethodResultCache(string methodName, int expirationPeriod = 30)
        {
            _methodName = methodName;
            _expirationPeriod = new TimeSpan(0, 0, 0, expirationPeriod);
            _cache = new MemoryCache(methodName);
        }

        private string GetCacheKey(IEnumerable<object> arguments)
        {
            var key = string.Format(
              "{0}({1})",
              _methodName,
              string.Join(", ", arguments.Select(x => x != null ? x.ToString() : "<Null>")));
            return key;
        }

        public void RemoveCallResult(IEnumerable<object> arguments)
        {
            _cache.Remove(GetCacheKey(arguments));
        }

        public void CacheCallResult(object result, IEnumerable<object> arguments)
        {
            _cache.Set(GetCacheKey(arguments), result, DateTimeOffset.Now.Add(_expirationPeriod));
        }

        public object GetCachedResult(IEnumerable<object> arguments)
        {
            return _cache.Get(GetCacheKey(arguments));
        }

        public void ClearCachedResults()
        {
            _cache.Dispose();
            _cache = new MemoryCache(_methodName);
        }

        public static MethodResultCache GetCache(string methodName, int expirationPeriod = 30)
        {
            if (MethodCaches.ContainsKey(methodName))
                return MethodCaches[methodName];
            var cache = new MethodResultCache(methodName, expirationPeriod);
            MethodCaches.Add(methodName, cache);
            return cache;
        }

        public static MethodResultCache GetCacheIfExists(string methodName, int expirationPeriod = 30)
        {
            if (MethodCaches.ContainsKey(methodName))
                return MethodCaches[methodName];

            return null;
        }

        public static MethodResultCache GetCache(MemberInfo methodInfo, int expirationPeriod = 30)
        {
            //Namespace + Class Name + Method Name
            var methodName = string.Format("{0}.{1}.{2}",
                                           methodInfo.ReflectedType.Namespace,
                                           methodInfo.ReflectedType.Name,
                                           methodInfo.Name);
            return GetCache(methodName, expirationPeriod);
        }
    }
}