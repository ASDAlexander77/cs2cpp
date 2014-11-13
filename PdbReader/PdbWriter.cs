// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="PdbWriter.cs">
//   
// </copyright>
// <summary>
//   
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace PdbReader
{
    using System;
    using System.IO;

    /// <summary>
    /// </summary>
    internal class PdbWriter
    {
        /// <summary>
        /// </summary>
        internal readonly int pageSize;

        /// <summary>
        /// </summary>
        private int usedBytes;

        /// <summary>
        /// </summary>
        private readonly Stream writer;

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="pageSize">
        /// </param>
        internal PdbWriter(Stream writer, int pageSize)
        {
            this.pageSize = pageSize;
            this.usedBytes = pageSize * 3;
            this.writer = writer;

            writer.SetLength(this.usedBytes);
        }

        /// <summary>
        /// </summary>
        internal int PageSize
        {
            get
            {
                return this.pageSize;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="count">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        internal int AllocatePages(int count)
        {
            var begin = this.usedBytes;

            this.usedBytes += count * this.pageSize;
            this.writer.SetLength(this.usedBytes);

            if (this.usedBytes > this.pageSize * this.pageSize * 8)
            {
                throw new Exception("PdbWriter does not support multiple free maps.");
            }

            return begin / this.pageSize;
        }

        /// <summary>
        /// </summary>
        /// <param name="page">
        /// </param>
        /// <param name="offset">
        /// </param>
        internal void Seek(int page, int offset)
        {
            this.writer.Seek(page * this.pageSize + offset, SeekOrigin.Begin);
        }

        /// <summary>
        /// </summary>
        /// <param name="bytes">
        /// </param>
        /// <param name="offset">
        /// </param>
        /// <param name="count">
        /// </param>
        internal void Write(byte[] bytes, int offset, int count)
        {
            this.writer.Write(bytes, offset, count);
        }

        /// <summary>
        /// </summary>
        /// <param name="streams">
        /// </param>
        /// <param name="bits">
        /// </param>
        internal void WriteMeta(DataStream[] streams, BitAccess bits)
        {
            var head = new PdbFileHeader(this.pageSize);

            this.WriteDirectory(streams, out head.directoryRoot, out head.directorySize, bits);
            this.WriteFreeMap();

            head.freePageMap = 2;
            head.pagesUsed = this.usedBytes / this.pageSize;

            this.writer.Seek(0, SeekOrigin.Begin);
            head.Write(this.writer, bits);
        }

        /// <summary>
        /// </summary>
        /// <param name="streams">
        /// </param>
        /// <param name="directoryRoot">
        /// </param>
        /// <param name="directorySize">
        /// </param>
        /// <param name="bits">
        /// </param>
        private void WriteDirectory(DataStream[] streams, out int directoryRoot, out int directorySize, BitAccess bits)
        {
            var directory = new DataStream();

            var pages = 0;
            for (var s = 0; s < streams.Length; s++)
            {
                if (streams[s].Length > 0)
                {
                    pages += streams[s].Pages;
                }
            }

            var use = 4 * (1 + streams.Length + pages);
            bits.MinCapacity(use);
            bits.WriteInt32(streams.Length);
            for (var s = 0; s < streams.Length; s++)
            {
                bits.WriteInt32(streams[s].Length);
            }

            for (var s = 0; s < streams.Length; s++)
            {
                if (streams[s].Length > 0)
                {
                    bits.WriteInt32(streams[s].pages);
                }
            }

            directory.Write(this, bits.Buffer, use);
            directorySize = directory.Length;

            use = 4 * directory.Pages;
            bits.MinCapacity(use);
            bits.WriteInt32(directory.pages);

            var ddir = new DataStream();
            ddir.Write(this, bits.Buffer, use);

            directoryRoot = ddir.pages[0];
        }

        /// <summary>
        /// </summary>
        private void WriteFreeMap()
        {
            var buffer = new byte[this.pageSize];

            // We configure the old free map with only the first 3 pages allocated.
            buffer[0] = 0xf8;
            for (var i = 1; i < this.pageSize; i++)
            {
                buffer[i] = 0xff;
            }

            this.Seek(1, 0);
            this.Write(buffer, 0, this.pageSize);

            // We configure the new free map with all of the used pages gone.
            var count = this.usedBytes / this.pageSize;
            var full = count / 8;
            for (var i = 0; i < full; i++)
            {
                buffer[i] = 0;
            }

            var rema = count % 8;
            buffer[full] = (byte)(0xff << rema);

            this.Seek(2, 0);
            this.Write(buffer, 0, this.pageSize);
        }

        //////////////////////////////////////////////////////////////////////
    }
}