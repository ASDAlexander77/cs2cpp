// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="PdbLine.cs">
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
    internal struct PdbLine
    {
        /// <summary>
        /// </summary>
        internal ushort colBegin;

        /// <summary>
        /// </summary>
        internal ushort colEnd;

        /// <summary>
        /// </summary>
        internal uint lineBegin;

        /// <summary>
        /// </summary>
        internal uint lineEnd;

        /// <summary>
        /// </summary>
        internal uint offset;

        /// <summary>
        /// </summary>
        /// <param name="offset">
        /// </param>
        /// <param name="lineBegin">
        /// </param>
        /// <param name="colBegin">
        /// </param>
        /// <param name="lineEnd">
        /// </param>
        /// <param name="colEnd">
        /// </param>
        internal PdbLine(uint offset, uint lineBegin, ushort colBegin, uint lineEnd, ushort colEnd)
        {
            this.offset = offset;
            this.lineBegin = lineBegin;
            this.colBegin = colBegin;
            this.lineEnd = lineEnd;
            this.colEnd = colEnd;
        }
    }
}