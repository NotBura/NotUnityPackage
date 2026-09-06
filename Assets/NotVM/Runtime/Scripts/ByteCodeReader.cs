using System;
using System.Runtime.InteropServices;

namespace NotBura.Packages
{
    public struct ByteCodeReader
    {
        private byte[] m_memory;
        private int m_offset;

        public ByteCodeReader(byte[] source)
        {
            m_memory = source;
            m_offset = 0;
        }

        public ReadOnlySpan<byte> Read(int length)
        {
            var _result = m_memory.AsSpan(m_offset, length);
            m_offset += length;
            return _result;
        }

        public unsafe T Read<T>()
            where T : unmanaged
        {
            var _size = sizeof(T);

            var _raw = m_memory.AsSpan(m_offset, _size);
            var _result = MemoryMarshal.Read<T>(_raw);
            m_offset += _size;
            return _result;
        }
    }
}
