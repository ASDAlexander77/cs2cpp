// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetadataGenericContext.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PEAssemblyReader
{
    using System;
    using System.Collections.Immutable;
    using System.Diagnostics;
    using System.Linq;

    using Microsoft.CodeAnalysis.CSharp.Symbols;

    /// <summary>
    /// </summary>
    public class MetadataGenericContext : IGenericContext
    {
        private AbstractTypeParameterMap cachedMap;

        /// <summary>
        /// </summary>
        protected MetadataGenericContext()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="allowToUseDefinitionAsSpecialization">
        /// </param>
        protected MetadataGenericContext(IType type, bool allowToUseDefinitionAsSpecialization = false)
            : this()
        {
            this.Init(type, allowToUseDefinitionAsSpecialization);
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <param name="allowToUseDefinitionAsSpecialization">
        /// </param>
        protected MetadataGenericContext(IMethod method, bool allowToUseDefinitionAsSpecialization = false)
            : this(method.DeclaringType, allowToUseDefinitionAsSpecialization)
        {
            this.Init(method, allowToUseDefinitionAsSpecialization);
        }

        public static bool IsNullOrEmptyOrNoSpecializations(IGenericContext genericContext)
        {
            return genericContext == null || genericContext.IsEmpty || !genericContext.AnySpecializations;
        }

        /// <summary>
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return this.TypeDefinition == null && this.TypeSpecialization == null
                       && this.MethodDefinition == null && this.MethodSpecialization == null
                       && this.CustomTypeSubstitution == null;
            }
        }

        public bool AnySpecializations
        {
            get
            {
                return this.TypeSpecialization != null || this.MethodSpecialization != null || this.CustomTypeSubstitution != null;
            }
        }

        /// <summary>
        /// </summary>
        public IMethod MethodDefinition { get; private set; }

        /// <summary>
        /// </summary>
        public IMethod MethodSpecialization { get; private set; }

        /// <summary>
        /// </summary>
        public IType TypeDefinition { get; set; }

        /// <summary>
        /// </summary>
        public IType TypeSpecialization { get; private set; }

        /// <summary>
        /// </summary>
        internal AbstractTypeParameterMap CustomTypeSubstitution { get; set; }

        public IGenericContext Clone()
        {
            return (IGenericContext)this.MemberwiseClone();
        }

        public static IGenericContext Create(IType typeDefinition, IType typeSpecialization, IMethod methodDefinition, IMethod methodSpecialization)
        {
            var context = new MetadataGenericContext();
            context.Init(typeDefinition);
            context.TypeSpecialization = typeSpecialization;
            context.Init(methodDefinition);
            context.MethodSpecialization = methodSpecialization;
            return context;
        }

        public static IGenericContext Create(IType typeDefinition, IType typeSpecialization)
        {
            var context = new MetadataGenericContext(typeDefinition);
            context.TypeSpecialization = typeSpecialization;
            return context;
        }

        public static IGenericContext Create(IMethod methodDefinition, IMethod methodSpecialization)
        {
            var context = new MetadataGenericContext(methodDefinition);
            context.MethodSpecialization = methodSpecialization;
            return context;
        }

        public static IGenericContext CreateCustomMap(IMethod methodDefinition, IMethod methodSpecialization, IMethod additionalMethodDefinition = null)
        {
            var context = new MetadataGenericContext();
            context.CustomTypeSubstitution = CreateMap(methodDefinition, methodSpecialization, additionalMethodDefinition);
            return context;
        }

        private static MutableTypeMap CreateMap(IMethod methodDefinition, IMethod methodSpecialization, IMethod additionalMethodDefinition = null)
        {
            var customTypeSubstitution = new MutableTypeMap();

            var methodSpecAdapter = methodSpecialization as MetadataMethodAdapter;
            if (methodSpecAdapter != null)
            {
                var methodSymbolSpec = methodSpecAdapter.MethodDef;
                AppendMapping(customTypeSubstitution, methodSymbolSpec);
            }

            var methodDefAdapter = methodDefinition as MetadataMethodAdapter;
            if (methodDefAdapter != null)
            {
                var methodSymbolDef = methodDefAdapter.MethodDef;
                AppendMapping(customTypeSubstitution, methodSymbolDef, true);
            }

            if (additionalMethodDefinition != null)
            {
                var metadataMethodAdapter = additionalMethodDefinition as MetadataMethodAdapter;
                if (metadataMethodAdapter != null && methodSpecAdapter != null)
                {
                    var additionalMethodSymbolDef = metadataMethodAdapter.MethodDef;
                    AppendMethodDirectMapping(customTypeSubstitution, methodSpecAdapter.MethodDef, additionalMethodSymbolDef.OriginalDefinition);

                    // to support explicit interfaces
                    ////AppendMappingSpecialCaseForBaseType(customTypeSubstitution, methodSpecAdapter.MethodDef.ReceiverType as NamedTypeSymbol, additionalMethodSymbolDef.OriginalDefinition.ReceiverType as NamedTypeSymbol);

                    // additional map from base type or interface
                    var baseType = FindBaseOrInterface(methodSpecAdapter.MethodDef.ContainingType, additionalMethodSymbolDef.ContainingType);
                    AppendMappingSpecialCaseForBaseType(customTypeSubstitution, methodSpecAdapter.MethodDef.ContainingType, baseType);
                }
            }

            return customTypeSubstitution;
        }

        private static NamedTypeSymbol FindBaseOrInterface(NamedTypeSymbol baseTypeOrInterface, NamedTypeSymbol type)
        {
            if (baseTypeOrInterface.IsInterface)
            {
                return type.AllInterfaces.FirstOrDefault(i => i.MetadataName == baseTypeOrInterface.MetadataName);
            }

            var currentBase = type.BaseType;
            while (currentBase != null && currentBase.MetadataName != baseTypeOrInterface.MetadataName)
            {
                currentBase = currentBase.BaseType;
            }

            return currentBase;
        }

        private static MutableTypeMap CreateMap(IType typeDefinition, IType typeSpecialization, IMethod methodDefinition, IMethod methodSpecialization, IMethod additionalMethodDefinition = null)
        {
            var typeMap = CreateMap(methodDefinition, methodSpecialization, additionalMethodDefinition);

            var typeSpecAdapter = typeSpecialization as MetadataTypeAdapter;
            if (typeSpecAdapter != null)
            {
                var typeSymbolSpec = typeSpecAdapter.TypeDef;
                AppendMapping(typeMap, typeSymbolSpec as NamedTypeSymbol);
            }

            var typeDefAdapter = typeDefinition as MetadataTypeAdapter;
            if (typeDefAdapter != null)
            {
                var typeSymbolDef = typeDefAdapter.TypeDef;
                AppendMapping(typeMap, typeSymbolDef as NamedTypeSymbol, true);
            }

            return typeMap;
        }

        private static void AppendMapping(MutableTypeMap customTypeSubstitution, MethodSymbol methodSymbol, bool invert = false)
        {
            for (var i = 0; i < methodSymbol.TypeParameters.Length; i++)
            {
                var typeParameterSymbol = methodSymbol.TypeParameters[i];
                var typeArgument = methodSymbol.TypeArguments[i];
                if (!ReferenceEquals(typeParameterSymbol, typeArgument))
                {
                    if (invert)
                    {
                        Debug.Assert(typeArgument is TypeParameterSymbol, "TypeParameterSymbol is required");
                        customTypeSubstitution.Add(typeArgument as TypeParameterSymbol, customTypeSubstitution.SubstituteType(typeParameterSymbol));
                    }
                    else
                    {
                        customTypeSubstitution.Add(typeParameterSymbol, typeArgument);
                    }
                }
            }

            AppendMapping(customTypeSubstitution, methodSymbol.ContainingType, invert);
        }

        private static void AppendMapping(MutableTypeMap customTypeSubstitution, NamedTypeSymbol namedTypeSymbol, bool invert = false)
        {
            if (namedTypeSymbol == null)
            {
                return;
            }

            for (var i = 0; i < namedTypeSymbol.TypeParameters.Length; i++)
            {
                var typeParameterSymbol = namedTypeSymbol.TypeParameters[i];
                var typeArgument = namedTypeSymbol.TypeArguments[i];
                if (!ReferenceEquals(typeParameterSymbol, typeArgument))
                {
                    if (invert)
                    {
                        var parameterSymbol = typeArgument as TypeParameterSymbol;
                        if (parameterSymbol != null)
                        {
                            customTypeSubstitution.Add(parameterSymbol, customTypeSubstitution.SubstituteType(typeParameterSymbol));
                        }
                    }
                    else
                    {
                        customTypeSubstitution.Add(typeParameterSymbol, typeArgument);
                    }
                }
            }
        }

        private static void AppendMethodDirectMapping(MutableTypeMap customTypeSubstitution, MethodSymbol methodSymbolSpec, MethodSymbol methodSymbolDef)
        {
            for (var i = 0; i < methodSymbolSpec.TypeParameters.Length; i++)
            {
                var typeParameterSymbol = methodSymbolDef.TypeParameters[i];
                var typeArgument = methodSymbolSpec.TypeArguments[i];
                if (!ReferenceEquals(typeParameterSymbol, typeArgument))
                {
                    customTypeSubstitution.Add(typeParameterSymbol, typeArgument);
                }
            }
        }

        private static void AppendMappingSpecialCaseForBaseType(
            MutableTypeMap customTypeSubstitution, NamedTypeSymbol namedTypeSymbolSpec, NamedTypeSymbol namedTypeSymbolDef)
        {
            if (namedTypeSymbolSpec == null || namedTypeSymbolDef == null)
            {
                return;
            }

            for (var i = 0; i < namedTypeSymbolDef.TypeParameters.Length; i++)
            {
                var typeParameterSymbol = namedTypeSymbolDef.TypeArguments[i] as TypeParameterSymbol;
                var typeArgument = namedTypeSymbolSpec.TypeArguments[i];
                if (!ReferenceEquals(typeParameterSymbol, typeArgument) && typeArgument != null && typeParameterSymbol != null)
                {
                    if (customTypeSubstitution.SubstituteType(typeParameterSymbol) == typeParameterSymbol)
                    {
                        customTypeSubstitution.Add(typeParameterSymbol, typeArgument);
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <param name="allowToUseDefinitionAsSpecialization">
        /// </param>
        /// <returns>
        /// </returns>
        public static IGenericContext DiscoverFrom(IMethod method, bool allowToUseDefinitionAsSpecialization = false)
        {
            if (method.IsGenericMethod || method.IsGenericMethodDefinition)
            {
                return new MetadataGenericContext(method, allowToUseDefinitionAsSpecialization);
            }

            return DiscoverFrom(method.DeclaringType, allowToUseDefinitionAsSpecialization);
        }

        public static IGenericContext DiscoverFrom(IType declType, bool allowToUseDefinitionAsSpecialization = false)
        {
            while (declType != null)
            {
                if (declType.IsGenericType || declType.IsGenericTypeDefinition)
                {
                    return new MetadataGenericContext(declType, allowToUseDefinitionAsSpecialization);
                }

                if (declType.IsNested)
                {
                    declType = declType.DeclaringType;
                    continue;
                }

                break;
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="typeParameter">
        /// </param>
        /// <returns>
        /// </returns>
        public IType ResolveTypeParameter(IType typeParameter)
        {
            if (!typeParameter.IsGenericTypeDefinition)
            {
                return typeParameter;
            }

            if (this.cachedMap == null)
            {
                this.cachedMap = CreateMap(this.TypeDefinition, this.TypeSpecialization, this.MethodDefinition, this.MethodSpecialization);
            }

            var map = this.cachedMap;

            var typeDefAdapter = typeParameter as MetadataTypeAdapter;
            if (typeDefAdapter != null)
            {
                IType elementType;
                if (typeDefAdapter.IsPointer)
                {
                    elementType = typeDefAdapter.GetElementType();
                    return ResolveTypeParameter(elementType).ToPointerType();
                }

                if (typeDefAdapter.IsByRef)
                {
                    elementType = typeDefAdapter.GetElementType();
                    if (elementType.IsPinned)
                    {
                        return ResolveTypeParameter(elementType).ToByRefTypeAndPinned();
                    }

                    return ResolveTypeParameter(elementType).ToByRefType();
                }

                if (typeDefAdapter.IsArray)
                {
                    elementType = typeDefAdapter.GetElementType();
                    return ResolveTypeParameter(elementType).ToArrayType(typeDefAdapter.ArrayRank);
                }

                if (!typeParameter.IsGenericParameter)
                {
                    return typeParameter;
                }

                var typeSymbolDef = typeDefAdapter.TypeDef;
                var newType = map.SubstituteType(typeSymbolDef);
                return new MetadataTypeAdapter(newType);
            }

            return typeParameter;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="allowToUseDefinitionAsSpecialization">
        /// </param>
        private void Init(IType type, bool allowToUseDefinitionAsSpecialization = false)
        {
            if (type == null)
            {
                return;
            }

            if (type.IsGenericTypeDefinition)
            {
                this.TypeDefinition = type;
                if (allowToUseDefinitionAsSpecialization)
                {
                    this.TypeSpecialization = type;
                }
            }

            if (type.IsGenericType)
            {
                this.TypeSpecialization = type;
                if (this.TypeDefinition == null)
                {
                    this.TypeDefinition = type.GetTypeDefinition();
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <param name="allowToUseDefinitionAsSpecialization">
        /// </param>
        private void Init(IMethod method, bool allowToUseDefinitionAsSpecialization = false)
        {
            if (method == null)
            {
                return;
            }

            if (method.IsGenericMethodDefinition)
            {
                this.MethodDefinition = method;
                if (allowToUseDefinitionAsSpecialization)
                {
                    this.MethodSpecialization = method;
                }
            }

            if (method.IsGenericMethod)
            {
                this.MethodSpecialization = method;
                if (this.MethodDefinition == null)
                {
                    this.MethodDefinition = method.GetMethodDefinition();
                }
            }
        }
    }
}