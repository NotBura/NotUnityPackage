using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace NotBura.Core
{
#if UNITY_EDITOR
    [DebuggerDisplay("{ToString()}")]
#endif
    [Serializable]
    [StructLayout(LayoutKind.Explicit)]
    public struct UUIDV7
        : IUUID
        , IEquatable<UUIDV7>
        , IComparable<UUIDV7>
        , IFormattable
    {
        [FieldOffset(0), SerializeField] private ulong m_high;
        [FieldOffset(8), SerializeField] private ulong m_low;

        public int Version
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (int)(m_high >> 12) & 0xF;
        }

        public int Variant
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (int)(m_low >> (12 + (16 * 3)));
        }

        // NOTE: Unsafe.SkipInitで確保したエリアに書き込む方が早い(はず)
        public UUIDV7(ulong high, ulong low)
        {
            m_high = high;
            m_low = low;
        }

        #region public method

        public unsafe ref UUID ToUUID()
        {
            fixed (void* pointer = &this)
            {
                return ref *(UUID*)pointer;
            }
        }

        #endregion public method

        #region interface method

        public unsafe bool Equals(UUIDV7 other)
        {
            fixed (void* pointer = &this)
            {
                return UnsafeUtility.MemCmp(pointer, &other, 16) == 0;
            }
        }

        public unsafe int CompareTo(UUIDV7 other)
        {
            fixed (void* pointer = &this)
            {
                return UnsafeUtility.MemCmp(pointer, &other, 16);
            }
        }

        public unsafe string ToString(string format, IFormatProvider formatProvider)
        {
            fixed (void* pointer = &this)
            {
                return IUUID.ToStringLower(pointer);
            }
        }

        #endregion interface method

        #region override method

        [Obsolete("Called boxing method.")]
#pragma warning disable CS0809
        public override bool Equals(object obj)
#pragma warning restore CS0809
        {
            return obj is UUIDV7 cast && Equals(cast);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(m_high, m_low);
        }

        public override unsafe string ToString()
        {
            fixed (void* pointer = &this)
            {
                return IUUID.ToStringLower(pointer);
            }
        }

        #endregion override method

        #region implicit operator

        public static implicit operator UUID(UUIDV7 other)
        {
            // NOTE: unsafeコンテキストで直接ポインタ操作しても速度差がほぼなかった
            return UnsafeUtility.As<UUIDV7, UUID>(ref other);
        }

        public static implicit operator UUIDV7(UUID other)
        {
            return UnsafeUtility.As<UUID, UUIDV7>(ref other);
        }

        #endregion implicit operator
    }
}
