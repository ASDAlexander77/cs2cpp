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
    public class MetadataMethodAdapter : IMethod
    {
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
                return new MetadataTypeAdapter(this.methodDef.ContainingType);
            }
        }

        /// <summary>
        /// </summary>
        public IEnumerable<IExceptionHandlingClause> ExceptionHandlingClauses
        {
            get
            {
                var peModuleSymbol = this.methodDef.ContainingModule as PEModuleSymbol;
                var peModule = peModuleSymbol.Module;
                var peMethodSymbol = this.methodDef as PEMethodSymbol;
                if (peMethodSymbol != null)
                {
                    var methodBodyBlock = this.GetMethodBodyBlock(peModuleSymbol, peMethodSymbol);
                    if (methodBodyBlock != null)
                    {
                        return
                            methodBodyBlock.ExceptionRegions.Select(
                                er =>
                                new MetadataExceptionHandlingClauseAdapter(
                                    er, !er.CatchType.IsNil ? new MetadataDecoder(peModuleSymbol).GetTypeOfToken(er.CatchType) : null));
                    }
                }

                return new IExceptionHandlingClause[0];
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

                if (this.methodDef.ContainingType != null && !string.IsNullOrWhiteSpace(this.methodDef.ContainingType.Name))
                {
                    if (this.methodDef.ContainingType.IsNestedType())
                    {
                        result.Append(this.methodDef.ContainingType.ContainingType.Name);
                        result.Append('+');
                    }

                    result.Append(this.methodDef.ContainingType.Name);
                    result.Append('.');
                }

                result.Append(this.methodDef.Name);

                return result.ToString();
            }
        }

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
        public bool IsGenericMethod
        {
            get
            {
                return this.methodDef.TypeParameters.Any();
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
                return this.methodDef.IsVirtual;
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

        public bool IsInternalCall
        {
            get 
            {
                return this.methodDef.ImplementationAttributes.HasFlag(MethodImplAttributes.ManagedMask);
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
                    var peModuleSymbol = this.methodDef.ContainingModule as PEModuleSymbol;
                    var peModule = peModuleSymbol.Module;
                    var peMethodSymbol = this.methodDef as PEMethodSymbol;
                    if (peMethodSymbol != null)
                    {
                        var methodBody = this.GetMethodBodyBlock(peModuleSymbol, peMethodSymbol);
                        if (methodBody != null && !methodBody.LocalSignature.IsNil)
                        {
                            var signature = peModule.MetadataReader.GetLocalSignature(methodBody.LocalSignature);
                            localInfo = new MetadataDecoder(peModuleSymbol).DecodeLocalSignatureOrThrow(signature);
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
                    yield return new MetadataLocalVariableAdapter(li, index++);
                }
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
                return this.methodDef.Name;
            }
        }

        public string ExplicitName
        {
            get
            {
                var result = new StringBuilder();

                if (this.methodDef.ContainingType != null && !string.IsNullOrWhiteSpace(this.methodDef.ContainingType.Name))
                {
                    if (this.methodDef.ContainingType.IsNestedType())
                    {
                        result.Append(this.methodDef.ContainingType.ContainingType.Name);
                        result.Append('+');
                    }

                    result.Append(this.methodDef.ContainingType.Name);
                    result.Append('.');
                }

                result.Append(this.methodDef.Name);

                return result.ToString();
            }
        }

        /// <summary>
        /// </summary>
        public string Namespace
        {
            get
            {
                if (this.methodDef.ContainingType.IsNestedType())
                {
                    return this.methodDef.ContainingType.ContainingNamespace.ToString();
                }

                return this.methodDef.ContainingNamespace.ToString();
            }
        }

        /// <summary>
        /// </summary>
        public IType ReturnType
        {
            get
            {
                return new MetadataTypeAdapter(this.methodDef.ReturnType);
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

            var val = name.Name.CompareTo(this.Name);
            if (val != 0)
            {
                return val;
            }

            val = name.Namespace.CompareTo(this.Namespace);
            if (val != 0)
            {
                return val;
            }

            return 0;
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
        public byte[] GetILAsByteArray()
        {
            var peModuleSymbol = this.methodDef.ContainingModule as PEModuleSymbol;
            var peModule = peModuleSymbol.Module;
            var peMethodSymbol = this.methodDef as PEMethodSymbol;
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
        /// <returns>
        /// </returns>
        public IMethodBody GetMethodBody()
        {
            var peModuleSymbol = this.methodDef.ContainingModule as PEModuleSymbol;
            var peModule = peModuleSymbol.Module;
            var peMethodSymbol = this.methodDef as PEMethodSymbol;
            if (peMethodSymbol != null)
            {
                var methodBody = this.GetMethodBodyBlock(peModuleSymbol, peMethodSymbol);
                if (methodBody != null && methodBody.GetILBytes() != null)
                {
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
            return this.methodDef.Parameters.Select(p => new MetadataParameterAdapter(p));
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

        public string ToString(IType ownerOfExplicitInterface)
        {
            var result = new StringBuilder();

            // write return type
            result.Append(this.ReturnType);
            result.Append(' ');

            // write Full Name
            result.Append(ownerOfExplicitInterface.FullName);
            result.Append('.');
            result.Append(this.ExplicitName);

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

            ;

            return null;
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
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
    }
}