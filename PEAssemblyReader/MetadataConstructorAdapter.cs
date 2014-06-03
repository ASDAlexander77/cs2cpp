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
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    /// <summary>
    /// </summary>
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
    }
}