namespace System.IO
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Text;

    public class SafeFileHandle
    {
        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        private extern unsafe static void* fopen(byte* fileName, byte* mode);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        private extern unsafe static void* fclose(void* fileHandler);

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
            if (this.IsInvalid)
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
    }
}
