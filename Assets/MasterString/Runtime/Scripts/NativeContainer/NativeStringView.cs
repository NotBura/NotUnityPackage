using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using static NotBura.Packages.NativeStringConstants;

namespace NotBura.Packages
{
    public readonly ref struct NativeStringView
    {
        private readonly int m_state;
        private readonly unsafe void* m_buffer;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        private readonly AtomicSafetyHandle m_safety;
#endif

        public bool IsUTF16
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (m_state & MASK_ENCODE) == 0;
        }

        public bool IsUTF8
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => m_state < 0;
        }

        public int BytesCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => m_state & MASK_LENGTH;
        }

        internal unsafe NativeStringView(
            int state
            , void* buffer
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            , AtomicSafetyHandle safety
#endif
        )
        {
            m_state = state;
            m_buffer = buffer;
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            m_safety = safety;
#endif
        }

        public unsafe override string ToString()
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckReadAndThrow(m_safety);
#endif

            if (IsUTF16)
            {
                return new((char*)m_buffer, 0, BytesCount >> 1);
            }

            return Encoding.UTF8.GetString((byte*)m_buffer, BytesCount);
        }

        public unsafe NativeString ToNativeString(Allocator allocator)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckReadAndThrow(m_safety);
#endif
            return new(m_state, m_buffer, allocator);
        }
    }
}
