namespace System.IO
{
    using Runtime.CompilerServices;
    using Text;

    public class SafeFileHandle
    {
        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        private extern unsafe static void* fopen(byte* fileName, byte* mode);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        private extern unsafe static int fclose(void* stream);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        private extern unsafe static int fread(void* ptr, int size, int count, void* stream);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        private extern unsafe static int fwrite(void* ptr, int size, int count, void* stream);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        private extern unsafe static int fflush(void* stream);

        private unsafe void* file;

        internal bool IsInvalid
        {
            get
            {
                unsafe
                {
                    return file == null;
                }
            }
        }

        internal bool IsClosed
        {
            get
            {
                unsafe
                {
                    return file == null;
                }
            }
        }

        internal void Dispose()
        {
            if (!this.IsInvalid)
            {
                unsafe
                {
                    fclose(file);
                }
            }
        }

        internal void OpenFile(string fileName, FileMode mode, FileAccess access)
        {
            // convert mode to string
            var sb = new StringBuilder(3, 10);
            switch (mode)
            {
                case FileMode.CreateNew:
                    sb.Append("wb");
                    break;
                case FileMode.Create:
                    sb.Append("wb");
                    break;
                case FileMode.Open:
                    sb.Append("rb");
                    break;
                case FileMode.OpenOrCreate:
                    sb.Append("rb");
                    break;
                case FileMode.Truncate:
                    break;
                case FileMode.Append:
                    sb.Append("ab");
                    break;
                default:
                    break;
            }

            switch (access)
            {
                case FileAccess.Read:
                    break;
                case FileAccess.Write:
                    break;
                case FileAccess.ReadWrite:
                    sb.Append("+");
                    break;
                default:
                    break;
            }

            var fileMode = sb.ToString();

            unsafe
            {
                fixed (byte* fileNamePtr = &Encoding.ASCII.GetBytes(fileName)[0])
                fixed (byte* modePtr = &Encoding.ASCII.GetBytes(fileMode)[0])
                {
                    this.file = fopen(fileNamePtr, modePtr);
                }
            }
        }

        internal int ReadFile(byte[] bytes, int offset, int count)
        {
            unsafe
            {
                fixed (byte* bytesPtr = &bytes[offset])
                {
                    return fread(bytesPtr, sizeof(byte), count, this.file);
                }
            }
        }

        internal int WriteFile(byte[] bytes, int offset, int count)
        {
            unsafe
            {
                fixed (byte* bytesPtr = &bytes[offset])
                {
                    return fwrite(bytesPtr, sizeof(byte), count, this.file);
                }
            }
        }

        public void Flush()
        {
            unsafe
            {
                fflush(this.file);
            }
        }
    }
}
