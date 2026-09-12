using System;
using System.Runtime.CompilerServices;

namespace NotBura.Packages
{
    public static class UTF16Helper
    {
        public static unsafe uint ByteCount(string[] source)
        {
            if (source is null || source.Length is 0)
            {
                return 0;
            }

            var _result = 0U;

            for (int i = 0; i < source.Length; ++i)
            {
                if (source[i] is null)
                {
                    continue;
                }

                _result += (uint)source[i].Length;
            }

            return _result << 1;
        }

        public static unsafe uint ByteCount(ReadOnlySpan<string> source)
        {
            if (source.Length is 0)
            {
                return 0;
            }

            var _result = 0U;

            for (int i = 0; i < source.Length; ++i)
            {
                if (source[i] is null)
                {
                    continue;
                }

                _result += (uint)source[i].Length;
            }

            return _result << 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe uint ByteCount(string source)
        {
            if (source is null)
            {
                return 0;
            }

            return ((uint)source.Length) << 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe uint ByteCount(char[] source)
        {
            if (source is null)
            {
                return 0;
            }

            return ((uint)source.Length) << 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe uint ByteCount(ReadOnlySpan<char> source)
        {
            return ((uint)source.Length) << 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe uint ByteCount(int length)
        {
            return ((uint)length) << 1;
        }
    }
}
