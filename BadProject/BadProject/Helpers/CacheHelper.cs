using BadProject.Eums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using ThirdParty;

namespace BadProject.Helpers
{
    public class CacheHelper : ICacheHelper
    {
        private static MemoryCache cache = new MemoryCache(
            Assembly.GetExecutingAssembly().GetName().Name,
            new System.Collections.Specialized.NameValueCollection());

        readonly int _cacheInMinutes;

        public CacheHelper(int cacheInMinutes=5)
        {
            _cacheInMinutes = cacheInMinutes;
        }

        public bool IsExists(string key)
        {
            return cache.Contains(key);
        }
        public T Get<T>(string key)
        {
            if (IsExists(key))
            {
               return (T)cache.Get(key);
            }

            return default(T);
        }

        public void Set<T>(string key, T model)
        {
            if(IsExists(key))
            {
                cache.Remove(key);
            }

            cache.Set(key, model, DateTimeOffset.Now.AddMinutes(_cacheInMinutes));
        }

        public void Remove(string key)
        {
            if (IsExists(key))
            {
                cache.Remove(key);
            }
        }
    }
}
