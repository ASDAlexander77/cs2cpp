namespace PEAssemblyReader
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Metadata;
    using System.Text;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;
    using Microsoft.CodeAnalysis.CSharp.Symbols.Metadata.PE;

    public class MetadataMethodBodyAdapter : IMethodBody
    {
        /// <summary>
        /// </summary>
        private readonly MethodSymbol methodDef;

        private Lazy<MethodBodyBlock> lazyMethodBodyBlock;

        /// <summary>
        /// </summary>
        /// <param name="methodDef">
        /// </param>
        internal MetadataMethodBodyAdapter(MethodSymbol methodDef)
        {
            Debug.Assert(methodDef != null);
            this.methodDef = methodDef;
            this.lazyMethodBodyBlock = new Lazy<MethodBodyBlock>(GetMethodBodyBlock);
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
        public IGenericContext GenericContext { get; set; }

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
        public IEnumerable<IExceptionHandlingClause> ExceptionHandlingClauses
        {
            get
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
                                    er, !er.CatchType.IsNil ? new MetadataDecoder(peModuleSymbol).GetTypeOfToken(er.CatchType) : null, this.GenericContext));
                    }
                }

                return new IExceptionHandlingClause[0];
            }
        }

        /// <summary>
        /// </summary>
        public IEnumerable<ILocalVariable> LocalVariables
        {
            get
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
                        }
                        else
                        {
                            localInfo = ImmutableArray<MetadataDecoder<TypeSymbol, MethodSymbol, FieldSymbol, AssemblySymbol, Symbol>.LocalInfo>.Empty;
                        }
                    }
                }
                catch (UnsupportedSignatureContent)
                {
                }
                catch (BadImageFormatException)
                {
                }

                var index = 0;
                foreach (var li in localInfo)
                {
                    yield return new MetadataLocalVariableAdapter(li, index++, this.GenericContext);
                }
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

        private MethodBodyBlock GetMethodBodyBlock()
        {
            PEModuleSymbol peModuleSymbol;
            PEMethodSymbol peMethodSymbol;
            this.GetPEMethodSymbol(out peModuleSymbol, out peMethodSymbol);

            if (peMethodSymbol != null)
            {
                return this.GetMethodBodyBlock(peModuleSymbol, peMethodSymbol);
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
                return peModule.GetMethodBodyOrThrow(peMethodSymbol.Handle);
            }

            return null;
        }
    }
}
