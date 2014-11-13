// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="DataStream.cs">
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

    /// <summary>
    /// </summary>
    internal class DataStream
    {
        /// <summary>
        /// </summary>
        internal int contentSize;

        /// <summary>
        /// </summary>
        internal int[] pages;

        /// <summary>
        /// </summary>
        internal DataStream()
        {
            this.contentSize = 0;
            this.pages = null;
        }

        /// <summary>
        /// </summary>
        /// <param name="contentSize">
        /// </param>
        /// <param name="bits">
        /// </param>
        /// <param name="count">
        /// </param>
        internal DataStream(int contentSize, BitAccess bits, int count)
        {
            this.contentSize = contentSize;
            if (count > 0)
            {
                this.pages = new int[count];
                bits.ReadInt32(this.pages);
            }
        }

        /// <summary>
        /// </summary>
        internal int Length
        {
            get
            {
                return this.contentSize;
            }
        }

        /// <summary>
        /// </summary>
        internal int Pages
        {
            get
            {
                return this.pages == null ? 0 : this.pages.Length;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="index">
        /// </param>
        /// <returns>
        /// </returns>
        internal int GetPage(int index)
        {
            return this.pages[index];
        }

        /// <summary>
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <param name="bits">
        /// </param>
        internal void Read(PdbReader reader, BitAccess bits)
        {
            bits.MinCapacity(this.contentSize);
            this.Read(reader, 0, bits.Buffer, 0, this.contentSize);
        }

        /// <summary>
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <param name="position">
        /// </param>
        /// <param name="bytes">
        /// </param>
        /// <param name="offset">
        /// </param>
        /// <param name="data">
        /// </param>
        /// <exception cref="PdbException">
        /// </exception>
        internal void Read(PdbReader reader, int position, byte[] bytes, int offset, int data)
        {
            if (position + data > this.contentSize)
            {
                throw new PdbException("DataStream can't read off end of stream. " + "(pos={0},siz={1})", position, data);
            }

            if (position == this.contentSize)
            {
                return;
            }

            var left = data;
            var page = position / reader.pageSize;
            var rema = position % reader.pageSize;

            // First get remained of first page.
            if (rema != 0)
            {
                var todo = reader.pageSize - rema;
                if (todo > left)
                {
                    todo = left;
                }

                reader.Seek(this.pages[page], rema);
                reader.Read(bytes, offset, todo);

                offset += todo;
                left -= todo;
                page++;
            }

            // Now get the remaining pages.
            while (left > 0)
            {
                var todo = reader.pageSize;
                if (todo > left)
                {
                    todo = left;
                }

                reader.Seek(this.pages[page], 0);
                reader.Read(bytes, offset, todo);

                offset += todo;
                left -= todo;
                page++;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="bytes">
        /// </param>
        internal void Write(PdbWriter writer, byte[] bytes)
        {
            this.Write(writer, bytes, bytes.Length);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="bytes">
        /// </param>
        /// <param name="data">
        /// </param>
        internal void Write(PdbWriter writer, byte[] bytes, int data)
        {
            if (bytes == null || data == 0)
            {
                return;
            }

            var left = data;
            var used = 0;
            var rema = this.contentSize % writer.pageSize;
            if (rema != 0)
            {
                var todo = writer.pageSize - rema;
                if (todo > left)
                {
                    todo = left;
                }

                var lastPage = this.pages[this.pages.Length - 1];
                writer.Seek(lastPage, rema);
                writer.Write(bytes, used, todo);
                used += todo;
                left -= todo;
            }

            if (left > 0)
            {
                var count = (left + writer.pageSize - 1) / writer.pageSize;
                var page0 = writer.AllocatePages(count);

                writer.Seek(page0, 0);
                writer.Write(bytes, used, left);

                this.AddPages(page0, count);
            }

            this.contentSize += data;
        }

        /// <summary>
        /// </summary>
        /// <param name="page0">
        /// </param>
        /// <param name="count">
        /// </param>
        private void AddPages(int page0, int count)
        {
            if (this.pages == null)
            {
                this.pages = new int[count];
                for (var i = 0; i < count; i++)
                {
                    this.pages[i] = page0 + i;
                }
            }
            else
            {
                var old = this.pages;
                var used = old.Length;

                this.pages = new int[used + count];
                Array.Copy(old, this.pages, used);
                for (var i = 0; i < count; i++)
                {
                    this.pages[used + i] = page0 + i;
                }
            }
        }
    }
}