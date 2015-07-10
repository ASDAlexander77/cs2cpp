namespace System.IO
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Runtime.CompilerServices;
    using System.Text;
    using Microsoft.Win32.SafeHandles;

    // Class for creating FileStream objects, and some basic file management
    // routines such as Delete, etc.
    /// <summary>
    /// </summary>
    [ComVisible(true)]
    public static class File
    {
        private const short F_OK = 0;

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        private extern unsafe static void* remove(byte* fileName);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        private extern unsafe static short access(byte* fileName, short mode);

        /// <summary>
        /// </summary>
        private const int GetFileExInfoStandard = 0;

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public static StreamReader OpenText(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            return new StreamReader(path);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public static StreamWriter CreateText(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            return new StreamWriter(path, false);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public static StreamWriter AppendText(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            return new StreamWriter(path, true);
        }

        // Copies an existing file to a new file. An exception is raised if the
        // destination file already exists. Use the 
        // Copy(String, String, boolean) method to allow 
        // overwriting an existing file.
        // The caller must have certain FileIOPermissions.  The caller must have
        // Read permission to sourceFileName and Create
        // and Write permissions to destFileName.

        /// <summary>
        /// </summary>
        /// <param name="sourceFileName">
        /// </param>
        /// <param name="destFileName">
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        public static void Copy(string sourceFileName, string destFileName)
        {
            if (sourceFileName == null)
            {
                throw new ArgumentNullException("sourceFileName", Environment.GetResourceString("ArgumentNull_FileName"));
            }

            if (destFileName == null)
            {
                throw new ArgumentNullException("destFileName", Environment.GetResourceString("ArgumentNull_FileName"));
            }

            if (sourceFileName.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyFileName"), "sourceFileName");
            }

            if (destFileName.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyFileName"), "destFileName");
            }

            InternalCopy(sourceFileName, destFileName, false, true);
        }

        // Copies an existing file to a new file. If overwrite is 
        // false, then an IOException is thrown if the destination file 
        // already exists.  If overwrite is true, the file is 
        // overwritten.
        // The caller must have certain FileIOPermissions.  The caller must have
        // Read permission to sourceFileName 
        // and Write permissions to destFileName.

        /// <summary>
        /// </summary>
        /// <param name="sourceFileName">
        /// </param>
        /// <param name="destFileName">
        /// </param>
        /// <param name="overwrite">
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        public static void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            if (sourceFileName == null)
            {
                throw new ArgumentNullException("sourceFileName", Environment.GetResourceString("ArgumentNull_FileName"));
            }

            if (destFileName == null)
            {
                throw new ArgumentNullException("destFileName", Environment.GetResourceString("ArgumentNull_FileName"));
            }

            if (sourceFileName.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyFileName"), "sourceFileName");
            }

            if (destFileName.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyFileName"), "destFileName");
            }

            InternalCopy(sourceFileName, destFileName, overwrite, true);
        }

        /// <summary>
        /// </summary>
        /// <param name="sourceFileName">
        /// </param>
        /// <param name="destFileName">
        /// </param>
        /// <param name="overwrite">
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        internal static void UnsafeCopy(string sourceFileName, string destFileName, bool overwrite)
        {
            if (sourceFileName == null)
            {
                throw new ArgumentNullException("sourceFileName", Environment.GetResourceString("ArgumentNull_FileName"));
            }

            if (destFileName == null)
            {
                throw new ArgumentNullException("destFileName", Environment.GetResourceString("ArgumentNull_FileName"));
            }

            if (sourceFileName.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyFileName"), "sourceFileName");
            }

            if (destFileName.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyFileName"), "destFileName");
            }

            InternalCopy(sourceFileName, destFileName, overwrite, false);
        }

        /// <summary>
        /// </summary>
        /// <param name="sourceFileName">
        /// </param>
        /// <param name="destFileName">
        /// </param>
        /// <param name="overwrite">
        /// </param>
        /// <param name="checkHost">
        /// </param>
        /// <devdoc>
        ///    Note: This returns the fully qualified name of the destination file.
        /// </devdoc>
        /// <returns>
        /// </returns>
        internal static string InternalCopy(string sourceFileName, string destFileName, bool overwrite, bool checkHost)
        {
            throw new NotImplementedException();
        }

        // Creates a file in a particular path.  If the file exists, it is replaced.
        // The file is opened with ReadWrite accessand cannot be opened by another 
        // application until it has been closed.  An IOException is thrown if the 
        // directory specified doesn't exist.
        // Your application must have Create, Read, and Write permissions to
        // the file.

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <returns>
        /// </returns>
        public static FileStream Create(string path)
        {
            return Create(path, FileStream.DefaultBufferSize);
        }

        // Creates a file in a particular path.  If the file exists, it is replaced.
        // The file is opened with ReadWrite access and cannot be opened by another 
        // application until it has been closed.  An IOException is thrown if the 
        // directory specified doesn't exist.
        // Your application must have Create, Read, and Write permissions to
        // the file.

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="bufferSize">
        /// </param>
        /// <returns>
        /// </returns>
        public static FileStream Create(string path, int bufferSize)
        {
            return new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None, bufferSize);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="bufferSize">
        /// </param>
        /// <param name="options">
        /// </param>
        /// <returns>
        /// </returns>
        public static FileStream Create(string path, int bufferSize, FileOptions options)
        {
            return new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None, bufferSize, options);
        }

        // Deletes a file. The file specified by the designated path is deleted.
        // If the file does not exist, Delete succeeds without throwing
        // an exception.
        // On NT, Delete will fail for a file that is open for normal I/O
        // or a file that is memory mapped.  
        // Your application must have Delete permission to the target file.

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public static void Delete(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            InternalDelete(path, true);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        internal static void UnsafeDelete(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            InternalDelete(path, false);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="checkHost">
        /// </param>
        internal static void InternalDelete(string path, bool checkHost)
        {
            unsafe
            {
                fixed (byte* p = Encoding.UTF8.GetBytes(path))
                {
                    remove(p);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public static void Decrypt(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public static void Encrypt(string path)
        {
            throw new NotImplementedException();
        }

        // Tests if a file exists. The result is true if the file
        // given by the specified path exists; otherwise, the result is
        // false.  Note that if path describes a directory,
        // Exists will return true.
        // Your application must have Read permission for the target directory.

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool Exists(string path)
        {
            return InternalExistsHelper(path, true);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <returns>
        /// </returns>
        internal static bool UnsafeExists(string path)
        {
            return InternalExistsHelper(path, false);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="checkHost">
        /// </param>
        /// <returns>
        /// </returns>
        private static bool InternalExistsHelper(string path, bool checkHost)
        {
            try
            {
                if (path == null)
                {
                    return false;
                }

                if (path.Length == 0)
                {
                    return false;
                }

                return InternalExists(path);
            }
            catch (ArgumentException)
            {
            }
            catch (NotSupportedException)
            {
            }
 // Security can throw this on ":"
            catch (IOException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }

            return false;
        }

        // auto-generated
        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        internal static bool InternalExists(string path)
        {
            unsafe
            {
                fixed (byte* p = Encoding.UTF8.GetBytes(path))
                {
                    return access(p, F_OK) == 0;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="mode">
        /// </param>
        /// <returns>
        /// </returns>
        public static FileStream Open(string path, FileMode mode)
        {
            return Open(path, mode, mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="mode">
        /// </param>
        /// <param name="access">
        /// </param>
        /// <returns>
        /// </returns>
        public static FileStream Open(string path, FileMode mode, FileAccess access)
        {
            return Open(path, mode, access, FileShare.None);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="mode">
        /// </param>
        /// <param name="access">
        /// </param>
        /// <param name="share">
        /// </param>
        /// <returns>
        /// </returns>
        public static FileStream Open(string path, FileMode mode, FileAccess access, FileShare share)
        {
            return new FileStream(path, mode, access, share);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="creationTime">
        /// </param>
        public static void SetCreationTime(string path, DateTime creationTime)
        {
            SetCreationTimeUtc(path, creationTime.ToUniversalTime());
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="creationTimeUtc">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public static unsafe void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <returns>
        /// </returns>
        public static DateTime GetCreationTime(string path)
        {
            return InternalGetCreationTimeUtc(path, true).ToLocalTime();
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <returns>
        /// </returns>
        public static DateTime GetCreationTimeUtc(string path)
        {
            return InternalGetCreationTimeUtc(path, false); // this API isn't exposed in Silverlight
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="checkHost">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        private static DateTime InternalGetCreationTimeUtc(string path, bool checkHost)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="lastAccessTime">
        /// </param>
        public static void SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            SetLastAccessTimeUtc(path, lastAccessTime.ToUniversalTime());
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="lastAccessTimeUtc">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public static unsafe void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <returns>
        /// </returns>
        public static DateTime GetLastAccessTime(string path)
        {
            return InternalGetLastAccessTimeUtc(path, true).ToLocalTime();
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <returns>
        /// </returns>
        public static DateTime GetLastAccessTimeUtc(string path)
        {
            return InternalGetLastAccessTimeUtc(path, false); // this API isn't exposed in Silverlight
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="checkHost">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        private static DateTime InternalGetLastAccessTimeUtc(string path, bool checkHost)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="lastWriteTime">
        /// </param>
        public static void SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            SetLastWriteTimeUtc(path, lastWriteTime.ToUniversalTime());
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="lastWriteTimeUtc">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public static unsafe void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <returns>
        /// </returns>
        public static DateTime GetLastWriteTime(string path)
        {
            return InternalGetLastWriteTimeUtc(path, true).ToLocalTime();
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <returns>
        /// </returns>
        public static DateTime GetLastWriteTimeUtc(string path)
        {
            return InternalGetLastWriteTimeUtc(path, false); // this API isn't exposed in Silverlight
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="checkHost">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        private static DateTime InternalGetLastWriteTimeUtc(string path, bool checkHost)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <returns>
        /// </returns>
        public static FileStream OpenRead(string path)
        {
            return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <returns>
        /// </returns>
        public static FileStream OpenWrite(string path)
        {
            return new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        public static string ReadAllText(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            if (path.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyPath"));
            }

            return InternalReadAllText(path, Encoding.UTF8, true);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="encoding">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        public static string ReadAllText(string path, Encoding encoding)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            if (path.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyPath"));
            }

            return InternalReadAllText(path, encoding, true);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        internal static string UnsafeReadAllText(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            if (path.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyPath"));
            }

            return InternalReadAllText(path, Encoding.UTF8, false);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="encoding">
        /// </param>
        /// <param name="checkHost">
        /// </param>
        /// <returns>
        /// </returns>
        private static string InternalReadAllText(string path, Encoding encoding, bool checkHost)
        {
            using (StreamReader sr = new StreamReader(path, encoding, true, StreamReader.DefaultBufferSize, checkHost)) return sr.ReadToEnd();
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="contents">
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        public static void WriteAllText(string path, string contents)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            if (path.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyPath"));
            }

            InternalWriteAllText(path, contents, StreamWriter.UTF8NoBOM, true);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="contents">
        /// </param>
        /// <param name="encoding">
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        public static void WriteAllText(string path, string contents, Encoding encoding)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            if (path.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyPath"));
            }

            InternalWriteAllText(path, contents, encoding, true);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="contents">
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        internal static void UnsafeWriteAllText(string path, string contents)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            if (path.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyPath"));
            }

            InternalWriteAllText(path, contents, StreamWriter.UTF8NoBOM, false);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="contents">
        /// </param>
        /// <param name="encoding">
        /// </param>
        /// <param name="checkHost">
        /// </param>
        private static void InternalWriteAllText(string path, string contents, Encoding encoding, bool checkHost)
        {
            using (StreamWriter sw = new StreamWriter(path, false, encoding, StreamWriter.DefaultBufferSize, checkHost)) sw.Write(contents);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <returns>
        /// </returns>
        public static byte[] ReadAllBytes(string path)
        {
            return InternalReadAllBytes(path, true);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <returns>
        /// </returns>
        internal static byte[] UnsafeReadAllBytes(string path)
        {
            return InternalReadAllBytes(path, false);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="checkHost">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="IOException">
        /// </exception>
        private static byte[] InternalReadAllBytes(string path, bool checkHost)
        {
            byte[] bytes;
            using (
                FileStream fs = new FileStream(
                    path, FileMode.Open, FileAccess.Read, FileShare.Read, FileStream.DefaultBufferSize, FileOptions.None, path, false, false, checkHost))
            {
                // Do a blocking read
                int index = 0;
                long fileLength = fs.Length;
                if (fileLength > int.MaxValue)
                {
                    throw new IOException(Environment.GetResourceString("IO.IO_FileTooLong2GB"));
                }

                int count = (int)fileLength;
                bytes = new byte[count];
                while (count > 0)
                {
                    int n = fs.Read(bytes, index, count);
                    if (n == 0)
                    {
                        __Error.EndOfFile();
                    }

                    index += n;
                    count -= n;
                }
            }

            return bytes;
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="bytes">
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        public static void WriteAllBytes(string path, byte[] bytes)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path", Environment.GetResourceString("ArgumentNull_Path"));
            }

            if (path.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyPath"));
            }

            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }

            InternalWriteAllBytes(path, bytes, true);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="bytes">
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        internal static void UnsafeWriteAllBytes(string path, byte[] bytes)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path", Environment.GetResourceString("ArgumentNull_Path"));
            }

            if (path.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyPath"));
            }

            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }

            InternalWriteAllBytes(path, bytes, false);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="bytes">
        /// </param>
        /// <param name="checkHost">
        /// </param>
        private static void InternalWriteAllBytes(string path, byte[] bytes, bool checkHost)
        {
            using (
                FileStream fs = new FileStream(
                    path, FileMode.Create, FileAccess.Write, FileShare.Read, FileStream.DefaultBufferSize, FileOptions.None, path, false, false, checkHost))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        public static string[] ReadAllLines(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            if (path.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyPath"));
            }

            return InternalReadAllLines(path, Encoding.UTF8);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="encoding">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        public static string[] ReadAllLines(string path, Encoding encoding)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            if (path.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyPath"));
            }

            return InternalReadAllLines(path, encoding);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="encoding">
        /// </param>
        /// <returns>
        /// </returns>
        private static string[] InternalReadAllLines(string path, Encoding encoding)
        {
            string line;
            List<string> lines = new List<string>();

            using (StreamReader sr = new StreamReader(path, encoding))
                while ((line = sr.ReadLine()) != null)
                {
                    lines.Add(line);
                }

            return lines.ToArray();
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        public static IEnumerable<string> ReadLines(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            if (path.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyPath"), "path");
            }

            return ReadLinesIterator.CreateIterator(path, Encoding.UTF8);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="encoding">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        public static IEnumerable<string> ReadLines(string path, Encoding encoding)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            if (path.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyPath"), "path");
            }

            return ReadLinesIterator.CreateIterator(path, encoding);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="contents">
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        public static void WriteAllLines(string path, string[] contents)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            if (contents == null)
            {
                throw new ArgumentNullException("contents");
            }

            if (path.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyPath"));
            }

            InternalWriteAllLines(new StreamWriter(path, false, StreamWriter.UTF8NoBOM), contents);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="contents">
        /// </param>
        /// <param name="encoding">
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        public static void WriteAllLines(string path, string[] contents, Encoding encoding)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            if (contents == null)
            {
                throw new ArgumentNullException("contents");
            }

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            if (path.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyPath"));
            }

            InternalWriteAllLines(new StreamWriter(path, false, encoding), contents);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="contents">
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        public static void WriteAllLines(string path, IEnumerable<string> contents)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            if (contents == null)
            {
                throw new ArgumentNullException("contents");
            }

            if (path.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyPath"));
            }

            InternalWriteAllLines(new StreamWriter(path, false, StreamWriter.UTF8NoBOM), contents);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="contents">
        /// </param>
        /// <param name="encoding">
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        public static void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            if (contents == null)
            {
                throw new ArgumentNullException("contents");
            }

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            if (path.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyPath"));
            }

            InternalWriteAllLines(new StreamWriter(path, false, encoding), contents);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="contents">
        /// </param>
        private static void InternalWriteAllLines(TextWriter writer, IEnumerable<string> contents)
        {
            using (writer)
            {
                foreach (string line in contents)
                {
                    writer.WriteLine(line);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="contents">
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        public static void AppendAllText(string path, string contents)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            if (path.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyPath"));
            }

            InternalAppendAllText(path, contents, StreamWriter.UTF8NoBOM);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="contents">
        /// </param>
        /// <param name="encoding">
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        public static void AppendAllText(string path, string contents, Encoding encoding)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            if (path.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyPath"));
            }

            InternalAppendAllText(path, contents, encoding);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="contents">
        /// </param>
        /// <param name="encoding">
        /// </param>
        private static void InternalAppendAllText(string path, string contents, Encoding encoding)
        {
            using (StreamWriter sw = new StreamWriter(path, true, encoding)) sw.Write(contents);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="contents">
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        public static void AppendAllLines(string path, IEnumerable<string> contents)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            if (contents == null)
            {
                throw new ArgumentNullException("contents");
            }

            if (path.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyPath"));
            }

            InternalWriteAllLines(new StreamWriter(path, true, StreamWriter.UTF8NoBOM), contents);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="contents">
        /// </param>
        /// <param name="encoding">
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        public static void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            if (contents == null)
            {
                throw new ArgumentNullException("contents");
            }

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            if (path.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyPath"));
            }

            InternalWriteAllLines(new StreamWriter(path, true, encoding), contents);
        }

        // Moves a specified file to a new location and potentially a new file name.
        // This method does work across volumes.
        // The caller must have certain FileIOPermissions.  The caller must
        // have Read and Write permission to 
        // sourceFileName and Write 
        // permissions to destFileName.

        /// <summary>
        /// </summary>
        /// <param name="sourceFileName">
        /// </param>
        /// <param name="destFileName">
        /// </param>
        public static void Move(string sourceFileName, string destFileName)
        {
            InternalMove(sourceFileName, destFileName, true);
        }

        /// <summary>
        /// </summary>
        /// <param name="sourceFileName">
        /// </param>
        /// <param name="destFileName">
        /// </param>
        internal static void UnsafeMove(string sourceFileName, string destFileName)
        {
            InternalMove(sourceFileName, destFileName, false);
        }

        /// <summary>
        /// </summary>
        /// <param name="sourceFileName">
        /// </param>
        /// <param name="destFileName">
        /// </param>
        /// <param name="checkHost">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        private static void InternalMove(string sourceFileName, string destFileName, bool checkHost)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="sourceFileName">
        /// </param>
        /// <param name="destinationFileName">
        /// </param>
        /// <param name="destinationBackupFileName">
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public static void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName)
        {
            if (sourceFileName == null)
            {
                throw new ArgumentNullException("sourceFileName");
            }

            if (destinationFileName == null)
            {
                throw new ArgumentNullException("destinationFileName");
            }

            InternalReplace(sourceFileName, destinationFileName, destinationBackupFileName, false);
        }

        /// <summary>
        /// </summary>
        /// <param name="sourceFileName">
        /// </param>
        /// <param name="destinationFileName">
        /// </param>
        /// <param name="destinationBackupFileName">
        /// </param>
        /// <param name="ignoreMetadataErrors">
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public static void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
            if (sourceFileName == null)
            {
                throw new ArgumentNullException("sourceFileName");
            }

            if (destinationFileName == null)
            {
                throw new ArgumentNullException("destinationFileName");
            }

            InternalReplace(sourceFileName, destinationFileName, destinationBackupFileName, ignoreMetadataErrors);
        }

        /// <summary>
        /// </summary>
        /// <param name="sourceFileName">
        /// </param>
        /// <param name="destinationFileName">
        /// </param>
        /// <param name="destinationBackupFileName">
        /// </param>
        /// <param name="ignoreMetadataErrors">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        private static void InternalReplace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="access">
        /// </param>
        /// <param name="handle">
        /// </param>
        /// <returns>
        /// </returns>
        private static FileStream OpenFile(string path, FileAccess access, out SafeFileHandle handle)
        {
            FileStream fs = new FileStream(path, FileMode.Open, access, FileShare.ReadWrite, 1);
            handle = fs.SafeFileHandle;

            if (handle.IsInvalid)
            {
                __Error.WinIOError(0, path);
            }

            return fs;
        }
    }
}