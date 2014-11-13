// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="PdbException.cs">
//   
// </copyright>
// <summary>
//   
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace PdbReader
{
    using System;
    using System.IO;

    /// <summary>
    /// </summary>
    internal class PdbException : IOException
    {
        /// <summary>
        /// </summary>
        /// <param name="format">
        /// </param>
        /// <param name="args">
        /// </param>
        internal PdbException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
    }
}