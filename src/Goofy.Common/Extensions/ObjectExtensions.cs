using System;
using System.Collections.Generic;
using System.Text;

namespace Goofy.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsDefault<T>(this T obj) => obj.Equals(default(T));

        public static bool IsNull<T>(this T obj) => obj == null;

        public static bool IsNotNull<T>(this T obj) => obj != null;

        public static bool IsNullOrDefault<T>(this T obj) => obj == null || obj.Equals(default(T));
        
        public static string SafeToString<T>(this T obj) => obj == null ? string.Empty : obj.ToString();
    }
}
