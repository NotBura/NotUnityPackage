using System;
using System.Runtime.CompilerServices;

namespace NotBura.Packages
{
    public struct Where<TIterator, T>
        : INotQueryIterator<T>
        where TIterator : struct, INotQueryIterator<T>
    {
        private TIterator m_source;
        private Func<T, bool> m_predicate;

        public Where(TIterator source, Func<T, bool> predicate)
        {
            m_source = source;
            m_predicate = predicate;
        }

        public bool TryMoveNext(out T current)
        {
            ref var _iterator = ref m_source;
            var _predicate = m_predicate;

            while (_iterator.TryMoveNext(out var _result))
            {
                if (_predicate(_result))
                {
                    current = _result;
                    return true;
                }
            }

            Unsafe.SkipInit(out current);
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WhereSelect<TIterator, T, TTo> ToSelect<TTo>(Func<T, TTo> selector)
        {
            return new(m_source, m_predicate, selector);
        }
    }
}
