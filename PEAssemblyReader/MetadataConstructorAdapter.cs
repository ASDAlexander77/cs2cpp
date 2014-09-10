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
        private MethodSymbol methodDef;

        /// <summary>
        /// </summary>
        /// <param name="methodDef">
        /// </param>
        internal MetadataConstructorAdapter(MethodSymbol methodDef)
            : base(methodDef)
        {
            this.methodDef = methodDef;
            this.IsConstructor = true;
        }

        /// <summary>
        /// </summary>
        /// <param name="methodDef">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        internal MetadataConstructorAdapter(MethodSymbol methodDef, IGenericContext genericContext)
            : base(methodDef, genericContext)
        {
            this.methodDef = methodDef;
            this.IsConstructor = true;
        }
    }
}