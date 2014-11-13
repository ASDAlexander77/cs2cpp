// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="PdbDebugException.cs">
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
    using System.IO;

    /// <summary>
    /// </summary>
    internal class PdbDebugException : IOException
    {
        /// <summary>
        /// </summary>
        /// <param name="format">
        /// </param>
        /// <param name="args">
        /// </param>
        internal PdbDebugException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
    }
}