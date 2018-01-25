using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Goofy.Common.Extensions
{
    public static class ByteExtensions
    {
        public static MemoryStream ToMemoryStream(this byte[] bytes) => new MemoryStream(bytes);

        public static string GetString(this byte[] bytes, Encoding encoding) => encoding.GetString(bytes);

        public static string GetString(this byte[] bytes) => bytes.GetString(Encoding.UTF8);

        public static string ToBase64String(this byte[] bytes) => Convert.ToBase64String(bytes);

        public static string ToHexString(this byte[] bytes)
        {
            var builder = new StringBuilder();
            foreach (var @byte in bytes)
            {
                builder.Append(@byte.ToString("X2"));
            }
            return builder.ToString();
        }

        public static byte[] ToBytes(this bool num) => BitConverter.GetBytes(num);

        public static byte[] ToBytes(this char num) => BitConverter.GetBytes(num);

        public static byte[] ToBytes(this short num) => BitConverter.GetBytes(num);

        public static byte[] ToBytes(this int num) => BitConverter.GetBytes(num);

        public static byte[] ToBytes(this long num) => BitConverter.GetBytes(num);

        public static byte[] ToBytes(this ushort num) => BitConverter.GetBytes(num);

        public static byte[] ToBytes(this uint num) => BitConverter.GetBytes(num);

        public static byte[] ToBytes(this ulong num) => BitConverter.GetBytes(num);

        public static byte[] ToBytes(this float num) => BitConverter.GetBytes(num);

        public static byte[] ToBytes(this double num) => BitConverter.GetBytes(num);

        private static byte[] ToBytes(IEnumerable<bool> bits, int count)
        {
            var numBytes = count / 8;
            if (count % 8 != 0) numBytes++;

            var bytes = new byte[numBytes];
            int byteIndex = 0, bitIndex = 0;

            foreach (var bit in bits)
            {
                if (bit) bytes[byteIndex] |= (byte)(1 << bitIndex);
                ++bitIndex;
                if (bitIndex == 8)
                {
                    bitIndex = 0;
                    ++byteIndex;
                }

            }
            return bytes;
        }

        public static byte[] ToBytes(this bool[] bits) => ToBytes(bits, bits.Length);

        public static byte[] ToBytes(this List<bool> bits) => ToBytes(bits, bits.Count);

        public static short ToInt16(this byte[] bytes, int startIndex = 0) => BitConverter.ToInt16(bytes, startIndex);
        public static short ReadInt16(this byte[] bytes, ref int startIndex) => ReadUnmanagedStruct<short>(bytes, ref startIndex);

        public static ushort ToUInt16(this byte[] bytes, int startIndex = 0) => BitConverter.ToUInt16(bytes, startIndex);
        public static ushort ReadUInt16(this byte[] bytes, ref int startIndex) => ReadUnmanagedStruct<ushort>(bytes, ref startIndex);

        public static int ToInt32(this byte[] bytes, int startIndex = 0) => BitConverter.ToInt32(bytes, startIndex);
        public static int ReadInt32(this byte[] bytes, ref int startIndex) => ReadUnmanagedStruct<int>(bytes, ref startIndex);

        public static uint ToUInt32(this byte[] bytes, int startIndex = 0) => BitConverter.ToUInt32(bytes, startIndex);
        public static uint ReadUInt32(this byte[] bytes, ref int startIndex) => ReadUnmanagedStruct<uint>(bytes, ref startIndex);

        public static long ToInt64(this byte[] bytes, int startIndex = 0) => BitConverter.ToInt64(bytes, startIndex);
        public static long ReadInt64(this byte[] bytes, ref int startIndex) => ReadUnmanagedStruct<long>(bytes, ref startIndex);

        public static ulong ToUInt64(this byte[] bytes, int startIndex = 0) => BitConverter.ToUInt64(bytes, startIndex);
        public static ulong ReadUInt64(this byte[] bytes, ref int startIndex) => ReadUnmanagedStruct<ulong>(bytes, ref startIndex);

        public static float ToFloat(this byte[] bytes, int startIndex = 0) => BitConverter.ToSingle(bytes, startIndex);
        public static float ReadFloat(this byte[] bytes, ref int startIndex) => ReadUnmanagedStruct<float>(bytes, ref startIndex);

        public static double ToDouble(this byte[] bytes, int startIndex = 0) => BitConverter.ToDouble(bytes, startIndex);
        public static double ReadDouble(this byte[] bytes, ref int startIndex) => ReadUnmanagedStruct<double>(bytes, ref startIndex);

        public static T ReadUnmanagedStruct<T>(this byte[] bytes, ref int startIndex) where T : struct
        {
            var length = Marshal.SizeOf<T>();
            var ptr = Marshal.AllocHGlobal(length);
            Marshal.Copy(bytes, startIndex, ptr, length);
            var obj = Marshal.PtrToStructure<T>(ptr);
            Marshal.FreeHGlobal(ptr);
            startIndex += length;
            return obj;
        }

        public static byte[] ToUnmanagedBytes<T>(this T obj) where T : struct
        {
            var length = Marshal.SizeOf(obj);
            var bufByte = new byte[length];
            var ptr = Marshal.AllocHGlobal(length);
            Marshal.StructureToPtr(obj, ptr, true);
            Marshal.Copy(ptr, bufByte, 0, length);
            Marshal.FreeHGlobal(ptr);
            return bufByte;
        }

        public static void WriteTo(this byte[] bytes, Stream stream) => stream.Write(bytes, 0, bytes.Length);

        public static Task WriteToAsync(this byte[] bytes, Stream stream) => stream.WriteAsync(bytes, 0, bytes.Length);
    }
}
