using System;
using System.Collections.Generic;

namespace SdbBackbone.Caching
{
    public class FakeCache : ICacheProvider
    {
        private readonly Dictionary<string, object> _cache = new Dictionary<string, object>(); 
        
        public void AddItem(string key, object obj)
        {
            if(_cache.ContainsKey(key))
                return;
            
            _cache.Add(key, obj);    
        }

        public void AddItem(string key, object obj, int minutesDuration)
        {
            AddItem(key, obj);
        }

        public object ReadItem(string key)
        {
            return (_cache.ContainsKey(key)) ? _cache[key] : null;
        }

        public void AddApplicationItem(string key, object obj)
        {
            AddItem(key, obj);
        }

        public T Get<T>(string key, Func<T> function, int minutesDuration)
        {
            return function();
        }

        public void Clear()
        {
            foreach (var o in _cache)
            {
                _cache.Remove(o.Key);
            }
        }
    }
}
