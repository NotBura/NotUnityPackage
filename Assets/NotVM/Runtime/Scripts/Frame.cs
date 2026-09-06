using System;
using System.Runtime.InteropServices;

namespace NotBura.Packages
{
    public struct Frame
    {
        private byte[] m_memory;

        public Frame(byte[] source)
        {
            m_memory = source;
        }

        public unsafe T Read<T>(int offset)
            where T : unmanaged
        {
            var _size = sizeof(T);

            var _raw = m_memory.AsSpan(offset, _size);
            var _result = MemoryMarshal.Read<T>(_raw);

            return _result;
        }
    }
}
