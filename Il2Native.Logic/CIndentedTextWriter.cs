// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LlvmIndentedTextWriter.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic
{
    using System;
    using System.CodeDom.Compiler;
    using System.IO;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class CIndentedTextWriter : IndentedTextWriter
    {
        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        public CIndentedTextWriter(TextWriter writer)
            : base(writer)
        {
        }

        /// <summary>
        /// </summary>
        public void EndMethodBody()
        {
        }

        /// <summary>
        /// </summary>
        public void StartMethodBody()
        {
        }
    }
}