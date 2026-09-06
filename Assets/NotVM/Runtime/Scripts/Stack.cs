using System;
using System.Runtime.InteropServices;
using Unity.Collections;

namespace NotBura.Packages
{
    public struct Stack
        : IDisposable
    {
        private NativeArray<byte> m_memory;
        private int m_offset;

        public Stack(int size)
        {
            m_memory = new(size, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
            m_offset = 0;
        }

        public void Dispose()
        {
            m_memory.Dispose();
        }

        public void Push<T>(T source)
            where T : unmanaged
        {
            var _span = MemoryMarshal.CreateReadOnlySpan(ref source, 1);
            var _binary = MemoryMarshal.AsBytes(_span);

            var _size = _binary.Length;

            var _destination = m_memory.AsSpan().Slice(m_offset, _size);
            _binary.CopyTo(_destination);

            m_offset += _size;
        }

        public Span<byte> Pop(int length)
        {
            var _source = m_memory.AsSpan().Slice(m_offset - length, length);
            m_offset -= length;
            return _source;
        }

        public unsafe T Pop<T>()
            where T : unmanaged
        {
            var _size = sizeof(T);
            var _source = m_memory.AsSpan().Slice(m_offset - _size, _size);
            m_offset -= _size;

            var _cast = MemoryMarshal.Read<T>(_source);
            return _cast;
        }
    }
}
