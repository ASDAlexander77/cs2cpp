using System.Runtime.InteropServices;

namespace System.IO
{
    using System.Runtime.CompilerServices;

    using Collections.Generic;
    using Text;

    [ComVisible(true)]
    public static class File
    {
        private const short F_OK = 0;

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        private extern unsafe static void* remove(byte* fileName);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        private extern unsafe static short access(byte* fileName, short mode);

        public static bool Exists(String path)
        {
            if (path == null)
                return false;
            if (path.Length == 0)
                return false;

            unsafe
            {
                fixed (byte* p = Encoding.UTF8.GetBytes(path))
                {
                    return access(p, F_OK) == 0;
                }
            }
        }

        public static void Delete(String path)
        {
            if (path == null) throw new ArgumentNullException("path");

            unsafe
            {
                fixed (byte* p = Encoding.UTF8.GetBytes(path))
                {
                    remove(p);
                }
            }
        }

        public static byte[] ReadAllBytes(String path)
        {
            return InternalReadAllBytes(path, true);
        }

        private static byte[] InternalReadAllBytes(String path, bool checkHost)
        {
            byte[] bytes;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read,
                FileStream.DefaultBufferSize, FileOptions.None, path, false, false, checkHost))
            {
                // Do a blocking read
                int index = 0;
                long fileLength = fs.Length;
                if (fileLength > Int32.MaxValue)
                    throw new IOException("FileTooLong2GB");
                int count = (int)fileLength;
                bytes = new byte[count];
                while (count > 0)
                {
                    int n = fs.Read(bytes, index, count);
                    if (n == 0)
                        __Error.EndOfFile();
                    index += n;
                    count -= n;
                }
            }
            return bytes;
        }

        public static void WriteAllBytes(String path, byte[] bytes)
        {
            if (path == null)
                throw new ArgumentNullException("path", "Path");
            if (path.Length == 0)
                throw new ArgumentException("EmptyPath");
            if (bytes == null)
                throw new ArgumentNullException("bytes");

            InternalWriteAllBytes(path, bytes, true);
        }

        internal static void UnsafeWriteAllBytes(String path, byte[] bytes)
        {
            if (path == null)
                throw new ArgumentNullException("path", "Path");
            if (path.Length == 0)
                throw new ArgumentException("EmptyPath");
            if (bytes == null)
                throw new ArgumentNullException("bytes");

            InternalWriteAllBytes(path, bytes, false);
        }

        private static void InternalWriteAllBytes(String path, byte[] bytes, bool checkHost)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read,
                    FileStream.DefaultBufferSize, FileOptions.None, path, false, false, checkHost))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
        }

        public static String[] ReadAllLines(String path)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            if (path.Length == 0)
                throw new ArgumentException("EmptyPath");

            return InternalReadAllLines(path, Encoding.UTF8);
        }

        public static String[] ReadAllLines(String path, Encoding encoding)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            if (encoding == null)
                throw new ArgumentNullException("encoding");
            if (path.Length == 0)
                throw new ArgumentException("EmptyPath");

            return InternalReadAllLines(path, encoding);
        }

        private static String[] InternalReadAllLines(String path, Encoding encoding)
        {
            String line;
            List<String> lines = new List<String>();

            using (StreamReader sr = new StreamReader(path, encoding))
                while ((line = sr.ReadLine()) != null)
                    lines.Add(line);

            return lines.ToArray();
        }

        public static IEnumerable<String> ReadLines(String path)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            if (path.Length == 0)
                throw new ArgumentException("EmptyPath", "path");

            return ReadLinesIterator.CreateIterator(path, Encoding.UTF8);
        }

        public static IEnumerable<String> ReadLines(String path, Encoding encoding)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            if (encoding == null)
                throw new ArgumentNullException("encoding");
            if (path.Length == 0)
                throw new ArgumentException("EmptyPath", "path");

            return ReadLinesIterator.CreateIterator(path, encoding);
        }

        public static void WriteAllLines(String path, String[] contents)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            if (contents == null)
                throw new ArgumentNullException("contents");
            if (path.Length == 0)
                throw new ArgumentException("EmptyPath");

            InternalWriteAllLines(new StreamWriter(path, false, StreamWriter.UTF8NoBOM), contents);
        }

        public static void WriteAllLines(String path, String[] contents, Encoding encoding)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            if (contents == null)
                throw new ArgumentNullException("contents");
            if (encoding == null)
                throw new ArgumentNullException("encoding");
            if (path.Length == 0)
                throw new ArgumentException("EmptyPath");

            InternalWriteAllLines(new StreamWriter(path, false, encoding), contents);
        }

        public static void WriteAllLines(String path, IEnumerable<String> contents)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            if (contents == null)
                throw new ArgumentNullException("contents");
            if (path.Length == 0)
                throw new ArgumentException("EmptyPath");

            InternalWriteAllLines(new StreamWriter(path, false, StreamWriter.UTF8NoBOM), contents);
        }

        public static void WriteAllLines(String path, IEnumerable<String> contents, Encoding encoding)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            if (contents == null)
                throw new ArgumentNullException("contents");
            if (encoding == null)
                throw new ArgumentNullException("encoding");
            if (path.Length == 0)
                throw new ArgumentException("EmptyPath");

            InternalWriteAllLines(new StreamWriter(path, false, encoding), contents);
        }

        private static void InternalWriteAllLines(TextWriter writer, IEnumerable<String> contents)
        {
            using (writer)
            {
                foreach (String line in contents)
                {
                    writer.WriteLine(line);
                }
            }
        }

        public static void AppendAllText(String path, String contents)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            if (path.Length == 0)
                throw new ArgumentException("EmptyPath");

            InternalAppendAllText(path, contents, StreamWriter.UTF8NoBOM);
        }

        public static void AppendAllText(String path, String contents, Encoding encoding)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            if (encoding == null)
                throw new ArgumentNullException("encoding");
            if (path.Length == 0)
                throw new ArgumentException("EmptyPath");

            InternalAppendAllText(path, contents, encoding);
        }

        private static void InternalAppendAllText(String path, String contents, Encoding encoding)
        {
            using (StreamWriter sw = new StreamWriter(path, true, encoding))
                sw.Write(contents);
        }

        public static void AppendAllLines(String path, IEnumerable<String> contents)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            if (contents == null)
                throw new ArgumentNullException("contents");
            if (path.Length == 0)
                throw new ArgumentException("EmptyPath");

            InternalWriteAllLines(new StreamWriter(path, true, StreamWriter.UTF8NoBOM), contents);
        }

        public static void AppendAllLines(String path, IEnumerable<String> contents, Encoding encoding)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            if (contents == null)
                throw new ArgumentNullException("contents");
            if (encoding == null)
                throw new ArgumentNullException("encoding");
            if (path.Length == 0)
                throw new ArgumentException("EmptyPath");

            InternalWriteAllLines(new StreamWriter(path, true, encoding), contents);
        }
    }
}