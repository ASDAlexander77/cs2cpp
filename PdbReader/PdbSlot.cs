// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="PdbSlot.cs">
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
    internal class PdbSlot
    {
        /// <summary>
        /// </summary>
        internal uint address;

        /// <summary>
        /// </summary>
        internal ushort flags;

        /// <summary>
        /// </summary>
        internal string name;

        /// <summary>
        /// </summary>
        internal uint segment;

        /// <summary>
        /// </summary>
        internal uint slot;

        /// <summary>
        /// </summary>
        /// <param name="bits">
        /// </param>
        /// <param name="typind">
        /// </param>
        internal PdbSlot(BitAccess bits, out uint typind)
        {
            AttrSlotSym slot;

            bits.ReadUInt32(out slot.index);
            bits.ReadUInt32(out slot.typind);
            bits.ReadUInt32(out slot.offCod);
            bits.ReadUInt16(out slot.segCod);
            bits.ReadUInt16(out slot.flags);
            bits.ReadCString(out slot.name);

            this.slot = slot.index;
            this.name = slot.name;
            this.flags = slot.flags;
            this.segment = slot.segCod;
            this.address = slot.offCod;

            typind = slot.typind;
        }
    }
}