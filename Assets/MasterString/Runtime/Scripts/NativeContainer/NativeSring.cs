using System;
using System.Runtime.CompilerServices;
using Unity.Collections;

namespace NotBura.Packages
{
    public struct NativeString
        : IDisposable
    {
        private NativeArray<char> _buffer;

        public unsafe NativeString(int state, void* buffer, Allocator allocator)
        {
            _buffer = default;
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
    }
}
