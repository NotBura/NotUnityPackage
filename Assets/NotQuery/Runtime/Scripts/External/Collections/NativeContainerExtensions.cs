using Unity.Collections;

namespace NotBura.Packages
{
    public static class NativeContainerExtensions
    {
        public static NotQuery<NativeListIterator<T>, T> AsQuery<T>(this in NativeList<T> source)
            where T : unmanaged
        {
            return new(new(source));
        }
    }
}
