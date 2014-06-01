namespace PEAssemblyReader
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Reflection;

    using Microsoft.CodeAnalysis;
    using System.Reflection.Metadata;
    using Microsoft.CodeAnalysis.CSharp.Symbols;
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis.CSharp.Symbols.Metadata.PE;
    using System.Diagnostics;

    public class MetadataMethodAdapter : IMethod
    {
        #region Fields

        private MethodSymbol methodDef;

        #endregion

        #region Constructors and Destructors

        internal MetadataMethodAdapter(MethodSymbol methodDef)
        {
            this.methodDef = methodDef;
        }

        #endregion

        #region Public Properties

        public string AssemblyQualifiedName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public CallingConventions CallingConvention
        {
            get
            {
                CallingConventions callConv = 0;
                if (this.methodDef.CallingConvention.HasFlag(Microsoft.Cci.CallingConvention.Standard))
                    callConv |= CallingConventions.Standard;
                if (this.methodDef.CallingConvention.HasFlag(Microsoft.Cci.CallingConvention.ExtraArguments))
                    callConv |= CallingConventions.VarArgs;
                if (this.methodDef.CallingConvention.HasFlag(Microsoft.Cci.CallingConvention.HasThis))
                    callConv |= CallingConventions.HasThis;
                if (this.methodDef.CallingConvention.HasFlag(Microsoft.Cci.CallingConvention.ExplicitThis))
                    callConv |= CallingConventions.ExplicitThis;

                return callConv;
            }
        }

        public IType DeclaringType
        {
            get
            {
                return new MetadataTypeAdapter(methodDef.ContainingType);
            }
        }

        public IEnumerable<IExceptionHandlingClause> ExceptionHandlingClauses
        {
            get
            {
                var peModuleSymbol = this.methodDef.ContainingModule as PEModuleSymbol;
                var peModule = peModuleSymbol.Module;
                var peMethodSymbol = this.methodDef as PEMethodSymbol;
                if (peMethodSymbol != null)
                {
                    var methodBodyBlock = GetMethodBodyBlock(peModuleSymbol, peMethodSymbol);
                    if (methodBodyBlock != null)
                    {
                        return methodBodyBlock
                            .ExceptionRegions
                            .Select(er => new MetadataExceptionHandlingClauseAdapter(er, new MetadataDecoder(peModuleSymbol).GetTypeOfToken(er.CatchType)));
                    }
                }

                return null;
            }
        }

        public string FullName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsAbstract
        {
            get
            {
                return this.methodDef.IsAbstract;
            }
        }

        public bool IsConstructor
        {
            get;
            set;
        }

        public bool IsGenericMethod
        {
            get
            {
                return this.methodDef.TypeParameters.Any();
            }
        }

        public bool IsStatic
        {
            get
            {
                return this.methodDef.IsStatic;
            }
        }

        public bool IsVirtual
        {
            get
            {
                return this.methodDef.IsVirtual;
            }
        }

        public IEnumerable<ILocalVariable> LocalVariables
        {
            get
            {
                var localInfo = default(ImmutableArray<MetadataDecoder.LocalInfo>);
                try
                {
                    var peModuleSymbol = this.methodDef.ContainingModule as PEModuleSymbol;
                    var peModule = peModuleSymbol.Module;
                    var peMethodSymbol = this.methodDef as PEMethodSymbol;
                    if (peMethodSymbol != null)
                    {
                        var methodBody = GetMethodBodyBlock(peModuleSymbol, peMethodSymbol);
                        if (methodBody != null && !methodBody.LocalSignature.IsNil)
                        {
                            var signature = peModule.MetadataReader.GetLocalSignature(methodBody.LocalSignature);
                            localInfo = new MetadataDecoder(peModuleSymbol).DecodeLocalSignatureOrThrow(signature);
                        }
                        else
                        {
                            localInfo = ImmutableArray<MetadataDecoder.LocalInfo>.Empty;
                        }
                    }
                    else
                    {
                        var synthesizedInstanceConstructor = this.methodDef as SynthesizedInstanceConstructor;
                        if (synthesizedInstanceConstructor != null)
                        {

                        }
                        else
                        {
                            var synthesizedStaticConstructor = this.methodDef as SynthesizedStaticConstructor;
                            if (synthesizedStaticConstructor != null)
                            {
                            }
                        }
                    }
                }
                catch (UnsupportedSignatureContent)
                {
                }
                catch (BadImageFormatException)
                {
                }

                return localInfo.Select(l => new MetadataLocalVariableAdapter(l));
            }
        }

        public IModule Module
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Namespace
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IType ReturnType
        {
            get
            {
                return new MetadataTypeAdapter(this.methodDef.ReturnType);
            }
        }

        #endregion

        #region Public Methods and Operators

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IType> GetGenericArguments()
        {
            throw new NotImplementedException();
        }

        public byte[] GetILAsByteArray()
        {
            throw new NotImplementedException();
        }

        public IMethodBody GetMethodBody()
        {
            return this;
        }

        public IEnumerable<IParameter> GetParameters()
        {
            return this.methodDef.Parameters.Select(p => new MetadataParameterAdapter(p));
        }

        private MethodBodyBlock GetMethodBodyBlock(PEModuleSymbol peModuleSymbol, PEMethodSymbol peMethodSymbol)
        {
            var peModule = peModuleSymbol.Module;
            if (peMethodSymbol != null)
            {
                Debug.Assert(peModule.HasIL);
                return peModule.GetMethodBodyOrThrow(peMethodSymbol.Handle);
            };

            return null;
        }

        #endregion
    }
}