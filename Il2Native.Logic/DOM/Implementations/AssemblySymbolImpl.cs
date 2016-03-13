namespace Il2Native.Logic.DOM.Implementations
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Globalization;
    using System.Threading;
    using Microsoft.CodeAnalysis;
    public class AssemblySymbolImpl : IAssemblySymbol
    {
        public SymbolKind Kind { get; private set; }
        public string Language { get; private set; }
        public string Name { get; set; }
        public string MetadataName { get; set; }
        public ISymbol ContainingSymbol { get; private set; }
        public IAssemblySymbol ContainingAssembly { get; private set; }
        public IModuleSymbol ContainingModule { get; private set; }
        public INamedTypeSymbol ContainingType { get; private set; }
        public INamespaceSymbol ContainingNamespace { get; private set; }
        public bool IsDefinition { get; private set; }
        public bool IsStatic { get; private set; }
        public bool IsVirtual { get; private set; }
        public bool IsOverride { get; private set; }
        public bool IsAbstract { get; private set; }
        public bool IsSealed { get; private set; }
        public bool IsExtern { get; private set; }
        public bool IsImplicitlyDeclared { get; private set; }
        public bool CanBeReferencedByName { get; private set; }
        public ImmutableArray<Location> Locations { get; private set; }
        public ImmutableArray<SyntaxReference> DeclaringSyntaxReferences { get; private set; }
        public ImmutableArray<AttributeData> GetAttributes()
        {
            throw new System.NotImplementedException();
        }

        public Accessibility DeclaredAccessibility { get; private set; }
        public ISymbol OriginalDefinition { get; private set; }
        public void Accept(SymbolVisitor visitor)
        {
            throw new System.NotImplementedException();
        }

        public TResult Accept<TResult>(SymbolVisitor<TResult> visitor)
        {
            throw new System.NotImplementedException();
        }

        public string GetDocumentationCommentId()
        {
            throw new System.NotImplementedException();
        }

        public string GetDocumentationCommentXml(
            CultureInfo preferredCulture = null,
            bool expandIncludes = false,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public string ToDisplayString(SymbolDisplayFormat format = null)
        {
            throw new System.NotImplementedException();
        }

        public ImmutableArray<SymbolDisplayPart> ToDisplayParts(SymbolDisplayFormat format = null)
        {
            throw new System.NotImplementedException();
        }

        public string ToMinimalDisplayString(SemanticModel semanticModel, int position, SymbolDisplayFormat format = null)
        {
            throw new System.NotImplementedException();
        }

        public ImmutableArray<SymbolDisplayPart> ToMinimalDisplayParts(SemanticModel semanticModel, int position, SymbolDisplayFormat format = null)
        {
            throw new System.NotImplementedException();
        }

        public bool HasUnsupportedMetadata { get; private set; }
        public bool IsInteractive { get; private set; }
        public AssemblyIdentity Identity { get; private set; }
        public INamespaceSymbol GlobalNamespace { get; private set; }
        public IEnumerable<IModuleSymbol> Modules { get; private set; }
        public ICollection<string> TypeNames { get; private set; }
        public ICollection<string> NamespaceNames { get; private set; }
        public bool GivesAccessTo(IAssemblySymbol toAssembly)
        {
            throw new System.NotImplementedException();
        }

        public INamedTypeSymbol GetTypeByMetadataName(string fullyQualifiedMetadataName)
        {
            throw new System.NotImplementedException();
        }

        public bool MightContainExtensionMethods { get; private set; }
        public INamedTypeSymbol ResolveForwardedType(string fullyQualifiedMetadataName)
        {
            throw new System.NotImplementedException();
        }
    }
}
