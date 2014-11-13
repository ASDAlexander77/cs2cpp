// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="PdbScope.cs">
//   
// </copyright>
// <summary>
//   
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace Microsoft.Cci.Pdb
{
    using System;

    /// <summary>
    /// </summary>
    internal class PdbScope
    {
        /// <summary>
        /// </summary>
        internal uint address;

        /// <summary>
        /// </summary>
        internal PdbConstant[] constants;

        /// <summary>
        /// </summary>
        internal uint length;

        /// <summary>
        /// </summary>
        internal PdbScope[] scopes;

        /// <summary>
        /// </summary>
        internal uint segment;

        /// <summary>
        /// </summary>
        internal PdbSlot[] slots;

        /// <summary>
        /// </summary>
        internal string[] usedNamespaces;

        /// <summary>
        /// </summary>
        /// <param name="block">
        /// </param>
        /// <param name="bits">
        /// </param>
        /// <param name="typind">
        /// </param>
        /// <exception cref="PdbException">
        /// </exception>
        /// <exception cref="Exception">
        /// </exception>
        internal PdbScope(BlockSym32 block, BitAccess bits, out uint typind)
        {
            this.segment = block.seg;
            this.address = block.off;
            this.length = block.len;
            typind = 0;

            int constantCount;
            int scopeCount;
            int slotCount;
            int namespaceCount;
            PdbFunction.CountScopesAndSlots(bits, block.end, out constantCount, out scopeCount, out slotCount, out namespaceCount);
            this.constants = new PdbConstant[constantCount];
            this.scopes = new PdbScope[scopeCount];
            this.slots = new PdbSlot[slotCount];
            this.usedNamespaces = new string[namespaceCount];
            var constant = 0;
            var scope = 0;
            var slot = 0;
            var usedNs = 0;

            while (bits.Position < block.end)
            {
                ushort siz;
                ushort rec;

                bits.ReadUInt16(out siz);
                var star = bits.Position;
                var stop = bits.Position + siz;
                bits.Position = star;
                bits.ReadUInt16(out rec);

                switch ((SYM)rec)
                {
                    case SYM.S_BLOCK32:
                        {
                            var sub = new BlockSym32();

                            bits.ReadUInt32(out sub.parent);
                            bits.ReadUInt32(out sub.end);
                            bits.ReadUInt32(out sub.len);
                            bits.ReadUInt32(out sub.off);
                            bits.ReadUInt16(out sub.seg);
                            bits.SkipCString(out sub.name);

                            bits.Position = stop;
                            this.scopes[scope++] = new PdbScope(sub, bits, out typind);
                            break;
                        }

                    case SYM.S_MANSLOT:
                        this.slots[slot++] = new PdbSlot(bits, out typind);
                        bits.Position = stop;
                        break;

                    case SYM.S_UNAMESPACE:
                        bits.ReadCString(out this.usedNamespaces[usedNs++]);
                        bits.Position = stop;
                        break;

                    case SYM.S_END:
                        bits.Position = stop;
                        break;

                    case SYM.S_MANCONSTANT:
                        this.constants[constant++] = new PdbConstant(bits);
                        bits.Position = stop;
                        break;

                    default:
                        throw new PdbException("Unknown SYM in scope {0}", (SYM)rec);

                        // bits.Position = stop;
                }
            }

            if (bits.Position != block.end)
            {
                throw new Exception("Not at S_END");
            }

            ushort esiz;
            ushort erec;
            bits.ReadUInt16(out esiz);
            bits.ReadUInt16(out erec);

            if (erec != (ushort)SYM.S_END)
            {
                throw new Exception("Missing S_END");
            }
        }
    }
}