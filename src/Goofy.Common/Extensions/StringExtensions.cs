using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Goofy.Common.Extensions
{
    public static class StringExtensions
    {
        public static byte[] HexToBytes(this string hex)
        {
            return Enumerable.Range(0, hex.Length / 2)
                .Select(x => Convert.ToByte(hex.Substring(x * 2, 2), 16))
                .ToArray();
        }

        public static byte[] ToUTF8Bytes(this string input) => Encoding.UTF8.GetBytes(input);

        public static byte[] ToBytes(this string input, Encoding encoding) => encoding.GetBytes(input);

        public static string UrlEncode(this string url) => WebUtility.UrlEncode(url);

        public static string UrlDecode(this string url) => WebUtility.UrlDecode(url);

        public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);

        public static bool IsNullOrWhiteSpace(this string str) => string.IsNullOrWhiteSpace(str);

        public static string RegexReplace(this string str, string rex, string replacement) => Regex.Replace(str, rex, replacement);

        public static string GetOrEmpty(this string str) => str ?? "";

        public static string JoinWith(this IEnumerable<string> strs, string separator) => string.Join(separator, strs);

        public static bool ContainsAny(this string src, IEnumerable<string> items) => items.Any(src.Contains);

        public static bool ContainsAll(this string src, IEnumerable<string> items) => items.All(src.Contains);
    }
}
