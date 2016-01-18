namespace Il2Native.Logic
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Symbols;
    using Microsoft.CodeAnalysis.CSharp.Symbols.Metadata.PE;

    public class CCodeGenerator
    {
        private Stack<CCodeUnit> cunits = new Stack<CCodeUnit>();  

        public CCodeGenerator(AssemblyMetadata assembly)
        {
            this.Assembly = assembly;
        }

        protected AssemblyMetadata Assembly { get; set; }

        public void Build()
        {
            foreach (var enumAllType in this.EnumAllTypes())
            {
                
            }
        }

        private static IEnumerable<NamespaceSymbol> GetAllNamespaces(NamespaceSymbol source)
        {
            yield return source;
            foreach (var namespaceSymbolSub in source.GetNamespaceMembers().SelectMany(GetAllNamespaces))
            {
                yield return namespaceSymbolSub;
            }
        }

        private static IEnumerable<NamespaceOrTypeSymbol> EnumAllNestedTypes(NamespaceOrTypeSymbol type)
        {
            foreach (var nestedType in type.GetTypeMembers())
            {
                yield return nestedType;
                foreach (var subNestedType in EnumAllNestedTypes(nestedType))
                {
                    yield return subNestedType;
                }
            }
        }

        private IEnumerable<NamespaceOrTypeSymbol> EnumAllTypes()
        {
            foreach (var module in this.Assembly.Modules.Cast<PEModuleSymbol>())
            {
                foreach (var metadataTypeAdapter in GetAllNamespaces(module.GlobalNamespace).SelectMany(n => n.GetTypeMembers()))
                {
                    yield return metadataTypeAdapter;
                    foreach (var nestedType in EnumAllNestedTypes(metadataTypeAdapter))
                    {
                        yield return nestedType;
                    }
                }
            }
        }
    }
}
