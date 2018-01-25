using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Goofy.Common.Extensions
{
    public static class StreamExtensions
    {
        public static string ToString(this Stream stream, Encoding encoding = null)
        {
            using (var sr = new StreamReader(stream, encoding ?? Encoding.UTF8))
            {
                return sr.ReadToEnd();
            }
        }

        public static byte[] ToBytes(this Stream stream)
        {
            var bytes = new byte[stream.Length - stream.Position];
            stream.Read(bytes, (int)stream.Position, bytes.Length);
            return bytes;
        }

        public static void Write(this Stream stream, byte[] bytes) => stream.Write(bytes, 0, bytes.Length);

        public static Task WriteAsync(this Stream stream, byte[] bytes) => stream.WriteAsync(bytes, 0, bytes.Length);
    }
}
