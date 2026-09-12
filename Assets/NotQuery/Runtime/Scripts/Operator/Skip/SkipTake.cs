using System.Runtime.CompilerServices;

namespace NotBura.Packages
{
    public struct SkipTake<TIterator, T>
        : INotQueryIterator<T>
        where TIterator : INotQueryIterator<T>
    {
        private TIterator m_source;
        private uint m_skipCount;
        private uint m_takeCount;
        private uint m_skipIndex;
        private uint m_takeIndex;

        public SkipTake(TIterator source, uint skipCount, uint takeCount)
        {
            m_source = source;
            m_skipCount = skipCount;
            m_takeCount = takeCount;
            m_skipIndex = 0;
            m_takeIndex = 0;
        }

        public bool TryMoveNext(out T current)
        {
            ref var _iterator = ref m_source;

            if (m_skipIndex < m_skipCount)
            {
                var _index = m_skipIndex;
                var _count = m_skipCount;

                do
                {
                    if (false == _iterator.TryMoveNext(out current))
                    {
                        m_skipIndex = _index;
                        return false;
                    }

                    ++_index;
                }
                while (_index < _count);

                m_skipIndex = _index;
            }

            if (m_takeIndex < m_takeCount && _iterator.TryMoveNext(out current))
            {
                ++m_takeIndex;
                return true;
            }

            Unsafe.SkipInit(out current);
            return false;
        }
    }
}
