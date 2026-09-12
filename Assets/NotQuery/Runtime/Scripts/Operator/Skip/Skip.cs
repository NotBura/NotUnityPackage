using System.Runtime.CompilerServices;

namespace NotBura.Packages
{
    public struct Skip<TIterator, T>
        : INotQueryIterator<T>
    where TIterator : INotQueryIterator<T>
    {
        private TIterator m_source;
        private uint m_count;
        private uint m_index;

        public Skip(TIterator source, uint count)
        {
            m_source = source;
            m_count = count;
            m_index = 0;
        }

        public bool TryMoveNext(out T current)
        {
            ref var _iterator = ref m_source;

            if (m_index < m_count)
            {
                var _index = m_index;
                var _count = m_count;

                do
                {
                    if (false == _iterator.TryMoveNext(out current))
                    {
                        m_index = _index;
                        return false;
                    }

                    ++_index;
                }
                while (_index < _count);

                m_index = _index;
            }

            return _iterator.TryMoveNext(out current);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SkipTake<TIterator, T> ToTake(uint count)
        {
            return new(m_source, m_count, count);
        }
    }
}
