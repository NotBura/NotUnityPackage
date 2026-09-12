using System.Runtime.CompilerServices;
using Unity.Collections;

namespace NotBura.Packages
{
    public struct NativeArrayIterator<T>
        : INotQueryIterator<T>
        where T : unmanaged
    {
        private NativeArray<T> m_source;
        private int m_index;

        public NativeArrayIterator(in NativeArray<T> source)
        {
            m_source = source;
            m_index = 0;
        }

        public bool TryMoveNext(out T current)
        {
            if ((uint)m_index >= (uint)m_source.Length)
            {
                Unsafe.SkipInit(out current);
                return false;
            }

            current = m_source[m_index];
            ++m_index;
            return true;
        }
    }
}
