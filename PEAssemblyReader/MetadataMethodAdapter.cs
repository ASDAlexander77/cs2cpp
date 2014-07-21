// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetadataMethodAdapter.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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

    /// <summary>
    /// </summary>
    [DebuggerDisplay("Name = {ExplicitName}")]
    public class MetadataMethodAdapter : IMethod
    {
        /// <summary>
        /// </summary>
        private readonly Lazy<string> lazyNamespace;

        /// <summary>
        /// </summary>
        private readonly MethodSymbol methodDef;

        /// <summary>
        /// </summary>
        /// <param name="methodDef">
        /// </param>
        internal MetadataMethodAdapter(MethodSymbol methodDef)
        {
            Debug.Assert(methodDef != null);
            this.methodDef = methodDef;
            this.lazyNamespace = new Lazy<string>(this.calculateNamespace);
        }

        /// <summary>
        /// </summary>
        /// <param name="methodDef">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        internal MetadataMethodAdapter(MethodSymbol methodDef, IGenericContext genericContext)
            : this(methodDef)
        {
            this.GenericContext = genericContext;
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public string AssemblyQualifiedName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// </summary>
        public CallingConventions CallingConvention
        {
            get
            {
                var callConv = CallingConventions.Standard;
                if (this.methodDef.CallingConvention.HasFlag(Microsoft.Cci.CallingConvention.ExtraArguments))
                {
                    callConv |= CallingConventions.VarArgs;
                }

                if (this.methodDef.CallingConvention.HasFlag(Microsoft.Cci.CallingConvention.HasThis))
                {
                    callConv |= CallingConventions.HasThis;
                }

                if (this.methodDef.CallingConvention.HasFlag(Microsoft.Cci.CallingConvention.ExplicitThis))
                {
                    callConv |= CallingConventions.ExplicitThis;
                }

                return callConv;
            }
        }

        /// <summary>
        /// </summary>
        public IType DeclaringType
        {
            get
            {
                return this.methodDef.ContainingType.ResolveGeneric(this.GenericContext);
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
        public string ExplicitName
        {
            get
            {
                var result = new StringBuilder();

                if (this.methodDef.ContainingType != null && !string.IsNullOrWhiteSpace(this.methodDef.ContainingType.Name))
                {
                    if (this.methodDef.ContainingType.IsNestedType())
                    {
                        result.Append(this.methodDef.ContainingType.ContainingType.ResolveGeneric(this.GenericContext).Name);
                        result.Append('+');
                    }

                    result.Append(this.methodDef.ContainingType.ResolveGeneric(this.GenericContext).Name);
                    result.Append('.');
                }

                result.Append(this.Name);

                return result.ToString();
            }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public string FullName
        {
            get
            {
                var result = new StringBuilder();

                if (!string.IsNullOrWhiteSpace(this.Namespace))
                {
                    result.Append(this.Namespace);
                    result.Append('.');
                }

                result.Append(this.ExplicitName);

                return result.ToString();
            }
        }

        /// <summary>
        /// </summary>
        public IGenericContext GenericContext { get; set; }

        /// <summary>
        /// </summary>
        public bool IsAbstract
        {
            get
            {
                return this.methodDef.IsAbstract;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsConstructor { get; set; }

        /// <summary>
        /// </summary>
        public bool IsExternal
        {
            get
            {
                return this.methodDef.IsExternal;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsGenericMethod
        {
            get
            {
                return this.methodDef.TypeParameters.Any();
            }
        }

        /// <summary>
        /// </summary>
        public bool IsInternalCall
        {
            get
            {
                return this.methodDef.ImplementationAttributes.HasFlag(MethodImplAttributes.ManagedMask);
            }
        }

        /// <summary>
        /// </summary>
        public bool IsOverride
        {
            get
            {
                return this.methodDef.IsOverride;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsStatic
        {
            get
            {
                return this.methodDef.IsStatic;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsVirtual
        {
            get
            {
                if (this.FullName == "System.Object.Finalize")
                {
                    return true;
                }

                return this.methodDef.IsVirtual;
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
                            localInfo = peModuleSymbol.GetMetadataDecoder(this.GenericContext).DecodeLocalSignatureOrThrow(ref signatureReader);
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
        public string MetadataFullName
        {
            get
            {
                var sb = new StringBuilder();
                this.methodDef.AppendFullNamespace(sb, this.Namespace, this.DeclaringType, true);
                sb.Append(this.MetadataName);

                return sb.ToString();
            }
        }

        /// <summary>
        /// </summary>
        public string MetadataName
        {
            get
            {
                var sb = new StringBuilder();

                sb.Append(this.methodDef.Name);

                if (this.IsGenericMethod)
                {
                    sb.Append('`');
                    sb.Append(this.methodDef.GetArity());
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// </summary>
        public IModule Module
        {
            get
            {
                return new MetadataModuleAdapter(this.methodDef.ContainingModule);
            }
        }

        /// <summary>
        /// </summary>
        public string Name
        {
            get
            {
                var sb = new StringBuilder();

                sb.Append(this.methodDef.Name);

                if (this.IsGenericMethod)
                {
                    sb.Append('<');

                    var index = 0;
                    foreach (var genArg in this.GetGenericArguments())
                    {
                        if (index++ > 0)
                        {
                            sb.Append(", ");
                        }

                        sb.Append(genArg.FullName);
                    }

                    sb.Append('>');
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// </summary>
        public string Namespace
        {
            get
            {
#if DEBUG
                return this.methodDef.CalculateNamespace();
#else
                return this.lazyNamespace.Value;
#endif
            }
        }

        /// <summary>
        /// </summary>
        public IType ReturnType
        {
            get
            {
                return this.methodDef.ReturnType.ResolveGeneric(this.GenericContext);
            }
        }

        /// <summary>
        /// </summary>
        internal MethodSymbol MethodDef
        {
            get
            {
                return this.methodDef;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="obj">
        /// </param>
        /// <returns>
        /// </returns>
        public int CompareTo(object obj)
        {
            var name = obj as IName;
            if (name == null)
            {
                return 1;
            }

            return this.MetadataFullName.CompareTo(name.MetadataFullName);
        }

        /// <summary>
        /// </summary>
        /// <param name="obj">
        /// </param>
        /// <returns>
        /// </returns>
        public override bool Equals(object obj)
        {
            var type = obj as IName;
            if (type != null)
            {
                return this.CompareTo(type) == 0;
            }

            return base.Equals(obj);
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IEnumerable<IType> GetGenericArguments()
        {
            return this.methodDef.TypeArguments.Select(a => new MetadataTypeAdapter(a));
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IEnumerable<IType> GetGenericParameters()
        {
            return this.methodDef.TypeParameters.Select(a => new MetadataTypeAdapter(a));
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public byte[] GetILAsByteArray()
        {
            PEModuleSymbol peModuleSymbol;
            PEMethodSymbol peMethodSymbol;
            this.GetPEMethodSymbol(out peModuleSymbol, out peMethodSymbol);

            if (peMethodSymbol != null)
            {
                var methodBody = this.GetMethodBodyBlock(peModuleSymbol, peMethodSymbol);
                if (methodBody != null)
                {
                    return methodBody.GetILBytes();
                }
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="genericContext">
        /// </param>
        /// <returns>
        /// </returns>
        public IMethodBody GetMethodBody(IGenericContext genericContext = null)
        {
            PEModuleSymbol peModuleSymbol;
            PEMethodSymbol peMethodSymbol;
            this.GetPEMethodSymbol(out peModuleSymbol, out peMethodSymbol);

            if (peMethodSymbol != null)
            {
                var methodBody = this.GetMethodBodyBlock(peModuleSymbol, peMethodSymbol);
                if (methodBody != null && methodBody.GetILBytes() != null)
                {
                    if (genericContext != null && this.GenericContext == null)
                    {
                        this.GenericContext = genericContext;
                    }

                    return this;
                }
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IEnumerable<IParameter> GetParameters()
        {
            return this.methodDef.Parameters.Select(p => new MetadataParameterAdapter(p, this.GenericContext));
        }

        /// <summary>
        /// </summary>
        /// <param name="typeParameter">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// </exception>
        public IType ResolveTypeParameter(IType typeParameter)
        {
            var typeParameters = this.GetGenericParameters().ToList();
            var typeArguments = this.GetGenericArguments().ToList();

            for (var index = 0; index < typeArguments.Count; index++)
            {
                if (typeParameters[index].TypeEquals(typeParameter))
                {
                    return typeArguments[index];
                }
            }

            throw new KeyNotFoundException();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public override string ToString()
        {
            var result = new StringBuilder();

            // write return type
            result.Append(this.ReturnType);
            result.Append(' ');

            // write Full Name
            result.Append(this.FullName);

            // write Parameter Types
            result.Append('(');
            var index = 0;
            foreach (var parameterType in this.GetParameters())
            {
                if (index != 0)
                {
                    result.Append(", ");
                }

                result.Append(parameterType);
                index++;
            }

            result.Append(')');

            return result.ToString();
        }

        /// <summary>
        /// </summary>
        /// <param name="ownerOfExplicitInterface">
        /// </param>
        /// <returns>
        /// </returns>
        public string ToString(IType ownerOfExplicitInterface)
        {
            var result = new StringBuilder();

            // write return type
            result.Append(this.ReturnType);
            result.Append(' ');

            // write Full Name
            result.Append(ownerOfExplicitInterface.FullName);
            result.Append('.');
            result.Append(this.FullName);

            // write Parameter Types
            result.Append('(');
            var index = 0;
            foreach (var parameterType in this.GetParameters())
            {
                if (index != 0)
                {
                    result.Append(", ");
                }

                result.Append(parameterType);
                index++;
            }

            result.Append(')');

            return result.ToString();
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

        /// <summary>
        /// </summary>
        /// <param name="peModuleSymbol">
        /// </param>
        /// <param name="peMethodSymbol">
        /// </param>
        private void GetPEMethodSymbol(out PEModuleSymbol peModuleSymbol, out PEMethodSymbol peMethodSymbol)
        {
            peModuleSymbol = this.methodDef.ContainingModule as PEModuleSymbol;
            var peModule = peModuleSymbol.Module;
            peMethodSymbol = this.methodDef as PEMethodSymbol;
            if (peMethodSymbol == null)
            {
                peMethodSymbol = this.methodDef.OriginalDefinition as PEMethodSymbol;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private string calculateNamespace()
        {
            return this.methodDef.CalculateNamespace();
        }
    }
}