using System;
using System.Runtime.CompilerServices;

namespace NotBura.Packages
{
    public struct Select<TIterator, TFrom, TTo>
        : INotQueryIterator<TTo>
        where TIterator : struct, INotQueryIterator<TFrom>
    {
        private TIterator m_source;
        private Func<TFrom, TTo> m_selector;

        public Select(TIterator source, Func<TFrom, TTo> selector)
        {
            m_source = source;
            m_selector = selector;
        }

        public bool TryMoveNext(out TTo current)
        {
            ref var _iterator = ref m_source;
            var _selector = m_selector;

            while (_iterator.TryMoveNext(out var _result))
            {
                current = _selector(_result);
                return true;
            }

            Unsafe.SkipInit(out current);
            return false;
        }
    }

    public unsafe struct SelectD<TIterator, TFrom, TTo>
        : INotQueryIterator<TTo>
        where TIterator : struct, INotQueryIterator<TFrom>
    {
        private TIterator m_source;
        private delegate*<in TFrom, TTo> m_selector;

        public SelectD(TIterator source, delegate*<in TFrom, TTo> selector)
        {
            m_source = source;
            m_selector = selector;
        }

        public bool TryMoveNext(out TTo current)
        {
            ref var _iterator = ref m_source;
            var _selector = m_selector;

            while (_iterator.TryMoveNext(out var _result))
            {
                current = _selector(_result);
                return true;
            }

            Unsafe.SkipInit(out current);
            return false;
        }
    }
}
