// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetadataConstructorAdapter.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PEAssemblyReader
{
    using System.Diagnostics;

    using Microsoft.CodeAnalysis.CSharp.Symbols;

    /// <summary>
    /// </summary>
    [DebuggerDisplay("Name = {Name}, Type = {FullName}")]
    public class MetadataConstructorAdapter : MetadataMethodAdapter, IConstructor
    {
        /// <summary>
        /// </summary>
        /// <param name="methodDef">
        /// </param>
        internal MetadataConstructorAdapter(MethodSymbol methodDef)
            : base(methodDef)
        {
        }
    }
}