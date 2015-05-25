// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetadataModuleAdapter.cs" company="">
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
    using System.Reflection.Metadata;
    using System.Reflection.Metadata.Ecma335;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Symbols;
    using Microsoft.CodeAnalysis.CSharp.Symbols.Metadata.PE;

    /// <summary>
    /// </summary>
    public class MetadataModuleAdapter : IModule
    {
        /// <summary>
        /// </summary>
        /// <param name="moduleDef">
        /// </param>
        internal MetadataModuleAdapter(ModuleSymbol moduleDef)
        {
            Debug.Assert(moduleDef != null);
            this.moduleDef = moduleDef;
        }

        /// <summary>
        /// </summary>
        private ModuleSymbol moduleDef { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="token">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// </exception>
        public IField ResolveField(int token, IGenericContext genericContext)
        {
            var peModuleSymbol = this.moduleDef as PEModuleSymbol;

            var fieldHandle = MetadataTokens.Handle(token);

            var fieldSymbol = peModuleSymbol.GetMetadataDecoder(genericContext).GetSymbolForILToken(fieldHandle) as FieldSymbol;
            if (fieldSymbol != null)
            {
                return new MetadataFieldAdapter(SubstituteFieldSymbolIfNeeded(fieldSymbol, genericContext));
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="token">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public IMember ResolveMember(int token, IGenericContext genericContext)
        {
            var peModuleSymbol = this.moduleDef as PEModuleSymbol;

            var methodHandle = MetadataTokens.Handle(token);

            var methodSymbol = peModuleSymbol.GetMetadataDecoder(genericContext).GetSymbolForILToken(methodHandle) as MethodSymbol;
            if (methodSymbol != null)
            {
                if (methodSymbol.MethodKind == MethodKind.Constructor)
                {
                    return new MetadataConstructorAdapter(SubstituteMethodSymbolIfNeeded(methodSymbol, genericContext));
                }

                return new MetadataMethodAdapter(SubstituteMethodSymbolIfNeeded(methodSymbol, genericContext));
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="token">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// </exception>
        public IMethod ResolveMethod(int token, IGenericContext genericContext)
        {
            var peModuleSymbol = this.moduleDef as PEModuleSymbol;

            var methodHandle = MetadataTokens.Handle(token);

            /*
             * FIX for Roslyn code - to be able to load Array method references 
             
             
                        public override ImmutableArray<Symbol> GetMembers(string name)
                        {
                            // TODO: ASD: MY ADDON
                            if (name == ".ctor")
                            {
                                return ImmutableArray.Create<Symbol>(Enumerable.Range(1, 7).Select(n => new ArrayConstructor(this)).ToArray<Symbol>());
                            }

                            if (name == "Set")
                            {
                                return ImmutableArray.Create<Symbol>(Enumerable.Range(1, 7).Select(n => new ArraySetValueMethod(this)).ToArray<Symbol>());
                            }

                            if (name == "Get")
                            {
                                return ImmutableArray.Create<Symbol>(Enumerable.Range(1, 7).Select(n => new ArrayGetValueMethod(this)).ToArray<Symbol>());
                            }

                            if (name == "Address")
                            {
                                return ImmutableArray.Create<Symbol>(Enumerable.Range(1, 7).Select(n => new ArrayAddressMethod(this)).ToArray<Symbol>());
                            }
                            // TODO: END - ASD: MY ADDON
             
                            return ImmutableArray<Symbol>.Empty;
                        }
              
                        // TODO: ASD: MY ADDON
                        private sealed class ArrayConstructor : SynthesizedInstanceMethodSymbol
                        {
                            private readonly ImmutableArray<ParameterSymbol> parameters;
                            private readonly ArrayTypeSymbol arrayTypeSymbol;

                            public ArrayConstructor(ArrayTypeSymbol arrayTypeSymbol)
                            {
                                this.arrayTypeSymbol = arrayTypeSymbol;
                                var intType = arrayTypeSymbol.BaseType.ContainingAssembly.GetSpecialType(SpecialType.System_Int32);
                                this.parameters = ImmutableArray.Create<ParameterSymbol>(
                                    Enumerable.Range(0, arrayTypeSymbol.Rank).Select(n => new SynthesizedParameterSymbol(this, intType, n, RefKind.None)).ToArray<ParameterSymbol>());
                            }

                            public override ImmutableArray<ParameterSymbol> Parameters
                            {
                                get { return parameters; }
                            }

                            //
                            // Consider overriding when implementing a synthesized subclass.
                            //

                            internal override bool GenerateDebugInfo
                            {
                                get { return false; }
                            }

                            public override Accessibility DeclaredAccessibility
                            {
                                get { return ContainingType.IsAbstract ? Accessibility.Protected : Accessibility.Public; }
                            }

                            internal override bool IsMetadataFinal()
                            {
                                return false;
                            }

                            #region Sealed

                            public sealed override Symbol ContainingSymbol
                            {
                                get
                                {
                                    return this.arrayTypeSymbol.BaseType;
                                }
                            }

                            public sealed override NamedTypeSymbol ContainingType
                            {
                                get
                                {
                                    return this.arrayTypeSymbol.BaseType;
                                }
                            }

                            public override NamespaceSymbol ContainingNamespace
                            {
                                get { return this.arrayTypeSymbol.ElementType.ContainingNamespace; }
                            }

                            public sealed override string Name
                            {
                                get { return WellKnownMemberNames.InstanceConstructorName; }
                            }

                            internal sealed override bool HasSpecialName
                            {
                                get { return true; }
                            }

                            internal sealed override System.Reflection.MethodImplAttributes ImplementationAttributes
                            {
                                get
                                {
                                    var containingType = this.arrayTypeSymbol.BaseType;
                                    if (containingType.IsComImport)
                                    {
                                        Debug.Assert(containingType.TypeKind == TypeKind.Class);
                                        return System.Reflection.MethodImplAttributes.Runtime | System.Reflection.MethodImplAttributes.InternalCall;
                                    }

                                    if (containingType.TypeKind == TypeKind.Delegate)
                                    {
                                        return System.Reflection.MethodImplAttributes.Runtime;
                                    }

                                    return default(System.Reflection.MethodImplAttributes);
                                }
                            }

                            internal sealed override bool RequiresSecurityObject
                            {
                                get { return false; }
                            }

                            public sealed override DllImportData GetDllImportData()
                            {
                                return null;
                            }

                            internal sealed override MarshalPseudoCustomAttributeData ReturnValueMarshallingInformation
                            {
                                get { return null; }
                            }

                            internal sealed override bool HasDeclarativeSecurity
                            {
                                get { return false; }
                            }

                            internal sealed override IEnumerable<Microsoft.Cci.SecurityAttribute> GetSecurityInformation()
                            {
                                throw ExceptionUtilities.Unreachable;
                            }

                            internal sealed override ImmutableArray<string> GetAppliedConditionalSymbols()
                            {
                                return ImmutableArray<string>.Empty;
                            }

                            public sealed override bool IsVararg
                            {
                                get { return false; }
                            }

                            public sealed override ImmutableArray<TypeParameterSymbol> TypeParameters
                            {
                                get { return ImmutableArray<TypeParameterSymbol>.Empty; }
                            }

                            internal sealed override LexicalSortKey GetLexicalSortKey()
                            {
                                //For the sake of matching the metadata output of the native compiler, make synthesized constructors appear last in the metadata.
                                //This is not critical, but it makes it easier on tools that are comparing metadata.
                                return LexicalSortKey.Last;
                            }

                            public sealed override ImmutableArray<Location> Locations
                            {
                                get { return ContainingType.Locations; }
                            }

                            public sealed override TypeSymbol ReturnType
                            {
                                get { return this.arrayTypeSymbol.BaseType.ContainingAssembly.GetSpecialType(SpecialType.System_Void); }
                            }

                            public sealed override ImmutableArray<CustomModifier> ReturnTypeCustomModifiers
                            {
                                get { return ImmutableArray<CustomModifier>.Empty; }
                            }

                            public sealed override ImmutableArray<TypeSymbol> TypeArguments
                            {
                                get { return ImmutableArray<TypeSymbol>.Empty; }
                            }

                            public sealed override Symbol AssociatedSymbol
                            {
                                get { return this.arrayTypeSymbol; }
                            }

                            public sealed override int Arity
                            {
                                get { return 0; }
                            }

                            public sealed override bool ReturnsVoid
                            {
                                get { return true; }
                            }

                            public sealed override MethodKind MethodKind
                            {
                                get { return MethodKind.Constructor; }
                            }

                            public sealed override bool IsExtern
                            {
                                get
                                {
                                    // Synthesized constructors of ComImport type are extern
                                    NamedTypeSymbol containingType = this.ContainingType;
                                    return (object)containingType != null && containingType.IsComImport;
                                }
                            }

                            public sealed override bool IsSealed
                            {
                                get { return false; }
                            }

                            public sealed override bool IsAbstract
                            {
                                get { return false; }
                            }

                            public sealed override bool IsOverride
                            {
                                get { return false; }
                            }

                            public sealed override bool IsVirtual
                            {
                                get { return false; }
                            }

                            public sealed override bool IsStatic
                            {
                                get { return false; }
                            }

                            public sealed override bool IsAsync
                            {
                                get { return false; }
                            }

                            public sealed override bool HidesBaseMethodsByName
                            {
                                get { return false; }
                            }

                            internal sealed override bool IsMetadataNewSlot(bool ignoreInterfaceImplementationChanges = false)
                            {
                                return false;
                            }

                            internal sealed override bool IsMetadataVirtual(bool ignoreInterfaceImplementationChanges = false)
                            {
                                return false;
                            }

                            public sealed override bool IsExtensionMethod
                            {
                                get { return false; }
                            }

                            internal sealed override Microsoft.Cci.CallingConvention CallingConvention
                            {
                                get { return Microsoft.Cci.CallingConvention.HasThis; }
                            }

                            internal sealed override bool IsExplicitInterfaceImplementation
                            {
                                get { return false; }
                            }

                            public sealed override ImmutableArray<MethodSymbol> ExplicitInterfaceImplementations
                            {
                                get { return ImmutableArray<MethodSymbol>.Empty; }
                            }

                            #endregion
                        }

                        private sealed class ArraySetValueMethod : SynthesizedInstanceMethodSymbol
                        {
                            private readonly ImmutableArray<ParameterSymbol> parameters;
                            private readonly ArrayTypeSymbol arrayTypeSymbol;

                            internal ArraySetValueMethod(ArrayTypeSymbol arrayTypeSymbol)
                            {
                                this.arrayTypeSymbol = arrayTypeSymbol;
                                var intType = arrayTypeSymbol.BaseType.ContainingAssembly.GetSpecialType(SpecialType.System_Int32);
                                this.parameters = ImmutableArray.Create<ParameterSymbol>(
                                    Enumerable.Range(0, arrayTypeSymbol.Rank).Select(n => new SynthesizedParameterSymbol(this, intType, n, RefKind.None))
                                    .ToArray<ParameterSymbol>()
                                    .Append(new SynthesizedParameterSymbol(this, arrayTypeSymbol.ElementType, arrayTypeSymbol.Rank + 1, RefKind.None)));
                            }

                            public override ImmutableArray<ParameterSymbol> Parameters
                            {
                                get { return parameters; }
                            }

                            #region Sealed

                            public sealed override Symbol ContainingSymbol
                            {
                                get
                                {
                                    return this.arrayTypeSymbol.BaseType;
                                }
                            }

                            public sealed override NamedTypeSymbol ContainingType
                            {
                                get
                                {
                                    return this.arrayTypeSymbol.BaseType;
                                }
                            }
                            
                            public override NamespaceSymbol ContainingNamespace
                            {
                                get { return this.arrayTypeSymbol.ElementType.ContainingNamespace; }
                            }
             
                            public sealed override string Name
                            {
                                get
                                {
                                    return "Set";
                                }
                            }

                            internal sealed override bool HasSpecialName
                            {
                                get { return true; }
                            }

                            internal sealed override System.Reflection.MethodImplAttributes ImplementationAttributes
                            {
                                get
                                {
                                    var containingType = this.arrayTypeSymbol.BaseType;
                                    if (containingType.IsComImport)
                                    {
                                        Debug.Assert(containingType.TypeKind == TypeKind.Class);
                                        return System.Reflection.MethodImplAttributes.Runtime | System.Reflection.MethodImplAttributes.InternalCall;
                                    }

                                    if (containingType.TypeKind == TypeKind.Delegate)
                                    {
                                        return System.Reflection.MethodImplAttributes.Runtime;
                                    }

                                    return default(System.Reflection.MethodImplAttributes);
                                }
                            }

                            internal sealed override bool RequiresSecurityObject
                            {
                                get { return false; }
                            }

                            public sealed override DllImportData GetDllImportData()
                            {
                                return null;
                            }

                            internal sealed override MarshalPseudoCustomAttributeData ReturnValueMarshallingInformation
                            {
                                get { return null; }
                            }

                            internal sealed override bool HasDeclarativeSecurity
                            {
                                get { return false; }
                            }

                            internal sealed override IEnumerable<Microsoft.Cci.SecurityAttribute> GetSecurityInformation()
                            {
                                throw ExceptionUtilities.Unreachable;
                            }

                            internal sealed override ImmutableArray<string> GetAppliedConditionalSymbols()
                            {
                                return ImmutableArray<string>.Empty;
                            }

                            public sealed override bool IsVararg
                            {
                                get { return false; }
                            }

                            public sealed override ImmutableArray<TypeParameterSymbol> TypeParameters
                            {
                                get { return ImmutableArray<TypeParameterSymbol>.Empty; }
                            }

                            internal sealed override LexicalSortKey GetLexicalSortKey()
                            {
                                //For the sake of matching the metadata output of the native compiler, make synthesized constructors appear last in the metadata.
                                //This is not critical, but it makes it easier on tools that are comparing metadata.
                                return LexicalSortKey.Last;
                            }

                            public sealed override ImmutableArray<Location> Locations
                            {
                                get { return ContainingType.Locations; }
                            }

                            public sealed override TypeSymbol ReturnType
                            {
                                get { return this.arrayTypeSymbol.BaseType.ContainingAssembly.GetSpecialType(SpecialType.System_Void); }
                            }

                            public sealed override ImmutableArray<CustomModifier> ReturnTypeCustomModifiers
                            {
                                get { return ImmutableArray<CustomModifier>.Empty; }
                            }

                            public sealed override ImmutableArray<TypeSymbol> TypeArguments
                            {
                                get { return ImmutableArray<TypeSymbol>.Empty; }
                            }

                            public sealed override Symbol AssociatedSymbol
                            {
                                get { return this.arrayTypeSymbol; }
                            }

                            public sealed override int Arity
                            {
                                get { return 0; }
                            }

                            public sealed override bool ReturnsVoid
                            {
                                get { return true; }
                            }

                            public sealed override MethodKind MethodKind
                            {
                                get { return MethodKind.PropertySet; }
                            }

                            public sealed override bool IsExtern
                            {
                                get
                                {
                                    // Synthesized constructors of ComImport type are extern
                                    NamedTypeSymbol containingType = this.ContainingType;
                                    return (object)containingType != null && containingType.IsComImport;
                                }
                            }

                            public sealed override bool IsSealed
                            {
                                get { return false; }
                            }

                            public sealed override bool IsAbstract
                            {
                                get { return false; }
                            }

                            public sealed override bool IsOverride
                            {
                                get { return false; }
                            }

                            public sealed override bool IsVirtual
                            {
                                get { return false; }
                            }

                            public sealed override bool IsStatic
                            {
                                get { return false; }
                            }

                            public sealed override bool IsAsync
                            {
                                get { return false; }
                            }

                            public sealed override bool HidesBaseMethodsByName
                            {
                                get { return false; }
                            }

                            internal sealed override bool IsMetadataNewSlot(bool ignoreInterfaceImplementationChanges = false)
                            {
                                return false;
                            }

                            internal sealed override bool IsMetadataVirtual(bool ignoreInterfaceImplementationChanges = false)
                            {
                                return false;
                            }

                            public sealed override bool IsExtensionMethod
                            {
                                get { return false; }
                            }

                            internal sealed override Microsoft.Cci.CallingConvention CallingConvention
                            {
                                get { return Microsoft.Cci.CallingConvention.HasThis; }
                            }

                            internal sealed override bool IsExplicitInterfaceImplementation
                            {
                                get { return false; }
                            }

                            public sealed override ImmutableArray<MethodSymbol> ExplicitInterfaceImplementations
                            {
                                get { return ImmutableArray<MethodSymbol>.Empty; }
                            }

                            #endregion

                            internal override bool IsMetadataFinal()
                            {
                                return false;
                            }

                            internal override bool GenerateDebugInfo
                            {
                                get { return false; }
                            }

                            public override Accessibility DeclaredAccessibility
                            {
                                get { return Accessibility.Public; }
                            }
                        }

                        private sealed class ArrayGetValueMethod : SynthesizedInstanceMethodSymbol
                        {
                            private readonly ImmutableArray<ParameterSymbol> parameters;
                            private readonly ArrayTypeSymbol arrayTypeSymbol;

                            internal ArrayGetValueMethod(ArrayTypeSymbol arrayTypeSymbol)
                            {
                                this.arrayTypeSymbol = arrayTypeSymbol;
                                var intType = arrayTypeSymbol.BaseType.ContainingAssembly.GetSpecialType(SpecialType.System_Int32);
                                this.parameters = ImmutableArray.Create<ParameterSymbol>(
                                    Enumerable.Range(0, arrayTypeSymbol.Rank).Select(n => new SynthesizedParameterSymbol(this, intType, n, RefKind.None))
                                    .ToArray<ParameterSymbol>());
                            }

                            public override ImmutableArray<ParameterSymbol> Parameters
                            {
                                get { return parameters; }
                            }

                            #region Sealed

                            public sealed override Symbol ContainingSymbol
                            {
                                get
                                {
                                    return this.arrayTypeSymbol.BaseType;
                                }
                            }

                            public sealed override NamedTypeSymbol ContainingType
                            {
                                get
                                {
                                    return this.arrayTypeSymbol.BaseType;
                                }
                            }

                            public override NamespaceSymbol ContainingNamespace
                            {
                                get { return this.arrayTypeSymbol.ElementType.ContainingNamespace; }
                            }

                            public sealed override string Name
                            {
                                get
                                {
                                    return "Get";
                                }
                            }

                            internal sealed override bool HasSpecialName
                            {
                                get { return true; }
                            }

                            internal sealed override System.Reflection.MethodImplAttributes ImplementationAttributes
                            {
                                get
                                {
                                    var containingType = this.arrayTypeSymbol.BaseType;
                                    if (containingType.IsComImport)
                                    {
                                        Debug.Assert(containingType.TypeKind == TypeKind.Class);
                                        return System.Reflection.MethodImplAttributes.Runtime | System.Reflection.MethodImplAttributes.InternalCall;
                                    }

                                    if (containingType.TypeKind == TypeKind.Delegate)
                                    {
                                        return System.Reflection.MethodImplAttributes.Runtime;
                                    }

                                    return default(System.Reflection.MethodImplAttributes);
                                }
                            }

                            internal sealed override bool RequiresSecurityObject
                            {
                                get { return false; }
                            }

                            public sealed override DllImportData GetDllImportData()
                            {
                                return null;
                            }

                            internal sealed override MarshalPseudoCustomAttributeData ReturnValueMarshallingInformation
                            {
                                get { return null; }
                            }

                            internal sealed override bool HasDeclarativeSecurity
                            {
                                get { return false; }
                            }

                            internal sealed override IEnumerable<Microsoft.Cci.SecurityAttribute> GetSecurityInformation()
                            {
                                throw ExceptionUtilities.Unreachable;
                            }

                            internal sealed override ImmutableArray<string> GetAppliedConditionalSymbols()
                            {
                                return ImmutableArray<string>.Empty;
                            }

                            public sealed override bool IsVararg
                            {
                                get { return false; }
                            }

                            public sealed override ImmutableArray<TypeParameterSymbol> TypeParameters
                            {
                                get { return ImmutableArray<TypeParameterSymbol>.Empty; }
                            }

                            internal sealed override LexicalSortKey GetLexicalSortKey()
                            {
                                //For the sake of matching the metadata output of the native compiler, make synthesized constructors appear last in the metadata.
                                //This is not critical, but it makes it easier on tools that are comparing metadata.
                                return LexicalSortKey.Last;
                            }

                            public sealed override ImmutableArray<Location> Locations
                            {
                                get { return ContainingType.Locations; }
                            }

                            public sealed override TypeSymbol ReturnType
                            {
                                get { return this.arrayTypeSymbol.ElementType; }
                            }

                            public sealed override ImmutableArray<CustomModifier> ReturnTypeCustomModifiers
                            {
                                get { return ImmutableArray<CustomModifier>.Empty; }
                            }

                            public sealed override ImmutableArray<TypeSymbol> TypeArguments
                            {
                                get { return ImmutableArray<TypeSymbol>.Empty; }
                            }

                            public sealed override Symbol AssociatedSymbol
                            {
                                get { return this.arrayTypeSymbol; }
                            }

                            public sealed override int Arity
                            {
                                get { return 0; }
                            }

                            public sealed override bool ReturnsVoid
                            {
                                get { return false; }
                            }

                            public sealed override MethodKind MethodKind
                            {
                                get { return MethodKind.PropertyGet; }
                            }

                            public sealed override bool IsExtern
                            {
                                get
                                {
                                    // Synthesized constructors of ComImport type are extern
                                    NamedTypeSymbol containingType = this.ContainingType;
                                    return (object)containingType != null && containingType.IsComImport;
                                }
                            }

                            public sealed override bool IsSealed
                            {
                                get { return false; }
                            }

                            public sealed override bool IsAbstract
                            {
                                get { return false; }
                            }

                            public sealed override bool IsOverride
                            {
                                get { return false; }
                            }

                            public sealed override bool IsVirtual
                            {
                                get { return false; }
                            }

                            public sealed override bool IsStatic
                            {
                                get { return false; }
                            }

                            public sealed override bool IsAsync
                            {
                                get { return false; }
                            }

                            public sealed override bool HidesBaseMethodsByName
                            {
                                get { return false; }
                            }

                            internal sealed override bool IsMetadataNewSlot(bool ignoreInterfaceImplementationChanges = false)
                            {
                                return false;
                            }

                            internal sealed override bool IsMetadataVirtual(bool ignoreInterfaceImplementationChanges = false)
                            {
                                return false;
                            }

                            public sealed override bool IsExtensionMethod
                            {
                                get { return false; }
                            }

                            internal sealed override Microsoft.Cci.CallingConvention CallingConvention
                            {
                                get { return Microsoft.Cci.CallingConvention.HasThis; }
                            }

                            internal sealed override bool IsExplicitInterfaceImplementation
                            {
                                get { return false; }
                            }

                            public sealed override ImmutableArray<MethodSymbol> ExplicitInterfaceImplementations
                            {
                                get { return ImmutableArray<MethodSymbol>.Empty; }
                            }

                            #endregion

                            internal override bool IsMetadataFinal()
                            {
                                return false;
                            }

                            internal override bool GenerateDebugInfo
                            {
                                get { return false; }
                            }

                            public override Accessibility DeclaredAccessibility
                            {
                                get { return Accessibility.Public; }
                            }
                        }

                        private sealed class ArrayAddressMethod : SynthesizedInstanceMethodSymbol
                        {
                            private readonly ImmutableArray<ParameterSymbol> parameters;
                            private readonly ArrayTypeSymbol arrayTypeSymbol;

                            internal ArrayAddressMethod(ArrayTypeSymbol arrayTypeSymbol)
                            {
                                this.arrayTypeSymbol = arrayTypeSymbol;
                                var intType = arrayTypeSymbol.BaseType.ContainingAssembly.GetSpecialType(SpecialType.System_Int32);
                                this.parameters = ImmutableArray.Create<ParameterSymbol>(
                                    Enumerable.Range(0, arrayTypeSymbol.Rank).Select(n => new SynthesizedParameterSymbol(this, intType, n, RefKind.None))
                                    .ToArray<ParameterSymbol>());
                            }

                            public override ImmutableArray<ParameterSymbol> Parameters
                            {
                                get { return parameters; }
                            }

                            #region Sealed

                            public sealed override Symbol ContainingSymbol
                            {
                                get
                                {
                                    return this.arrayTypeSymbol.BaseType;
                                }
                            }

                            public sealed override NamedTypeSymbol ContainingType
                            {
                                get
                                {
                                    return this.arrayTypeSymbol.BaseType;
                                }
                            }

                            public override NamespaceSymbol ContainingNamespace
                            {
                                get { return this.arrayTypeSymbol.ElementType.ContainingNamespace; }
                            }

                            public sealed override string Name
                            {
                                get
                                {
                                    return "Address";
                                }
                            }

                            internal sealed override bool HasSpecialName
                            {
                                get { return true; }
                            }

                            internal sealed override System.Reflection.MethodImplAttributes ImplementationAttributes
                            {
                                get
                                {
                                    var containingType = this.arrayTypeSymbol.BaseType;
                                    if (containingType.IsComImport)
                                    {
                                        Debug.Assert(containingType.TypeKind == TypeKind.Class);
                                        return System.Reflection.MethodImplAttributes.Runtime | System.Reflection.MethodImplAttributes.InternalCall;
                                    }

                                    if (containingType.TypeKind == TypeKind.Delegate)
                                    {
                                        return System.Reflection.MethodImplAttributes.Runtime;
                                    }

                                    return default(System.Reflection.MethodImplAttributes);
                                }
                            }

                            internal sealed override bool RequiresSecurityObject
                            {
                                get { return false; }
                            }

                            public sealed override DllImportData GetDllImportData()
                            {
                                return null;
                            }

                            internal sealed override MarshalPseudoCustomAttributeData ReturnValueMarshallingInformation
                            {
                                get { return null; }
                            }

                            internal sealed override bool HasDeclarativeSecurity
                            {
                                get { return false; }
                            }

                            internal sealed override IEnumerable<Microsoft.Cci.SecurityAttribute> GetSecurityInformation()
                            {
                                throw ExceptionUtilities.Unreachable;
                            }

                            internal sealed override ImmutableArray<string> GetAppliedConditionalSymbols()
                            {
                                return ImmutableArray<string>.Empty;
                            }

                            public sealed override bool IsVararg
                            {
                                get { return false; }
                            }

                            public sealed override ImmutableArray<TypeParameterSymbol> TypeParameters
                            {
                                get { return ImmutableArray<TypeParameterSymbol>.Empty; }
                            }

                            internal sealed override LexicalSortKey GetLexicalSortKey()
                            {
                                //For the sake of matching the metadata output of the native compiler, make synthesized constructors appear last in the metadata.
                                //This is not critical, but it makes it easier on tools that are comparing metadata.
                                return LexicalSortKey.Last;
                            }

                            public sealed override ImmutableArray<Location> Locations
                            {
                                get { return ContainingType.Locations; }
                            }

                            public sealed override TypeSymbol ReturnType
                            {
                                get { return new ByRefReturnErrorTypeSymbol(this.arrayTypeSymbol.ElementType); }
                            }

                            public sealed override ImmutableArray<CustomModifier> ReturnTypeCustomModifiers
                            {
                                get { return ImmutableArray<CustomModifier>.Empty; }
                            }

                            public sealed override ImmutableArray<TypeSymbol> TypeArguments
                            {
                                get { return ImmutableArray<TypeSymbol>.Empty; }
                            }

                            public sealed override Symbol AssociatedSymbol
                            {
                                get { return this.arrayTypeSymbol; }
                            }

                            public sealed override int Arity
                            {
                                get { return 0; }
                            }

                            public sealed override bool ReturnsVoid
                            {
                                get { return false; }
                            }

                            public sealed override MethodKind MethodKind
                            {
                                get { return MethodKind.PropertyGet; }
                            }

                            public sealed override bool IsExtern
                            {
                                get
                                {
                                    // Synthesized constructors of ComImport type are extern
                                    NamedTypeSymbol containingType = this.ContainingType;
                                    return (object)containingType != null && containingType.IsComImport;
                                }
                            }

                            public sealed override bool IsSealed
                            {
                                get { return false; }
                            }

                            public sealed override bool IsAbstract
                            {
                                get { return false; }
                            }

                            public sealed override bool IsOverride
                            {
                                get { return false; }
                            }

                            public sealed override bool IsVirtual
                            {
                                get { return false; }
                            }

                            public sealed override bool IsStatic
                            {
                                get { return false; }
                            }

                            public sealed override bool IsAsync
                            {
                                get { return false; }
                            }

                            public sealed override bool HidesBaseMethodsByName
                            {
                                get { return false; }
                            }

                            internal sealed override bool IsMetadataNewSlot(bool ignoreInterfaceImplementationChanges = false)
                            {
                                return false;
                            }

                            internal sealed override bool IsMetadataVirtual(bool ignoreInterfaceImplementationChanges = false)
                            {
                                return false;
                            }

                            public sealed override bool IsExtensionMethod
                            {
                                get { return false; }
                            }

                            internal sealed override Microsoft.Cci.CallingConvention CallingConvention
                            {
                                get { return Microsoft.Cci.CallingConvention.HasThis; }
                            }

                            internal sealed override bool IsExplicitInterfaceImplementation
                            {
                                get { return false; }
                            }

                            public sealed override ImmutableArray<MethodSymbol> ExplicitInterfaceImplementations
                            {
                                get { return ImmutableArray<MethodSymbol>.Empty; }
                            }

                            #endregion

                            internal override bool IsMetadataFinal()
                            {
                                return false;
                            }

                            internal override bool GenerateDebugInfo
                            {
                                get { return false; }
                            }

                            public override Accessibility DeclaredAccessibility
                            {
                                get { return Accessibility.Public; }
                            }
                        }
             
             */

            /*
             * FIX for Roslyn code
             *  internal override Symbol GetSymbolForMemberRef(MemberReferenceHandle memberRef, TypeSymbol scope = null, bool methodsOnly = false)
                {
                    TypeSymbol targetTypeSymbol = GetMemberRefTypeSymbol(memberRef);

                    // TODO: ASD: MY ADDON
                    if (targetTypeSymbol == null)
                    {
                        Handle container = Module.GetContainingTypeOrThrow(memberRef);
                        HandleType containerType = container.HandleType;
                        if (containerType == HandleType.Method)
                        {
                            return GetSymbolForILToken(container);
                        }
                    }
             
                    ...
             */

            /*
             * FIX for Roslyn Code - 2
                protected override TypeSymbol GetGenericTypeParamSymbol(int position)
                {
                    ...
                    // TODO: ASD ADDON
                    ArrayTypeSymbol arrayTypeSymbol = this.containingType as ArrayTypeSymbol;
                    if ((object)arrayTypeSymbol != null && position == 0 && arrayTypeSymbol.ElementType is TypeParameterSymbol)
                    {
                        return arrayTypeSymbol.ElementType;
                    }

                    ...
                }             
             */

            var symbolForIlToken = peModuleSymbol.GetMetadataDecoder(genericContext).GetSymbolForILToken(methodHandle);
            var methodSymbol = symbolForIlToken as MethodSymbol;
            if (methodSymbol != null)
            {
                if (methodSymbol.MethodKind == MethodKind.Constructor)
                {
                    return new MetadataConstructorAdapter(SubstituteMethodSymbolIfNeeded(methodSymbol, genericContext));
                }

                return new MetadataMethodAdapter(SubstituteMethodSymbolIfNeeded(methodSymbol, genericContext));
            }

            throw new KeyNotFoundException();
        }

        internal static TypeSymbol SubstituteTypeSymbolIfNeeded(TypeSymbol typeSymbol, IGenericContext genericContext)
        {
            if (typeSymbol == null || typeSymbol.TypeKind == TypeKind.Error)
            {
                throw new KeyNotFoundException();
            }

            if (MetadataGenericContext.IsNullOrEmptyOrNoSpecializations(genericContext))
            {
                return typeSymbol;
            }

            var typeSubstitution = GetTypeSubstitutionMap(genericContext);
            var resolvedType = typeSubstitution.SubstituteType(typeSymbol);
            return resolvedType;
        }

        private static AbstractTypeParameterMap GetTypeSubstitutionMap(IGenericContext genericContext)
        {
            if (MetadataGenericContext.IsNullOrEmptyOrNoSpecializations(genericContext))
            {
                throw new ArgumentException("genericContext");
            }

            var metadataGenericContext = genericContext as MetadataGenericContext;
            if (metadataGenericContext != null && metadataGenericContext.CustomTypeSubstitution != null)
            {
                return metadataGenericContext.CustomTypeSubstitution;
            }


            if (genericContext.MethodSpecialization != null)
            {
                var metadataMethodAdapter = (genericContext.MethodSpecialization as MetadataMethodAdapter);
                if (metadataMethodAdapter != null)
                {
                    return metadataMethodAdapter.MethodDef.TypeSubstitution;
                }
            }

            if (genericContext.TypeSpecialization != null)
            {
                var metadataTypeAdapter = (genericContext.TypeSpecialization as MetadataTypeAdapter);
                if (metadataTypeAdapter != null)
                {
                    var namedTypeSymbol = metadataTypeAdapter.TypeDef as NamedTypeSymbol;
                    if (namedTypeSymbol != null)
                    {
                        return namedTypeSymbol.TypeSubstitution;
                    }
                }
            }

            throw new Exception("Context is empty");
        }

        private static FieldSymbol SubstituteFieldSymbolIfNeeded(FieldSymbol fieldSymbol, IGenericContext genericContext)
        {
            if (MetadataGenericContext.IsNullOrEmptyOrNoSpecializations(genericContext))
            {
                return fieldSymbol;
            }

            var resolvedFieldSymbol = fieldSymbol;
            var field = new MetadataFieldAdapter(fieldSymbol);
            if (field.DeclaringType.IsGenericTypeDefinition)
            {
                var constructedContainingType = SubstituteTypeSymbolIfNeeded(fieldSymbol.ContainingType, genericContext);
                var substitutedNamedTypeSymbol = constructedContainingType as SubstitutedNamedTypeSymbol;
                resolvedFieldSymbol = new SubstitutedFieldSymbol(substitutedNamedTypeSymbol, fieldSymbol);
                return resolvedFieldSymbol;
            }

            return resolvedFieldSymbol;
        }

        internal static MethodSymbol SubstituteMethodSymbolIfNeeded(MethodSymbol methodSymbol, IGenericContext genericContext)
        {
            if (MetadataGenericContext.IsNullOrEmptyOrNoSpecializations(genericContext))
            {
                return methodSymbol;
            }

            var method = new MetadataMethodAdapter(methodSymbol);
            if (!method.IsGenericMethodDefinition && !method.DeclaringType.IsGenericTypeDefinition)
            {
                return methodSymbol;
            }

            var resolvedMethodSymbol = methodSymbol;
            if (method.DeclaringType.IsGenericTypeDefinition)
            {
                var constructedContainingType = SubstituteTypeSymbolIfNeeded(methodSymbol.ContainingType, genericContext);
                var substitutedNamedTypeSymbol = constructedContainingType as SubstitutedNamedTypeSymbol;
                if (substitutedNamedTypeSymbol != null)
                {
                    resolvedMethodSymbol = new SubstitutedMethodSymbol(
                        substitutedNamedTypeSymbol,
                        methodSymbol.ConstructedFrom.OriginalDefinition);
                }
                else
                {
                    var typeDef = (method.DeclaringType as MetadataTypeAdapter).TypeDef;
                    constructedContainingType = SubstituteTypeSymbolIfNeeded(typeDef, genericContext);
                    var arrayTypeSymbol = constructedContainingType as ArrayTypeSymbol;
                    if (arrayTypeSymbol != null)
                    {
                        resolvedMethodSymbol = arrayTypeSymbol.GetMembers(methodSymbol.Name)[0] as MethodSymbol;
                    }
                    else
                    {
                        Debug.Fail("Not implemented yet");
                    }
                }
            }

            if (method.IsGenericMethodDefinition || method.IsGenericMethod)
            {
                return ConstructMethodSymbol(resolvedMethodSymbol, methodSymbol.TypeSubstitution, genericContext);
            }

            return resolvedMethodSymbol;
        }

        private static ImmutableArray<TypeSymbol> GetTypeArguments(ImmutableArray<TypeParameterSymbol> definitionTypeParameters, NamedTypeSymbol namedTypeSymbol, AbstractTypeParameterMap typeSubstitution)
        {
            var current = namedTypeSymbol;
            while (current != null && current.TypeArguments.Length == 0)
            {
                current = namedTypeSymbol.ContainingType;
            }

            if (current == null)
            {
                throw new Exception("Could not find TypeArguments in Generic Context");
            }

            return GetTypeArguments(definitionTypeParameters, current.TypeSubstitution, typeSubstitution);
        }

        private static ImmutableArray<TypeSymbol> GetTypeArguments(ImmutableArray<TypeParameterSymbol> definitionTypeParameters, AbstractTypeParameterMap currentTypeMap, AbstractTypeParameterMap typeSubstitutionOpt)
        {
            var typeArguments = ImmutableArray.CreateBuilder<TypeSymbol>(definitionTypeParameters.Length);

            foreach (var typeParameterSymbol in definitionTypeParameters)
            {
                if (typeSubstitutionOpt != null)
                {
                    var substitutedType = currentTypeMap.SubstituteType(typeParameterSymbol);
                    if (!object.ReferenceEquals(substitutedType, typeParameterSymbol))
                    {
                        typeArguments.Add(currentTypeMap.SubstituteType(substitutedType));
                    }
                    else
                    {
                        var substitutedTypeOpt = typeSubstitutionOpt.SubstituteType(typeParameterSymbol.OriginalDefinition);
                        typeArguments.Add(currentTypeMap.SubstituteType(substitutedTypeOpt));
                    }
                }
                else
                {
                    typeArguments.Add(currentTypeMap.SubstituteType(typeParameterSymbol));
                }
            }

            return typeArguments.ToImmutableArray();
        }

        private static ImmutableArray<TypeSymbol> GetTypeArguments(ImmutableArray<TypeParameterSymbol> definitionTypeParameters, MethodSymbol methodSymbol, AbstractTypeParameterMap typeSubstitution)
        {
            var current = methodSymbol;
            if (current == null)
            {
                throw new Exception("Could not find TypeArguments in Generic Context");
            }

            return GetTypeArguments(definitionTypeParameters, current.TypeSubstitution, typeSubstitution);
        }

        private static MethodSymbol ConstructMethodSymbol(MethodSymbol methodSymbol, AbstractTypeParameterMap methodTypeSubstitution, IGenericContext genericContext)
        {
            var metadataGenericContext = genericContext as MetadataGenericContext;
            if (metadataGenericContext != null)
            {
                var customTypeSubstitution = metadataGenericContext.CustomTypeSubstitution;
                if (customTypeSubstitution != null)
                {
                    return new ConstructedMethodSymbol(methodSymbol, GetTypeArguments(methodSymbol.OriginalDefinition.TypeParameters, customTypeSubstitution, null));
                }
            }

            var metadataMethodAdapter = genericContext.MethodSpecialization as MetadataMethodAdapter;
            if (metadataMethodAdapter != null)
            {
                var methodSymbolContext = metadataMethodAdapter.MethodDef;
                var constructedFrom = methodSymbol.ConstructedFrom;
                return new ConstructedMethodSymbol(constructedFrom.ConstructedFrom, GetTypeArguments(constructedFrom.TypeParameters, methodSymbolContext, methodTypeSubstitution));
            }

            var metadataTypeAdapter = genericContext.TypeSpecialization as MetadataTypeAdapter;
            if (metadataTypeAdapter != null)
            {
                var namedTypeSymbolContext = metadataTypeAdapter.TypeDef as NamedTypeSymbol;
                var constructedFrom = methodSymbol.ConstructedFrom;
                return new ConstructedMethodSymbol(constructedFrom.ConstructedFrom, GetTypeArguments(constructedFrom.TypeParameters, namedTypeSymbolContext, methodTypeSubstitution));
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="token">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public string ResolveString(int token)
        {
            var peModuleSymbol = this.moduleDef as PEModuleSymbol;
            var peModule = peModuleSymbol.Module;

            var stringHandle = MetadataTokens.Handle(token);

            switch (stringHandle.HandleType)
            {
                case HandleType.UserString:
                    return peModule.MetadataReader.GetUserString((UserStringHandle)stringHandle);
                case HandleType.String:
                    return peModule.MetadataReader.GetString((StringHandle)stringHandle);
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="token">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// </exception>
        public object ResolveToken(int token, IGenericContext genericContext)
        {
            var peModuleSymbol = this.moduleDef as PEModuleSymbol;

            var typeDefHandle = MetadataTokens.Handle(token);
            var symbolForIlToken = peModuleSymbol.GetMetadataDecoder(genericContext).GetSymbolForILToken(typeDefHandle);
            var typeSymbol = symbolForIlToken as TypeSymbol;
            if (typeSymbol != null && typeSymbol.TypeKind != TypeKind.Error)
            {
                return SubstituteTypeSymbolIfNeeded(typeSymbol, genericContext).ToAdapter();
            }

            var fieldSymbol = symbolForIlToken as FieldSymbol;
            if (fieldSymbol != null)
            {
                return new MetadataFieldAdapter(SubstituteFieldSymbolIfNeeded(fieldSymbol, genericContext));
            }

            var methodSymbol = symbolForIlToken as MethodSymbol;
            if (methodSymbol != null)
            {
                return new MetadataMethodAdapter(SubstituteMethodSymbolIfNeeded(methodSymbol, genericContext));
            }

            throw new KeyNotFoundException();
        }

        /// <summary>
        /// </summary>
        /// <param name="token">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// </exception>
        public IType ResolveType(int token, IGenericContext genericContext)
        {
            var peModuleSymbol = this.moduleDef as PEModuleSymbol;
            var typeDefHandle = MetadataTokens.Handle(token);
            var typeSymbol = peModuleSymbol.GetMetadataDecoder(genericContext).GetSymbolForILToken(typeDefHandle) as TypeSymbol;
            return SubstituteTypeSymbolIfNeeded(typeSymbol, genericContext).ToAdapter();
        }

        /// <summary>
        /// </summary>
        /// <param name="fullName">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// </exception>
        public IType ResolveType(string fullName, IGenericContext genericContext)
        {
            var peModuleSymbol = this.moduleDef as PEModuleSymbol;

            var typeSymbol = peModuleSymbol.GetMetadataDecoder(genericContext).GetTypeSymbolForSerializedType(fullName);
            if (typeSymbol.TypeKind == TypeKind.Error)
            {
                // try to find it in CoreLib
                typeSymbol =
                    new MetadataDecoder(peModuleSymbol.ContainingAssembly.CorLibrary.Modules[0] as PEModuleSymbol).GetTypeSymbolForSerializedType(fullName);
            }

            if (typeSymbol != null && typeSymbol.TypeKind != TypeKind.Error)
            {
                if (genericContext == null)
                {
                    return typeSymbol.OriginalDefinition.ToAdapter();
                }

                return typeSymbol.ToAdapter();
            }

            throw new KeyNotFoundException();
        }
    }
}