// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="PdbSource.cs">
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
    internal class PdbSource
    {
        /// <summary>
        /// </summary>
        internal Guid doctype;

        /// <summary>
        /// </summary>
        internal uint index;

        /// <summary>
        /// </summary>
        internal Guid language;

        /// <summary>
        /// </summary>
        internal string name;

        /// <summary>
        /// </summary>
        internal Guid vendor;

        /// <summary>
        /// </summary>
        /// <param name="index">
        /// </param>
        /// <param name="name">
        /// </param>
        /// <param name="doctype">
        /// </param>
        /// <param name="language">
        /// </param>
        /// <param name="vendor">
        /// </param>
        internal PdbSource(uint index, string name, Guid doctype, Guid language, Guid vendor)
        {
            this.index = index;
            this.name = name;
            this.doctype = doctype;
            this.language = language;
            this.vendor = vendor;
        }
    }
}