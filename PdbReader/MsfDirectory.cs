// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="MsfDirectory.cs">
//   
// </copyright>
// <summary>
//   
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace PdbReader
{
    /// <summary>
    /// </summary>
    internal class MsfDirectory
    {
        /// <summary>
        /// </summary>
        internal DataStream[] streams;

        /// <summary>
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <param name="head">
        /// </param>
        /// <param name="bits">
        /// </param>
        internal MsfDirectory(PdbReader reader, PdbFileHeader head, BitAccess bits)
        {
            bits.MinCapacity(head.directorySize);
            var pages = reader.PagesFromSize(head.directorySize);

            // 0..n in page of directory pages.
            reader.Seek(head.directoryRoot, 0);
            bits.FillBuffer(reader.reader, pages * 4);

            var stream = new DataStream(head.directorySize, bits, pages);
            bits.MinCapacity(head.directorySize);
            stream.Read(reader, bits);

            // 0..3 in directory pages
            int count;
            bits.ReadInt32(out count);

            // 4..n
            var sizes = new int[count];
            bits.ReadInt32(sizes);

            // n..m
            this.streams = new DataStream[count];
            for (var i = 0; i < count; i++)
            {
                if (sizes[i] <= 0)
                {
                    this.streams[i] = new DataStream();
                }
                else
                {
                    this.streams[i] = new DataStream(sizes[i], bits, reader.PagesFromSize(sizes[i]));
                }
            }
        }
    }
}