namespace System.IO
{
    [Flags]
    public enum FileOptions
    {
        None = 0,
        WriteThrough = unchecked((int)0x80000000),
        Asynchronous = unchecked((int)0x40000000), 
        RandomAccess = 0x10000000,
        DeleteOnClose = 0x04000000,
        SequentialScan = 0x08000000,
        Encrypted = 0x00004000, 
    }
}

