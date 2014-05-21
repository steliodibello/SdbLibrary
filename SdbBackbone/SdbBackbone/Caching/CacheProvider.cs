using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;

namespace SdbBackbone.Caching
{
    public class CacheProvider : ICacheProvider
    {
        private const int DefaultCacheDurationInMinutes = 1;

        private static Cache Cache { get { return HttpRuntime.Cache; } }
        private int _applicationCacheDuration = -1;

        private int ApplicationCacheDuration
        {
            get
            {
                if (_applicationCacheDuration == -1)
                {

                    _applicationCacheDuration = 5;
                }

                return _applicationCacheDuration;
            }
        }

        public void AddItem(string key, object obj)
        {
            AddItem(key, obj, DefaultCacheDurationInMinutes);
        }

        public object ReadItem(string key)
        {
            return Cache.Get(key);
        }

        public void AddItem(string key, object obj, int minutesDuration)
        {
            Cache.Add(key, obj, null, DateTime.Now.AddMinutes(minutesDuration), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
        }

        public void AddApplicationItem(string key, object obj)
        {
            AddItem(key, obj, ApplicationCacheDuration);
        }

        public T Get<T>(string key, Func<T> function, int minutesDuration) 
        {
            if (ReadItem(key) != null)
                return (T)ReadItem(key);

            var item = function();
            AddItem(key, item, minutesDuration);
            return item;
        }

        public void Clear()
        {
            var keys = new List<string>();

            // retrieve application Cache enumerator
            IDictionaryEnumerator enumerator = Cache.GetEnumerator();

            // copy all keys that currently exist in Cache
            while (enumerator.MoveNext())
            {
                keys.Add(enumerator.Key.ToString());
            }

            // delete every key from cache
            foreach (var t in keys)
            {
                Cache.Remove(t);
            }
        }
    }
}