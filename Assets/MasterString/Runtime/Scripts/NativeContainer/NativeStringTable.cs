using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using static NotBura.Packages.NativeStringConstants;

namespace NotBura.Packages
{
    [NativeContainer]
    [NativeContainerIsReadOnly]
#if UNITY_EDITOR
    [DebuggerDisplay("Encode = {" + nameof(IsUTF16) + " ? \"UTF16\" : \"UTF8\"} Length = {" + nameof(Length) + "}")]
    [DebuggerTypeProxy(typeof(NativeStringTableDebugView))]
#endif
    public struct NativeStringTable
        : IDisposable
    {
        internal int m_state;
        internal unsafe void* m_buffer;

        // NOTE: "m_AllocatorLabel"固定である必要がある
        internal Allocator m_AllocatorLabel;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        // NOTE: "m_Safety"固定である必要がある
        internal AtomicSafetyHandle m_Safety;
        private static int s_staticSafetyId = AtomicSafetyHandle.NewStaticSafetyId<NativeStringTable>();
#endif

        public unsafe bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => m_buffer is not null;
        }

        public unsafe bool IsInvalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => m_buffer is null;
        }

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

        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => m_state & MASK_LENGTH;
        }

        public unsafe NativeStringView this[int index]
        {
            get
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                AtomicSafetyHandle.CheckReadAndThrow(m_Safety);
#endif
                var length = (uint)Length;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
                if ((uint)index >= length)
                {
                    throw new IndexOutOfRangeException(nameof(index));
                }
#endif

                var table = (uint*)m_buffer;
                var offset = table[index];
                var state = (int)(table[index + 1] - offset);
                state |= m_state & MASK_ENCODE;

                var pointer = (byte*)((uint*)m_buffer + length + 1) + offset;

                return new(
                    state
                    , pointer
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                    , m_Safety
#endif
                );
            }
        }

        internal unsafe NativeStringTable(int state, void* buffer, Allocator allocator)
        {
            m_state = state;
            m_buffer = buffer;
            m_AllocatorLabel = allocator;
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            m_Safety = AtomicSafetyHandle.Create();
            AtomicSafetyHandle.SetStaticSafetyId(ref m_Safety, s_staticSafetyId);
#endif
        }

        public unsafe void Dispose()
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (m_AllocatorLabel != Allocator.None && false == AtomicSafetyHandle.IsDefaultValue(m_Safety))
            {
                AtomicSafetyHandle.CheckExistsAndThrow(m_Safety);
            }
#endif

            if (IsInvalid)
            {
                return;
            }

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (m_AllocatorLabel == Allocator.Invalid)
            {
                throw new InvalidOperationException("The NativeArray can not be Disposed because it was not allocated with a valid allocator.");
            }

            if (m_AllocatorLabel >= Allocator.FirstUserIndex)
            {
                throw new InvalidOperationException("The NativeArray can not be Disposed because it was allocated with a custom allocator, use CollectionHelper.Dispose in com.unity.collections package.");
            }
#endif

            if (m_AllocatorLabel > Allocator.None)
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                CollectionHelper.DisposeSafetyHandle(ref m_Safety);
#endif
                UnsafeUtility.FreeTracked(m_buffer, m_AllocatorLabel);
                m_AllocatorLabel = Allocator.Invalid;
            }

            m_buffer = null!;
        }

        public unsafe static NativeStringTable FromSource(string[] source, Allocator allocator, NativeStringEncodingTypes encoding)
        {
            var state = source.Length;
            var bufferSize = ((long)source.Length + 1) * sizeof(uint);

            if (encoding == NativeStringEncodingTypes.UTF16)
            {
                bufferSize += UTF16Helper.GetByteCount(source);
                var buffer = UnsafeUtility.MallocTracked(bufferSize, UnsafeUtility.AlignOf<byte>(), allocator, 0);

                var byteOffset = 0U;
                var charOffset = 0U;
                var offset = (uint*)buffer;
                var container = (char*)(void*)(offset + source.Length + 1);

                for (int i = 0; i < source.Length; ++i)
                {
                    var text = source[i];
                    var length = (uint)text.Length;

                    if (text is null || length is 0)
                    {
                        offset[i] = byteOffset;
                        continue;
                    }

                    var size = length << 1;

                    var lhs = container + charOffset;
                    fixed (void* rhs = text)
                    {
                        UnsafeUtility.MemCpy(lhs, rhs, size);
                    }

                    charOffset += length;

                    offset[i] = byteOffset;
                    byteOffset += size;
                }

                offset[source.Length] = byteOffset;

                return new(state, buffer, allocator);
            }
            else
            {
                state |= MASK_ENCODE;

                bufferSize += UTF8Helper.GetByteCount(source);
                var buffer = UnsafeUtility.MallocTracked(bufferSize, UnsafeUtility.AlignOf<byte>(), allocator, 0);

                return new(state, buffer, allocator);
            }
        }
    }
}
