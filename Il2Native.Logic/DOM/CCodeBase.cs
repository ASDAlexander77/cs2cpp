namespace Il2Native.Logic.DOM
{
    using System.CodeDom.Compiler;
    using Microsoft.CodeAnalysis;

    public enum WriteSettings
    {
        Name,
        Token
    }

    public abstract class CCodeBase
    {
        public static void WriteNamespace(IndentedTextWriter itw, INamespaceSymbol namespaceSymbol)
        {
            foreach (var namespaceNode in namespaceSymbol.EnumNamespaces())
            {
                itw.Write("::");
                itw.Write(namespaceNode.MetadataName);
            }
        }

        public static void WriteName(IndentedTextWriter itw, ISymbol symbol)
        {
            itw.Write(symbol.MetadataName.CleanUpName());
        }

        public abstract void WriteTo(IndentedTextWriter itw, WriteSettings settings);
    }
}
