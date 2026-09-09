namespace NotBura.Packages
{
    public enum NativeStringEncodingTypes
    {
        UTF16,
        UTF8,
    }

    public static class NativeStringConstants
    {
        public const int MASK_LENGTH = 0x7F_FF_FF_FF;
        public const int MASK_ENCODE = unchecked((int)0x80_00_00_00);
    }
}
