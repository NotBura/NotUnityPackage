using System;
using System.Runtime.CompilerServices;

namespace NotBura.Packages
{
    public static class UTF8Helper
    {
        public static unsafe uint CharCount(byte* source, int length)
        {
            if (length is 0)
            {
                return 0;
            }

            const ulong LENGTH_TABLE = 0x_4322_1111_1111_1111UL;

            var _index = 0U;
            var _offset = 0UL;
            var _loop = (ulong)length;

            while (_offset < _loop)
            {
                var _nibble = (*(source + _offset)) >> 4;
                var _count = (LENGTH_TABLE >> (_nibble << 2)) & 0x0F;

                // NOTE: サロゲートペアの場合は追加で1文字加算
                _index += 1U + (uint)(_count >> 2);
                _offset += _count;
            }

            return _index;
        }

        private static unsafe ulong CharCountInternal(byte source)
        {
            const ulong LENGTH_TABLE = 0x_4322_1111_1111_1111UL;

            var _nibble = source >> 4;
            return (LENGTH_TABLE >> (_nibble * 4)) & 0x0F;
        }

        #region byte count

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

                fixed (char* _pointer = source[i])
                {
                    _result += ByteCountInternal(_pointer, source[i].Length);
                }
            }

            return _result;
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

                fixed (char* _pointer = source[i])
                {
                    _result += ByteCountInternal(_pointer, source[i].Length);
                }
            }

            return _result;
        }

        public static unsafe uint ByteCount(string source)
        {
            if (source is null || source.Length is 0)
            {
                return 0;
            }

            fixed (char* _pointer = source)
            {
                return ByteCountInternal(_pointer, source.Length);
            }
        }

        public static unsafe uint ByteCount(char[] source)
        {
            if (source is null || source.Length is 0)
            {
                return 0;
            }

            fixed (char* _pointer = source)
            {
                return ByteCountInternal(_pointer, source.Length);
            }
        }

        public static unsafe uint ByteCount(ReadOnlySpan<char> source)
        {
            if (source.Length is 0)
            {
                return 0;
            }

            fixed (char* _pointer = source)
            {
                return ByteCountInternal(_pointer, source.Length);
            }
        }

        public static unsafe uint ByteCount(char* source, int length)
        {
            if (length is 0)
            {
                return 0;
            }

            return ByteCountInternal(source, length);
        }

        #endregion byte count

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe uint ByteCountInternal(char* source, int length)
        {
            var _result = 0U;

            for (int i = 0; i < length; ++i)
            {
                if (source[i] <= 0x007F)
                {
                    _result += 1;
                    continue;
                }

                if (source[i] <= 0x07FF)
                {
                    _result += 2;
                    continue;
                }

                if (source[i] < 0xD800 || source[i] > 0xDFFF)
                {
                    _result += 3;
                    continue;
                }

                _result += 4;
                ++i;
            }

            return _result;
        }
    }
}
