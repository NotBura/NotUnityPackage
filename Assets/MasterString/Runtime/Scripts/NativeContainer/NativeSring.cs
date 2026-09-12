using System;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using static NotBura.Packages.NativeStringConstants;

namespace NotBura.Packages
{
    public struct NativeString
        : IDisposable
    {
        private NativeArray<char> _buffer;

        public char this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _buffer[index];
        }

        public unsafe NativeString(int state, void* buffer, Allocator allocator)
        {
            var _byteCount = state & MASK_LENGTH;

            if ((state & MASK_ENCODE) == 0)
            {
                var _result = new NativeArray<char>(_byteCount >> 1, allocator, NativeArrayOptions.UninitializedMemory);

                UnsafeUtility.MemCpy(_result.GetUnsafePtr(), buffer, _byteCount);

                _buffer = _result;
            }
            else
            {
                var _pointer = (byte*)buffer;
                var _length = (int)UTF8Helper.CharCount(_pointer, _byteCount);
                var _result = new NativeArray<char>(_length, allocator, NativeArrayOptions.UninitializedMemory);

                Encoding.UTF8.GetChars(_pointer, _byteCount, (char*)_result.GetUnsafePtr(), _length);

                _buffer = _result;
            }
        }

        public void Dispose()
        {
            _buffer.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<char> AsSpan()
        {
            return _buffer.AsReadOnlySpan();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<char> AsSpan(int start)
        {
            return _buffer.AsReadOnlySpan().Slice(start);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<char> AsSpan(int start, int length)
        {
            return _buffer.AsReadOnlySpan().Slice(start, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return _buffer.AsReadOnlySpan().ToString();
        }
    }
}
