// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="DbiDbgHdr.cs">
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
    internal struct DbiDbgHdr
    {
        /// <summary>
        /// </summary>
        internal ushort snException; // 2..3 (deprecated)

        /// <summary>
        /// </summary>
        internal ushort snFPO; // 0..1

        /// <summary>
        /// </summary>
        internal ushort snFixup; // 4..5

        /// <summary>
        /// </summary>
        internal ushort snNewFPO; // 18..19

        /// <summary>
        /// </summary>
        internal ushort snOmapFromSrc; // 8..9

        /// <summary>
        /// </summary>
        internal ushort snOmapToSrc; // 6..7

        /// <summary>
        /// </summary>
        internal ushort snPdata; // 16..17

        /// <summary>
        /// </summary>
        internal ushort snSectionHdr; // 10..11

        /// <summary>
        /// </summary>
        internal ushort snSectionHdrOrig; // 20..21

        /// <summary>
        /// </summary>
        internal ushort snTokenRidMap; // 12..13

        /// <summary>
        /// </summary>
        internal ushort snXdata; // 14..15

        /// <summary>
        /// </summary>
        /// <param name="bits">
        /// </param>
        internal DbiDbgHdr(BitAccess bits)
        {
            bits.ReadUInt16(out this.snFPO);
            bits.ReadUInt16(out this.snException);
            bits.ReadUInt16(out this.snFixup);
            bits.ReadUInt16(out this.snOmapToSrc);
            bits.ReadUInt16(out this.snOmapFromSrc);
            bits.ReadUInt16(out this.snSectionHdr);
            bits.ReadUInt16(out this.snTokenRidMap);
            bits.ReadUInt16(out this.snXdata);
            bits.ReadUInt16(out this.snPdata);
            bits.ReadUInt16(out this.snNewFPO);
            bits.ReadUInt16(out this.snSectionHdrOrig);
        }
    }
}