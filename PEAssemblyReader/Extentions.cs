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
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;
    using Microsoft.CodeAnalysis.CSharp.Symbols.Metadata.PE;

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

            if (type.IsObject)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="methodInfo">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        /// <returns>
        /// </returns>
        public static IMethodBody ResolveMethodBody(this IMethod methodInfo, IGenericContext genericContext)
        {
            return methodInfo.GetMethodBody(genericContext)
                   ?? (genericContext != null && genericContext.MethodDefinition != null ? genericContext.MethodDefinition.GetMethodBody(genericContext) : null);
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
        /// <param name="sb">
        /// </param>
        /// <param name="namespace">
        /// </param>
        /// <param name="declaringType">
        /// </param>
        /// <param name="metadataName">
        /// </param>
        /// <param name="declTypeJoinChar">
        /// </param>
        internal static void AppendFullNamespace(
            this Symbol symbol, StringBuilder sb, string @namespace, IType declaringType, bool metadataName = false, char declTypeJoinChar = '+')
        {
            if (symbol.ContainingType != null && symbol.Kind != SymbolKind.TypeParameter)
            {
                sb.Append(metadataName ? declaringType.MetadataFullName : declaringType.FullName);
                sb.Append(declTypeJoinChar);
            }
            else
            {
                sb.Append(@namespace);
                if (sb.Length > 0)
                {
                    sb.Append('.');
                }
            }
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

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="map">
        /// </param>
        /// <returns>
        /// </returns>
        internal static IDictionary<IType, IType> GenericMap(this IType type, IDictionary<IType, IType> map)
        {
            if (type == null)
            {
                return map;
            }

            map.GenericMap(type.GenericTypeParameters, type.GenericTypeArguments);
            if (type.DeclaringType != null)
            {
                map = type.DeclaringType.GenericMap(map);
            }

            return map;
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <param name="map">
        /// </param>
        /// <returns>
        /// </returns>
        internal static IDictionary<IType, IType> GenericMap(this IMethod method, IDictionary<IType, IType> map)
        {
            if (method == null)
            {
                return map;
            }

            map.GenericMap(method.GetGenericParameters(), method.GetGenericArguments());
            if (method.DeclaringType != null)
            {
                map = method.DeclaringType.GenericMap(map);
            }

            return map;
        }

        /// <summary>
        /// </summary>
        /// <param name="map">
        /// </param>
        /// <param name="parameters">
        /// </param>
        /// <param name="arguments">
        /// </param>
        /// <returns>
        /// </returns>
        internal static IDictionary<IType, IType> GenericMap(this IDictionary<IType, IType> map, IEnumerable<IType> parameters, IEnumerable<IType> arguments)
        {
            var typeParameters = parameters.ToList();
            var typeArguments = arguments.ToList();

            for (var index = 0; index < typeArguments.Count; index++)
            {
                var typeParameter = typeParameters[index];
                var typeArgument = typeArguments[index];

                Debug.Assert(typeParameter.IsGenericParameter);
                Debug.Assert(!typeParameter.IsByRef);
                Debug.Assert(!typeArgument.IsByRef);
                
                map[typeParameter] = typeArgument.ToDereferencedType();
            }

            return map;
        }

        /// <summary>
        /// </summary>
        /// <param name="peModuleSymbol">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        /// <returns>
        /// </returns>
        internal static MetadataDecoder GetMetadataDecoder(this PEModuleSymbol peModuleSymbol, IGenericContext genericContext)
        {
            // TODO: make a review as you did for LocalVariables, so check if you need to use new MetadataDecoder(peModuleSymbol, peMethodSymbol) or new MetadataDecoder(peModuleSymbol, peTypeSymbol)
            if (genericContext != null)
            {
                var methodDefOrSpec = genericContext.MethodDefinition ?? genericContext.MethodSpecialization;
                if (methodDefOrSpec != null)
                {
                    var methodDef = ((MetadataMethodAdapter)methodDefOrSpec).MethodDef;
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

                var typeDefOrSpec = genericContext.TypeDefinition ?? genericContext.TypeSpecialization;
                if (typeDefOrSpec != null)
                {
                    var typeDef = ((MetadataTypeAdapter)typeDefOrSpec).TypeDef;
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

        /// <summary>
        /// </summary>
        /// <param name="typeSymbol">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        /// <returns>
        /// </returns>
        internal static IType ResolveGeneric(this TypeSymbol typeSymbol, IGenericContext genericContext, bool isByRef = false, bool isPinned = false)
        {
            if (genericContext != null && !genericContext.IsEmpty)
            {
                if (typeSymbol.IsTypeParameter())
                {
                    return genericContext.ResolveTypeParameter(new MetadataTypeAdapter(typeSymbol, isByRef, isPinned), isByRef, isPinned);
                }

                var arrayType = typeSymbol as ArrayTypeSymbol;
                if (arrayType != null)
                {
                    return arrayType.ElementType.ResolveGeneric(genericContext, isByRef, isPinned).ToArrayType(arrayType.Rank);
                }

                var namedTypeSymbol = typeSymbol as NamedTypeSymbol;
                if (namedTypeSymbol != null)
                {
                    var metadataType = new MetadataTypeAdapter(namedTypeSymbol, isByRef, isPinned);
                    if (metadataType.IsGenericTypeDefinition && !genericContext.IsEmpty)
                    {
                        return new MetadataTypeAdapter(namedTypeSymbol, genericContext, isByRef, isPinned);
                    }
                }
            }

            return new MetadataTypeAdapter(typeSymbol, genericContext, isByRef, isPinned);
        }

        /// <summary>
        /// </summary>
        /// <param name="methodSymbol">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        /// <returns>
        /// </returns>
        internal static IMethod ResolveGeneric(this MethodSymbol methodSymbol, IGenericContext genericContext)
        {
            return new MetadataMethodAdapter(methodSymbol, genericContext);
        }

        /// <summary>
        /// </summary>
        /// <param name="methodSymbol">
        /// </param>
        /// <param name="map">
        /// </param>
        /// <returns>
        /// </returns>
        private static ConstructedMethodSymbol ConstructGenericMethodSymbol(MethodSymbol methodSymbol, IDictionary<IType, IType> map)
        {
            var mapFilteredByTypeParameters = methodSymbol.ContainingType.TypeArguments != null
                                                  ? SelectGenericsFromArguments(methodSymbol.ContainingType, map)
                                                  : SelectGenericsFromParameters(methodSymbol.ContainingType, map);

            if (methodSymbol.IsGenericMethod)
            {
                var mapFilteredByMethodParameters = methodSymbol.TypeArguments != null
                                                        ? SelectGenericsFromArguments(methodSymbol, map)
                                                        : SelectGenericsFromParameters(methodSymbol, map);

                mapFilteredByTypeParameters = mapFilteredByTypeParameters.Union(mapFilteredByMethodParameters).ToArray();
            }

            Debug.Assert(mapFilteredByTypeParameters.Any());

            var newType = new ConstructedMethodSymbol(methodSymbol, ImmutableArray.Create(mapFilteredByTypeParameters));
            return newType;
        }

        /// <summary>
        /// </summary>
        /// <param name="namedTypeSymbol">
        /// </param>
        /// <param name="map">
        /// </param>
        /// <returns>
        /// </returns>
        private static ConstructedNamedTypeSymbol ConstructGenericTypeSymbol(NamedTypeSymbol namedTypeSymbol, IDictionary<IType, IType> map)
        {
            var mapFilteredByTypeParameters = namedTypeSymbol.TypeArguments != null
                                                  ? SelectGenericsFromArguments(namedTypeSymbol, map)
                                                  : SelectGenericsFromParameters(namedTypeSymbol, map);
            if (mapFilteredByTypeParameters == null)
            {
                return null;
            }

            var newType = new ConstructedNamedTypeSymbol(namedTypeSymbol.ConstructedFrom, ImmutableArray.Create(mapFilteredByTypeParameters));
            return newType;
        }

        /// <summary>
        /// </summary>
        /// <param name="namedTypeSymbol">
        /// </param>
        /// <param name="map">
        /// </param>
        /// <returns>
        /// </returns>
        private static TypeSymbol[] SelectGenericsFromArguments(NamedTypeSymbol namedTypeSymbol, IDictionary<IType, IType> map)
        {
            var resolvedTypes = new List<TypeSymbol>();
            if (!SelectGenericsFromArgumentsForOneLevel(namedTypeSymbol, map, resolvedTypes))
            {
                return null;
            }

            return resolvedTypes.ToArray();
        }

        /// <summary>
        /// </summary>
        /// <param name="methodSymbol">
        /// </param>
        /// <param name="map">
        /// </param>
        /// <returns>
        /// </returns>
        private static TypeSymbol[] SelectGenericsFromArguments(MethodSymbol methodSymbol, IDictionary<IType, IType> map)
        {
            var resolvedTypes = new List<TypeSymbol>();
            SelectGenericsFromArgumentsForOneLevel(methodSymbol, map, resolvedTypes);
            return resolvedTypes.ToArray();
        }

        /// <summary>
        /// </summary>
        /// <param name="namedTypeSymbol">
        /// </param>
        /// <param name="map">
        /// </param>
        /// <param name="resolvedTypes">
        /// </param>
        /// <returns>
        /// </returns>
        private static bool SelectGenericsFromArgumentsForOneLevel(
            NamedTypeSymbol namedTypeSymbol, IDictionary<IType, IType> map, List<TypeSymbol> resolvedTypes)
        {
            foreach (var typeSymbol in namedTypeSymbol.TypeArguments)
            {
                if (typeSymbol.Kind == SymbolKind.TypeParameter)
                {
                    var foundType = map.FirstOrDefault(pair => pair.Key.Name == typeSymbol.Name);
                    if (foundType.Key == null)
                    {
                        return false;
                    }

                    resolvedTypes.Add((foundType.Value as MetadataTypeAdapter).TypeDef);
                    continue;
                }

                var subTypeNamedTypeSymbol = typeSymbol as NamedTypeSymbol;
                if (subTypeNamedTypeSymbol != null && subTypeNamedTypeSymbol.Arity > 0)
                {
                    resolvedTypes.Add(ConstructGenericTypeSymbol(subTypeNamedTypeSymbol, map));
                    continue;
                }

                resolvedTypes.Add(subTypeNamedTypeSymbol);
            }

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="methodSymbol">
        /// </param>
        /// <param name="map">
        /// </param>
        /// <param name="resolvedTypes">
        /// </param>
        private static void SelectGenericsFromArgumentsForOneLevel(MethodSymbol methodSymbol, IDictionary<IType, IType> map, List<TypeSymbol> resolvedTypes)
        {
            ////if (namedTypeSymbol.IsNestedType())
            ////{
            ////    SelectGenericsFromArgumentsForOneLevel(namedTypeSymbol.ContainingType, map, resolvedTypes);
            ////}
            foreach (var typeSymbol in methodSymbol.TypeArguments)
            {
                if (typeSymbol.Kind == SymbolKind.TypeParameter)
                {
                    var foundType = map.First(pair => pair.Key.Name == typeSymbol.Name);
                    resolvedTypes.Add((foundType.Value as MetadataTypeAdapter).TypeDef);
                    continue;
                }

                var subTypeNamedTypeSymbol = typeSymbol as NamedTypeSymbol;
                if (subTypeNamedTypeSymbol != null && subTypeNamedTypeSymbol.Arity > 0)
                {
                    resolvedTypes.Add(ConstructGenericTypeSymbol(subTypeNamedTypeSymbol, map));
                    continue;
                }

                resolvedTypes.Add(subTypeNamedTypeSymbol);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="namedTypeSymbol">
        /// </param>
        /// <param name="map">
        /// </param>
        /// <returns>
        /// </returns>
        private static TypeSymbol[] SelectGenericsFromParameters(NamedTypeSymbol namedTypeSymbol, IDictionary<IType, IType> map)
        {
            return
                map.Where(pair => namedTypeSymbol.TypeParameters.Select(t => t).Any(tp => tp.Name == pair.Key.Name))
                   .Select(pair => (pair.Value as MetadataTypeAdapter).TypeDef)
                   .ToArray();
        }

        /// <summary>
        /// </summary>
        /// <param name="methodSymbol">
        /// </param>
        /// <param name="map">
        /// </param>
        /// <returns>
        /// </returns>
        private static TypeSymbol[] SelectGenericsFromParameters(MethodSymbol methodSymbol, IDictionary<IType, IType> map)
        {
            return
                map.Where(pair => methodSymbol.TypeParameters.Select(t => t).Any(tp => tp.Name == pair.Key.Name))
                   .Select(pair => (pair.Value as MetadataTypeAdapter).TypeDef)
                   .ToArray();
        }
    }
}