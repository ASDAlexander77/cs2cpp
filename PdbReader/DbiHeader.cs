// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="DbiHeader.cs">
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
    internal struct DbiHeader
    {
        /// <summary>
        /// </summary>
        internal int age; // 8..11

        /// <summary>
        /// </summary>
        internal int dbghdrSize; // 48..51

        /// <summary>
        /// </summary>
        internal int ecinfoSize; // 52..55

        /// <summary>
        /// </summary>
        internal int filinfSize; // 36..39

        /// <summary>
        /// </summary>
        internal ushort flags; // 56..57

        /// <summary>
        /// </summary>
        internal int gpmodiSize; // 24..27

        /// <summary>
        /// </summary>
        internal short gssymStream; // 12..13

        /// <summary>
        /// </summary>
        internal ushort machine; // 58..59

        /// <summary>
        /// </summary>
        internal int mfcIndex; // 44..47

        /// <summary>
        /// </summary>
        internal ushort pdbver; // 18..19

        /// <summary>
        /// </summary>
        internal ushort pdbver2; // 22..23

        /// <summary>
        /// </summary>
        internal short pssymStream; // 16..17

        /// <summary>
        /// </summary>
        internal int reserved; // 60..63

        /// <summary>
        /// </summary>
        internal int secconSize; // 28..31

        /// <summary>
        /// </summary>
        internal int secmapSize; // 32..35

        /// <summary>
        /// </summary>
        internal int sig; // 0..3

        /// <summary>
        /// </summary>
        internal short symrecStream; // 20..21

        /// <summary>
        /// </summary>
        internal int tsmapSize; // 40..43

        /// <summary>
        /// </summary>
        internal int ver; // 4..7

        /// <summary>
        /// </summary>
        internal ushort vers; // 14..15

        /// <summary>
        /// </summary>
        /// <param name="bits">
        /// </param>
        internal DbiHeader(BitAccess bits)
        {
            bits.ReadInt32(out this.sig);
            bits.ReadInt32(out this.ver);
            bits.ReadInt32(out this.age);
            bits.ReadInt16(out this.gssymStream);
            bits.ReadUInt16(out this.vers);
            bits.ReadInt16(out this.pssymStream);
            bits.ReadUInt16(out this.pdbver);
            bits.ReadInt16(out this.symrecStream);
            bits.ReadUInt16(out this.pdbver2);
            bits.ReadInt32(out this.gpmodiSize);
            bits.ReadInt32(out this.secconSize);
            bits.ReadInt32(out this.secmapSize);
            bits.ReadInt32(out this.filinfSize);
            bits.ReadInt32(out this.tsmapSize);
            bits.ReadInt32(out this.mfcIndex);
            bits.ReadInt32(out this.dbghdrSize);
            bits.ReadInt32(out this.ecinfoSize);
            bits.ReadUInt16(out this.flags);
            bits.ReadUInt16(out this.machine);
            bits.ReadInt32(out this.reserved);
        }
    }
}