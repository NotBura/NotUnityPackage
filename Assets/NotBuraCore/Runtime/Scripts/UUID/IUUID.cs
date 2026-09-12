using System;
using System.Runtime.CompilerServices;

namespace NotBura.Core
{
    public interface IUUID
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe UUID FromCharSpan(ReadOnlySpan<char> source)
        {
            // TODO: エンディアンに各対応した最速命令に置き換える

            fixed (char* _pointer = source)
            {
                var _high = ReadValue(_pointer, 0 + 0, 8);
                _high <<= 8 * 2;
                _high |= ReadValue(_pointer, 8 + 1, 4);
                _high <<= 8 * 2;
                _high |= ReadValue(_pointer, 12 + 2, 4);

                var _low = ReadValue(_pointer, 16 + 3, 4);
                _low <<= 8 * 6;
                _low |= ReadValue(_pointer, 20 + 4, 12);

                return new(_high, _low);
            }

            static ulong ReadValue(char* source, int offset, int length)
            {
                // NOTE: write byte spanやポインタで実装したものはこれより遅かった
                // 本格的な最適化を行えばより良い実装があるだろうが可読性も考慮し現状とする

                var _result = 0UL;
                uint _buffer;

                const uint SUCTION = unchecked((uint)~('a' - 'A'));

                for (int i = 0; i < length; ++i)
                {
                    _buffer = source[offset + i];
                    _buffer = _buffer <= '9'
                        ? _buffer - '0'
                        : (_buffer & SUCTION) - 'A' + 10U;

                    _result <<= 4;
                    _result |= _buffer;
                }

                return _result;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe string ToStringLower(void* source)
        {
            // TODO: エンディアンに各対応した最速命令に置き換える
            const int LENGTH = 8 + 1 + 4 + 1 + 4 + 1 + 4 + 1 + 12;

            var _result = new string('-', LENGTH);

            fixed (char* _destination = _result)
            {
                SetValue(_destination, 00 + 0, 08, 0x0000_0000_FFFF_FFFF & (*(ulong*)source) >> 32);
                SetValue(_destination, 08 + 1, 04, 0x0000_0000_0000_FFFF & (*(ulong*)source) >> 16);
                SetValue(_destination, 12 + 2, 04, 0x0000_0000_0000_FFFF & (*(ulong*)source));
                SetValue(_destination, 16 + 3, 04, 0x0000_0000_0000_FFFF & (*((ulong*)source + 1)) >> 48);
                SetValue(_destination, 20 + 4, 12, 0x0000_FFFF_FFFF_FFFF & (*((ulong*)source + 1)));
            }

            return _result;

            static void SetValue(char* destination, int offset, int length, ulong value)
            {
                int _buffer;

                // NOTE: テーブルを活用する方法は検証した結果遅かった

                // NOTE: 以下命令は可読性にも優れ速度もかなり早いが分岐予測を最大まで活かす為の処理
                // _buffer < 10 ? _buffer + '0' : (_buffer - 10) + 'a';

                for (int i = 0; i < length; ++i)
                {
                    _buffer = (int)(0xF & (value >> ((length - 1 - i) << 2)));
                    destination[offset + i] = (char)(_buffer + 48 + (((9 - _buffer) >> 31) & 39));
                }
            }
        }
    }
}
