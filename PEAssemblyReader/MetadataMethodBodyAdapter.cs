// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetadataMethodBodyAdapter.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#define USE_BODY_CACHE

namespace PEAssemblyReader
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection.Metadata;
    using System.Reflection.Metadata.Ecma335;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;
    using Microsoft.CodeAnalysis.CSharp.Symbols.Metadata.PE;

    /// <summary>
    /// </summary>
    public class MetadataMethodBodyAdapter : IMethodBody
    {
        /// <summary>
        /// </summary>
        private readonly Lazy<MethodBodyBlock> lazyMethodBodyBlock;

        /// <summary>
        /// </summary>
        private readonly Lazy<IEnumerable<ILocalVariable>> lazyLocalVariables;

        /// <summary>
        /// </summary>
        private readonly Lazy<IEnumerable<IExceptionHandlingClause>> lazyExceptionHandlingClauses;

        /// <summary>
        /// </summary>
        private readonly MethodSymbol methodDef;        

        /// <summary>
        /// </summary>
        /// <param name="methodDef">
        /// </param>
        internal MetadataMethodBodyAdapter(MethodSymbol methodDef)
        {
            Debug.Assert(methodDef != null);
            this.methodDef = methodDef;
            this.lazyMethodBodyBlock = new Lazy<MethodBodyBlock>(this.GetMethodBodyBlock);
            this.lazyLocalVariables = new Lazy<IEnumerable<ILocalVariable>>(this.GetLocalVariables);
            this.lazyExceptionHandlingClauses = new Lazy<IEnumerable<IExceptionHandlingClause>>(this.GetExceptionHandlingClauses);
        }

        /// <summary>
        /// </summary>
        /// <param name="methodDef">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        internal MetadataMethodBodyAdapter(MethodSymbol methodDef, IGenericContext genericContext)
            : this(methodDef)
        {
            this.GenericContext = genericContext;
        }

        /// <summary>
        /// </summary>
        public IEnumerable<IExceptionHandlingClause> ExceptionHandlingClauses
        {
            get
            {
                return this.lazyExceptionHandlingClauses.Value;
            }
        }

        /// <summary>
        /// </summary>
        public IGenericContext GenericContext { get; set; }

        /// <summary>
        /// </summary>
        public bool HasBody
        {
            get
            {
                var block = this.lazyMethodBodyBlock.Value;
                if (block == null)
                {
                    return false;
                }

                return block.GetILBytes() != null;
            }
        }

        /// <summary>
        /// </summary>
        public IEnumerable<ILocalVariable> LocalVariables
        {
            get
            {
                return this.lazyLocalVariables.Value;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public byte[] GetILAsByteArray()
        {
            var methodBody = this.lazyMethodBodyBlock.Value;
            if (methodBody != null)
            {
                return methodBody.GetILBytes();
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private MethodBodyBlock GetMethodBodyBlock()
        {
            PEModuleSymbol peModuleSymbol;
            PEMethodSymbol peMethodSymbol;
            this.GetPEMethodSymbol(out peModuleSymbol, out peMethodSymbol);

            if (peMethodSymbol != null)
            {
                var methodBodyBlock = this.GetMethodBodyBlock(peModuleSymbol, peMethodSymbol);
                return methodBodyBlock;
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="peModuleSymbol">
        /// </param>
        /// <param name="peMethodSymbol">
        /// </param>
        /// <returns>
        /// </returns>
        private MethodBodyBlock GetMethodBodyBlock(PEModuleSymbol peModuleSymbol, PEMethodSymbol peMethodSymbol)
        {
            var peModule = peModuleSymbol.Module;
            if (peMethodSymbol != null)
            {
                Debug.Assert(peModule.HasIL);
                lock (peModuleSymbol)
                {
                    return peModule.GetMethodBodyOrThrow(peMethodSymbol.Handle);
                }
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="peModuleSymbol">
        /// </param>
        /// <param name="peMethodSymbol">
        /// </param>
        private void GetPEMethodSymbol(out PEModuleSymbol peModuleSymbol, out PEMethodSymbol peMethodSymbol)
        {
            peModuleSymbol = this.methodDef.ContainingModule as PEModuleSymbol;
            peMethodSymbol = this.methodDef as PEMethodSymbol;
            if (peMethodSymbol == null)
            {
                peMethodSymbol = this.methodDef.OriginalDefinition as PEMethodSymbol;
            }
        }

        private IEnumerable<ILocalVariable> GetLocalVariables()
        {
            var localInfo = default(ImmutableArray<MetadataDecoder<TypeSymbol, MethodSymbol, FieldSymbol, AssemblySymbol, Symbol>.LocalInfo>);
            try
            {
                PEModuleSymbol peModuleSymbol;
                PEMethodSymbol peMethodSymbol;
                this.GetPEMethodSymbol(out peModuleSymbol, out peMethodSymbol);

                if (peMethodSymbol != null)
                {
                    var methodBody = this.GetMethodBodyBlock(peModuleSymbol, peMethodSymbol);
                    if (methodBody != null && !methodBody.LocalSignature.IsNil)
                    {
                        var module = peModuleSymbol.Module;
                        var signatureHandle = module.MetadataReader.GetLocalSignature(methodBody.LocalSignature);
                        var signatureReader = module.GetMemoryReaderOrThrow(signatureHandle);

                        localInfo = new MetadataDecoder(peModuleSymbol, peMethodSymbol).DecodeLocalSignatureOrThrow(ref signatureReader);
                        var index = 0;
                        return localInfo.Select(li => new MetadataLocalVariableAdapter(li, index++, this.GenericContext)).ToArray();
                    }
                }
            }
            catch (UnsupportedSignatureContent)
            {
            }
            catch (BadImageFormatException)
            {
            }

            return new ILocalVariable[0];
        }

        private IEnumerable<IExceptionHandlingClause> GetExceptionHandlingClauses()
        {
            PEModuleSymbol peModuleSymbol;
            PEMethodSymbol peMethodSymbol;
            this.GetPEMethodSymbol(out peModuleSymbol, out peMethodSymbol);

            if (peMethodSymbol != null)
            {
                var methodBodyBlock = this.GetMethodBodyBlock(peModuleSymbol, peMethodSymbol);
                if (methodBodyBlock != null)
                {
                    return
                        methodBodyBlock.ExceptionRegions.Select(
                            er =>
                            new MetadataExceptionHandlingClauseAdapter(
                                er,
                                !er.CatchType.IsNil
                                    ? new MetadataModuleAdapter(peModuleSymbol).ResolveType(MetadataTokens.GetToken(er.CatchType), this.GenericContext)
                                    : null)).ToArray();
                }
            }

            return new IExceptionHandlingClause[0];
        }
    }
}