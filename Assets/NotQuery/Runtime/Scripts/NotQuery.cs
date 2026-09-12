using System.Runtime.CompilerServices;

namespace NotBura.Packages
{
    public interface INotQueryIterator<T>
    {
        public bool TryMoveNext(out T current);
    }

    public struct NotQuery<TIterator, T>
        : INotQueryIterator<T>
        where TIterator : struct, INotQueryIterator<T>
    {
        public ref struct Enumerator
        {
            private TIterator m_iterator;
            private T m_current;

            public T Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => m_current;
            }

            public Enumerator(TIterator iterator)
            {
                m_iterator = iterator;
                Unsafe.SkipInit(out m_current);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                return m_iterator.TryMoveNext(out m_current);
            }
        }

        internal TIterator Iterator;

        public NotQuery(TIterator iterator)
        {
            Iterator = iterator;
        }

        public bool TryMoveNext(out T current)
        {
            ref var _iterator = ref Iterator;

            while (_iterator.TryMoveNext(out current))
            {
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator()
        {
            // NOTE: 構造体イテレータが新規に生成される(内部参照は使いまわされる)
            return new(Iterator);
        }
    }
}
