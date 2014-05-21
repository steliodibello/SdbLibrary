using System;

namespace SdbBackbone.Caching
{
    public interface ICacheProvider
    {
        void AddItem(string key, object obj);
        void AddItem(string key, object obj, int minutesDuration);
        void AddApplicationItem(string key, object obj);
        object ReadItem(string key);
        T Get<T>(string key, Func<T> function, int minutesDuration);
        void Clear();
    }
}