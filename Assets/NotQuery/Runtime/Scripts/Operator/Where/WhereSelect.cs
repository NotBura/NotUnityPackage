using System;
using System.Runtime.CompilerServices;

namespace NotBura.Packages
{
    public struct WhereSelect<TIterator, TFrom, TTo>
        : INotQueryIterator<TTo>
        where TIterator : struct, INotQueryIterator<TFrom>
    {
        private TIterator m_source;
        private Func<TFrom, bool> m_predicate;
        private Func<TFrom, TTo> m_selector;

        public WhereSelect(TIterator source, Func<TFrom, bool> predicate, Func<TFrom, TTo> selector)
        {
            m_source = source;
            m_predicate = predicate;
            m_selector = selector;
        }

        public bool TryMoveNext(out TTo current)
        {
            ref var _iterator = ref m_source;
            var _predicate = m_predicate;

            while (_iterator.TryMoveNext(out var _result))
            {
                if (_predicate(_result))
                {
                    current = m_selector(_result);
                    return true;
                }
            }

            Unsafe.SkipInit(out current);
            return false;
        }
    }
}
