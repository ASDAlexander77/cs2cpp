// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="PdbReader.cs">
//   
// </copyright>
// <summary>
//   
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace Microsoft.Cci.Pdb
{
    using System.IO;

    /// <summary>
    /// </summary>
    internal class PdbReader
    {
        /// <summary>
        /// </summary>
        internal readonly int pageSize;

        /// <summary>
        /// </summary>
        internal readonly Stream reader;

        /// <summary>
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <param name="pageSize">
        /// </param>
        internal PdbReader(Stream reader, int pageSize)
        {
            this.pageSize = pageSize;
            this.reader = reader;
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
        /// <param name="size">
        /// </param>
        /// <returns>
        /// </returns>
        internal int PagesFromSize(int size)
        {
            return (size + this.pageSize - 1) / this.pageSize;
        }

        /// <summary>
        /// </summary>
        /// <param name="bytes">
        /// </param>
        /// <param name="offset">
        /// </param>
        /// <param name="count">
        /// </param>
        internal void Read(byte[] bytes, int offset, int count)
        {
            this.reader.Read(bytes, offset, count);
        }

        /// <summary>
        /// </summary>
        /// <param name="page">
        /// </param>
        /// <param name="offset">
        /// </param>
        internal void Seek(int page, int offset)
        {
            this.reader.Seek(page * this.pageSize + offset, SeekOrigin.Begin);
        }
    }
}