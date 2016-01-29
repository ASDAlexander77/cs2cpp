namespace Il2Native.Logic.DOM
{
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;

    public static class Helpers
    {
        public static IEnumerable<INamespaceSymbol> EnumNamespaces(this INamespaceSymbol namespaceSymbol)
        {
            if (namespaceSymbol == null)
            {
                yield break;
            }

            foreach (var enumNamespace in namespaceSymbol.ContainingNamespace.EnumNamespaces())
            {
                yield return enumNamespace;
            }

            yield return namespaceSymbol;
        }

        public static ITypeSymbol GetBaseType(this ITypeSymbol type)
        {
            var current = type;
            while (current.BaseType != null)
            {
                current = current.BaseType;
            }

            return current;
        }
    }
}
