namespace Il2Native.Logic.DOM
{
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;

    public static class Helpers
    {
        public static IEnumerable<INamespaceSymbol> EnumNamespaces(this INamespaceSymbol namespaceSymbol)
        {
            if (namespaceSymbol == null || namespaceSymbol.IsGlobalNamespace)
            {
                yield break;
            }

            foreach (var enumNamespace in namespaceSymbol.ContainingNamespace.EnumNamespaces())
            {
                if (enumNamespace.IsGlobalNamespace)
                {
                    continue;
                }

                yield return enumNamespace;
            }

            yield return namespaceSymbol;
        }
    }
}
