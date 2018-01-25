using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Goofy.Common.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue defaultValue = default(TValue))
        {
            return dic.TryGetValue(key, out var value) ? value : defaultValue;
        }

        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dic, IEnumerable<KeyValuePair<TKey, TValue>> pairs)
        {
            foreach (var pair in pairs)
            {
                if (!dic.ContainsKey(pair.Key))
                {
                    dic.Add(pair);
                }
            }
        }

        public static string ToQueryString(this IDictionary<string, string> dic)
        {
            return dic.IsNullOrEmpty() ? string.Empty :
                string.Join("&", dic.Select(item => $"{item.Key.UrlEncode()}={item.Value.UrlEncode()}"));
        }

    }


}
