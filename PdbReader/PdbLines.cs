// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="PdbLines.cs">
//   
// </copyright>
// <summary>
//   
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace Microsoft.Cci.Pdb
{
    /// <summary>
    /// </summary>
    internal class PdbLines
    {
        /// <summary>
        /// </summary>
        internal PdbSource file;

        /// <summary>
        /// </summary>
        internal PdbLine[] lines;

        /// <summary>
        /// </summary>
        /// <param name="file">
        /// </param>
        /// <param name="count">
        /// </param>
        internal PdbLines(PdbSource file, uint count)
        {
            this.file = file;
            this.lines = new PdbLine[count];
        }
    }
}