using System;
using System.Collections.Generic;
using System.Text;

namespace Goofy.Common.Extensions
{

    public static class DateTimeExtensions
    {
        private static readonly DateTime Jan1St1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Unix时间戳 秒
        /// </summary>
        public static long ToTimestamp(this DateTime d)
        {
            return (long)(DateTime.UtcNow - Jan1St1970).TotalSeconds;
        }

        /// <summary>
        /// Unix时间戳 毫秒
        /// </summary>
        public static long ToTimestampMilli(this DateTime d)
        {
            return (long)(DateTime.UtcNow - Jan1St1970).TotalMilliseconds;
        }
    }
}
