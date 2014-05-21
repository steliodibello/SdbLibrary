using System.Collections.Generic;

namespace SdbBackbone.Extensions
{
   public static class ListConversions
    {
        public static T To<T>(this IEnumerable<string> strings)
        where T : IList<string>, new()
        {
            var newList = new T();
            foreach (var s in strings)
                newList.Add(s);
            return newList;
        }
    }

    
}
