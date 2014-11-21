// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="DbiSecCon.cs">
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
    internal struct DbiSecCon
    {
        /// <summary>
        /// </summary>
        internal uint dataCrc; // 20..23

        /// <summary>
        /// </summary>
        internal uint flags; // 12..15

        /// <summary>
        /// </summary>
        internal short module; // 16..17

        /// <summary>
        /// </summary>
        internal int offset; // 4..7

        /// <summary>
        /// </summary>
        internal short pad1; // 2..3

        /// <summary>
        /// </summary>
        internal short pad2; // 18..19

        /// <summary>
        /// </summary>
        internal uint relocCrc; // 24..27

        /// <summary>
        /// </summary>
        internal short section; // 0..1

        /// <summary>
        /// </summary>
        internal int size; // 8..11

        /// <summary>
        /// </summary>
        /// <param name="bits">
        /// </param>
        internal DbiSecCon(BitAccess bits)
        {
            bits.ReadInt16(out this.section);
            bits.ReadInt16(out this.pad1);
            bits.ReadInt32(out this.offset);
            bits.ReadInt32(out this.size);
            bits.ReadUInt32(out this.flags);
            bits.ReadInt16(out this.module);
            bits.ReadInt16(out this.pad2);
            bits.ReadUInt32(out this.dataCrc);
            bits.ReadUInt32(out this.relocCrc);

            // if (pad1 != 0 || pad2 != 0) {
            // throw new PdbException("Invalid DBI section. "+
            // "(pad1={0}, pad2={1})",
            // pad1, pad2);
            // }
        }
    }
}