// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Extentions.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PEAssemblyReader
{
    using System.Collections.Immutable;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Symbols.Metadata.PE;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// </summary>
    public static class Extentions
    {
        /// <summary>
        /// </summary>
        /// <param name="thisType">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsDerivedFrom(this IType thisType, IType type)
        {
            Debug.Assert(type != null, "type is null");

            if (thisType.Equals(type))
            {
                return false;
            }

            var t = thisType.BaseType;
            while (t != null)
            {
                if (type.TypeEquals(t))
                {
                    return true;
                }

                t = t.BaseType;
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="other">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool TypeEquals(this IType type, IType other)
        {
            return type != null && other.CompareTo(type) == 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="other">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool TypeNotEquals(this IType type, IType other)
        {
            return !type.TypeEquals(other);
        }

        /// <summary>
        /// </summary>
        /// <param name="symbol">
        /// </param>
        /// <returns>
        /// </returns>
        internal static string CalculateNamespace(this Symbol symbol)
        {
            var namespaceSrc = symbol.ContainingNamespace;

            if (symbol.ContainingType.IsNestedType())
            {
                namespaceSrc = symbol.ContainingType.ContainingNamespace;
            }

            if (namespaceSrc == null)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            foreach (var namespacePart in namespaceSrc.ConstituentNamespaces)
            {
                if (namespacePart.IsGlobalNamespace)
                {
                    continue;
                }

                sb.Append(namespacePart);
            }

            return sb.ToString();
        }

        internal static IType ToType(this TypeSymbol typeSymbol)
        {
            return new MetadataTypeAdapter(typeSymbol);
        }

        internal static IDictionary<IType, IType> GenericMap(this IType type, IDictionary<IType, IType> map)
        {
            map.GenericMap(type.GenericTypeParameters, type.GenericTypeArguments);
            if (type.DeclaringType != null)
            {
                map = type.DeclaringType.GenericMap(map);
            }

            return map;
        }

        internal static IDictionary<IType, IType> GenericMap(this IMethod method, IDictionary<IType, IType> map)
        {
            map.GenericMap(method.GetGenericParameters(), method.GetGenericArguments());
            if (method.DeclaringType != null)
            {
                map = method.DeclaringType.GenericMap(map);
            }

            return map;
        }

        internal static IDictionary<IType, IType> GenericMap(this IDictionary<IType, IType> map, IEnumerable<IType> parameters, IEnumerable<IType> arguments)
        {
            var typeParameters = parameters.ToList();
            var typeArguments = arguments.ToList();

            for (var index = 0; index < typeArguments.Count; index++)
            {
                map[typeParameters[index]] = typeArguments[index];
            }

            return map;
        }

        internal static IType ResolveGeneric(this TypeSymbol typeSymbol, IGenericContext genericContext)
        {
            IType effectiveType = new MetadataTypeAdapter(typeSymbol, genericContext);
            if (genericContext != null && !genericContext.IsEmpty)
            {
                if (typeSymbol.IsTypeParameter())
                {
                    if (genericContext.TypeSpecialization != null)
                    {
                        var newType = genericContext.TypeSpecialization.ResolveTypeParameter(effectiveType);
                        if (newType != null)
                        {
                            return newType;
                        }
                    }

                    if (genericContext.MethodSpecialization != null)
                    {
                        var newType = genericContext.MethodSpecialization.ResolveTypeParameter(effectiveType);
                        if (newType != null)
                        {
                            return newType;
                        }
                    }

                    Debug.Assert(false, "Generic parameter has not bee resolved");
                }

                var arrayType = typeSymbol as ArrayTypeSymbol;
                if (arrayType != null)
                {
                    return arrayType.ElementType.ResolveGeneric(genericContext).ToArrayType(arrayType.Rank);
                }

                var namedTypeSymbol = typeSymbol as NamedTypeSymbol;
                if (namedTypeSymbol != null)
                {
                    var metadataType = new MetadataTypeAdapter(namedTypeSymbol);
                    if (metadataType.IsGenericTypeDefinition)
                    {
                        var map = genericContext.TypeSpecialization.GenericMap(genericContext.Map);
                        var mapFilteredByTypeParameters = namedTypeSymbol.TypeArguments != null
                            ? SelectGenericsFromArguments(namedTypeSymbol, map)
                            : SelectGenericsFromParameters(namedTypeSymbol, map);

                        var newType = new ConstructedNamedTypeSymbol(
                            namedTypeSymbol.ConstructedFrom,
                            ImmutableArray.Create(mapFilteredByTypeParameters));

                        return new MetadataTypeAdapter(newType, genericContext);
                    }
                }
            }

            return effectiveType;
        }

        private static TypeSymbol[] SelectGenericsFromParameters(NamedTypeSymbol namedTypeSymbol, IDictionary<IType, IType> map)
        {
            return map
                .Where(pair => (namedTypeSymbol.TypeParameters).Select(t => t).Any(tp => tp.Name == pair.Key.Name))
                .Select(pair => (pair.Value as MetadataTypeAdapter).TypeDef).ToArray();
        }

        private static TypeSymbol[] SelectGenericsFromArguments(NamedTypeSymbol namedTypeSymbol, IDictionary<IType, IType> map)
        {
            return map
                .Where(pair => (namedTypeSymbol.TypeArguments).Select(t => t).Any(tp => tp.Name == pair.Key.Name))
                .Select(pair => (pair.Value as MetadataTypeAdapter).TypeDef).ToArray();
        }

        internal static void AppendFullNamespace(this Symbol symbol, StringBuilder sb, string @namespace, IType declaringType, bool metadataName = false)
        {
            sb.Append(@namespace);
            if (sb.Length > 0)
            {
                sb.Append('.');
            }

            if (symbol.ContainingType != null && symbol.Kind != SymbolKind.TypeParameter)
            {
                sb.Append(metadataName ? declaringType.MetadataName : declaringType.Name);
                sb.Append('+');
            }
        }

        public static IMethodBody ResolveMethodBody(this IMethod methodInfo, IGenericContext genericContext)
        {
            return methodInfo.GetMethodBody()
                            ?? (genericContext != null && genericContext.MethodDefinition != null
                                ? genericContext.MethodDefinition.GetMethodBody(genericContext)
                                : null);
        }

        internal static MetadataDecoder GetMetadataDecoder(this PEModuleSymbol peModuleSymbol, IGenericContext genericContext)
        {
            if (genericContext != null)
            {
                if (genericContext.MethodDefinition != null)
                {
                    var methodDef = ((MetadataMethodAdapter)genericContext.MethodDefinition).MethodDef;
                    var contextMethod = methodDef as PEMethodSymbol;
                    if (contextMethod != null)
                    {
                        return new MetadataDecoder(peModuleSymbol, contextMethod);
                    }

                    var contextMethodOrig = methodDef.OriginalDefinition as PEMethodSymbol;
                    if (contextMethodOrig != null)
                    {
                        return new MetadataDecoder(peModuleSymbol, contextMethodOrig);
                    }

                    Debug.Assert(false, "Could not resolve Generic");
                }

                if (genericContext.TypeDefinition != null)
                {
                    var typeDef = ((MetadataTypeAdapter)genericContext.TypeDefinition).TypeDef;
                    var contextType = typeDef as PENamedTypeSymbol;
                    if (contextType != null)
                    {
                        return new MetadataDecoder(peModuleSymbol, contextType);
                    }

                    var contextTypeOrig = typeDef.OriginalDefinition as PENamedTypeSymbol;
                    if (contextTypeOrig != null)
                    {
                        return new MetadataDecoder(peModuleSymbol, contextTypeOrig);
                    }

                    Debug.Assert(false, "Could not resolve Generic");
                }
            }

            return new MetadataDecoder(peModuleSymbol);
        }
    }
}