// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;

    public static class CodeAnalysisHelpers
    {
        public static IEnumerable<INamespaceOrTypeSymbol> EnumAllNamespaces(this INamespaceOrTypeSymbol source)
        {
            yield return source;
            foreach (var namespaceSymbolSub in source.GetMembers().OfType<INamespaceOrTypeSymbol>().SelectMany(EnumAllNamespaces))
            {
                yield return namespaceSymbolSub;
            }
        }

        public static IEnumerable<ITypeSymbol> EnumAllNestedTypes(this INamespaceOrTypeSymbol source)
        {
            return source.GetTypeMembers().SelectMany(nestedType => EnumAllNestedTypes(nestedType));
        }

        public static IEnumerable<ITypeSymbol> EnumAllTypes(this IModuleSymbol module)
        {
            foreach (var metadataTypeAdapter in module.GlobalNamespace.EnumAllNamespaces().SelectMany(n => n.GetTypeMembers()))
            {
                yield return metadataTypeAdapter;
                foreach (var nestedType in metadataTypeAdapter.EnumAllNestedTypes())
                {
                    yield return nestedType;
                }
            }
        }

        public static IEnumerable<IMethodSymbol> EnumerateAllMethodsRecursevly(this INamedTypeSymbol type)
        {
            /*
            if (type.TypeKind == TypeKind.Interface)
            {
                foreach (var memberBase in type.Interfaces.SelectMany(i => i.EnumerateAllMethodsRecursevly()))
                {
                    yield return memberBase;
                }
            }
            */ 

            if (type.BaseType != null)
            {
                foreach (var memberBase in type.BaseType.EnumerateAllMethodsRecursevly())
                {
                    yield return memberBase;
                }
            }

            foreach (var member in type.GetMembers()
                        .OfType<IMethodSymbol>()
                        .Where(m => m.MethodKind != MethodKind.Constructor && m.MethodKind != MethodKind.StaticConstructor && m.MethodKind != MethodKind.Destructor))
            {
                yield return member;
            }
        }

        public static bool IsDerivedFrom(this ITypeSymbol source, ITypeSymbol from)
        {
            var current = source.BaseType;
            while (current != null && current != from)
            {
                current = current.BaseType;
            }

            return current != null;
        }

        public static IEnumerable<IMethodSymbol> IterateAllMethodsWithTheSameNames(this ITypeSymbol type)
        {
            return IterateAllMethodsWithTheSameNames((INamedTypeSymbol)type);
        }

        public static IEnumerable<IMethodSymbol> IterateAllMethodsWithTheSameNamesTakeOnlyOne(this ITypeSymbol type)
        {
            return IterateAllMethodsWithTheSameNamesTakeOnlyOne((INamedTypeSymbol)type);
        }

        private static IEnumerable<IMethodSymbol> IterateAllMethodsWithTheSameNames(INamedTypeSymbol type)
        {
            if (type.TypeKind != TypeKind.Interface && type.BaseType == null)
            {
                return new IMethodSymbol[0];
            }

            var methods = new List<IMethodSymbol>();

            var groupsByName = type.EnumerateAllMethodsRecursevly().GroupBy(m => m.MetadataName);
            foreach (var groupByName in groupsByName)
            {
                var groupByType = groupByName.GroupBy(g => g.ContainingType);
                if (groupByType.Count() < 2)
                {
                    continue;
                }

                methods.AddRange(groupByName.Distinct(new KeyStringEqualityComparer()).Where(m => m.ContainingType != type));
            }

            return methods;
        }

        private static IEnumerable<IMethodSymbol> IterateAllMethodsWithTheSameNamesTakeOnlyOne(INamedTypeSymbol type)
        {
            if (type.TypeKind != TypeKind.Interface && type.BaseType == null || type.TypeKind == TypeKind.Interface)
            {
                return new IMethodSymbol[0];
            }

            var methods = new List<IMethodSymbol>();

            var groupsByName = type.EnumerateAllMethodsRecursevly().GroupBy(m => m.MetadataName);
            foreach (var groupByName in groupsByName)
            {
                var groupByType = groupByName.GroupBy(g => g.ContainingType);
                if (groupByType.Count() < 2)
                {
                    continue;
                }

                var lastOrDefault = groupByName.Distinct(new KeyStringEqualityComparer()).LastOrDefault(m => m.ContainingType != type);
                if (lastOrDefault != null)
                {
                    methods.Add(lastOrDefault);
                }
            }

            return methods;
        }

        public class KeyStringEqualityComparer : IEqualityComparer<IMethodSymbol>
        {
            public bool Equals(IMethodSymbol x, IMethodSymbol y)
            {
                if (string.Compare(x.ContainingType.ToString(), y.ContainingType.ToString(), StringComparison.Ordinal) != 0)
                {
                    return false;
                }

                return string.Compare(x.MetadataName, y.MetadataName, StringComparison.Ordinal) == 0;
            }

            public int GetHashCode(IMethodSymbol obj)
            {
                var hash = 17;
                hash = hash * 31 + obj.ContainingType.GetHashCode();
                hash = hash * 31 + obj.MetadataName.GetHashCode();
                return hash;
            }
        }

        public class TypeParameterByNameEqualityComparer : IEqualityComparer<ITypeParameterSymbol>
        {
            public bool Equals(ITypeParameterSymbol x, ITypeParameterSymbol y)
            {
                return string.Compare(x.Name, y.Name, StringComparison.Ordinal) == 0;
            }

            public int GetHashCode(ITypeParameterSymbol obj)
            {
                var hash = 17;
                hash = hash * 31 + obj.Name.GetHashCode();
                return hash;
            }
        }

        public class TypeParameterByReferenceEqualityComparer : IEqualityComparer<ITypeParameterSymbol>
        {
            public bool Equals(ITypeParameterSymbol x, ITypeParameterSymbol y)
            {
                return x.Name == y.Name;
            }

            public int GetHashCode(ITypeParameterSymbol obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
