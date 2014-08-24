// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetadataLocalVariableAdapter.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PEAssemblyReader
{
    using System;
    using System.Diagnostics;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    /// <summary>
    /// </summary>
    [DebuggerDisplay("Index = {LocalIndex}, Type = {LocalType.FullName}")]
    public class MetadataLocalVariableAdapter : ILocalVariable
    {
        /// <summary>
        /// </summary>
        private MetadataDecoder<TypeSymbol, MethodSymbol, FieldSymbol, AssemblySymbol, Symbol>.LocalInfo localInfo;

        /// <summary>
        /// </summary>
        /// <param name="localInfo">
        /// </param>
        /// <param name="index">
        /// </param>
        internal MetadataLocalVariableAdapter(MetadataDecoder<TypeSymbol, MethodSymbol, FieldSymbol, AssemblySymbol, Symbol>.LocalInfo localInfo, int index)
        {
            Debug.Assert(localInfo.Type.TypeKind != TypeKind.Error);
            this.localInfo = localInfo;
            this.LocalIndex = index;
        }

        /// <summary>
        /// </summary>
        /// <param name="localInfo">
        /// </param>
        /// <param name="index">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        internal MetadataLocalVariableAdapter(
            MetadataDecoder<TypeSymbol, MethodSymbol, FieldSymbol, AssemblySymbol, Symbol>.LocalInfo localInfo, int index, IGenericContext genericContext)
            : this(localInfo, index)
        {
            this.GenericContext = genericContext;
        }

        /// <summary>
        /// </summary>
        public IGenericContext GenericContext { get; set; }

        /// <summary>
        /// </summary>
        public int LocalIndex { get; protected set; }

        /// <summary>
        /// </summary>
        public IType LocalType
        {
            get
            {
                var localType = this.localInfo.Type.ResolveGeneric(this.GenericContext);
                localType.IsByRef = this.localInfo.IsByRef;
                localType.IsPinned = this.localInfo.IsPinned;
                return localType;
            }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}