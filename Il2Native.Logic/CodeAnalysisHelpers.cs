namespace Il2Native.Logic
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;

    public static class CodeAnalysisHelpers
    {
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

        public static IEnumerable<INamespaceOrTypeSymbol> EnumAllNamespaces(this INamespaceOrTypeSymbol source)
        {
            yield return source;
            foreach (var namespaceSymbolSub in source.GetTypeMembers().OfType<INamespaceOrTypeSymbol>().SelectMany(EnumAllNamespaces))
            {
                yield return namespaceSymbolSub;
            }
        }

        public static IEnumerable<ITypeSymbol> EnumAllNestedTypes(this INamespaceOrTypeSymbol source)
        {
            foreach (var nestedType in source.GetTypeMembers())
            {
                yield return nestedType;
                foreach (var subNestedType in EnumAllNestedTypes(nestedType))
                {
                    yield return subNestedType;
                }
            }
        }
    }
}
