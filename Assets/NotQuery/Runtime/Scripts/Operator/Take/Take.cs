using System.Runtime.CompilerServices;

namespace NotBura.Packages
{
    public struct Take<TIterator, T>
        : INotQueryIterator<T>
        where TIterator : INotQueryIterator<T>
    {
        private TIterator m_source;
        private uint m_count;
        private uint m_index;

        public Take(TIterator source, uint count)
        {
            m_source = source;
            m_count = count;
            m_index = 0;
        }

        public bool TryMoveNext(out T current)
        {
            if (m_index < m_count && m_source.TryMoveNext(out current))
            {
                ++m_index;
                return true;
            }

            Unsafe.SkipInit(out current);
            return false;
        }
    }
}
