// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="DbiModuleInfo.cs">
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
    internal class DbiModuleInfo
    {
        /// <summary>
        /// </summary>
        internal int cbLines; // 44..57

        /// <summary>
        /// </summary>
        internal int cbOldLines; // 40..43

        /// <summary>
        /// </summary>
        internal int cbSyms; // 36..39

        /// <summary>
        /// </summary>
        internal short files; // 48..49

        /// <summary>
        /// </summary>
        internal ushort flags; // 32..33

        /// <summary>
        /// </summary>
        internal string moduleName;

        /// <summary>
        /// </summary>
        internal int niCompiler;

        /// <summary>
        /// </summary>
        internal int niSource;

        /// <summary>
        /// </summary>
        internal string objectName;

        /// <summary>
        /// </summary>
        internal uint offsets;

        /// <summary>
        /// </summary>
        internal int opened; // 0..3

        /// <summary>
        /// </summary>
        internal short pad1; // 50..51

        /// <summary>
        /// </summary>
        internal DbiSecCon section; // 4..31

        /// <summary>
        /// </summary>
        internal short stream; // 34..35

        /// <summary>
        /// </summary>
        /// <param name="bits">
        /// </param>
        /// <param name="readStrings">
        /// </param>
        internal DbiModuleInfo(BitAccess bits, bool readStrings)
        {
            bits.ReadInt32(out this.opened);
            this.section = new DbiSecCon(bits);
            bits.ReadUInt16(out this.flags);
            bits.ReadInt16(out this.stream);
            bits.ReadInt32(out this.cbSyms);
            bits.ReadInt32(out this.cbOldLines);
            bits.ReadInt32(out this.cbLines);
            bits.ReadInt16(out this.files);
            bits.ReadInt16(out this.pad1);
            bits.ReadUInt32(out this.offsets);
            bits.ReadInt32(out this.niSource);
            bits.ReadInt32(out this.niCompiler);
            if (readStrings)
            {
                bits.ReadCString(out this.moduleName);
                bits.ReadCString(out this.objectName);
            }
            else
            {
                bits.SkipCString(out this.moduleName);
                bits.SkipCString(out this.objectName);
            }

            bits.Align(4);

            // if (opened != 0 || pad1 != 0) {
            // throw new PdbException("Invalid DBI module. "+
            // "(opened={0}, pad={1})", opened, pad1);
            // }
        }
    }
}