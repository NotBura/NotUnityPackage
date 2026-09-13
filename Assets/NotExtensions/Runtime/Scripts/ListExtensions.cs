using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NotBura.Packages
{
    public static class ListExtensions
    {
        private sealed class ListPrivateFieldAccess<T>
        {
            internal T[] _items;
            internal int _size;
            internal int _version;
        }

        #region as span

        public static Span<T> AsSpan<T>(this List<T> source)
        {
            var _cast = Unsafe.As<ListPrivateFieldAccess<T>>(source);
            return _cast._items.AsSpan(0, _cast._size);
        }

        public static Span<T> AsSpan<T>(this List<T> source, int start)
        {
            var _cast = Unsafe.As<ListPrivateFieldAccess<T>>(source);
            return _cast._items.AsSpan(start, _cast._size - start);
        }

        public static Span<T> AsSpan<T>(this List<T> source, int start, int length)
        {
            var _cast = Unsafe.As<ListPrivateFieldAccess<T>>(source);
            return _cast._items.AsSpan(0, _cast._size);
        }

        #endregion as span
    }
}
